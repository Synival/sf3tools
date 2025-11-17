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
using CommonLib.Extensions;
using CommonLib.Types;
using SF3.NamedValues;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        public override int RamAddress => c_RamAddress;
        public override int RamAddressLimit => 0x002D0000;

        private const int c_RamAddress = 0x00290000;

        protected MPD_File(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts, ScenarioType? fallbackScenario = null)
        : base(data, nameContexts?[DetectScenario(data) ?? fallbackScenario ?? ScenarioType.Other], DetectScenario(data) ?? fallbackScenario ?? ScenarioType.Other) {
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

        public static MPD_File Create(IByteData data, INameGetterContext nameContext, ScenarioType fallbackScenario)
            => Create(data, new Dictionary<ScenarioType, INameGetterContext>() { { fallbackScenario, nameContext } }, fallbackScenario);

        public static MPD_File Create(IByteData data, NameGetterContext nameContext)
            => Create(data, new Dictionary<ScenarioType, INameGetterContext>() { { nameContext.Scenario, nameContext } }, nameContext.Scenario);

        public static MPD_File Create(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts, ScenarioType? fallbackScenario) {
            var newFile = new MPD_File(data, nameContexts, fallbackScenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize MPD_File");
            return newFile;
        }

        public static ScenarioType? DetectScenario(IByteData data) {
            try {
                // Get addresses we need to check.
                var headerAddrPtr = data.GetDouble(0x0000) - c_RamAddress;
                var headerAddr    = data.GetDouble(headerAddrPtr) - c_RamAddress;

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
            catch {
                return null;
            }
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
            var headerAddrPtr = Data.GetDouble(0x0000) - RamAddress;
            var headerAddr = Data.GetDouble(headerAddrPtr) - RamAddress;
            MPDHeader = new MPDHeaderModel(Data, 0, "MPDHeader", headerAddr, Scenario);
            MPDFlags = new MPDFlags(MPDHeader);
            return MPDHeader;
        }

        private ITable[] MakeHeaderTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            tables.AddRange(MakeLightingTables(header));
            tables.AddRange(MakeTexturePaletteTables(header));
            tables.AddRange(MakeTextureAnimationTables(header, areAnimatedTextures32Bit));
            tables.Add(BoundariesTable = BoundaryTable.Create(Data, "Boundaries", ResourceFile("BoundaryList.xml"), header.OffsetBoundaries - RamAddress));
            tables.AddRange(MakeMovableModelCollections(header));
            tables.AddRange(MakeUnknownTables(header));

            return tables.ToArray();
        }

        private ChunkLocationTable MakeChunkHeaderTable()
            => ChunkLocations = ChunkLocationTable.Create(Data, "ChunkHeader", 0x2000);

        private ITable[] MakeTexturePaletteTables(MPDHeaderModel header) {
            PaletteTables = new ColorTable[3];
            var headerRamAddr = header.Address + RamAddress;

            // Sometimes palette addresses are placed in an odd place at or just before the header actually begins.
            // This is most likely an error in the MPD file; it results in garbage data.
            // Don't load the palettes in these cases.
            if (header.OffsetPal1 >= RamAddress && (headerRamAddr - header.OffsetPal1) / 2 >= 256)
                PaletteTables[0] = ColorTable.Create(Data, "TexturePalette1", header.OffsetPal1 - RamAddress, 256);
            if (header.OffsetPal2 >= RamAddress && (headerRamAddr - header.OffsetPal2) / 2 >= 256)
                PaletteTables[1] = ColorTable.Create(Data, "TexturePalette2", header.OffsetPal2 - RamAddress, 256);
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 >= RamAddress && (headerRamAddr - header.OffsetPal3) / 2 >= 256)
                PaletteTables[2] = ColorTable.Create(Data, "TexturePalette3", header.OffsetPal3 - RamAddress, 256);

            return PaletteTables.Where(x => x != null).ToArray();
        }

        private ITable[] MakeLightingTables(MPDHeaderModel header) {
            var tables = new List<ITable>();

            if (header.OffsetLightPalette != 0)
                tables.Add(LightPalette = ColorTable.Create(Data, "LightPalette", header.OffsetLightPalette - RamAddress, 32));
            if (header.OffsetLightPosition != 0)
                LightPosition = new LightPosition(Data, 0, "LightPositions", header.OffsetLightPosition - RamAddress);
            if (header.OffsetLightAdjustment != 0)
                LightAdjustment = new LightAdjustmentModel(Data, 0, "LightAdjustment", header.OffsetLightAdjustment - RamAddress, Scenario);

            if (header.OffsetGradient != 0)
                tables.Add(GradientTable = GradientTable.Create(Data, "Gradients", header.OffsetGradient - RamAddress));

            return tables.ToArray();
        }

        private ITable[] MakeTextureAnimationTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            if (header.OffsetTextureAnimations != 0) {
                try {
                    tables.Add(TextureAnimations = TextureAnimationTable.Create(Data, "TextureAnimations", header.OffsetTextureAnimations - RamAddress, areAnimatedTextures32Bit));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            if (header.OffsetTextureAnimAlt != 0) {
                try {
                    tables.Add(TextureAnimationsAlt = TextureIDTable.Create(Data, "TextureAnimationsAlt", header.OffsetTextureAnimAlt - RamAddress, 2, 0x100));
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

                var newModel = ModelCollection.Create(Data, NameGetterContext, offset - RamAddress, "MovableModels" + (i + 1), i);
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
                tables.Add(ModelSwitchGroupsTable = ModelSwitchGroupsTable.Create(Data, "ModelSwitchGroups", header.OffsetModelSwitchGroups - RamAddress));

                VisibleModelsWhenFlagOffByAddr = ModelSwitchGroupsTable
                    .Where(x => x.VisibleModelsWhenFlagOffOffset > 0)
                    .ToDictionary(
                        x => (int) x.VisibleModelsWhenFlagOffOffset,
                        x => ModelIDTable.Create(Data, x.Name + "_FlagOffIDs (0x" + x.VisibleModelsWhenFlagOffOffset.ToString("X") + ")", (int) x.VisibleModelsWhenFlagOffOffset - RamAddress)
                    );

                VisibleModelsWhenFlagOnByAddr = ModelSwitchGroupsTable
                    .Where(x => x.VisibleModelsWhenFlagOnOffset > 0)
                    .ToDictionary(
                        x => (int) x.VisibleModelsWhenFlagOnOffset,
                        x => ModelIDTable.Create(Data, x.Name + "_FlagOnIDs (0x" + x.VisibleModelsWhenFlagOnOffset.ToString("X") + ")", (int) x.VisibleModelsWhenFlagOnOffset - RamAddress)
                    );

                tables.AddRange(VisibleModelsWhenFlagOffByAddr.Values);
                tables.AddRange(VisibleModelsWhenFlagOnByAddr.Values);
            }

            // TODO: put somewhere else!!
            if (header.OffsetGroundAnimation != 0)
                tables.Add(GroundAnimationTable = UnknownUInt8Table.Create(Data, "ScrollScreenAnimations", header.OffsetGroundAnimation - RamAddress, null, 0xFF));

            // TODO: put somewhere else!!
            if (header.OffsetIndexedTextures != 0)
                tables.Add(IndexedTextureTable = TextureIDTable.Create(Data, "IndexedTextures", header.OffsetIndexedTextures - RamAddress, 4, 0x100));

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
                    tables.Add(Unknown1Table = UnknownUInt16Table.Create(Data, "Unknown1", header.OffsetUnknown1 - RamAddress, size, null));
            }

            if (header.OffsetUnknown2 != 0) {
                var maxSize = (header.OffsetGroundAnimation != 0) ? (header.OffsetGroundAnimation - header.OffsetUnknown2 - 2) : 32;
                tables.Add(Unknown2Table = UnknownUInt16Table.Create(Data, "Unknown2", header.OffsetUnknown2 - RamAddress, maxSize, 0xFFFF));
            }

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
            if (MPDFlags.GroundImageType == GroundImageType.Repeated) {
                if (chunks[RepeatingGroundChunk1Index].Exists)
                    repeatingGroundChunks.Add(_ = MakeChunkData(RepeatingGroundChunk1Index, ChunkType.Palette1Image, CompressionType.Compressed));
                if (chunks[RepeatingGroundChunk2Index].Exists)
                    repeatingGroundChunks.Add(_ = MakeChunkData(RepeatingGroundChunk2Index, ChunkType.Palette1Image, CompressionType.Compressed));
            }
            RepeatingGroundChunks = repeatingGroundChunks.ToArray();

            // Tiled ground images
            var tiledGroundTileChunks = new List<IChunkData>();
            var tiledGroundMapChunks = new List<IChunkData>();
            if (MPDFlags.GroundImageType == GroundImageType.Tiled) {
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
            if (MPDFlags.HasAnySkyBox) {
                if (chunks[SkyBoxChunk1Index].Exists)
                    skyBoxChunks.Add(_ = MakeChunkData(SkyBoxChunk1Index, ChunkType.Palette2Image, CompressionType.Compressed));
                if (chunks[SkyBoxChunk2Index].Exists)
                    skyBoxChunks.Add(_ = MakeChunkData(SkyBoxChunk2Index, ChunkType.Palette2Image, CompressionType.Compressed));
            }
            SkyBoxChunks = skyBoxChunks.ToArray();

            // Background image
            var backgroundChunks = new List<IChunkData>();
            if (MPDFlags.BackgroundImageType.HasFlag(BackgroundImageType.Still)) {
                if (chunks[BackgroundChunk1Index].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(BackgroundChunk1Index, ChunkType.Palette1Image, CompressionType.Compressed));
                if (chunks[BackgroundChunk2Index].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(BackgroundChunk2Index, ChunkType.Palette1Image, CompressionType.Compressed));
            }
            BackgroundChunks = backgroundChunks.ToArray();

            // Foreground image tiles
            var foregroundTileChunks = new List<IChunkData>();
            if (MPDFlags.BackgroundImageType.HasFlag(BackgroundImageType.Tiled)) {
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
            ByteArray byteArray = null;
            ChunkData chunkData = null;

            try {
                byteArray = new ByteArray(Data.Data.GetDataCopyAt(ChunkLocations[chunkIndex].ChunkFileAddress, ChunkLocations[chunkIndex].ChunkSize));
                chunkData = new ChunkData(byteArray, isCompressed, chunkIndex);
            }
            catch {
                // TODO: what to do???
                return null;
            }
            var chunkLocation = ChunkLocations[chunkIndex];

            chunkLocation.DecompressedSize = chunkData.DecompressedData.Length;
            chunkData.DecompressedData.Data.RangeModified += (s, a) => {
                if (a.Resized)
                    chunkLocation.DecompressedSize = chunkData.DecompressedData.Length;
            };

            chunkData.Data.RangeModified += (s, a) => {
                // If the data hasn't been modified, do nothing.
                if (chunkLocation.ChunkSize == chunkData.Length)
                    return;

                // Determine how much the next chunks should be moved by.
                var oldNextChunkOffset = (int) (Math.Ceiling((chunkLocation.ChunkRAMAddress + chunkLocation.ChunkSize) / 4.0) * 4.0);
                var newNextChunkOffset = (int) (Math.Ceiling((chunkLocation.ChunkRAMAddress + chunkData.Length) / 4.0) * 4.0);

                // Set the new chunk size.
                chunkLocation.ChunkSize = chunkData.Length;

                // Don't move proceeding chunks if not requested.
                if (!UpdateChunkTableOnChunkResize)
                    return;

                // Adjust the offset/address of every chunk after this one.
                var nextChunkOffsetDelta = newNextChunkOffset - oldNextChunkOffset;
                if (nextChunkOffsetDelta != 0) {
                    var thisRamAddr = chunkLocation.ChunkRAMAddress;
                    var thisIndex = chunkLocation.ID;
                    foreach (var loc in ChunkLocations) {
                        var ramAddr = loc.ChunkRAMAddress;
                        var index = loc.ID;

                        // Offset the chunk if either:
                        //   1) its offset/address is later, or
                        //   2) it's the same but is a higher ID (this happens when the earlier chunk has a size of 0)
                        if (ramAddr > thisRamAddr || (ramAddr == thisRamAddr && index > thisIndex))
                            loc.ChunkRAMAddress += nextChunkOffsetDelta;
                    }
                }
            };

            chunkLocation.ChunkType = type;
            chunkLocation.CompressionType = compressionType;

            ChunkData[chunkIndex] = chunkData;
            return chunkData;
        }

        private int[] GetModelsChunkIndices(ChunkLocation[] chunks) {
            var flags = MPDFlags;
            var indices = new List<int>();

            if (chunks[20].Exists && flags.Chunk20IsModels)
                indices.Add(20);

            if (chunks[1].Exists)
                if (!flags.Chunk20IsModels || flags.HasExtraChunk1ModelWithChunk21Textures)
                    indices.Add(1);

            if (chunks[19].Exists && flags.HasChunk19Model)
                indices.Add(19);

            return indices.ToArray();
        }

        private int? GetSurfaceModelChunkIndex(ChunkLocation[] chunks) {
            var flags = MPDFlags;
            if (!flags.HasSurfaceModel)
                return null;

            return
                chunks[2].Exists ? 2 :
                (flags.Chunk20IsSurfaceModel && chunks[20].Exists) ? 20 :
                (int?) null;
        }

        private ITable[] MakeChunkTables(ChunkLocation[] chunkHeaders, IChunkData[] chunkDatas, IChunkData[] modelsChunks, IChunkData surfaceModelChunk) {
            TextureCollectionType TextureCollectionForChunkIndex(int chunkIndex) {
                if (chunkIndex == 10 && MPDFlags.HasChunk19Model)
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
                var collectionType = (mc.Index == 19 && MPDFlags.HasChunk19Model)
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
                    if (texCol.TextureTable != null) {
                        if (isMovableModelsChunk && texCol.TextureTable.Length > 0)
                            nextModelCollectionStartId = texCol.TextureTable.Last().ID + 1;

                        texColList.Add(texCol);
                        tables.AddRange(texCol.Tables);
                    }
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

            // Precalculations for tile lookups
            var tileInputX = new int[tileCount];
            var tileInputY = new int[tileCount];
            int pos = 0;
            for (int y = 0; y < tileCountY; y++) {
                for (int x = 0; x < tileCountX; x++) {
                    tileInputX[pos]   = x * 8;
                    tileInputY[pos++] = y * 8;
                }
            }

            int blockXMax = blockCountX * tileSize;
            int tile = 0, tileInBlock = 0, blockX = 0, blockY = 0, tileInBlockX = 0, tileInBlockY = 0;
            foreach (var tileMap in tileMaps) {
                var data = tileMap.GetDataCopyOrReference();
                for (var dataPos = 0; dataPos < data.Length - 1; tile++, tileInBlock++, tileInBlockX++) {
                    // Reset some tile locations when we've reached the end of a block.
                    if (tileInBlock == tilesPerBlock) {
                        tileInBlock = 0;
                        tileInBlockX = 0;
                        tileInBlockY = 0;

                        // Move ahead one block, wrapping when blockXMax is reached.
                        blockX += tileSize;
                        if (blockX == blockXMax) {
                            blockX = 0;
                            blockY += tileSize;
                        }
                    }
                    // Make sure that tileInBlockX wraps.
                    else if (tileInBlockX == tileSize) {
                        tileInBlockX = 0;
                        tileInBlockY++;
                    }

                    var tileIndex = ((data[dataPos++] << 8) + data[dataPos++]) / 2;
                    if (tileIndex >= tileCount) {
                        System.Diagnostics.Debug.WriteLine($"{dataPos:X4}: {tileIndex}");
                        continue;
                    }

                    var inputX = tileInputX[tileIndex];
                    var inputY = tileInputY[tileIndex];

                    var outputX = (tileInBlockX + blockX) * 8;
                    var outputY = (tileInBlockY + blockY) * 8;

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
                        byteArray.Redefine(byteArray.Offset, newData.LastDecompressBytesRead.Value); // Sets the correct compressed size, which wasn't available before
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

        public void RecompressChunks(bool onlyModified) {
            var framesModified = Chunk3Frames?.Any(x => x.Data.IsModified || x.Data.NeedsRecompression) ?? false;
            var chunksModified = framesModified || ChunkData.Any(x => x != null && (x.IsModified || x.NeedsRecompression));

            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !framesModified && !chunksModified)
                return;

            // Chunk 3 is made up of several individually-compressed images that need to be recompressed.
            RecompressChunk3Frames(onlyModified);

            // Perform recompression.
            foreach (var chunkData in ChunkData) {
                if (chunkData == null || !chunkData.IsCompressed)
                    continue;
                if (chunkData.IsModified || !onlyModified)
                    chunkData.Recompress();
            }

            return;
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

        public void RebuildChunkTable() {
            // Chunks always start at file offset 0x2100.
            int nextChunkOffset = 0x2100;

            foreach (var loc in ChunkLocations) {
                if (loc.ChunkRAMAddress == 0)
                    break;

                var chunkData = ChunkData[loc.ID];
                if (chunkData == null) {
                    loc.ChunkFileAddress = nextChunkOffset;
                    loc.ChunkSize = 0;
                }
                else {
                    loc.ChunkFileAddress = nextChunkOffset;
                    loc.ChunkSize = chunkData.Length;
                    nextChunkOffset += (int) (Math.Ceiling(loc.ChunkSize / 4.0) * 4.0);
                }
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

        public override string[] GetErrors() {
            var errors = base.GetErrors().ToList();

            foreach (var chunk in ChunkData)
                if (chunk?.NeedsRecompression == true)
                    errors.Add($"Chunk[{chunk.Index}] must be recompressed");

            errors.AddRange(GetHeaderPaddingErrors());
            errors.AddRange(GetPaletteErrors());
            errors.AddRange(GetChunkTableErrors());
            errors.AddRange(GetChunkTypeErrors());
            errors.AddRange(GetModelChunkErrors());
            errors.AddRange(GetSurfaceChunkErrors());
            errors.AddRange(GetImageChunkErrors());
            errors.AddRange(GetSurfaceTileErrors());

            return errors.ToArray();
        }

        private string[] GetHeaderPaddingErrors() {
            var header = MPDHeader;
            var errors = new List<string>();

            void CheckPadding(string prop, int value) {
                if (value != 0)
                    errors.Add($"{prop} has non-zero data: {value.ToString("X4")}");
            }

            CheckPadding(nameof(header.Padding1), header.Padding1);
            CheckPadding(nameof(header.Padding2), header.Padding2);
            CheckPadding(nameof(header.Padding3), header.Padding3);
            CheckPadding(nameof(header.Padding4), header.Padding4);

            return errors.ToArray();
        }

        private string[] GetPaletteErrors() {
            var header = MPDHeader;
            var errors = new List<string>();

            // TODO: improve logic -- check for Scenario1+2, and what palettes should be present based on chunks
            // TODO: check palette sizes

            // If the disc is Scenario 3, all palettes should be present.
            if (Scenario >= ScenarioType.Scenario3) {
                if (PaletteTables[0] == null)
                    errors.Add("Palette1 is required for Scenario 3 and above but it is missing");
                if (PaletteTables[1] == null)
                    errors.Add("Palette2 is required for Scenario 3 and above but it is missing");
                if (PaletteTables[2] == null)
                    errors.Add("Palette3 is required for Scenario 3 and above but it is missing");
            }

            return errors.ToArray();
        }

        private string[] GetChunkTableErrors() {
            var errors = new List<string>();

            ChunkLocation[] GetIntersectingChunks(ChunkLocation loc) {
                if (loc.ChunkRAMAddress == 0)
                    return new ChunkLocation[0];

                var loc1Start = loc.ChunkRAMAddress;
                var loc1Stop = loc.ChunkRAMAddress + loc.ChunkSize;

                var locations = new List<ChunkLocation>();
                foreach (var loc2 in ChunkLocations) {
                    if (loc2 == loc || loc2.ChunkRAMAddress == 0)
                        continue;

                    var loc2Start = loc2.ChunkRAMAddress;
                    var loc2Stop = loc2.ChunkRAMAddress + loc2.ChunkSize;

                    var intersects = (loc1Start < loc2Stop) && (loc1Stop > loc2Start);
                    if (intersects)
                        locations.Add(loc2);
                }

                return locations.ToArray();
            };

            foreach (var chunkLoc in ChunkLocations) {
                var chunk = ChunkData[chunkLoc.ID];

                void AddChunkError(string error)
                    => errors.Add($"Chunk[{chunkLoc.ID}]: {error}");

                if (chunkLoc.ChunkRAMAddress == 0 && chunk != null)
                    AddChunkError($"RAM address in table is 0 but has chunk data (size = 0x{chunk.Length:X4})");
                if (chunkLoc.ChunkRAMAddress == 0 && chunkLoc.ChunkSize != 0)
                    AddChunkError($"RAM address in table is 0 but has size in table (0x{chunkLoc.ChunkSize:X4})");
                if (chunkLoc.ChunkRAMAddress > 0 && chunkLoc.ChunkRAMAddress < c_RamAddress + 0x2100)
                    AddChunkError($"RAM address in table is invalid value (0x{chunkLoc.ChunkRAMAddress:X6})");

                if (chunkLoc.ChunkSize == 0 && chunk != null)
                    AddChunkError($"Size in table is 0 but has chunk data (size = 0x{chunk.Length:X4})");
                if (chunkLoc.ChunkSize != 0 && chunk == null)
                    AddChunkError($"Size in table is non-zero (0x{chunkLoc.ChunkSize:X4}) but has no chunk data");
                if (chunkLoc.ChunkSize != 0 && chunk != null && chunk.Length != chunkLoc.ChunkSize)
                    AddChunkError($"Size in table (0x{chunkLoc.ChunkSize:X4}) does not match chunk data size (0x{chunk.Length:X4})");

                var intersectingChunks = GetIntersectingChunks(chunkLoc);
                foreach (var intersectingChunk in intersectingChunks)
                    AddChunkError($"Intersects with Chunk[{intersectingChunk.ID}]");
            }

            return errors.ToArray();
        }

        private string[] GetChunkTypeErrors() {
            var chunkHeaders = ChunkLocations;
            var errors = new List<string>();

            // Chunk[0] and Chunk[4] should always be empty.
            if (chunkHeaders[0].Exists)
                errors.Add("Chunk[0] exists -- this chunk should always be empty");
            if (chunkHeaders[4].Exists)
                errors.Add("Chunk[4] exists -- this chunk should always be empty");

            // Anything Scenario 2 or higher should have addresses for Chunk[20] and Chunk[21].
            bool shouldHaveChunk20_21 = Scenario >= ScenarioType.Scenario2;
            bool hasChunk20 = chunkHeaders[20].ChunkRAMAddress > 0;
            bool hasChunk21 = chunkHeaders[21].ChunkRAMAddress > 0;

            if (shouldHaveChunk20_21 != hasChunk20)
                errors.Add("Chunk[20] error: ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk20);
            if (shouldHaveChunk20_21 != hasChunk21)
                errors.Add("Chunk[21] errors: ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk21);

            // Make sure the header indicates the correct chunks.
            var flags = MPDFlags;
            if (flags.Chunk1IsModels && chunkHeaders[1].ChunkType != ChunkType.Models)
                errors.Add($"Chunk[1] type should be 'Models', but is {chunkHeaders[1].ChunkType}");
            if (flags.Chunk2IsSurfaceModel && chunkHeaders[2].ChunkType != ChunkType.SurfaceModel)
                errors.Add($"Chunk[2] type should be 'SurfaceModel', but is {chunkHeaders[2].ChunkType}");
            if (flags.Chunk20IsModels && chunkHeaders[20].ChunkType != ChunkType.Models)
                errors.Add($"Chunk[20] type should be 'Models', but is {chunkHeaders[20].ChunkType}");
            if (flags.Chunk20IsSurfaceModel && chunkHeaders[20].ChunkType != ChunkType.SurfaceModel)
                errors.Add($"Chunk[20] type should be 'SurfaceModel', but is {chunkHeaders[20].ChunkType}");

            return errors.ToArray();
        }

        private static bool? HasHighMemoryModels(ModelCollection mc) {
            if (mc == null)
                return null;
            return (mc.PDatasByMemoryAddress.Values.Count == 0)
                ? (bool?) null
                : mc.PDatasByMemoryAddress.Values.First().RamAddress >= 0x0600_0000;
        }

        private string[] GetModelChunkErrors() {
            var flags = MPDFlags;
            var errors = new List<string>();

            var mc1  = ModelCollections.FirstOrDefault(x => x.ChunkIndex == 1);
            var mc20 = ModelCollections.FirstOrDefault(x => x.ChunkIndex == 20);

            if (mc1 != null) {
                var expectedLmm = flags.Chunk1IsLoadedFromLowMemory;
                var expectedHmm = flags.Chunk1IsLoadedFromHighMemory;
                var actualHmm   = HasHighMemoryModels(mc1) == true;

                if (expectedLmm && expectedHmm)
                    errors.Add("Chunk[1] is somehow expected to be both in high and low memory");
                else if (!expectedLmm && !expectedHmm)
                    errors.Add("Chunk[1] is somehow expected to be neither in high and low memory");
                else {
                    if (expectedHmm && !actualHmm)
                        errors.Add("Chunk[1] has models in low memory but they should be in high memory");
                    else if (!expectedHmm && actualHmm)
                        errors.Add("Chunk[1] has models in high memory but they should be in low memory");
                }
            }

            if (mc20 != null && HasHighMemoryModels(mc20) == false)
                errors.Add("Chunk[20] has models in low memory but they should be in high memory");

            return errors.ToArray();
        }

        private string[] GetSurfaceChunkErrors() {
            var flags = MPDFlags;
            var chunkHeaders = ChunkLocations;
            var errors = new List<string>();

            var expectedIndex = (flags.Chunk20IsSurfaceModelIfExists && chunkHeaders[20].Exists) ? 20 : 2;
            var chunk2LooksLikeSurfaceChunk  = chunkHeaders[2].Exists  && chunkHeaders[2].ChunkSize  == 0xCF00;
            var chunk20LooksLikeSurfaceChunk = chunkHeaders[20].Exists && chunkHeaders[20].ChunkSize == 0xCF00;

            if (flags.HasSurfaceModel) {
                if (SurfaceModel == null) {
                    errors.Add("HasSurfaceModel flag is set but no surface model was created");
                    if (chunk2LooksLikeSurfaceChunk)
                        errors.Add($"  (Chunk[2] (expected={expectedIndex}) looks like one -- this could be an SF3Lib error)");
                    if (chunk20LooksLikeSurfaceChunk)
                        errors.Add($"  (Chunk[20] (expected={expectedIndex}) looks like one -- this could be an SF3Lib error)");
                }
                else if (SurfaceModelChunkIndex != expectedIndex)
                    errors.Add($"(Maybe not an error?) SurfaceModel in unexpected index. Expected in Chunk[{expectedIndex}] but found at Chunk[{SurfaceModelChunkIndex}]");
            }
            else if (!flags.HasSurfaceModel) {
                if (flags.Chunk20IsSurfaceModelIfExists)
                    errors.Add("HasSurfaceModel flag is unset but Chunk20IsSurfaceModelIfExists flag is set");
                if (chunk2LooksLikeSurfaceChunk)
                    errors.Add($"HasSurfaceModel flag is unset but Chunk[2] (expected={expectedIndex}) looks like one");
                if (chunk20LooksLikeSurfaceChunk)
                    errors.Add($"HasSurfaceModel flag is unset but Chunk[20] (expected={expectedIndex}) looks like one");
            }

            if (chunk2LooksLikeSurfaceChunk && chunk20LooksLikeSurfaceChunk)
                errors.Add("Only one surface chunk should be present but both Chunk[2] and Chunk[20] look like one");

            return errors.ToArray();
        }

        private string[] GetImageChunkErrors() {
            var flags = MPDFlags;
            var chunkHeaders = ChunkLocations;
            var errors = new List<string>();

            var chunkUses = new Dictionary<int, List<string>>() {
                { 14, new List<string>() },
                { 15, new List<string>() },
                { 16, new List<string>() },
                { 17, new List<string>() },
                { 18, new List<string>() },
                { 19, new List<string>() },
            };

            var typicalUse = new Dictionary<int, string>() {
                { 14, "GroundImageTop[Tiles]" },
                { 15, "GroundImageBottom[Tiles]" },
                { 16, "GroundImageTopTileMap" },
                { 17, "SkyBoxImageTop" },
                { 18, "SkyBoxImageBottom" },
                { 19, "GroundImageBottomTileMap" },
            };

            if (flags.GroundImageType.HasFlag(GroundImageType.Repeated)) {
                chunkUses[14].Add("GroundImageTop");
                chunkUses[15].Add("GroundImageBottom");
            }

            if (flags.GroundImageType.HasFlag(GroundImageType.Tiled)) {
                chunkUses[14].Add("GroundImageTopTiles");
                chunkUses[15].Add("GroundImageBottomTiles");
                chunkUses[16].Add("GroundImageTopTileMap");
                chunkUses[19].Add("GroundImageBottomTileMap");
            }

            if (flags.HasAnySkyBox) {
                chunkUses[17].Add("SkyBoxImageTop");
                chunkUses[18].Add("SkyBoxImageBottom");
            }

            if (flags.BackgroundImageType.HasFlag(BackgroundImageType.Still)) {
                chunkUses[14].Add("BackgroundImageTop");
                chunkUses[15].Add("BackgroundImageBottom");
            }

            if (flags.BackgroundImageType.HasFlag(BackgroundImageType.Tiled)) {
                chunkUses[17].Add("ForegroundImageTopTiles");
                chunkUses[18].Add("ForegroundImageBottomTiles");
                chunkUses[19].Add("ForegroundImageTileMap");
            }

            if (flags.HasChunk19Model)
                chunkUses[19].Add("ExtraModel");

            foreach (var cu in chunkUses) {
                if (cu.Value.Count == 0) {
                    if (chunkHeaders[cu.Key].Exists)
                        errors.Add($"Image Chunk[{cu.Key}] exists but has no flag to indicate its use (probably {typicalUse[cu.Key]})");
                }
                else {
                    var usesStr = string.Join(", ", cu.Value);
                    if (cu.Value.Count > 1)
                        errors.Add($"Image Chunk[{cu.Key}] has multiple uses indicated: {usesStr}");

                    if (!chunkHeaders[cu.Key].Exists) {
                        // The skybox is allowed to be missing from Scenario2 onward.
                        if (!(usesStr.StartsWith("SkyBoxImage") && Scenario >= ScenarioType.Scenario2))
                            errors.Add($"{usesStr} Chunk[{cu.Key}] is missing");
                    }
                }
            }

            return errors.ToArray();
        }

        private string[] GetSurfaceTileErrors() {
            if (SurfaceModel == null)
                return new string[0];

            var errors = new List<string>();
            var flags = MPDFlags;
            var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));

            foreach (var tile in Tiles) {
                // This *would* report irregularities in heightmaps, if the existed :)
                var moveHeights  = corners.ToDictionary(c => c, tile.GetMoveHeightmap);
                if (tile.ModelIsFlat) {
                    var br = CornerType.BottomRight;
                    foreach (var c in corners) {
                        if (c == br)
                            continue;
                        if (moveHeights[c] != moveHeights[br])
                            errors.Add("Flat tile (" + tile.X + ", " + tile.Y + ") corner '" + c.ToString() + " height doesn't match bottom-right corner height: " + moveHeights[c] + " != " + moveHeights[br]);
                    }
                }
                else {
                    var modelHeights = corners.ToDictionary(c => c, tile.GetModelVertexHeightmap);
                    foreach (var c in corners) {
                        if (moveHeights[c] != modelHeights[c])
                            errors.Add("Non-flat tile (" + tile.X + ", " + tile.Y + ") corner '" + c.ToString() + " height doesn't match surface model height: " + moveHeights[c] + " != " + modelHeights[c]);
                    }
                }

                // Report unknown or unhandled tile flags. Only Scenario 3+ has rotation flags 0x01 and 0x02.
                var weirdTexFlags = tile.ModelTextureFlags & ~0xB0;
                if (Scenario >= ScenarioType.Scenario3)
                    weirdTexFlags &= ~0x03;
                if (weirdTexFlags != 0x00)
                    errors.Add("Tile (" + tile.X + ", " + tile.Y + ") has unhandled texture flag 0x" + weirdTexFlags.ToString("X2"));
                if (Scenario >= ScenarioType.Scenario3 && !flags.HasSurfaceTextureRotation && (tile.ModelTextureFlags & 0x03) != 0)
                    errors.Add("HasSurfaceTextureRotation flag is off but tile (" + tile.X + ", " + tile.Y + ") has rotation flags 0x" + (weirdTexFlags & 0x03).ToString("X2"));
            }

            return errors.ToArray();
        }

        public override bool OnFinish() {
            // Recompress any chunk waiting for it.
            RecompressChunks(onlyModified: true);

            // Make sure the chunk table matches the chunks, in the expected order.
            if (RebuildChunkTableOnFinish)
                RebuildChunkTable();

            // Update the content of the file.
            CommitChunks();

            // Always return success.
            return true;
        }

        public void CommitChunks() {
            // We need to copy chunk data into the file -- get the new file size.
            var maxChunkEnd = ChunkLocations.Max(x => x.ChunkFileAddress + x.ChunkSize);
            var newFileSize = (int) (Math.Ceiling(maxChunkEnd / 4.0) * 4.0);

            // Copy all the chunk data into a clean buffer.
            var newChunkData = new byte[newFileSize - 0x2100];
            foreach (var chunk in ChunkData) {
                if (chunk == null)
                    continue;
                var copyToOffset = ChunkLocations[chunk.Index].ChunkFileAddress - 0x2100;
                if (copyToOffset >= 0)
                    chunk.GetDataCopyOrReference().CopyTo(newChunkData, copyToOffset);
                else
                    CommonLib.Logging.Logger.WriteLine($"Chunk[{chunk.Index}] position (0x{copyToOffset + 0x2100:X4}) is < 0x2100; not writing", CommonLib.Types.LogType.Error);
            }

            // Resize and update our file.
            Data.Data.Resize(newFileSize);
            Data.Data.SetDataAtTo(0x2100, newChunkData.Length, newChunkData);
        }

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
            var modelChunkIndex = MPDFlags.Chunk20IsModels ? 20 : 1;
            var mc = ModelCollections.FirstOrDefault(x => x.ChunkIndex == modelChunkIndex);
            if (mc == null)
                return null;

            // Look for the first PDATA with one polygon that uses the tree texture (usually 0, but not always).
            return mc.PDataTable.FirstOrDefault(x => {
                if (x.PolygonCount != 1)
                    return false;

                var attr = mc.AttrTablesByMemoryAddress[x.AttributesOffset][0];
                return attr.UseTexture;
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

                    // MPD textures should have end codes.
                    // One common texture used for the locked chest is encoded in an ever-so-slightly different way,
                    // so account for that to prevent "IsModified" from always being set.
                    bool applyEndCodesToBorder = true;
                    var tm = model as TextureModel;
                    if (tm != null && tm.ID == 0x109 && tm.ChunkIndex == 12)
                        applyEndCodesToBorder = false;
                    imageData.FixSaturnTransparency(useEndCodes: true, applyEndCodesToBorder);

                    if (tm != null)
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
        public MPDFlags MPDFlags { get; private set; }

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

        public static bool UpdateChunkTableOnChunkResize { get; set; } = true;
        public static bool RebuildChunkTableOnFinish { get; set; } = true;
    }
}
