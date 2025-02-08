using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Attributes;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        private const int c_RamOffset = 0x290000;
        private const int c_SurfaceModelChunkSize = 0xCF00;

        protected MPD_File(IByteData data, Dictionary<ScenarioType, INameGetterContext> nameContexts) : base(data, nameContexts?[DetectScenario(data)], DetectScenario(data)) { }

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
            var demoAddr      = data.GetDouble(headerAddr + 0x0030);

            if ((demoAddr & 0xFFFF0000) == 0x00290000)
                return ScenarioType.Ship2;

            var palette3Addr  = data.GetDouble(headerAddr + 0x0044);
            var chunk21Addr   = data.GetDouble(0x20A8);

            // Determine some things about this MPD file.
            var hasPalette3 = (palette3Addr & 0xFFFF0000) == 0x00290000;
            var hasChunk21  = (chunk21Addr > 0);

            // We should be able to accurately detect the scenario.
            // Scenario 3 and the Premium Disk have the same MPD format.
            if (hasPalette3)
                return ScenarioType.Scenario3;
            else if (hasChunk21)
                return ScenarioType.Scenario2;
            else
                return ScenarioType.Scenario1;
        }

        public override IEnumerable<ITable> MakeTables() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Load root headers
            var header = MakeHeaderTable()[0];
            var headerTables = MakeHeaderTables(header, areAnimatedTextures32Bit);

            // Load chunks
            var chunks = MakeChunkHeaderTable().Rows;
            var chunkDatas = MakeChunkDatas(chunks);
            var chunkTables = MakeChunkTables(chunks, chunkDatas, ModelsChunkData, SurfaceChunkData);

            // Add two-way communication between 'Modified' events from the root IByteData and its children.
            WireChildDataModifiedEvents();

            // Build a list of all data tables.
            var tables = new List<ITable>() {
                MPDHeader,
                ChunkHeader,
            };
            tables.AddRange(headerTables);
            tables.AddRange(chunkTables);

            InitTiles();
            return tables;
        }

        private MPDHeaderTable MakeHeaderTable() {
            var headerAddrPtr = Data.GetDouble(0x0000) - c_RamOffset;
            var headerAddr = Data.GetDouble(headerAddrPtr) - c_RamOffset;
            MPDHeader = MPDHeaderTable.Create(Data, "MPDHeader", headerAddr, Scenario);
            return MPDHeader;
        }

        private ITable[] MakeHeaderTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            tables.AddRange(MakeLightingTables(header));
            tables.AddRange(MakeTexturePaletteTables(header));
            tables.AddRange(MakeTextureAnimationTables(header, areAnimatedTextures32Bit));
            tables.Add(BoundariesTable = BoundaryTable.Create(Data, "Boundaries", ResourceFile("BoundaryList.xml"), header.OffsetBoundaries - c_RamOffset));
            tables.AddRange(MakeUnknownTables(header));

            return tables.ToArray();
        }

        private ChunkHeaderTable MakeChunkHeaderTable()
            => ChunkHeader = ChunkHeaderTable.Create(Data, "ChunkHeader", 0x2000);

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
                tables.Add(LightPositionTable = LightPositionTable.Create(Data, "LightPositions", header.OffsetLightPosition - c_RamOffset));

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
                    tables.Add(TextureAnimationsAlt = TextureIDTable.Create(Data, "TextureAnimationsAlt", header.OffsetTextureAnimAlt - c_RamOffset));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            return tables.ToArray();
        }

        private ITable[] MakeUnknownTables(MPDHeaderModel header) {
            var tables = new List<ITable>();

            if (header.OffsetUnknown1 != 0)
                tables.Add(OffsetUnknown1Table = UnknownInt16Table.Create(Data, "Unknown1", header.OffsetUnknown1 - c_RamOffset, (header.OffsetModelSwitchGroups - header.OffsetUnknown1) / 2, null));
            if (header.OffsetUnknown2 != 0)
                tables.Add(OffsetUnknown2Table = UnknownUInt16Table.Create(Data, "Unknown2", header.OffsetUnknown2 - c_RamOffset, 32, 0xFFFF));
            if (header.OffsetModelSwitchGroups != 0)
                tables.Add(ModelSwitchGroupsTable = ModelSwitchGroupsTable.Create(Data, "ModelSwitchGroups", header.OffsetModelSwitchGroups - c_RamOffset));
            if (header.OffsetScrollScreenAnimation != 0)
                tables.Add(RepeatingGroundAnimationTable = UnknownUInt8Table.Create(Data, "ScrollScreenAnimations", header.OffsetScrollScreenAnimation - c_RamOffset, null, 0xFF));
            if (header.OffsetIndexedTextures != 0)
                tables.Add(IndexedTextureTable = TextureIDTable.Create(Data, "IndexedTextures", header.OffsetIndexedTextures - c_RamOffset));

            return tables.ToArray();
        }

        private IChunkData[] MakeChunkDatas(ChunkHeader[] chunks) {
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
            this.ModelsChunkData = modelsChunksList.ToArray();

            // Animated textures chunk
            if (chunks[3].Exists)
                _ = MakeChunkData(3, ChunkType.AnimationFrames, CompressionType.IndividuallyCompressed);

            // Surface chunk (heightmap, terrain, event IDs)
            if (chunks[5].Exists)
                _ = MakeChunkData(5, ChunkType.Surface, CompressionType.Compressed);

            // Texture data, in chunks (6...13)
            var lastTextureChunk = (Scenario >= ScenarioType.Scenario1) ? 13 : 10;
            for (var i = 6; i <= lastTextureChunk; i++)
                _ = MakeChunkData(i, ChunkType.Textures, CompressionType.Compressed);
            if (Scenario >= ScenarioType.Scenario2 && chunks[21].Exists)
                _ = MakeChunkData(21, ChunkType.Textures, CompressionType.Compressed);

            // Repeating backgrounds
            var repeatingGroundChunks = new List<IChunkData>();
            if (MPDHeader[0].GroundImageType == GroundImageType.Repeated) {
                if (chunks[14].Exists)
                    repeatingGroundChunks.Add(_ = MakeChunkData(14, ChunkType.Palette1Image, CompressionType.Compressed));
                if (chunks[15].Exists)
                    repeatingGroundChunks.Add(_ = MakeChunkData(15, ChunkType.Palette1Image, CompressionType.Compressed));
            }
            RepeatingGroundChunkData = repeatingGroundChunks.Where(x => x != null).ToArray();

            // Tiled ground images
            var tiledGroundChunks = new List<IChunkData>();
            if (MPDHeader[0].GroundImageType == GroundImageType.Tiled) {
                if (chunks[14].Exists)
                    tiledGroundChunks.Add(_ = MakeChunkData(14, ChunkType.TiledGroundTiles, CompressionType.Compressed));
                if (chunks[15].Exists)
                    tiledGroundChunks.Add(_ = MakeChunkData(15, ChunkType.TiledGroundTiles, CompressionType.Compressed));
                if (chunks[16].Exists)
                    tiledGroundChunks.Add(_ = MakeChunkData(16, ChunkType.TiledGroundMap, CompressionType.Compressed));
                if (chunks[19].Exists)
                    tiledGroundChunks.Add(_ = MakeChunkData(19, ChunkType.TiledGroundMap, CompressionType.Compressed));
            }
            TiledGroundChunkData = tiledGroundChunks.Where(x => x != null).ToArray();

            // Sky boxes
            var skyboxChunks = new List<IChunkData>();
            if (MPDHeader[0].HasSkyBox) {
                if (chunks[17].Exists)
                    skyboxChunks.Add(_ = MakeChunkData(17, ChunkType.Palette2Image, CompressionType.Compressed));
                if (chunks[18].Exists)
                    skyboxChunks.Add(_ = MakeChunkData(18, ChunkType.Palette2Image, CompressionType.Compressed));
            }
            SkyBoxChunkData = skyboxChunks.Where(x => x != null).ToArray();

            // Background image
            var backgroundChunks = new List<IChunkData>();
            if (MPDHeader[0].BackgroundImageType.HasFlag(BackgroundImageType.Still)) {
                if (chunks[14].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(14, ChunkType.Palette1Image, CompressionType.Compressed));
                if (chunks[15].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(15, ChunkType.Palette1Image, CompressionType.Compressed));
            }
            BackgroundChunkData = backgroundChunks.Where(x => x != null).ToArray();

            // TODO: Tiled backgrounds

            // Add unhandled images/scroll planes.
            // TODO: on what conditions are we sure these are scroll planes?
            var scrollPlaneStart = (Scenario >= ScenarioType.Scenario1) ? 14 : 11;
            for (var i = scrollPlaneStart; i < scrollPlaneStart + 6; i++)
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
                byteArraySegment = new ByteArraySegment(Data.Data, ChunkHeader[chunkIndex].ChunkAddress - c_RamOffset, ChunkHeader[chunkIndex].ChunkSize);
                chunkData = new ChunkData(byteArraySegment, isCompressed, chunkIndex);
            }
            catch {
                // TODO: what to do???
                return null;
            }
            var chunkHeader = ChunkHeader[chunkIndex];

            chunkHeader.DecompressedSize = chunkData.DecompressedData.Length;
            chunkData.DecompressedData.Data.RangeModified += (s, a) => {
                if (a.Resized)
                    chunkHeader.DecompressedSize = chunkData.DecompressedData.Length;
            };

            chunkData.Data.RangeModified += (s, a) => {
                var chunkName = chunkHeader.Name;
                if (a.Moved) {
                    // Figure out how much the offset has changed.
                    var oldOffset = chunkHeader.ChunkAddress - c_RamOffset;
                    var newOffset = byteArraySegment.Offset;
                    var offsetDelta = newOffset - oldOffset;
                    if (offsetDelta == 0)
                        return;

                    // Update the address in the chunk table.
                    chunkHeader.ChunkAddress = newOffset + c_RamOffset;

                    // Chunks after this one with something assigned to ChunkData[] will have their
                    // ChunkAddress updated automatically. For chunks without a ChunkData[] after this one
                    // (but before the next ChunkData), update addresses manually.
                    for (var j = chunkIndex + 1; j < ChunkHeader.Length; j++) {
                        if (ChunkData[j] != null)
                            break;

                        var ch = ChunkHeader[j];
                        if (ch != null && ch.ChunkAddress != 0)
                            ch.ChunkAddress += offsetDelta;
                    }
                }
                if (a.Resized)
                    chunkHeader.ChunkSize = chunkData.Data.Length;
            };

            // Add some integrity checks after recompression.
            chunkData.Recompressed += (s, e) => {
                var errors = new List<string>();
                for (var i = 0; i < ChunkHeader.Length; i++) {
                    var ch = ChunkHeader[i];
                    var cd = ChunkData[i];
                    if (ch.ChunkAddress == 0 || cd == null)
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

            chunkHeader.ChunkType = type;
            chunkHeader.CompressionType = compressionType;

            ChunkData[chunkIndex] = chunkData;
            return chunkData;
        }

        private int[] GetModelsChunkIndices(ChunkHeader[] chunks) {
            var indices = new List<int>();
            if (chunks[1].Exists)
                indices.Add(1);
            if (chunks[19].Exists && MPDHeader[0].HasChunk19Model)
                indices.Add(19);
            if (chunks[20].Exists && SurfaceModelChunkIndex != 20)
                indices.Add(20);
            return indices.ToArray();
        }

        private int? GetSurfaceModelChunkIndex(ChunkHeader[] chunks) {

            if (chunks[2].Exists && chunks[2].ChunkSize == c_SurfaceModelChunkSize)
                return 2;
            else if (chunks[20].Exists && chunks[20].ChunkSize == c_SurfaceModelChunkSize)
                return 20;
            else
                return null;
        }

        private ITable[] MakeChunkTables(ChunkHeader[] chunkHeaders, IChunkData[] chunkDatas, IChunkData[] modelsChunks, IChunkData surfaceModelChunk) {
            TextureCollectionType TextureCollectionForChunkIndex(int chunkIndex) {
                switch (chunkIndex) {
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        return TextureCollectionType.PrimaryTextures;
                    case 10:
                        return MPDHeader[0].HasChunk19Model ? TextureCollectionType.Chunk19ModelTextures : TextureCollectionType.PrimaryTextures;
                    case 11:
                        return TextureCollectionType.MovableObjects1;
                    case 12:
                        return TextureCollectionType.MovableObjects2;
                    case 13:
                        return TextureCollectionType.MovableObjects3;
                    case 21:
                        return TextureCollectionType.Chunk1ModelTextures;
                    default:
                        throw new Exception("Unhandled case!");
                }
            }

            var tables = new List<ITable>();

            var modelsList = new List<ModelCollection>();
            foreach (var mc in modelsChunks) {
                var texCollection = (mc.Index == 19 && MPDHeader[0].HasChunk19Model)
                    ? TextureCollectionType.Chunk19ModelTextures
                    : (chunkDatas[21] != null && mc.Index == 1) ? TextureCollectionType.Chunk1ModelTextures
                    : TextureCollectionType.PrimaryTextures;

                var newModel = ModelCollection.Create(mc.DecompressedData, NameGetterContext, 0x00, "Models" + mc.Index, Scenario, texCollection, mc.Index);
                modelsList.Add(newModel);
                tables.AddRange(newModel.Tables);
            }
            ModelCollections = modelsList.ToArray();

            if (chunkDatas[5] != null) {
                Surface = Surface.Create(chunkDatas[5].DecompressedData, NameGetterContext, 0x00, "Surface", 5);
                tables.AddRange(Surface.Tables);
            }

            if (surfaceModelChunk != null) {
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

            TextureCollections = new TextureCollection[9];
            for (var i = 0; i < TextureCollections.Length; i++) {
                var chunkIndex = (i == 8) ? 21 : i + 6;
                if (chunkDatas[chunkIndex]?.Length > 0) {
                    var collection = TextureCollectionForChunkIndex(chunkIndex);

                    try {
                        TextureCollections[i] = TextureCollection.Create(
                            chunkDatas[chunkIndex].DecompressedData, NameGetterContext, 0x00, "TextureCollection" + (i + 1),
                            collection, pixelFormats[collection], palettes, chunkIndex
                        );
                        tables.AddRange(TextureCollections[i].Tables);
                    }
                    catch {
                        // TODO: what to do if we get an error here?
                    }
                }
            }

            // Now that textures are loaded, build the texture animation frame data.
            // TODO: This function is a MESS. Please refactor it!!
            BuildTextureAnimFrameData();

            // Add some images.
            if (RepeatingGroundChunkData?.Length == 2)
                RepeatingGroundImage = new TwoChunkImage(RepeatingGroundChunkData[0].DecompressedData, RepeatingGroundChunkData[1].DecompressedData, TexturePixelFormat.Palette1, CreatePalette(0));

            if (TiledGroundChunkData?.Length == 4) {
                var palette = CreatePalette(0);
                TiledGroundTileImage = new TwoChunkImage(TiledGroundChunkData[0].DecompressedData, TiledGroundChunkData[1].DecompressedData, TexturePixelFormat.Palette1, palette, true);

                var tiledGroundImageData = CreateTiledGroundImageData(
                    TiledGroundTileImage,
                    TiledGroundChunkData[2].DecompressedData.GetDataCopy(),
                    TiledGroundChunkData[3].DecompressedData.GetDataCopy()
                );
                TiledGroundImage = new TextureIndexed(0, 0, 0, tiledGroundImageData, TexturePixelFormat.Palette1, palette, true);
            }

            if (SkyBoxChunkData?.Length == 2)
                SkyBoxImage = new TwoChunkImage(SkyBoxChunkData[0].DecompressedData, SkyBoxChunkData[1].DecompressedData, TexturePixelFormat.Palette2, CreatePalette(1));

            if (BackgroundChunkData?.Length == 2)
                BackgroundImage = new TwoChunkImage(BackgroundChunkData[0].DecompressedData, BackgroundChunkData[1].DecompressedData, TexturePixelFormat.Palette1, CreatePalette(0));

            return tables.ToArray();
        }

        private byte[,] CreateTiledGroundImageData(TwoChunkImage tiledGroundTileImage, byte[] upperTileMap, byte[] lowerTileMap) {
            const int c_blockWidth  = 64;
            const int c_blockHeight = 64;
            const int c_tilesPerBlock = c_blockWidth * c_blockHeight;
            const int c_blockCountX = 4;
            const int c_blockCountY = 4;
            const int c_blockCount  = 16;

            if (upperTileMap.Length + lowerTileMap.Length != c_blockWidth * c_blockHeight * c_blockCount * 2)
                throw new ArgumentException($"Wrong size for {nameof(upperTileMap)} + {nameof(lowerTileMap)}");

            var tileImageData = tiledGroundTileImage.FullTexture.ImageData8Bit;
            var tileImageDataWidth  = tileImageData.GetLength(0);
            var tileImageDataHeight = tileImageData.GetLength(1);

            var tileCountX = tileImageDataWidth / 8;
            var tileCountY = tileImageDataHeight / 8;
            var tileCount = tileCountX * tileCountY;
            var outputImage = new byte[c_blockWidth * c_blockCountX * 8, c_blockHeight * c_blockCountY * 8];

            var tile = 0;
            void AddTileData(byte[] data) {
                for (var dataPos = 0; dataPos < upperTileMap.Length - 1; dataPos += 2, tile++) {
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

                    var blockIndex = tile / c_tilesPerBlock;
                    var blockX = blockIndex % c_blockCountX;
                    var blockY = blockIndex / c_blockCountX;

                    var tileInBlock = tile % c_tilesPerBlock;
                    var outputX = ((tileInBlock % c_blockWidth) + (blockX * c_blockWidth)) * 8;
                    var outputY = ((tileInBlock / c_blockWidth) + (blockY * c_blockHeight)) * 8;

                    for (int y = 0; y < 8; y++)
                        for (int x = 0; x < 8; x++)
                            outputImage[outputX + x, outputY + y] = tileImageData[inputX + x, inputY + y];
                }
            }

            AddTileData(upperTileMap);
            AddTileData(lowerTileMap);

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
                foreach (var frame in anim.Frames) {
                    var offset = frame.CompressedTextureOffset;
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
                        var affectedFrames = TextureAnimations.SelectMany(x => x.Frames).Where(x => x.CompressedTextureOffset == oldOffset).ToArray();
                        foreach (var frame in affectedFrames)
                            frame.CompressedTextureOffset = (uint) newOffset;
                    }
                };
                c3frame.Data.DecompressedData.Data.RangeModified += (s, a) => {
                    if (a.Resized || a.Modified) {
                        var offset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                        var affectedFrames = TextureAnimations.SelectMany(x => x.Frames).Where(x => x.CompressedTextureOffset == offset).ToArray();
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
            ChunkHeader[] GetSortedChunks() {
                return ChunkHeader
                    .Where(x => x.ChunkAddress != 0)
                    .OrderBy(x => x.ChunkAddress)
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
                                ChunkHeader[i].ChunkAddress = chunk.Address;
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
                        var pdataAddr = model.PData1;
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

                // Uncomment this code to automatically fix tree locations and terrain types.
#if false
                tree.Tile.MoveTerrainType = TerrainType.Forest;
                tree.Model.PositionX = (short) ((Math.Round(tree.Model.PositionX / -32.0f + 0.5f) - 0.5f) * -32.0f);
                tree.Model.PositionY = (short) (tree.Tile.GetAverageHeight() * -32.0f);
                tree.Model.PositionZ = (short) ((Math.Round(tree.Model.PositionZ / -32.0f + 0.5f) - 0.5f) * -32.0f);
#endif
            }
        }

        public void ResetTileTrees() {
            foreach (var tile in Tiles)
                tile.TreeModelID = null;
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

        public Palette CreatePalette(int index)
        {
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

        public IChunkData[] ChunkData { get; private set; }

        public IChunkData[] ModelsChunkData { get; private set; }
        public IChunkData SurfaceChunkData => (SurfaceModelChunkIndex.HasValue) ? ChunkData[SurfaceModelChunkIndex.Value] : null;

        [BulkCopyRecurse]
        public MPDHeaderTable MPDHeader { get; private set; }

        [BulkCopyRecurse]
        public ChunkHeaderTable ChunkHeader { get; private set; }

        [BulkCopyRecurse]
        public ColorTable LightPalette { get; private set; }

        [BulkCopyRecurse]
        public LightPositionTable LightPositionTable { get; private set; }

        [BulkCopyRecurse]
        public UnknownInt16Table OffsetUnknown1Table { get; private set; }

        [BulkCopyRecurse]
        public ModelSwitchGroupsTable ModelSwitchGroupsTable { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt8Table RepeatingGroundAnimationTable { get; private set; }

        [BulkCopyRecurse]
        public TextureIDTable TextureAnimationsAlt { get; private set; }

        [BulkCopyRecurse]
        public ColorTable[] PaletteTables { get; private set; }

        [BulkCopyRecurse]
        public TextureIDTable IndexedTextureTable { get; private set; }

        [BulkCopyRecurse]
        public TextureAnimationTable TextureAnimations { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table OffsetUnknown2Table { get; private set; }

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

        [BulkCopyRecurse]
        public TextureCollection[] TextureCollections { get; private set; }

        public Tile[,] Tiles { get; } = new Tile[64, 64];

        public IChunkData[] RepeatingGroundChunkData { get; private set; }
        public TwoChunkImage RepeatingGroundImage { get; private set; }

        public IChunkData[] TiledGroundChunkData { get; private set; }
        public TwoChunkImage TiledGroundTileImage { get; private set; }
        public ITexture TiledGroundImage { get; private set; }

        public IChunkData[] SkyBoxChunkData { get; private set; }
        public TwoChunkImage SkyBoxImage { get; private set; }

        public IChunkData[] BackgroundChunkData { get; private set; }
        public TwoChunkImage BackgroundImage { get; private set; }
    }
}
