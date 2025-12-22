using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.TextureAnimation;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        public override IEnumerable<ITable> MakeTables() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Load root headers
            var header = MakeHeader();
            var headerTables = MakeHeaderTables(header, areAnimatedTextures32Bit);

            // Load chunks
            var chunks = MakeChunkHeaderTable().Rows;
            var chunkDatas = MakeChunkDatas(chunks);
            var chunkTables = MakeChunkTables(chunks, chunkDatas, ModelChunkDatas, SurfaceChunkData);

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

        private MPD_HeaderModel MakeHeader() {
            var headerAddrPtr = Data.GetDouble(0x0000) - RamAddress;
            var headerAddr = Data.GetDouble(headerAddrPtr) - RamAddress;
            MPDHeader = new MPD_HeaderModel(Data, 0, "MPDHeader", headerAddr, Scenario);
            Flags     = new MPD_FlagsFromHeader(MPDHeader);
            Settings  = new MPD_Settings(this);
            return MPDHeader;
        }

        private ITable[] MakeHeaderTables(MPD_HeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            tables.AddRange(MakeLightingTables(header));
            tables.AddRange(MakeTexturePaletteTables(header));
            tables.AddRange(MakeTextureAnimationTables(header, areAnimatedTextures32Bit));
            tables.Add(BoundariesTable = BoundaryTable.Create(Data, "Boundaries", ResourceUtils.ResourceFile("BoundaryList.xml"), header.OffsetBoundaries - RamAddress));
            tables.AddRange(MakeHeaderModelCollections(header));
            tables.AddRange(MakeUnknownTables(header));
            tables.AddRange(CreateUnreferencedTables(header, tables));

            foreach (var collection in ((CollectionType[]) Enum.GetValues(typeof(CollectionType))).Where(x => x.IsHeaderModelCollection()).ToArray())
                if (!ModelCollections.ContainsKey(collection))
                    ModelCollections[collection] = new MissingModelChunk(this, collection);

            return tables.ToArray();
        }

        private ChunkLocationTable MakeChunkHeaderTable()
            => ChunkLocations = ChunkLocationTable.Create(Data, "ChunkHeader", 0x2000);

        private ITable[] MakeTexturePaletteTables(MPD_HeaderModel header) {
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

        private ITable[] MakeLightingTables(MPD_HeaderModel header) {
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

        private ITable[] MakeTextureAnimationTables(MPD_HeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            if (header.OffsetTextureAnimations != 0) {
                try {
                    tables.Add(TextureAnimations = TextureAnimationTable.Create(Data, nameof(TextureAnimations), header.OffsetTextureAnimations - RamAddress, areAnimatedTextures32Bit, this));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            if (header.OffsetTextureAnimAlt != 0) {
                try {
                    tables.Add(TextureAnimationsAlt = TextureIDTable.Create(Data, nameof(TextureAnimationsAlt), "TexAnimAlt", header.OffsetTextureAnimAlt - RamAddress, 2, 0x100));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            return tables.ToArray();
        }

        private ITable[] MakeHeaderModelCollections(MPD_HeaderModel header) {
            var tables = new List<ITable>();
            var offsets = new int[] { header.OffsetChestModel, header.OffsetLockedChestModel, header.OffsetBarrelModel };

            for (int i = 0; i < 3; i++) {
                var offset = offsets[i];
                var collection = CollectionType.Chest + i;

                if (offset != 0) {
                    ModelCollections[collection] = MakeHeaderModelCollection(offset - RamAddress, collection, out var newTables);
                    tables.AddRange(newTables);
                }
            }

            return tables.ToArray();
        }

        private ModelChunk MakeHeaderModelCollection(int offset, CollectionType collection, out ITable[] tablesOut, bool isUnreferenced = false) {
            var name = collection.ToString() + "Model";
            var newChunk = ModelChunk.Create(this, Data, NameGetterContext, offset, name, null, collection, isUnreferenced);
            tablesOut = newChunk.Tables.ToArray();
            return newChunk;
        }

        private ITable[] MakeUnknownTables(MPD_HeaderModel header) {
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
                tables.Add(IndexedTextureTable = TextureIDTable.Create(Data, "IndexedTextures", "IndexedTexture", header.OffsetIndexedTextures - RamAddress, 4, 0x100));

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

        private ITable[] CreateUnreferencedTables(MPD_HeaderModel header, IEnumerable<ITable> existingTables) {
            var newTables = new List<ITable>();

            var usedSpace = GetUsedHeaderSpace(header, existingTables);
            var contiguousUnusedSpace = GetContiguousUnusedHeaderSpace(usedSpace);

            newTables.AddRange(MakeUnreferencedHeaderModelCollections(usedSpace, contiguousUnusedSpace));


            return newTables.ToArray();
        }

        private ITable[] MakeUnreferencedHeaderModelCollections(bool[] usedSpace, ushort[] contiguousUnusedSpace) {
            var newTables = new List<ITable>();

            while (TryMakeUnreferencedHeaderModelCollection(usedSpace, contiguousUnusedSpace, out var newTablesSub))
                newTables.AddRange(newTablesSub);

            return newTables.ToArray();
        }

        private bool TryMakeUnreferencedHeaderModelCollection(bool[] usedSpace, ushort[] contiguousUnusedSpace, out ITable[] newTablesOut) {
            // See if there are any header model collections not yet used.
            var unusedModelChunks =
                new CollectionType?[] {
                    CollectionType.Chest,
                    CollectionType.LockedChest,
                    CollectionType.Barrel,
                }
                .FirstOrDefault(x => !ModelCollections.ContainsKey(x.Value));

            // If there isn't an unused one, don't try to add a new one.
            if (!unusedModelChunks.HasValue) {
                newTablesOut = null;
                return false;
            }

            // We're going to look for some models. Key it to the first unused set of models.
            var collection = unusedModelChunks.Value;

            bool LooksLikeHeaderPointer(int address) {
                if (address % 4 != 0 && contiguousUnusedSpace[address] < 4)
                    return false;
                var value = Data.GetDouble(address);
                return value >= 0x290000 && value <= 0x291FFC;
            }

            bool LooksLikeHeaderModelInstance(int address) {
                if (address % 4 != 0 || contiguousUnusedSpace[address] < 0x1C)
                    return false;

                var notPointers = new int[] {
                    address + 0x04,
                    address + 0x08,
                    address + 0x0C,
                    address + 0x10,
                    address + 0x14,
                    address + 0x18,
                };

                // Looking for pointers and non-pointers is enough.
                return LooksLikeHeaderPointer(address) && notPointers.All(x => !LooksLikeHeaderPointer(x));
            }

            bool IsAllZeroes(int address, int count) {
                if (address > Data.Length - count)
                    return false;
                var dataRef = Data.GetDataCopyOrReference();
                for (int i = 0; i < count; i++)
                    if (dataRef[address + count] != 0)
                        return false;
                return true;
            }
 
            // Get the possible locations with some reasonable criteria.
            var possibleHeaderModelInstancesPass1 = Enumerable
                .Range(0, contiguousUnusedSpace.Length)
                .Where(x => x % 4 == 0 && contiguousUnusedSpace[x] >= 0x1C && LooksLikeHeaderPointer(x))
                .ToArray();

            // Narrow it down a bit more.
            var possibleHeaderModelInstances = possibleHeaderModelInstancesPass1.Where(x => LooksLikeHeaderModelInstance(x)).ToArray();

            // Look for a table with a proper terminator (0x1C zero bytes)
            for (int i = 0; i < possibleHeaderModelInstances.Length; i++) {
                // Skip headers with a match before.
                var addr = possibleHeaderModelInstances[i];
                if (i > 0 && possibleHeaderModelInstances[i - 1] == addr - 0x1C)
                    continue;
    
                // Get the position of the end of the table.
                var endAddr = addr + 0x1C;
                for (int j = i + 1; j < possibleHeaderModelInstances.Length; j++)
                    if (possibleHeaderModelInstances[j] == endAddr)
                        endAddr += 0x1C;

                // If we found an appropriate end entry, then make the collection, mark the space as used, and return success.
                if (IsAllZeroes(endAddr, 0x1C)) {
                    ModelCollections[collection] = MakeHeaderModelCollection(addr, collection, out var newTables, isUnreferenced: true);
                    MarkAllocatedSpace(usedSpace, newTables);
                    MarkContiguousUnusedHeaderSpace(contiguousUnusedSpace, usedSpace);
                    newTablesOut = newTables;
                    return true;
                }
            }

            // We didn't find a table; return failure.
            newTablesOut = null;
            return false;
        }

        private ITable[] MakeChunkTables(ChunkLocation[] chunkHeaders, IChunkData[] chunkDatas, IChunkData[] modelChunks, IChunkData surfaceModelChunk) {
            CollectionType TextureCollectionForChunkIndex(int chunkIndex) {
                if (chunkIndex == 10 && Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures)
                    return CollectionType.ExtraModels;
                else if (chunkIndex == 21)
                    return CollectionType.ExtraModels;
                else if (chunkIndex >= PrimaryTextureChunksFirstIndex && chunkIndex <= PrimaryTextureChunksLastIndex)
                    return CollectionType.Primary;
                else if (chunkIndex == MeshTextureChunksFirstIndex + 0 && chunkIndex <= MeshTextureChunksLastIndex)
                    return CollectionType.Chest;
                else if (chunkIndex == MeshTextureChunksFirstIndex + 1 && chunkIndex <= MeshTextureChunksLastIndex)
                    return CollectionType.LockedChest;
                else if (chunkIndex == MeshTextureChunksFirstIndex + 2 && chunkIndex <= MeshTextureChunksLastIndex)
                    return CollectionType.Barrel;

                throw new Exception("Can't determine texture collection based on chunk index");
            }

            var tables = new List<ITable>();

            foreach (var mc in modelChunks) {
                var collection =
                    (mc.Index == 19 && Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures) ? CollectionType.ExtraModels :
                    (chunkDatas[21] != null && mc.Index == 1) ? CollectionType.ExtraModels :
                    CollectionType.Primary;

                var newChunk = ModelChunk.Create(this, mc.DecompressedData, NameGetterContext, 0x00, "Models" + mc.Index, mc.Index, collection);
                ModelCollections[collection] = newChunk;
            }

            if (chunkDatas[5] != null) {
                SurfaceDataChunk = SurfaceDataChunk.Create(chunkDatas[5].DecompressedData, NameGetterContext, 0x00, "Surface", 5);
                tables.AddRange(SurfaceDataChunk.Tables);
            }

            // TODO: get this chunk loading with Ship2?
            if (surfaceModelChunk != null && Scenario != ScenarioType.Ship2) {
                SurfaceModelChunk = SurfaceModelChunk.Create(surfaceModelChunk.DecompressedData, NameGetterContext, 0x00, "SurfaceModel", surfaceModelChunk.Index, Scenario);
                tables.AddRange(SurfaceModelChunk.Tables);
            }

            // Gather all information about texture picture formats from existing data.
            // Each texture collection needs its own info.
            var pixelFormats = new Dictionary<CollectionType, Dictionary<int, TexturePixelFormat>>();
            foreach (var texCollection in (CollectionType[]) Enum.GetValues(typeof(CollectionType)))
                pixelFormats[texCollection] = new Dictionary<int, TexturePixelFormat>();
            var primaryPixelFormats = pixelFormats[CollectionType.Primary];

            // Always ABGR1555 for surface tiles.
            if (SurfaceModelChunk != null) {
                var textureIds = SurfaceModelChunk.TileTextureRowTable
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
            foreach (var models in ModelCollections.Values) {
                if (models is ModelChunk fileModels) {
                    if (fileModels?.AttrTablesByMemoryAddress != null)
                        foreach (var attrTable in fileModels.AttrTablesByMemoryAddress.Values)
                            foreach (var attr in attrTable)
                                primaryPixelFormats[attr.TextureNo] = TexturePixelFormat.ABGR1555;
                }
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
            var texColList = new List<TextureChunk>();
            var texChunks = ChunkLocations
                .Where(x => x.Exists && x.ChunkType == ChunkType.Textures)
                .Select(x => chunkDatas[x.ID])
                .ToList();

            int nextPrimaryCollectionStartId = 0;
            int nextModelCollectionStartId = 0x102;

            int index = 0;
            foreach (var chunk in texChunks) {
                var collection = TextureCollectionForChunkIndex(chunk.Index);
                bool isHeaderModel = collection.IsHeaderModelCollection();

                int? startId = null;
                if (isHeaderModel)
                    startId = nextModelCollectionStartId;
                else if (collection == CollectionType.Primary)
                    startId = nextPrimaryCollectionStartId;
                else if (collection == CollectionType.ExtraModels)
                    startId = 0;

                try {
                    var texCol = TextureChunk.Create(
                        chunk.DecompressedData, NameGetterContext, 0x00, "TextureCollection" + index,
                        collection, pixelFormats[collection], chunk.Index, startId, this
                    );
                    if (texCol.TextureTable != null) {
                        if (isHeaderModel)
                            nextModelCollectionStartId += texCol.TextureTable.Length;
                        else if (collection == CollectionType.Primary)
                            nextPrimaryCollectionStartId += texCol.TextureTable.Length;

                        texColList.Add(texCol);
                        tables.AddRange(texCol.Tables);
                    }
                }
                catch {
                    // TODO: what to do if we get an error here?
                }

                index++;
            }

            TextureChunks = texColList.ToArray();

            // Now that textures are loaded, build the texture animation frame data.
            // TODO: This function is a MESS. Please refactor it!!
            BuildTextureAnimFrameData();

            // Now that textures are loaded, build the texture animation frame data.
            // TODO: Only use this one!!
            if (chunkDatas[3] != null) {
                var infoByOffset = TextureAnimations
                    .SelectMany(x => x.TextureAnimationFrameTable)
                    .GroupBy(x => (int) x.CompressedImageDataOffset)
                    .ToDictionary(x => x.Key, x => {
                        var frame = x.First();
                        return new UniqueTextureAnimationFrameInfo(frame.Width, frame.Height, frame.PixelFormat != TexturePixelFormat.ABGR1555);
                    });

                TextureAnimationFrameChunk = TextureAnimationFrameChunk.Create(chunkDatas[3], NameGetterContext, 0, nameof(TextureAnimationFrameChunk), infoByOffset, this);
                tables.AddRange(TextureAnimationFrameChunk.Tables);
            }

            // Add chunks with tables for ground plane tile assignment.
            GroundTileAssignmentChunks = GroundTileAssignmentChunkDatas
                .Select((x, i) => PlaneTileAssignmentChunk.Create(x.DecompressedData, NameGetterContext, 0, "GroundTiles" + (i + 1), x.Index, i * 64, 4))
                .ToArray();
            foreach (var chunk in GroundTileAssignmentChunks)
                tables.AddRange(chunk.Tables);

            // Add chunks with tables for foreground plane tile assignment.
            if (ForegroundTileAssignmentChunkData != null) {
                var ch = ForegroundTileAssignmentChunkData;
                ForegroundTileAssignmentChunk = PlaneTileAssignmentChunk.Create(ch.DecompressedData, NameGetterContext, 0, "ForegroundTiles", ch.Index, 0, 1);
                tables.AddRange(ForegroundTileAssignmentChunk.Tables);
            }

            // Add an abstract representation of planes.
            Planes = new MPD_Planes(this);

            // Add an abstract representation of collision lines.
            Collisions = new MPD_Collisions(this);

            return tables.ToArray();
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

        private void InitTiles() {
            var tiles = new Tile[64, 64];
            for (var x = 0; x < 64; x++)
                for (var y = 0; y < 64; y++)
                    tiles[x, y] = new Tile(this, x, y);

            Surface = new MPD_Surface(Settings, tiles, () => this.SurfaceModelChunk != null);
        }
    }
}
