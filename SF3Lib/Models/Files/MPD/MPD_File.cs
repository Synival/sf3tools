using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Structs.MPD.TextureAnimation;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static CommonLib.Imaging.PixelConversion;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        private const int c_RamOffset = 0x290000;
        private const int c_SurfaceModelChunkSize = 0xCF00;

        protected MPD_File(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts) : base(data, nameContexts?[DetectScenario(data)], DetectScenario(data)) {
            PrimaryTextureChunksFirstIndex = 6;
            PrimaryTextureChunksLastIndex  = PrimaryTextureChunksFirstIndex +
                ((Scenario >= ScenarioType.Other) ? 4 : 3);

            ExtraModelTextureChunkIndex = (Scenario >= ScenarioType.Scenario2) ? 21 : (int?) null;

            MeshTextureChunksFirstIndex = PrimaryTextureChunksLastIndex + 1;
            MeshTextureChunksLastIndex = MeshTextureChunksFirstIndex +
                ((Scenario >= ScenarioType.Scenario1) ? 2 : 1);

            RepeatingGroundChunk1Index = MeshTextureChunksLastIndex + 1;
            RepeatingGroundChunk2Index = MeshTextureChunksLastIndex + 2;

            TiledGroundTileChunk1Index = MeshTextureChunksLastIndex + 1;
            TiledGroundTileChunk2Index = MeshTextureChunksLastIndex + 2;
            TiledGroundMapChunks1Index = RepeatingGroundChunk2Index + 1;
            TiledGroundMapChunks2Index = RepeatingGroundChunk2Index + 4;

            SkyBoxChunk1Index          = RepeatingGroundChunk2Index + 2;
            SkyBoxChunk2Index          = RepeatingGroundChunk2Index + 3;

            BackgroundChunk1Index      = MeshTextureChunksLastIndex + 1;
            BackgroundChunk2Index      = MeshTextureChunksLastIndex + 2;

            ForegroundTileChunk1Index  = BackgroundChunk2Index + 2;
            ForegroundTileChunk2Index  = BackgroundChunk2Index + 3;
            ForegroundMapChunkIndex    = BackgroundChunk2Index + 4;
        }

        public static MPD_File Create(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts) {
            var newFile = new MPD_File(data, nameContexts);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public static ScenarioType DetectScenario(IByteData data) {
            // Get addresses we need to check.
            var headerAddrPtr = data.GetDouble(0x0000) - c_RamOffset;
            var headerAddr    = data.GetDouble(headerAddrPtr) - c_RamOffset;

            var chunk18Addr   = data.GetDouble(0x2090);
            var chunk19Addr   = data.GetDouble(0x2098);
            var palette3Addr  = data.GetDouble(headerAddr + 0x0044);
            var chunk21Addr   = data.GetDouble(0x20A8);

            // Determine some things about this MPD file.
            var hasChunk18  = (chunk18Addr > 0);
            var hasChunk19  = (chunk19Addr > 0);
            var hasPalette3 = (palette3Addr & 0xFFFF0000) == 0x00290000;
            var hasChunk21  = (chunk21Addr > 0);

            // We should be able to accurately detect the scenario.
            // Scenario 3 and the Premium Disk have the same MPD format.
            if (hasPalette3)
                return ScenarioType.Scenario3;
            else if (hasChunk21)
                return ScenarioType.Scenario2;
            else if (hasChunk19)
                return ScenarioType.Scenario1;
            else if (hasChunk18)
                return ScenarioType.Other;
            else
                return ScenarioType.Ship2;
        }

        public override IEnumerable<ITable> MakeTables() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Load root headers
            var header = MakeHeader();
            var headerTables = MakeHeaderTables(header, areAnimatedTextures32Bit);

            // Load chunks
            var chunks = MakeChunkHeaderTable().Rows;
            var chunkDatas = MakeChunkDatas(chunks);
            var chunkTables = MakeChunkTables(chunks, chunkDatas, ModelsChunkData, SurfaceChunkData);

            // Add two-way communication between 'Modified' events from the root IByteData and its children.
            WireChildDataModifiedEvents();

            // Build a list of all data tables.
            var tables = new List<ITable>() {
                ChunkLocations,
            };
            tables.AddRange(headerTables);
            tables.AddRange(chunkTables);

            InitTiles();
            return tables;
        }

        private MPDHeaderModel MakeHeader() {
            var headerAddrPtr = Data.GetDouble(0x0000) - c_RamOffset;
            var headerAddr = Data.GetDouble(headerAddrPtr) - c_RamOffset;
            return (MPDHeader = new MPDHeaderModel(Data, 0, "MPDHeader", headerAddr, Scenario));
        }

        private ITable[] MakeHeaderTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            tables.AddRange(MakeLightingTables(header));
            tables.AddRange(MakeTexturePaletteTables(header));
            tables.AddRange(MakeTextureAnimationTables(header, areAnimatedTextures32Bit));
            tables.Add(BoundariesTable = BoundaryTable.Create(Data, "Boundaries", ResourceFile("BoundaryList.xml"), header.OffsetBoundaries - c_RamOffset));
            tables.AddRange(MakeMovableModelCollections(header));
            tables.AddRange(MakeUnknownTables(header));

            return tables.ToArray();
        }

        private ChunkLocationTable MakeChunkHeaderTable()
            => ChunkLocations = ChunkLocationTable.Create(Data, "ChunkHeader", 0x2000);

        private ITable[] MakeTexturePaletteTables(MPDHeaderModel header) {
            PaletteTables = new ColorTable[3];
            var headerRamAddr = header.Address + c_RamOffset;

            // Sometimes palette addresses are placed in an odd place at or just before the header actually begins.
            // This is most likely an error in the MPD file; it results in garbage data.
            // Don't load the palettes in these cases.
            if (header.OffsetPal1 >= c_RamOffset && (headerRamAddr - header.OffsetPal1) / 2 >= 256)
                PaletteTables[0] = ColorTable.Create(Data, "TexturePalette1", header.OffsetPal1 - c_RamOffset, 256);
            if (header.OffsetPal2 >= c_RamOffset && (headerRamAddr - header.OffsetPal2) / 2 >= 256)
                PaletteTables[1] = ColorTable.Create(Data, "TexturePalette2", header.OffsetPal2 - c_RamOffset, 256);
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 >= c_RamOffset && (headerRamAddr - header.OffsetPal3) / 2 >= 256)
                PaletteTables[2] = ColorTable.Create(Data, "TexturePalette3", header.OffsetPal3 - c_RamOffset, 256);

            return PaletteTables.Where(x => x != null).ToArray();
        }

        private ITable[] MakeLightingTables(MPDHeaderModel header) {
            var tables = new List<ITable>();

            if (header.OffsetLightPalette != 0)
                tables.Add(LightPalette = ColorTable.Create(Data, "LightPalette", header.OffsetLightPalette - c_RamOffset, 32));
            if (header.OffsetLightPosition != 0)
                LightPosition = new LightPosition(Data, 0, "LightPositions", header.OffsetLightPosition - c_RamOffset);
            if (header.OffsetLightAdjustment != 0)
                LightAdjustment = new LightAdjustmentModel(Data, 0, "LightAdjustment", header.OffsetLightAdjustment - c_RamOffset, Scenario);

            if (header.OffsetGradient != 0)
                tables.Add(GradientTable = GradientTable.Create(Data, "Gradients", header.OffsetGradient - c_RamOffset));

            return tables.ToArray();
        }

        private ITable[] MakeTextureAnimationTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            if (header.OffsetTextureAnimations != 0) {
                try {
                    tables.Add(TextureAnimations = TextureAnimationTable.Create(Data, "TextureAnimations", header.OffsetTextureAnimations - c_RamOffset, areAnimatedTextures32Bit));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            if (header.OffsetTextureAnimAlt != 0) {
                try {
                    tables.Add(TextureAnimationsAlt = TextureIDTable.Create(Data, "TextureAnimationsAlt", header.OffsetTextureAnimAlt - c_RamOffset, 2, 0x100));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            return tables.ToArray();
        }

        private ITable[] MakeMovableModelCollections(MPDHeaderModel header) {
            var tables = new List<ITable>();

            var modelsList = new List<ModelCollection>();
            if (ModelCollections != null)
                modelsList.AddRange(ModelCollections);

            var offsets = new int[] { header.OffsetMesh1, header.OffsetMesh2, header.OffsetMesh3 };
            for (int i = 0; i < 3; i++) {
                var offset = offsets[i];
                if (offset == 0)
                    continue;

                var newModel = ModelCollection.Create(Data, NameGetterContext, offset - c_RamOffset, "MovableModels" + (i + 1), i);
                modelsList.Add(newModel);
                tables.AddRange(newModel.Tables);
            }

            ModelCollections = modelsList.ToArray();

            return tables.ToArray();
        }

        private ITable[] MakeUnknownTables(MPDHeaderModel header) {
            var tables = new List<ITable>();

            // TODO: put somewhere else!!
            if (header.OffsetModelSwitchGroups != 0) {
                tables.Add(ModelSwitchGroupsTable = ModelSwitchGroupsTable.Create(Data, "ModelSwitchGroups", header.OffsetModelSwitchGroups - c_RamOffset));

                VisibleModelsWhenFlagOffByAddr = ModelSwitchGroupsTable
                    .Where(x => x.VisibleModelsWhenFlagOffOffset > 0)
                    .ToDictionary(
                        x => (int) x.VisibleModelsWhenFlagOffOffset,
                        x => ModelIDTable.Create(Data, x.Name + "_FlagOffIDs (0x" + x.VisibleModelsWhenFlagOffOffset.ToString("X") + ")", (int) x.VisibleModelsWhenFlagOffOffset - c_RamOffset)
                    );

                VisibleModelsWhenFlagOnByAddr = ModelSwitchGroupsTable
                    .Where(x => x.VisibleModelsWhenFlagOnOffset > 0)
                    .ToDictionary(
                        x => (int) x.VisibleModelsWhenFlagOnOffset,
                        x => ModelIDTable.Create(Data, x.Name + "_FlagOnIDs (0x" + x.VisibleModelsWhenFlagOnOffset.ToString("X") + ")", (int) x.VisibleModelsWhenFlagOnOffset - c_RamOffset)
                    );

                tables.AddRange(VisibleModelsWhenFlagOffByAddr.Values);
                tables.AddRange(VisibleModelsWhenFlagOnByAddr.Values);
            }

            // TODO: put somewhere else!!
            if (header.OffsetGroundAnimation != 0)
                tables.Add(GroundAnimationTable = UnknownUInt8Table.Create(Data, "ScrollScreenAnimations", header.OffsetGroundAnimation - c_RamOffset, null, 0xFF));

            // TODO: put somewhere else!!
            if (header.OffsetIndexedTextures != 0)
                tables.Add(IndexedTextureTable = TextureIDTable.Create(Data, "IndexedTextures", header.OffsetIndexedTextures - c_RamOffset, 4, 0x100));

            // This table is only present before Scenario 2 and is always 32 bytes if it exists.
            if (header.OffsetUnknown1 != 0) {
                // Use at most 0x20 2-byte values (0x40 bytes total).
                int lowestOffset = header.OffsetUnknown1 + 0x40;

                void updateLowest(int value) {
                    if (value < lowestOffset && value >= header.OffsetUnknown1)
                        lowestOffset = value;
                }

                // The offset model switch groups usually (always?) occupy some space before its address.
                // Make sure this table doesn't occupy that space.
                updateLowest(header.OffsetModelSwitchGroups);
                foreach (var msg in ModelSwitchGroupsTable) {
                    updateLowest((int) msg.VisibleModelsWhenFlagOnOffset);
                    updateLowest((int) msg.VisibleModelsWhenFlagOffOffset);
                }
                updateLowest(header.OffsetTextureAnimations);
                updateLowest(header.OffsetUnknown2);

                // We have our best guess for the size. Add the table!
                var lengthInBytes = ((int) lowestOffset - header.OffsetUnknown1);
                var size = Math.Min(32, lengthInBytes / 2);

                if (size > 0)
                    tables.Add(Unknown1Table = UnknownUInt16Table.Create(Data, "Unknown1", header.OffsetUnknown1 - c_RamOffset, size, null));
            }

            if (header.OffsetUnknown2 != 0)
                tables.Add(Unknown2Table = UnknownUInt16Table.Create(Data, "Unknown2", header.OffsetUnknown2 - c_RamOffset, 32, 0xFFFF));

            return tables.ToArray();
        }

        private IChunkData[] MakeChunkDatas(ChunkLocation[] chunks) {
            ChunkData = new IChunkData[chunks.Length];

            // Surface model chunk
            SurfaceModelChunkIndex = GetSurfaceModelChunkIndex(chunks);
            if (SurfaceModelChunkIndex != null)
                _ = MakeChunkData(SurfaceModelChunkIndex.Value, ChunkType.SurfaceModel, CompressionType.Uncompressed);

            // All model chunks
            ModelsChunkIndices = GetModelsChunkIndices(chunks);
            var modelsChunksList = new List<IChunkData>();
            foreach (var i in ModelsChunkIndices) {
                _ = MakeChunkData(i, ChunkType.Models, CompressionType.Uncompressed);
                modelsChunksList.Add(ChunkData[i]);
            }
            ModelsChunkData = modelsChunksList.ToArray();

            // Animated textures chunk
            if (chunks[3].Exists)
                _ = MakeChunkData(3, ChunkType.AnimationFrames, CompressionType.IndividuallyCompressed);

            // Surface chunk (heightmap, terrain, event IDs)
            if (chunks[5].Exists)
                _ = MakeChunkData(5, ChunkType.Surface, CompressionType.Compressed);

            // Texture data, in chunks (6...13)
            for (var i = PrimaryTextureChunksFirstIndex; i <= MeshTextureChunksLastIndex; i++)
                if (chunks[i].Exists)
                    _ = MakeChunkData(i, ChunkType.Textures, CompressionType.Compressed);
            if (ExtraModelTextureChunkIndex.HasValue && chunks[ExtraModelTextureChunkIndex.Value].Exists)
                _ = MakeChunkData(ExtraModelTextureChunkIndex.Value, ChunkType.Textures, CompressionType.Compressed);

            // Repeating backgrounds
            var repeatingGroundChunks = new List<IChunkData>();
            if (MPDHeader.GroundImageType == GroundImageType.Repeated) {
                if (chunks[RepeatingGroundChunk1Index].Exists)
                    repeatingGroundChunks.Add(_ = MakeChunkData(RepeatingGroundChunk1Index, ChunkType.Palette1Image, CompressionType.Compressed));
                if (chunks[RepeatingGroundChunk2Index].Exists)
                    repeatingGroundChunks.Add(_ = MakeChunkData(RepeatingGroundChunk2Index, ChunkType.Palette1Image, CompressionType.Compressed));
            }
            RepeatingGroundChunks = repeatingGroundChunks.ToArray();

            // Tiled ground images
            var tiledGroundTileChunks = new List<IChunkData>();
            var tiledGroundMapChunks = new List<IChunkData>();
            if (MPDHeader.GroundImageType == GroundImageType.Tiled) {
                if (chunks[TiledGroundTileChunk1Index].Exists)
                    tiledGroundTileChunks.Add(_ = MakeChunkData(TiledGroundTileChunk1Index, ChunkType.TiledGroundTiles, CompressionType.Compressed));
                if (chunks[TiledGroundTileChunk2Index].Exists)
                    tiledGroundTileChunks.Add(_ = MakeChunkData(TiledGroundTileChunk2Index, ChunkType.TiledGroundTiles, CompressionType.Compressed));
                if (chunks[TiledGroundMapChunks1Index].Exists)
                    tiledGroundMapChunks.Add(_ = MakeChunkData(TiledGroundMapChunks1Index, ChunkType.TiledGroundMap, CompressionType.Compressed));
                if (chunks[TiledGroundMapChunks2Index].Exists)
                    tiledGroundMapChunks.Add(_ = MakeChunkData(TiledGroundMapChunks2Index, ChunkType.TiledGroundMap, CompressionType.Compressed));
            }
            TiledGroundTileChunks = tiledGroundTileChunks.ToArray();
            TiledGroundMapChunks = tiledGroundMapChunks.ToArray();

            // Sky boxes
            var skyBoxChunks = new List<IChunkData>();
            if (MPDHeader.HasSkyBox) {
                if (chunks[SkyBoxChunk1Index].Exists)
                    skyBoxChunks.Add(_ = MakeChunkData(SkyBoxChunk1Index, ChunkType.Palette2Image, CompressionType.Compressed));
                if (chunks[SkyBoxChunk2Index].Exists)
                    skyBoxChunks.Add(_ = MakeChunkData(SkyBoxChunk2Index, ChunkType.Palette2Image, CompressionType.Compressed));
            }
            SkyBoxChunks = skyBoxChunks.ToArray();

            // Background image
            var backgroundChunks = new List<IChunkData>();
            if (MPDHeader.BackgroundImageType.HasFlag(BackgroundImageType.Still)) {
                if (chunks[BackgroundChunk1Index].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(BackgroundChunk1Index, ChunkType.Palette1Image, CompressionType.Compressed));
                if (chunks[BackgroundChunk2Index].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(BackgroundChunk2Index, ChunkType.Palette1Image, CompressionType.Compressed));
            }
            BackgroundChunks = backgroundChunks.ToArray();

            // Foreground image tiles
            var foregroundTileChunks = new List<IChunkData>();
            if (MPDHeader.BackgroundImageType.HasFlag(BackgroundImageType.Tiled)) {
                if (chunks[ForegroundTileChunk1Index].Exists)
                    foregroundTileChunks.Add(_ = MakeChunkData(ForegroundTileChunk1Index, ChunkType.ForegroundTiles, CompressionType.Compressed));
                if (chunks[ForegroundTileChunk2Index].Exists)
                    foregroundTileChunks.Add(_ = MakeChunkData(ForegroundTileChunk2Index, ChunkType.ForegroundTiles, CompressionType.Compressed));
                if (chunks[ForegroundMapChunkIndex].Exists)
                    ForegroundMapChunk = MakeChunkData(ForegroundMapChunkIndex, ChunkType.ForegroundMap, CompressionType.Compressed);
            }
            ForegroundTileChunks = foregroundTileChunks.ToArray();

            // Add unhandled images/scroll planes, in case they're not indicated by flags.
            // TODO: we should know what these are 100% of the time if the map flags are off.
            for (var i = RepeatingGroundChunk1Index; i <= TiledGroundMapChunks2Index; i++)
                if (ChunkData[i] == null && chunks[i].Exists)
                    _ = MakeChunkData(i, ChunkType.UnhandledImageOrData, CompressionType.Compressed);

            // Add remaining unhandled chunks.
            for (var i = 0; i < chunks.Length; i++)
                if (ChunkData[i] == null && chunks[i].Exists)
                    _ = MakeChunkData(i, ChunkType.Unknown, CompressionType.Uncompressed);

            return ChunkData;
        }

        public IChunkData MakeChunkData(int chunkIndex, ChunkType type, CompressionType compressionType) {
            if (ChunkData[chunkIndex] != null)
                throw new ArgumentException(nameof(chunkIndex));

            var isCompressed = (compressionType == CompressionType.Compressed);
            ByteArraySegment byteArraySegment = null;
            ChunkData chunkData = null;

            try {
                byteArraySegment = new ByteArraySegment(Data.Data, ChunkLocations[chunkIndex].ChunkRAMAddress - c_RamOffset, ChunkLocations[chunkIndex].ChunkSize);
                chunkData = new ChunkData(byteArraySegment, isCompressed, chunkIndex);
            }
            catch {
                // TODO: what to do???
                return null;
            }
            var chunkLocations = ChunkLocations[chunkIndex];

            chunkLocations.DecompressedSize = chunkData.DecompressedData.Length;
            chunkData.DecompressedData.Data.RangeModified += (s, a) => {
                if (a.Resized)
                    chunkLocations.DecompressedSize = chunkData.DecompressedData.Length;
            };

            chunkData.Data.RangeModified += (s, a) => {
                var chunkName = chunkLocations.Name;
                if (a.Moved) {
                    // Figure out how much the offset has changed.
                    var oldOffset = chunkLocations.ChunkRAMAddress - c_RamOffset;
                    var newOffset = byteArraySegment.Offset;
                    var offsetDelta = newOffset - oldOffset;
                    if (offsetDelta == 0)
                        return;

                    // Update the address in the chunk table.
                    chunkLocations.ChunkRAMAddress = newOffset + c_RamOffset;

                    // Chunks after this one with something assigned to ChunkData[] will have their
                    // ChunkAddress updated automatically. For chunks without a ChunkData[] after this one
                    // (but before the next ChunkData), update addresses manually.
                    for (var j = chunkIndex + 1; j < ChunkLocations.Length; j++) {
                        if (ChunkData[j] != null)
                            break;

                        var ch = ChunkLocations[j];
                        if (ch != null && ch.ChunkRAMAddress != 0)
                            ch.ChunkRAMAddress += offsetDelta;
                    }
                }
                if (a.Resized)
                    chunkLocations.ChunkSize = chunkData.Data.Length;
            };

            // Add some integrity checks after recompression.
            chunkData.Recompressed += (s, e) => {
                var errors = new List<string>();
                for (var i = 0; i < ChunkLocations.Length; i++) {
                    var ch = ChunkLocations[i];
                    var cd = ChunkData[i];
                    if (ch.ChunkRAMAddress == 0 || cd == null)
                        continue;

                    if (ch.ChunkFileAddress != cd.Offset) {
                        errors.Add(
                            $"ChunkHeader[{i}].ChunkFileAddress and ChunkData[{i}].Offset mismatch after recompress: " +
                            $"{ch.ChunkFileAddress} vs {cd.Offset}");
                    }
                    if (ch.ChunkSize != cd.Length) {
                        errors.Add(
                            $"ChunkHeader[{i}].ChunkSize and ChunkData[{i}].Length length/size mismatch after recompress: " +
                            $"{ch.ChunkSize} vs {cd.Length}");
                    }
                }

                if (errors.Count > 0) {
                    throw new InvalidOperationException(
                        "Integrity checks failed when recompressing chunk " + chunkIndex + ":\r\n" +
                        string.Join("\r\n", errors)
                    );
                }
            };

            chunkLocations.ChunkType = type;
            chunkLocations.CompressionType = compressionType;

            ChunkData[chunkIndex] = chunkData;
            return chunkData;
        }

        private int[] GetModelsChunkIndices(ChunkLocation[] chunks) {
            var header = MPDHeader;
            var indices = new List<int>();

            if (chunks[20].Exists && header.Chunk20IsModels)
                indices.Add(20);

            if (chunks[1].Exists)
                if (!header.Chunk20IsModels || header.HasExtraChunk1ModelWithChunk21Textures)
                    indices.Add(1);

            if (chunks[19].Exists && MPDHeader.HasChunk19Model)
                indices.Add(19);

            return indices.ToArray();
        }

        private int? GetSurfaceModelChunkIndex(ChunkLocation[] chunks) {
            var header = MPDHeader;

            if (!header.HasSurfaceModel)
                return null;

            var chunkIndex = header.SurfaceModelChunkIndex;
            if (chunkIndex == null || !chunks[chunkIndex.Value].Exists)
                return null;

            return chunkIndex.Value;
        }

        private ITable[] MakeChunkTables(ChunkLocation[] chunkHeaders, IChunkData[] chunkDatas, IChunkData[] modelsChunks, IChunkData surfaceModelChunk) {
            TextureCollectionType TextureCollectionForChunkIndex(int chunkIndex) {
                if (chunkIndex == 10 && MPDHeader.HasChunk19Model)
                    return TextureCollectionType.Chunk19ModelTextures;

                if (chunkIndex >= PrimaryTextureChunksFirstIndex && chunkIndex <= PrimaryTextureChunksLastIndex)
                    return TextureCollectionType.PrimaryTextures;

                if (chunkIndex == MeshTextureChunksFirstIndex + 0 && chunkIndex <= MeshTextureChunksLastIndex)
                    return TextureCollectionType.MovableModels1;
                if (chunkIndex == MeshTextureChunksFirstIndex + 1 && chunkIndex <= MeshTextureChunksLastIndex)
                    return TextureCollectionType.MovableModels2;
                if (chunkIndex == MeshTextureChunksFirstIndex + 2 && chunkIndex <= MeshTextureChunksLastIndex)
                    return TextureCollectionType.MovableModels3;

                if (chunkIndex == ExtraModelTextureChunkIndex)
                    return TextureCollectionType.Chunk1ModelTextures;

                throw new Exception("Can't determine texture collection based on chunk index");
            }

            var tables = new List<ITable>();

            var modelsList = new List<ModelCollection>();
            if (ModelCollections != null)
                modelsList.AddRange(ModelCollections);

            foreach (var mc in modelsChunks) {
                var collectionType = (mc.Index == 19 && MPDHeader.HasChunk19Model)
                    ? ModelCollectionType.Chunk19Model
                    : (chunkDatas[21] != null && mc.Index == 1) ? ModelCollectionType.Chunk1Model
                    : ModelCollectionType.PrimaryModels;

                var newModel = ModelCollection.Create(mc.DecompressedData, NameGetterContext, 0x00, "Models" + mc.Index, Scenario, mc.Index, collectionType);
                modelsList.Add(newModel);
                tables.AddRange(newModel.Tables);
            }
            ModelCollections = modelsList.ToArray();

            if (chunkDatas[5] != null) {
                Surface = Surface.Create(chunkDatas[5].DecompressedData, NameGetterContext, 0x00, "Surface", 5);
                tables.AddRange(Surface.Tables);
            }

            // TODO: get this chunk loading with Ship2?
            if (surfaceModelChunk != null && Scenario != ScenarioType.Ship2) {
                SurfaceModel = SurfaceModel.Create(surfaceModelChunk.DecompressedData, NameGetterContext, 0x00, "SurfaceModel", surfaceModelChunk.Index, Scenario);
                tables.AddRange(SurfaceModel.Tables);
            }

            // Gather all information about texture picture formats from existing data.
            // Each texture collection needs its own info.
            var pixelFormats = new Dictionary<TextureCollectionType, Dictionary<int, TexturePixelFormat>>();
            foreach (var texCollection in (TextureCollectionType[]) Enum.GetValues(typeof(TextureCollectionType)))
                pixelFormats[texCollection] = new Dictionary<int, TexturePixelFormat>();
            var primaryPixelFormats = pixelFormats[TextureCollectionType.PrimaryTextures];

            // Always ABGR1555 for surface tiles.
            if (SurfaceModel != null) {
                var textureIds = SurfaceModel.TileTextureRowTable
                    .SelectMany(row => {
                        var ids = new byte[64];
                        for (var x = 0; x < 64; x++)
                            ids[x] = row.GetTextureID(x);
                        return ids;
                    })
                    .Where(x => x != 0xFF)
                    .Distinct()
                    .ToArray();

                foreach (var id in textureIds)
                    primaryPixelFormats[id] = TexturePixelFormat.ABGR1555;
            }

            // Textures in models are ABGR1555.
            foreach (var models in ModelCollections) {
                if (models?.AttrTablesByMemoryAddress != null)
                    foreach (var attrTable in models.AttrTablesByMemoryAddress.Values)
                        foreach (var attr in attrTable)
                            primaryPixelFormats[attr.TextureNo] = TexturePixelFormat.ABGR1555;
            }

            // Textures in the alt animation frames table are ABGR1555.
            if (TextureAnimationsAlt != null)
                foreach (var tex in TextureAnimationsAlt)
                    primaryPixelFormats[tex.TextureID] = TexturePixelFormat.ABGR1555;

            // If the indexed textures table is present (Scenario 3 + PD only), assume Palette3.
            if (IndexedTextureTable != null)
                foreach (var tex in IndexedTextureTable)
                    primaryPixelFormats[tex.TextureID] = TexturePixelFormat.Palette3;

            // Gather palettes.
            var palettes = CreatePalettesForTextures();

            var texColList = new List<TextureCollection>();
            var texChunks = ChunkLocations.Where(x => x.Exists && x.ChunkType == ChunkType.Textures).Select(x => chunkDatas[x.ID]).ToList();

            int nextModelCollectionStartId = 0x102;
            int index = 0;
            foreach (var chunk in texChunks) {
                var collection = TextureCollectionForChunkIndex(chunk.Index);
                bool isMovableModelsChunk = collection >= TextureCollectionType.MovableModels1 && collection <= TextureCollectionType.MovableModels3;

                var startId = isMovableModelsChunk ? nextModelCollectionStartId : (int?) null;
                try {
                    var texCol = TextureCollection.Create(
                        chunk.DecompressedData, NameGetterContext, 0x00, "TextureCollection" + index,
                        collection, pixelFormats[collection], palettes, chunk.Index, startId
                    );
                    if (isMovableModelsChunk && texCol.TextureTable.Length > 0)
                        nextModelCollectionStartId = texCol.TextureTable.Last().ID + 1;

                    texColList.Add(texCol);
                    tables.AddRange(texCol.Tables);
                }
                catch {
                    // TODO: what to do if we get an error here?
                }

                index++;
            }
            TextureCollections = texColList.ToArray();

            // Now that textures are loaded, build the texture animation frame data.
            // TODO: This function is a MESS. Please refactor it!!
            BuildTextureAnimFrameData();

            // Add some images.
            if (RepeatingGroundChunks?.Any() == true) {
                try {
                    var palette = CreatePalette(0);
                    RepeatingGroundImage = new MultiChunkTextureIndexed(RepeatingGroundChunks.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette);
                }
                catch {
                    // TODO: what to do here??
                }
            }

            if (TiledGroundTileChunks?.Any() == true && TiledGroundMapChunks?.Any() == true) {
                var palette = CreatePalette(0);
                TiledGroundTileImage = new MultiChunkTextureIndexed(TiledGroundTileChunks.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);

                var tiledGroundImageData = CreateTiledImageData(TiledGroundTileImage, TiledGroundMapChunks.Select(x => x.DecompressedData).ToArray(), 64, 4);
                TiledGroundImage = new TextureIndexed(0, 0, 0, tiledGroundImageData, TexturePixelFormat.Palette1, palette, false);
            }

            if (SkyBoxChunks?.Any() == true)
                SkyBoxImage = new MultiChunkTextureIndexed(SkyBoxChunks.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette2, CreatePalette(1));

            if (BackgroundChunks?.Any() == true)
                BackgroundImage = new MultiChunkTextureIndexed(BackgroundChunks.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, CreatePalette(0));

            if (ForegroundTileChunks?.Any() == true) {
                var palette = CreatePalette(1);
                ForegroundTileImage = new MultiChunkTextureIndexed(ForegroundTileChunks.Select(x => x.DecompressedData).ToArray(), TexturePixelFormat.Palette1, palette, true);

                var foregroundImageData = CreateTiledImageData(ForegroundTileImage, new IByteData[] { ForegroundMapChunk.DecompressedData }, 64, 1);
                ForegroundImage = new TextureIndexed(0, 0, 0, foregroundImageData, TexturePixelFormat.Palette2, palette, true);
            }

            return tables.ToArray();
        }

        private byte[,] CreateTiledImageData(ITexture tiledGroundTileImage, IByteData[] tileMaps, int tileSize, int blockCountX) {
            int tilesPerBlock = tileSize * tileSize;

            // Count the number of tiles (they're 16 bits, so divide the byte count by 2)
            var tileMapTileCount = tileMaps.Sum(x => x.Length) / 2;

            var blockCountYf = (float) tileMapTileCount / tilesPerBlock / blockCountX;

            var tileImageData = tiledGroundTileImage.ImageData8Bit;
            var tileImageDataWidth  = tileImageData.GetLength(0);
            var tileImageDataHeight = tileImageData.GetLength(1);

            var tileCountX = tileImageDataWidth / 8;
            var tileCountY = tileImageDataHeight / 8;
            var tileCount = tileCountX * tileCountY;
            var outputImage = new byte[tileSize * blockCountX * 8, (int) Math.Ceiling(tileSize * blockCountYf) * 8];

            var tile = 0;
            foreach (var tileMap in tileMaps) {
                var data = tileMap.GetDataCopy();
                for (var dataPos = 0; dataPos < data.Length - 1; dataPos += 2, tile++) {
                    var high = data[dataPos];
                    var low = data[dataPos + 1];

                    var tileIndex = ((high << 8) + low) / 2;
                    if (tileIndex >= tileCount) {
                        System.Diagnostics.Debug.WriteLine($"{dataPos:X4}: {tileIndex} ({high}, {low})");
                        continue;
                    }

                    // TODO: refactor this to reduce all these calculations!!
                    var inputX = (tileIndex % tileCountX) * 8;
                    var inputY = (tileIndex / tileCountX) * 8;

                    var blockIndex = tile / tilesPerBlock;
                    var blockX = blockIndex % blockCountX;
                    var blockY = blockIndex / blockCountX;

                    var tileInBlock = tile % tilesPerBlock;
                    var outputX = ((tileInBlock % tileSize) + (blockX * tileSize)) * 8;
                    var outputY = ((tileInBlock / tileSize) + (blockY * tileSize)) * 8;

                    for (int y = 0; y < 8; y++)
                        for (int x = 0; x < 8; x++)
                            outputImage[outputX + x, outputY + y] = tileImageData[inputX + x, inputY + y];
                }
            }

            return outputImage;
        }

        private Dictionary<TexturePixelFormat, Palette> CreatePalettesForTextures() {
            var palettes = new Dictionary<TexturePixelFormat, Palette>();
            if (PaletteTables != null) {
                if (PaletteTables.Length >= 1 && PaletteTables[0] != null)
                    palettes[TexturePixelFormat.Palette1] = new Palette(PaletteTables[0].Select(x => x.ColorABGR1555).ToArray());
                if (PaletteTables.Length >= 2 && PaletteTables[1] != null)
                    palettes[TexturePixelFormat.Palette2] = new Palette(PaletteTables[1].Select(x => x.ColorABGR1555).ToArray());
                if (PaletteTables.Length >= 3 && PaletteTables[2] != null)
                    palettes[TexturePixelFormat.Palette3] = new Palette(PaletteTables[2].Select(x => x.ColorABGR1555).ToArray());
            }
            return palettes;
        }


        // TODO: refactor this mess!!
        private void BuildTextureAnimFrameData() {
            if (ChunkData[3] == null || TextureAnimations == null)
                return;

            if (Chunk3Frames == null)
                Chunk3Frames = new List<Chunk3Frame>();
            else
                Chunk3Frames.Clear();

            var chunk3Textures = new Dictionary<uint, ITexture>();
            var palettes = CreatePalettesForTextures();

            TextureModel GetTextureModelByID(int textureId) {
                if (TextureCollections == null)
                    return null;
                return TextureCollections.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x).FirstOrDefault(x => x.ID == textureId);
            }

            // TODO: Duplicate code, copied from TextureTable! BAD!!
            Palette GetPalette(TexturePixelFormat format) {
                switch (format) {
                    case TexturePixelFormat.UnknownPalette:
                    case TexturePixelFormat.Palette1: {
                        return palettes.TryGetValue(TexturePixelFormat.Palette1, out var outPalette) ? outPalette : null;
                    }
                    case TexturePixelFormat.Palette2: {
                        return palettes.TryGetValue(TexturePixelFormat.Palette2, out var outPalette) ? outPalette : null;
                    }
                    case TexturePixelFormat.Palette3: {
                        return palettes.TryGetValue(TexturePixelFormat.Palette3, out var outPalette) ? outPalette : null;
                    }
                    default:
                        return null;
                }
            }

            foreach (var anim in TextureAnimations) {
                foreach (var frame in anim.FrameTable) {
                    var offset = frame.CompressedImageDataOffset;
                    var existingFrame = Chunk3Frames.FirstOrDefault(x => x.Offset == offset);

                    var pixelFormat = ((frame.TextureID & 0x100) == 0x100) ? TexturePixelFormat.Palette3 : TexturePixelFormat.ABGR1555;
                    var referenceTexID = ((frame.TextureID & 0x100) == 0x100) ? frame.TextureID - 0x100 : frame.TextureID;
                    var referenceTex = GetTextureModelByID(referenceTexID)?.Texture;

                    if (existingFrame != null) {
                        frame.FetchAndCacheTexture(existingFrame.Data.DecompressedData, pixelFormat, GetPalette(pixelFormat), referenceTex);
                        continue;
                    }

                    var uncompressedBytes8 = frame.Width * frame.Height;
                    var uncompressedBytes16 = frame.Width * frame.Height * 2;

                    CompressedData newData = null;
                    ByteArraySegment byteArray = null;

                    try {
                        if (pixelFormat.BytesPerPixel() == 2) {
                            var compressedBytes = Math.Min(uncompressedBytes16 + 8, ChunkData[3].Length - (int) offset);
                            byteArray = new ByteArraySegment(ChunkData[3].Data, (int) offset, compressedBytes);
                            newData = new CompressedData(byteArray, uncompressedBytes16);
                            frame.FetchAndCacheTexture(newData.DecompressedData, pixelFormat, null, referenceTex);
                        }
                        else {
                            var compressedBytes = Math.Min(uncompressedBytes8 + 8, ChunkData[3].Length - (int) offset);
                            byteArray = new ByteArraySegment(ChunkData[3].Data, (int) offset, compressedBytes);
                            newData = new CompressedData(byteArray, uncompressedBytes8);
                            frame.FetchAndCacheTexture(newData.DecompressedData, pixelFormat, GetPalette(pixelFormat), referenceTex);
                        }
                    }
                    catch { }

                    if (newData != null && frame.Texture != null) {
                        byteArray.Redefine(byteArray.Offset, newData.LastDataReadForDecompress.Value); // Sets the correct compressed size, which wasn't available before
                        newData.IsModified = false;
                        chunk3Textures.Add(offset, frame.Texture);
                        Chunk3Frames.Add(new Chunk3Frame((int) offset, newData));
                    }
                }
            }

            // Add triggers to update texture animation frames when their offsets or content are modified.
            foreach (var c3frameLoop in Chunk3Frames) {
                var c3frame = c3frameLoop;
                c3frame.Data.Data.RangeModified += (s, a) => {
                    if (a.Moved) {
                        var newOffset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                        var oldOffset = newOffset - a.OffsetChange;
                        var affectedFrames = TextureAnimations.SelectMany(x => x.FrameTable).Where(x => x.CompressedImageDataOffset == oldOffset).ToArray();
                        foreach (var frame in affectedFrames)
                            frame.CompressedImageDataOffset = (uint) newOffset;
                    }
                };
                c3frame.Data.DecompressedData.Data.RangeModified += (s, a) => {
                    if (a.Resized || a.Modified) {
                        var offset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                        var affectedFrames = TextureAnimations.SelectMany(x => x.FrameTable).Where(x => x.CompressedImageDataOffset == offset).ToArray();
                        foreach (var frame in affectedFrames) {
                            var referenceTex = GetTextureModelByID(frame.TextureID)?.Texture;
                            frame.FetchAndCacheTexture(c3frame.Data.DecompressedData, frame.PixelFormat, GetPalette(frame.PixelFormat), referenceTex);
                        }
                    }
                };
            }
        }

        private void WireChildDataModifiedEvents() {
            // Add some callbacks to all child data.
            var allData = ChunkData
                .Where(x => x != null)
                .Cast<IByteData>()
                .Concat((Chunk3Frames != null) ? Chunk3Frames.Select(x => x.Data) : new CompressedData[0])
                .ToArray();

            foreach (var d in allData) {
                // If the data is marked as unmodified (such as after a save), mark child data as unmodified as well.
                Data.IsModifiedChanged += (s, e) => d.IsModified &= Data.IsModified;

                // If any of the child data is marked as modified, mark the parent data as modified as well.
                d.IsModifiedChanged += (s, e) => Data.IsModified |= d.IsModified;
            }
        }

        public bool Recompress(bool onlyModified) {
            var framesModified = Chunk3Frames?.Any(x => x.Data.IsModified || x.Data.NeedsRecompression) ?? false;
            var chunksModified = framesModified || ChunkData.Any(x => x != null && (x.IsModified || x.NeedsRecompression));

            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !framesModified && !chunksModified)
                return true;

            // Chunk 3 is made up of several individually-compressed images that need to be recompressed.
            RecompressChunk3Frames(onlyModified);

            // Recompress and update the chunk table.
            RebuildChunkTable(onlyModified);
            return true;
        }

        private void RecompressChunk3Frames(bool onlyModified) {
            if (Chunk3Frames == null)
                return;

            // Recompress texture frames and determine the new total size of Chunk[3].
            foreach (var frameDataKv in Chunk3Frames) {
                var frameData = frameDataKv.Data;
                if (!onlyModified || frameData.NeedsRecompression || frameData.IsModified)
                    _ = frameData.Finish();
            }
        }

        public void RebuildChunkTable(bool onlyModified) {
            // Gets all the chunks in order of their address, followed by the order in which they'll be updated.
            ChunkLocation[] GetSortedChunks() {
                return ChunkLocations
                    .Where(x => x.ChunkRAMAddress != 0)
                    .OrderBy(x => x.ChunkRAMAddress)
                    .ThenBy(x => x.ChunkSize)
                    .ThenBy(x => x.ID)
                    .ToArray();
            }

            // Finish compressed chunks.
            var oldChunksOrderedByPosition = GetSortedChunks();
            foreach (var cd in ChunkData)
                if (cd != null && cd.IsCompressed && (!onlyModified || cd.NeedsRecompression || cd.IsModified))
                    _ = cd.Recompress();

            // When fixing offsets and removing empty space, we want to apply changes in order from
            // lowest to highest address.
            var chunksOrderedByPosition = GetSortedChunks();

            // Perform sanity check.
            var oldOrderedChunkIds = oldChunksOrderedByPosition.Select(x => x.ID).ToArray();
            var newOrderedChunkIds = chunksOrderedByPosition.Select(x => x.ID).ToArray();
            if (!Enumerable.SequenceEqual<int>(oldOrderedChunkIds, newOrderedChunkIds))
                throw new InvalidOperationException("Chunks became out of order during decompression. This is bad, please contact the devs!");

            // We need to know when the first chunk begins 
            var firstChunkData = ChunkData.FirstOrDefault(x => x != null);
            var firstChunkDataIndex = firstChunkData?.Index ?? 100;

            int expectedFileAddr = 0x2100;
            int lastChunkEndFileAddr = expectedFileAddr;
            foreach (var chunk in chunksOrderedByPosition) {
                // Enforce alignment by inserting bytes where necessary before this chunk begins.
                if (chunk.ChunkFileAddress != expectedFileAddr) {
                    var resizeStart = lastChunkEndFileAddr;
                    var resizeEnd = Math.Min(Data.Data.Length, chunk.ChunkFileAddress);
                    var resizeOldSize = resizeEnd - resizeStart;
                    var resizeNewSize = expectedFileAddr - lastChunkEndFileAddr;

                    Data.Data.ResizeAt(resizeStart, resizeOldSize, resizeNewSize);

                    // If the chunk just moved isn't managed by any ChunkData, then it's one of the first
                    // chunks, and its empty. It should be at 0x2100, and so should every other empty chunk
                    // at the beginning.
                    if (chunk.ID < firstChunkDataIndex) {
                        if (expectedFileAddr != 0x2100)
                            throw new InvalidOperationException("Huh? We shouldn't be trying to fix chunks here!");
                        for (int i = 0; i < firstChunkDataIndex; i++)
                            if (i != chunk.ID)
                                ChunkLocations[i].ChunkRAMAddress = chunk.Address;
                    }
                }

                // Move forward. Make sure the next chunk starts at an alignment of 4 bytes.
                expectedFileAddr += chunk.ChunkSize;
                if (expectedFileAddr % 4 != 0)
                    expectedFileAddr += 4 - (expectedFileAddr % 4);

                // Keep track of where the last chunk ended. We'll add or remove zeroes here to enforce alignemnt.
                lastChunkEndFileAddr = chunk.ChunkFileAddress + chunk.ChunkSize;
            }
        }

        private void InitTiles() {
            for (var x = 0; x < 64; x++)
                for (var y = 0; y < 64; y++)
                    Tiles[x, y] = new Tile(this, x, y);
        }

        private struct TreeModelInfo {
            public ModelCollection ModelCollection;
            public Structs.MPD.Model.Model Model;
            public VECTOR TilePosition;
            public Tile Tile;
            public float Distance;
        }

        public void AssociateTilesWithTrees() {
            ResetTileTrees();

            // Gather a list of models that appear to be trees, associated with their tile.
            var treeModels = new List<TreeModelInfo>();
            foreach (var mc in ModelCollections) {
                if (mc == null || !mc.ChunkIndex.HasValue)
                    continue;

                foreach (var model in mc.ModelTable) {
                    try {
                        // Trees always face the camera.
                        if (!model.AlwaysFacesCamera)
                            continue;

                        // Look into the model...
                        var pdataAddr = model.PData0;
                        if (!mc.PDatasByMemoryAddress.ContainsKey(pdataAddr))
                            continue;
                        var pdata = mc.PDatasByMemoryAddress[pdataAddr];

                        // Trees always have one polygon.
                        var firstAttrAddr = pdata.AttributesOffset;
                        if (firstAttrAddr == 0 || !mc.AttrTablesByMemoryAddress.ContainsKey(firstAttrAddr))
                            continue;
                        var attr = mc.AttrTablesByMemoryAddress[firstAttrAddr];
                        if (attr.Length != 1)
                            continue;

                        // Get the tile at its location. Skip it if it's out of bounds.
                        var tilePosition = new VECTOR(model.PositionX / -32.0f - 0.5f, model.PositionY / -32.0f, model.PositionZ / -32.0f - 0.5f);
                        int tileX = (int) Math.Round(tilePosition.X.Float);
                        int tileZ = (int) Math.Round(tilePosition.Z.Float);
                        if (tileX < 0 || tileX >= 64 || tileZ < 0 || tileZ >= 64)
                            continue;

                        var tile = Tiles[tileX, tileZ];
                        var tileY = tile.GetAverageHeight();

                        // Trees should be very close to the center of the tile vertically.
                        var distance = (new VECTOR(tileX, tileY, tileZ) - tilePosition).GetLength();
                        if (Math.Abs(tileY - tilePosition.Y.Float) > 0.25f)
                            continue;

                        // Looks like a tree -- add it to the list.
                        treeModels.Add(new TreeModelInfo() {
                            ModelCollection = mc,
                            Model = model,
                            TilePosition = tilePosition,
                            Tile = tile,
                            Distance = distance
                        });
                    }
                    catch {
                        // TODO: what to do in this case??
                    }
                }
            }

            // Sort trees by their distance to their tile, from closest to farthest.
            // This can be considered their accuracy.
            var mostAccurateTreesForTile = treeModels
                .OrderBy(x => x.Distance)
                .ToArray();

            foreach (var tree in mostAccurateTreesForTile) {
                // Skip tiles already accounted for by more accurate trees.
                if (tree.Tile.TreeModelID.HasValue)
                    continue;
                tree.Tile.TreeModelChunkIndex = tree.ModelCollection.ChunkIndex;
                tree.Tile.TreeModelID = tree.Model.ID;
            }
        }

        public void ResetTileTrees() {
            foreach (var tile in Tiles) {
                tile.TreeModelID = null;
                tile.TreeModelChunkIndex = null;
            }
        }

        public override bool IsModified {
            get => base.IsModified | ChunkData.Any(x => x != null && x.IsModified);
            set {
                base.IsModified = value;
                foreach (var ce in ChunkData.Where(x => x != null))
                    ce.IsModified = value;
            }
        }

        public override bool OnFinish()
            => Recompress(onlyModified: true);

        public override void Dispose() {
            base.Dispose();
            if (ChunkData != null) {
                foreach (var cd in ChunkData.Where(x => x != null))
                    cd.Dispose();
            }
            if (Chunk3Frames != null) {
                foreach (var cd in Chunk3Frames)
                    cd.Data.Dispose();
                Chunk3Frames.Clear();
            }
        }

        public Palette CreatePalette(int index, int adjR, int adjG, int adjB) {
            var palette = CreatePalette(index);
            if (adjR != 0 || adjG != 0 || adjB != 0) {
                adjR = adjR * 255 / 31;
                adjG = adjG * 255 / 31;
                adjB = adjB * 255 / 31;

                for (int i = 0; i < palette.Channels.Length; i++) {
                    ref var ch = ref palette.Channels[i];
                    ch.r = (byte) MathHelpers.Clamp(ch.r + adjR, 0, 255);
                    ch.g = (byte) MathHelpers.Clamp(ch.g + adjG, 0, 255);
                    ch.b = (byte) MathHelpers.Clamp(ch.b + adjB, 0, 255);
                }
            }
            return palette;
        }

        public Palette CreatePalette(int index) {
            if (index < 0 || index > 2)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (PaletteTables == null || index >= PaletteTables.Length)
                return new Palette(256);

            var paletteTable = PaletteTables[index];
            if (paletteTable == null)
                return new Palette(256);

            if (paletteTable.Length != 256)
                throw new InvalidOperationException($"PaletteTable[{index}] should be 256 colors, instead it's {paletteTable.Length}");

            return new Palette(paletteTable.Select(x => x.ColorABGR1555).ToArray());
        }

        public PDataModel GetTreePData0() {
            var mc = ModelCollections.FirstOrDefault(x => x.ChunkIndex == MPDHeader.ModelsChunkIndex);
            if (mc == null)
                return null;

            // Look for the first PDATA with one polygon that uses the tree texture (seems to always be 0).
            return mc.PDataTable.FirstOrDefault(x => {
                if (x.PolygonCount != 1)
                    return false;

                var attr = mc.AttrTablesByMemoryAddress[x.AttributesOffset][0];
                return attr.HasTexture && attr.TextureNo == 0;
            });
        }

        private struct TextureModelAndTextureByName {
            public object Model;
            public ITexture Texture;
        }

        public ReplaceTexturesFromFilesResult ReplaceTexturesFromFiles(string[] files, Func<string, ushort[,]> abgr1555ImageDataLoader) {
            var textures1 = (TextureCollections == null) ? new Dictionary<string, TextureModelAndTextureByName>() : TextureCollections
                .Where(x => x != null && x.TextureTable != null)
                .SelectMany(x => x.TextureTable)
                .Where(x => x.TextureIsLoaded)
                .ToDictionary(x => x.ImportExportName, x => new TextureModelAndTextureByName { Model = x, Texture = x.Texture });

            var textures2 = (TextureAnimations == null) ? new Dictionary<string, TextureModelAndTextureByName>() : TextureAnimations
                .SelectMany(x => x.FrameTable)
                .GroupBy(x => x.CompressedImageDataOffset)
                .Select(x => x.First())
                .Where(x => x.TextureIsLoaded)
                .ToDictionary(x => x.ImportExportName, x => new TextureModelAndTextureByName { Model = x, Texture = x.Texture });

            var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

            int succeeded = 0;
            int failed    = 0;
            int missing   = 0;
            int skipped   = 0;

            foreach (var textureKv in textures) {
                var name = textureKv.Key;
                var model = textureKv.Value.Model;
                var texture = textureKv.Value.Texture;

                if (texture.PixelFormat != TexturePixelFormat.ABGR1555) {
                    skipped++;
                    continue;
                }

                var filename = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).ToLower() == name.ToLower());
                if (filename == null) {
                    missing++;
                    continue;
                }

                // Try to actually load the texture!
                try {
                    var imageData = abgr1555ImageDataLoader(filename);
                    if (imageData == null) {
                        failed++;
                        continue;
                    }

                    var imageDataWidth  = imageData.GetLength(0);
                    var imageDataHeight = imageData.GetLength(1);
                    if (imageDataWidth != texture.Width || imageDataHeight != texture.Height) {
                        failed++;
                        continue;
                    }

                    for (var ix = 0; ix < imageDataWidth; ix++) {
                        for (var iy = 0; iy < imageDataHeight; iy++) {
                            // If the alpha channel is clear, preserve whatever color was originally used.
                            // This should prevent marking data as 'modified' too often.
                            if ((imageData[ix, iy] & 0x8000u) == 0) {
                                var cached = ABGR1555toChannels(texture.ImageData16Bit[ix, iy]);
                                cached.a = 0;
                                imageData[ix, iy] = cached.ToABGR1555();
                            }
                        }
                    }

                    if (model is TextureModel tm)
                        tm.RawImageData16Bit = imageData;
                    else if (model is FrameModel fm) {
                        var referenceTex = TextureCollections.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x).FirstOrDefault(x => x.ID == fm.TextureID)?.Texture;
                        _ = fm.UpdateTextureABGR1555(Chunk3Frames.First(x => x.Offset == fm.CompressedImageDataOffset).Data.DecompressedData, imageData, referenceTex);
                    }
                    else
                        throw new NotSupportedException("Not sure what this is, but it's not supported here");

                    succeeded++;
                }
                catch {
                    failed++;
                }
            }

            return new ReplaceTexturesFromFilesResult {
                Replaced = succeeded,
                Missing  = missing,
                Failed   = failed,
                Skipped  = skipped
            };
        }

        public ExportTexturesToPathResult ExportTexturesToPath(string path, Action<string, ushort[,]> abgr1555ImageDataWriter) {
            var textures1 = (TextureCollections == null) ? new Dictionary<string, ITexture>() : TextureCollections
                .Where(x => x != null && x.TextureTable != null)
                .SelectMany(x => x.TextureTable)
                .Where(x => x.TextureIsLoaded)
                .ToDictionary(x => x.ImportExportName, x => x.Texture);

            var textures2 = (TextureAnimations == null) ? new Dictionary<string, ITexture>() : TextureAnimations
                .SelectMany(x => x.FrameTable)
                .GroupBy(x => x.CompressedImageDataOffset)
                .Select(x => x.First())
                .Where(x => x.TextureIsLoaded)
                .ToDictionary(x => x.ImportExportName, x => x.Texture);

            var textures = textures1.Concat(textures2).ToDictionary(x => x.Key, x => x.Value);

            int succeeded = 0;
            int failed = 0;
            int skipped = 0;

            foreach (var textureKv in textures) {
                var name = textureKv.Key;
                var texture = textureKv.Value;

                var filename = Path.Combine(path, name + ".png");
                try {
                    if (texture.PixelFormat != TexturePixelFormat.ABGR1555)
                        skipped++;
                    abgr1555ImageDataWriter(filename, texture.ImageData16Bit);
                    succeeded++;
                }
                catch {
                    failed++;
                }
            }

            return new ExportTexturesToPathResult {
                Exported = succeeded,
                Failed   = failed,
                Skipped  = skipped
            };
        }

        public IChunkData[] ChunkData { get; private set; }

        public IChunkData[] ModelsChunkData { get; private set; }
        public IChunkData SurfaceChunkData => (SurfaceModelChunkIndex.HasValue) ? ChunkData[SurfaceModelChunkIndex.Value] : null;

        [BulkCopyRecurse]
        public MPDHeaderModel MPDHeader { get; private set; }

        [BulkCopyRecurse]
        public ChunkLocationTable ChunkLocations { get; private set; }

        [BulkCopyRecurse]
        public ColorTable LightPalette { get; private set; }

        [BulkCopyRecurse]
        public LightPosition LightPosition { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown1Table { get; private set; }

        [BulkCopyRecurse]
        public LightAdjustmentModel LightAdjustment { get; private set; }

        [BulkCopyRecurse]
        public ModelSwitchGroupsTable ModelSwitchGroupsTable { get; private set; }

        public Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOffByAddr { get; private set; }
        public Dictionary<int, ModelIDTable> VisibleModelsWhenFlagOnByAddr { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt8Table GroundAnimationTable { get; private set; }

        [BulkCopyRecurse]
        public TextureIDTable TextureAnimationsAlt { get; private set; }

        [BulkCopyRecurse]
        public ColorTable[] PaletteTables { get; private set; }

        [BulkCopyRecurse]
        public TextureIDTable IndexedTextureTable { get; private set; }

        [BulkCopyRecurse]
        public TextureAnimationTable TextureAnimations { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Unknown2Table { get; private set; }

        [BulkCopyRecurse]
        public GradientTable GradientTable { get; private set; }

        public List<Chunk3Frame> Chunk3Frames { get; private set; }

        [BulkCopyRecurse]
        public BoundaryTable BoundariesTable { get; private set; }

        public int? SurfaceModelChunkIndex { get; private set; } = null;

        [BulkCopyRecurse]
        public SurfaceModel SurfaceModel { get; private set; }

        public int[] ModelsChunkIndices { get; private set; } = null;

        [BulkCopyRecurse]
        public ModelCollection[] ModelCollections { get; private set; }

        [BulkCopyRecurse]
        public Surface Surface { get; private set; }

        public int PrimaryTextureChunksFirstIndex { get; }
        public int PrimaryTextureChunksLastIndex { get; }
        public int MeshTextureChunksFirstIndex { get; }
        public int MeshTextureChunksLastIndex { get; }
        public int? ExtraModelTextureChunkIndex { get; }

        [BulkCopyRecurse]
        public TextureCollection[] TextureCollections { get; private set; }

        public Tile[,] Tiles { get; } = new Tile[64, 64];

        public int RepeatingGroundChunk1Index { get; }
        public int RepeatingGroundChunk2Index { get; }

        public IChunkData[] RepeatingGroundChunks { get; private set; }
        public ITexture RepeatingGroundImage { get; private set; }

        public int TiledGroundTileChunk1Index { get; }
        public int TiledGroundTileChunk2Index { get; }
        public int TiledGroundMapChunks1Index { get; }
        public int TiledGroundMapChunks2Index { get; }

        public IChunkData[] TiledGroundTileChunks { get; private set; }
        public IChunkData[] TiledGroundMapChunks { get; private set; }

        public ITexture TiledGroundTileImage { get; private set; }
        public ITexture TiledGroundImage { get; private set; }

        public int SkyBoxChunk1Index { get; }
        public int SkyBoxChunk2Index { get; }

        public IChunkData[] SkyBoxChunks { get; private set; }
        public ITexture SkyBoxImage { get; private set; }

        public int BackgroundChunk1Index { get; }
        public int BackgroundChunk2Index { get; }

        public IChunkData[] BackgroundChunks { get; private set; }
        public ITexture BackgroundImage { get; private set; }


        public int ForegroundTileChunk1Index { get; }
        public int ForegroundTileChunk2Index { get; }
        public int ForegroundMapChunkIndex { get; }

        public IChunkData[] ForegroundTileChunks { get; private set; }
        public IChunkData ForegroundMapChunk { get; private set; }
        public ITexture ForegroundTileImage { get; private set; }
        public ITexture ForegroundImage { get; private set; }

        public EventHandler ModelsUpdated { get; set; }
    }
}
