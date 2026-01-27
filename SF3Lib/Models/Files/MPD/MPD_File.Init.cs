using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.Animation;
using SF3.Models.Tables.MPD.Main;
using SF3.Models.Tables.Shared;
using SF3.MPD.Project;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        public override IEnumerable<ITable> MakeTables() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Load root headers
            var header = MakeHeader();

            // Make chunk data accessors
            var chunks = MakeChunkHeaderTable().Rows;
            var chunkDatas = MakeChunkDatas(chunks);

            // Load header and chunk tables
            var headerTables = MakeHeaderTables(header, areAnimatedTextures32Bit);
            var chunkTables = MakeChunkTables(chunks, chunkDatas, ModelChunkDatas, SurfaceModelChunkData);

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

        private MPD_Header MakeHeader() {
            var headerAddrPtr = Data.GetDouble(0x0000) - RamAddress;
            var headerAddr = Data.GetDouble(headerAddrPtr) - RamAddress;
            MPDHeader = new MPD_Header(Data, 0, nameof(MPDHeader), headerAddr, Scenario);
            Flags     = new MPD_FlagsFromHeader(MPDHeader);
            Settings  = new MPD_Settings(this);
            BinaryReproductionFlags = new MPD_BinaryReproductionFlags(this);
            return MPDHeader;
        }

        private ITable[] MakeHeaderTables(MPD_Header header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            // This is referenced when determining the length of other tables, so make it early.
            // TODO: put somewhere else!!
            if (header.OffsetModelSwitchGroups > 0) {
                tables.Add(ModelSwitchGroupsTable = ModelSwitchGroupsTable.Create(Data, "ModelSwitchGroups", header.OffsetModelSwitchGroups - RamAddress, NameGetterContext));
                foreach (var switchGroup in ModelSwitchGroupsTable) {
                    if (switchGroup.ModelInstancesVisibleWhenOffTable != null)
                        tables.Add(switchGroup.ModelInstancesVisibleWhenOffTable);
                    if (switchGroup.ModelInstancesVisibleWhenOnTable != null)
                        tables.Add(switchGroup.ModelInstancesVisibleWhenOnTable);
                }
            }

            tables.AddRange(MakeLightingTables(header));
            tables.AddRange(MakePaletteTables(header));
            tables.AddRange(MakeAnimationTables(header, areAnimatedTextures32Bit));
            tables.Add(BoundariesTable = BoundaryTable.Create(Data, nameof(BoundariesTable), ResourceUtils.ResourceFile("BoundaryList.xml"), header.OffsetBoundaries - RamAddress));
            tables.AddRange(MakeHeaderModelCollections(header));
            tables.AddRange(MakeOtherTables(header));
            tables.AddRange(MakeUnknownTables(header));
            tables.AddRange(CreateUnreferencedTables(header, tables));

            foreach (var collection in ((MPD_CollectionType[]) Enum.GetValues(typeof(MPD_CollectionType))).Where(x => x.IsHeaderModelCollection()).ToArray())
                if (!ModelCollections.ContainsKey(collection))
                    ModelCollections[collection] = new MissingModelChunk(this, collection);

            return tables.ToArray();
        }

        private ChunkLocationTable MakeChunkHeaderTable()
            => ChunkLocations = ChunkLocationTable.Create(Data, nameof(ChunkLocations), 0x2000);

        private ITable[] MakePaletteTables(MPD_Header header) {
            var tables = new List<ITable>();
            var headerRamAddr = header.Address + RamAddress;

            // Sometimes palette addresses are placed in an odd place at or just before the header actually begins.
            // This is most likely an error in the MPD file; it results in garbage data.
            // Don't load the palettes in these cases.
            if (header.OffsetGroundPalette >= RamAddress && (headerRamAddr - header.OffsetGroundPalette) / 2 >= 256)
                tables.Add(GroundPaletteColorTable = ColorTable.Create(Data, nameof(GroundPaletteColorTable), header.OffsetGroundPalette - RamAddress, 256));
            if (header.OffsetSkyPalette >= RamAddress && (headerRamAddr - header.OffsetSkyPalette) / 2 >= 256)
                tables.Add(SkyPaletteColorTable = ColorTable.Create(Data, nameof(SkyPaletteColorTable), header.OffsetSkyPalette - RamAddress, 256));
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetTexturePalette >= RamAddress && (headerRamAddr - header.OffsetTexturePalette) / 2 >= 256)
                tables.Add(TexturePaletteColorTable = ColorTable.Create(Data,  nameof(TexturePaletteColorTable), header.OffsetTexturePalette - RamAddress, 256));

            return tables.ToArray();
        }

        private ITable[] MakeLightingTables(MPD_Header header) {
            var tables = new List<ITable>();

            if (header.OffsetLightPalette > 0)
                tables.Add(LightPaletteColorTable = ColorTable.Create(Data, nameof(LightPaletteColorTable), header.OffsetLightPalette - RamAddress, 32));
            if (header.OffsetLightPosition > 0)
                LightPosition = new LightPosition(Data, 0, nameof(LightPosition), header.OffsetLightPosition - RamAddress);

            if (header.OffsetPaletteAdjustment > 0) {
                var msgAddr = ModelSwitchGroupsTable?.GetEarliestRamAddress() ?? 0;
                var diff = msgAddr - (uint) header.OffsetPaletteAdjustment;
                var isTruncated = (Scenario >= ScenarioType.Scenario3) && (msgAddr != 0 && diff < 0x0E);

                PaletteAdjustment = new PaletteAdjustment(Data, 0, nameof(PaletteAdjustment), header.OffsetPaletteAdjustment - RamAddress, Scenario >= ScenarioType.Scenario3, isTruncated);
            }

            if (header.OffsetGradient > 0) {
                var readUntil = (header.OffsetGroundAnimation == 0) ? (int?) null : (header.OffsetGroundAnimation - 0x290000);
                tables.Add(GradientTable = GradientTable.Create(Data, nameof(GradientTable), header.OffsetGradient - RamAddress, readUntil));
            }

            return tables.ToArray();
        }

        private ITable[] MakeAnimationTables(MPD_Header header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            if (header.OffsetAnimations > 0) {
                try {
                    tables.Add(Animations = AnimationTable.Create(Data, nameof(Animations), header.OffsetAnimations - RamAddress, areAnimatedTextures32Bit, this));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            if (header.OffsetIgnoredTextures > 0) {
                try {
                    var groundPaletteOffset = header.OffsetGroundPalette;
                    var readUntil = (groundPaletteOffset > 0) ? (int?) groundPaletteOffset - 0x290000 : null;
                    tables.Add(IgnoredTextureTable = IgnoredTextureTable.Create(Data, nameof(IgnoredTextureTable), header.OffsetIgnoredTextures - RamAddress, readUntil: readUntil, maxSize: 0x100));
                }
                catch {
                    // TODO: what to do here??
                }
            }

            return tables.ToArray();
        }

        private ITable[] MakeHeaderModelCollections(MPD_Header header) {
            var tables = new List<ITable>();
            var offsets = new int[] { header.OffsetChestModel, header.OffsetLockedChestModel, header.OffsetBarrelModel };

            for (int i = 0; i < 3; i++) {
                var offset = offsets[i];
                var collection = MPD_CollectionType.Chest + i;

                if (offset > 0) {
                    ModelCollections[collection] = MakeHeaderModelCollection(offset - RamAddress, collection, out var newTables);
                    tables.AddRange(newTables);
                }
            }

            return tables.ToArray();
        }

        private ModelChunk MakeHeaderModelCollection(int offset, MPD_CollectionType collection, out ITable[] tablesOut, bool isUnreferenced = false) {
            var name = collection.ToString() + "Model";
            var newChunk = ModelChunk.Create(this, Data, NameGetterContext, offset, name, null, collection, isUnreferenced);
            tablesOut = newChunk.Tables.ToArray();
            return newChunk;
        }

        private ITable[] MakeOtherTables(MPD_Header header) {
            var tables = new List<ITable>();

            // TODO: put somewhere else!!
            if (header.OffsetGroundAnimation > 0) {
                var readUntil = (header.OffsetBoundaries > 0) ? (header.OffsetBoundaries - 0x290000) : (int?) null;
                tables.Add(GroundAnimationTable = GroundAnimationTable.Create(Data, nameof(GroundAnimationTable), header.OffsetGroundAnimation - RamAddress, readUntil, null));
            }

            // TODO: put somewhere else!!
            if (header.OffsetIndexedTextures > 0)
                tables.Add(IndexedTextureTable = IndexedTextureTable.Create(Data, "IndexedTextures", header.OffsetIndexedTextures - RamAddress, 0x100));

            return tables.ToArray();
        }

        private ITable[] MakeUnknownTables(MPD_Header header) {
            var tables = new List<ITable>();

            // This table is only present before Scenario 2 and is always 32 bytes if it exists.
            if (header.OffsetUnknown1 > 0) {
                // Use at most 0x20 2-byte values (0x40 bytes total).
                int lowestOffset = header.OffsetUnknown1 + 0x40;

                void updateLowest(int value) {
                    if (value < lowestOffset && value >= header.OffsetUnknown1)
                        lowestOffset = value;
                }

                // The offset model switch groups usually (always?) occupy some space before its address.
                // Make sure this table doesn't occupy that space.
                updateLowest((int) (ModelSwitchGroupsTable?.GetEarliestRamAddress() ?? 0));
                updateLowest(header.OffsetAnimations);
                updateLowest(header.OffsetUnknown2);

                // We have our best guess for the size. Add the table!
                var lengthInBytes = ((int) lowestOffset - header.OffsetUnknown1);
                var size = Math.Min(32, lengthInBytes / 2);

                tables.Add(Unknown1Table = UnknownUInt16Table.Create(Data, "Unknown1", header.OffsetUnknown1 - RamAddress, size, null));
            }

            // This table is only present before Scenario 2 and varies in size.
            if (header.OffsetUnknown2 > 0 && header.OffsetGroundAnimation > 0) {
                var size = (header.OffsetGroundAnimation - header.OffsetUnknown2) / 2;
                if (size > 0) {
                    var addr = header.OffsetUnknown2 - RamAddress;
                    bool isDummiedOut = false;
                    if (Data.GetWord(addr) == 0xFFFF && size > 1) {
                        addr += 2;
                        isDummiedOut = true;
                        size--;
                    }
                    tables.Add(Unknown2Table = Unknown2Table.Create(Data, "Unknown2", addr, size, isDummiedOut));
                }
            }
            // Some files have an old, dummied out Unknown 2 (MUBAR, SARA22). Let's load them, just for fun.
            else if (header.OffsetGradient > 0 && header.OffsetGroundAnimation > 0) {
                var gradient = (GradientTable.Length > 0) ? GradientTable.Last() : null;
                var start = (gradient == null) ? header.OffsetGradient + 0x02 : (gradient.Address + gradient.Size + 0x02 + RamAddress);

                var size = (header.OffsetGroundAnimation - start) / 2;
                if (size > 1)
                    tables.Add(Unknown2Table = Unknown2Table.Create(Data, "Unknown2", start - RamAddress, size, true));
            }

            // This table is only present in SHIP2.
            if (header.OffsetUnknown3 > 0) {
                var maxSize = (header.OffsetUnknown4 > 0) ? ((header.OffsetUnknown4 - header.OffsetUnknown3) / 2) : 0;
                if (maxSize > 0)
                    tables.Add(Unknown3Table = UnknownUInt16Table.Create(Data, "Unknown3", header.OffsetUnknown3 - RamAddress, maxSize, null));
            }

            // This table is only present in SHIP2 and Prototype maps.
            if (header.OffsetUnknown4 > 0) {
                var maxSize = (header.OffsetGroundPalette > 0) ? ((header.OffsetGroundPalette - header.OffsetUnknown4) / 2) : 0;
                if (maxSize > 0)
                    tables.Add(Unknown4Table = UnknownUInt16Table.Create(Data, "Unknown4", header.OffsetUnknown4 - RamAddress, maxSize, null));
            }

            return tables.ToArray();
        }

        private ITable[] CreateUnreferencedTables(MPD_Header header, IEnumerable<ITable> existingTables) {
            var newTables = new List<ITable>();

            // Create a map of unused space. We're going to use that to determine where unreferenced data could be.
            var usedSpace = GetUsedHeaderSpace(header, existingTables);
            var contiguousUnusedSpace = GetContiguousUnusedHeaderSpace(usedSpace);

            // Look for chest/barrel models that sometimes exist but are unreferenced.
            newTables.AddRange(MakeUnreferencedHeaderModelCollections(usedSpace, contiguousUnusedSpace));

            // For Scenario2+, look for any dummy data that may exist after the PaletteAdjustment table.
            if (Scenario >= ScenarioType.Scenario2 && PaletteAdjustment != null) {
                var pos = PaletteAdjustment.Address + PaletteAdjustment.Size;
                if (pos % 4 != 0)
                    pos += 4 - (pos % 4);
                var size = contiguousUnusedSpace[pos];
                if (size > 0) {
                    newTables.Add(UnreferencedDataAfterPaletteAdjustmentTable = UnknownUInt8Table.Create(
                        Data, nameof(UnreferencedDataAfterPaletteAdjustmentTable), pos, size, null
                    ));
                }
            }

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
                new MPD_CollectionType?[] {
                    MPD_CollectionType.Chest,
                    MPD_CollectionType.LockedChest,
                    MPD_CollectionType.Barrel,
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
                    if (dataRef[address + i] != 0)
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
            MPD_CollectionType TextureCollectionForChunkIndex(int chunkIndex) {
                if (chunkIndex == 10 && Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures)
                    return MPD_CollectionType.ExtraModels;
                else if (chunkIndex == 21)
                    return MPD_CollectionType.ExtraModels;
                else if (chunkIndex >= PrimaryTextureChunksFirstIndex && chunkIndex <= PrimaryTextureChunksLastIndex)
                    return MPD_CollectionType.Primary;
                else if (chunkIndex == MeshTextureChunksFirstIndex + 0 && chunkIndex <= MeshTextureChunksLastIndex)
                    return MPD_CollectionType.Chest;
                else if (chunkIndex == MeshTextureChunksFirstIndex + 1 && chunkIndex <= MeshTextureChunksLastIndex)
                    return MPD_CollectionType.LockedChest;
                else if (chunkIndex == MeshTextureChunksFirstIndex + 2 && chunkIndex <= MeshTextureChunksLastIndex)
                    return MPD_CollectionType.Barrel;

                throw new Exception("Can't determine texture collection based on chunk index");
            }

            var tables = new List<ITable>();

            foreach (var mc in modelChunks) {
                var collection =
                    (mc.Index == 19 && Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures) ? MPD_CollectionType.ExtraModels :
                    (chunkDatas[21] != null && mc.Index == 1) ? MPD_CollectionType.ExtraModels :
                    MPD_CollectionType.Primary;

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
            var pixelFormats = new Dictionary<MPD_CollectionType, Dictionary<int, TexturePixelFormat>>();
            foreach (var texCollection in (MPD_CollectionType[]) Enum.GetValues(typeof(MPD_CollectionType)))
                pixelFormats[texCollection] = new Dictionary<int, TexturePixelFormat>();
            var primaryPixelFormats = pixelFormats[MPD_CollectionType.Primary];

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

            // Textures in the "skip textures" table are ABGR1555.
            if (IgnoredTextureTable != null)
                foreach (var tex in IgnoredTextureTable)
                    primaryPixelFormats[tex.TextureID] = TexturePixelFormat.ABGR1555;

            // Mark indexed textures as such.
            if (IndexedTextureTable != null)
                foreach (var tex in IndexedTextureTable)
                    primaryPixelFormats[tex.TextureID] = TexturePixelFormat.Indexed8Bit;

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
                else if (collection == MPD_CollectionType.Primary)
                    startId = nextPrimaryCollectionStartId;
                else if (collection == MPD_CollectionType.ExtraModels)
                    startId = 0;

                try {
                    var texCol = TextureChunk.Create(
                        chunk.DecompressedData, NameGetterContext, 0x00, "TextureCollection" + index,
                        collection, pixelFormats[collection], chunk.Index, startId, this
                    );
                    if (texCol.TextureTable != null) {
                        if (isHeaderModel)
                            nextModelCollectionStartId += texCol.TextureTable.Length;
                        else if (collection == MPD_CollectionType.Primary)
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

            // Now that textures are loaded, build the animation frame data.
            if (chunkDatas[3] != null) {
                var infoByOffset = (Animations != null) ? Animations
                    .SelectMany(x => x.AnimationFrameTable.Select(y => (Anim: x, Frame: y)))
                    .GroupBy(x => x.Frame.ImageDataOffset)
                    .ToDictionary(x => x.Key, x => {
                        var anim = x.First().Anim;
                        return new UniqueAnimationFrameInfo((int) anim.Width, (int) anim.Height, anim.IsIndexed);
                    })
                    : new Dictionary<int, UniqueAnimationFrameInfo>();

                AnimationFrameChunk = AnimationFrameChunk.Create(chunkDatas[3], NameGetterContext, 0, nameof(AnimationFrameChunk), infoByOffset, this);
                tables.AddRange(AnimationFrameChunk.Tables);
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
