using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Model;
using SF3.Types;
using SF3.Imaging;
using SF3.MPD.Interfaces;
using CommonLib.SGL;
using SF3.Models.Tables.Shared.SGL;

namespace SF3.Models.Files.MPD {
    public class ModelChunk : TableFile, IMPD_ModelCollection {
        protected ModelChunk(
            IMPD_File mpdFile,
            IByteData data, 
            INameGetterContext nameContext,
            int address,
            string name,
            int? chunkIndex,
            MPD_CollectionType collection,
            bool isUnreferenced = false
        ) : base(data, nameContext)
        {
            MPD_File   = mpdFile;
            Address    = address;
            Name       = name;
            Collection = collection;
            ChunkIndex = chunkIndex;
            IsUnreferenced = isUnreferenced;
        }

        public static ModelChunk Create(
            IMPD_File mpdFile, IByteData data, INameGetterContext nameContext, int address, string name,
            int? chunkIndex, MPD_CollectionType modelCollection, bool isUnreferenced = false
        ) {
            var newFile = new ModelChunk(mpdFile, data, nameContext, address, name, chunkIndex, modelCollection, isUnreferenced);
            newFile.Init();
            return newFile;
        }

        private struct ModelElementKey {
            public uint AddressInMemory;
            public int Count;
            public int Refs;
        }

        public override IEnumerable<ITable> MakeTables() {
            if (Collection.IsHeaderModelCollection())
                HeaderModelInstanceTable = HeaderModelInstanceTable.Create(Data, this, nameof(HeaderModelInstanceTable), Address);
            else {
                ModelsHeader = new ModelsHeader(Data, 0, nameof(ModelsHeader), Address + 0x0000);
                ModelInstanceTable = MPD_ModelInstanceTable.Create(Data, this, nameof(ModelInstanceTable), Address + 0x000C, ModelsHeader.NumModels, Scenario >= ScenarioType.Prototype);
            }

            var pdataAddressesPre =
                (ModelInstanceTable != null) ? ModelInstanceTable
                    .SelectMany(x => x.PDatas.Select((y, i) => new { PData0 = x.PDatas[0], PDataAddress = y.Value, LevelOfDetail = i }))
                : HeaderModelInstanceTable
                    .Select((x, i) => new { PData0 = x.PData0, PDataAddress = x.PData0, LevelOfDetail = 0 });

            var pdata0ToModelIdMap = pdataAddressesPre
                .GroupBy(x => x.PData0)
                .Select((x, i) => new { Addr = x.Key, ModelID = i })
                .ToDictionary(x => x.Addr, y => y.ModelID);

            var pdataAddresses = pdataAddressesPre
                .Where(x => x.PDataAddress != 0)
                .GroupBy(x => x.PDataAddress)
                .Select(x => new { AddressInMemory = x.Key, First = x.First(), Count = x.Count() })
                .Select(x => new { x.AddressInMemory, ModelID = pdata0ToModelIdMap[x.First.PData0], x.First.LevelOfDetail, x.Count })
                .OrderBy(x => x.AddressInMemory)
                .ToArray();

            var pdataRefs = pdataAddresses
                .Select(x => new MPD_SGL_Model_PDataTable.PDataRef() {
                    Address       = (int) GetOffsetInChunk(x.AddressInMemory),
                    Collection    = Collection,
                    ChunkIndex    = ChunkIndex,
                    ModelID       = x.ModelID,
                    LevelOfDetail = x.LevelOfDetail,
                    RefCount      = x.Count
                })
                .ToArray();

            PDataTable = MPD_SGL_Model_PDataTable.Create(Data, "PDATAs", MPD_File, pdataRefs);

            try {
                PDatasByMemoryAddress = PDataTable
                    .Select((PData, i) => new { PData, pdataAddresses[i].AddressInMemory })
                    .ToDictionary(x => x.AddressInMemory, x => x.PData);
                foreach (var pdata in PDatasByMemoryAddress)
                    pdata.Value.RamAddress = pdata.Key;
            }
            catch {
                // TODO: what to do on error??
                PDatasByMemoryAddress = new Dictionary<uint, MPD_SGL_Model_PDataStruct>();
            }

            try {
                VertexTablesByMemoryAddress = PDataTable
                    .Select(x => new ModelElementKey { AddressInMemory = x.VerticesOffset, Count = x.VertexCount })
                    .Where(x => x.AddressInMemory != 0)
                    .GroupBy(x => x.AddressInMemory)
                    .Select(x => {
                        var first = x.First();
                        first.Refs = x.Count();
                        return first;
                    })
                    .OrderBy(x => x.AddressInMemory)
                    .ThenBy(x => x.Count)
                    .ToDictionary(
                        x => x.AddressInMemory,
                        x => VertexTable.Create(Data, "POINTs @ 0x" + x.AddressInMemory.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", (int) GetOffsetInChunk(x.AddressInMemory), x.Count)
                    );
            }
            catch {
                // TODO: what to do on error??
                VertexTablesByMemoryAddress = new Dictionary<uint, VertexTable>();
            }

            try {
                PolygonTablesByMemoryAddress = PDataTable
                    .Select(x => new ModelElementKey { AddressInMemory = x.PolygonsOffset, Count = x.FaceCount })
                    .Where(x => x.AddressInMemory != 0)
                    .GroupBy(x => x.AddressInMemory)
                    .Select(x => {
                        var first = x.First();
                        first.Refs = x.Count();
                        return first;
                    })
                    .OrderBy(x => x.AddressInMemory)
                    .ThenBy(x => x.Count)
                    .ToDictionary(
                        x => x.AddressInMemory,
                        x => PolygonTable.Create(Data, "POLYGONs @ 0x" + x.AddressInMemory.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", (int) GetOffsetInChunk(x.AddressInMemory), x.Count)
                    );
            }
            catch {
                // TODO: what to do on error??
                PolygonTablesByMemoryAddress = new Dictionary<uint, PolygonTable>();
            }

            try {
                AttrTablesByMemoryAddress = PDataTable
                    .Select(x => new ModelElementKey { AddressInMemory = x.AttributesOffset, Count = x.FaceCount })
                    .Where(x => x.AddressInMemory != 0)
                    .GroupBy(x => x.AddressInMemory)
                    .Select(x => {
                        var first = x.First();
                        first.Refs = x.Count();
                        return first;
                    })
                    .OrderBy(x => x.AddressInMemory)
                    .ThenBy(x => x.Count)
                    .ToDictionary(
                        x => x.AddressInMemory,
                        x => AttrTable.Create(Data, "ATTRs @ 0x" + x.AddressInMemory.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", (int) GetOffsetInChunk(x.AddressInMemory), x.Count)
                    );
            }
            catch {
                // TODO: what to do on error??
                AttrTablesByMemoryAddress = new Dictionary<uint, AttrTable>();
            }

            if (ModelsHeader != null) {
                if (ModelsHeader.CollisionBlocksOffset != 0 &&
                    ModelsHeader.CollisionBlocksOffset != 0xDEADADD0 && /* found in some beta maps */
                    (ModelsHeader.CollisionBlocksOffset & 0xF000000) != 0xF000000 /* SHIP2.MPD */
                ) {
                    CollisionBlockTable = CollisionBlockTable.Create(Data, "CollisionBlocks", (int) GetOffsetInChunk(ModelsHeader.CollisionBlocksOffset));
                    CollisionLineIndexTablesByBlock = new Dictionary<int, CollisionLineIndexTable>();

                    var pos = 0;
                    for (var y = 0; y < CollisionBlockTable.Count; y++) {
                        var row = CollisionBlockTable[y];
                        for (var x = 0; x < row.Length; x++) {
                            try {
                                var addr = row[x];
                                if (addr > 0) {
                                    var name = $"CollisionBlockLineIndexTable[{x}][{y}] (0x{addr:X})";
                                    CollisionLineIndexTablesByBlock[pos] = CollisionLineIndexTable.Create(Data, name, (int) GetOffsetInChunk(addr), x, y);
                                }
                            }
                            catch {
                                // TODO: what to do here??
                            }
                            pos++;
                        }
                    }
                }

                if (ModelsHeader.CollisionLinesHeaderOffset != 0 &&
                    ModelsHeader.CollisionLinesHeaderOffset != 0xDEADADD0 && /* found in some beta maps */
                    (ModelsHeader.CollisionLinesHeaderOffset & 0xF000000) != 0xF000000 /* SHIP2.MPD */
                ) {
                    CollisionLinesHeader = new CollisionLinesHeader(Data, 0, "CollisionLinesHeader", (int) GetOffsetInChunk(ModelsHeader.CollisionLinesHeaderOffset));

                    try {
                        if (CollisionLinesHeader.PointsOffset != 0 && CollisionLinesHeader.LinesOffset != 0) {
                            var pointCount = (int) (CollisionLinesHeader.LinesOffset - CollisionLinesHeader.PointsOffset) / 4;
                            CollisionPointTable = CollisionPointTable.Create(Data, "CollisionPoints", (int) GetOffsetInChunk(CollisionLinesHeader.PointsOffset), pointCount);

                            var lineCount = (int) (ModelsHeader.CollisionBlocksOffset - CollisionLinesHeader.LinesOffset) / 0x08;
                            CollisionLineTable = CollisionLineTable.Create(Data, "CollisionLines", (int) GetOffsetInChunk(CollisionLinesHeader.LinesOffset), lineCount, CollisionPointTable, CollisionLineIndexTablesByBlock.Values);
                        }
                    }
                    catch {
                        // TODO: what to do here?
                    }
                }

                // ATBTL2 (S2), ELINB, and ATBTL2 (S3) have some additional data (ATTRs) in between the model instance table and the PDATA's referenced.
                if (ModelInstanceTable?.Count > 0 && PDatasByMemoryAddress?.Count > 0) {
                    var endOfInstanceTable = ModelInstanceTable.Address + ModelInstanceTable.SizeInBytesPlusTerminator;
                    var startOfPDatas = PDatasByMemoryAddress.Values.First().Address;
                    var diff = startOfPDatas - endOfInstanceTable;
                    if (diff > 0)
                        DataAfterInstancesTable = UnknownUInt8Table.Create(Data, nameof(DataAfterInstances), endOfInstanceTable, diff, readUntil: null);
                }
            }

            var tables =
                new List<ITable>() {
                    ModelInstanceTable,
                    HeaderModelInstanceTable,
                    PDataTable,
                    CollisionPointTable,
                    CollisionLineTable,
                    CollisionBlockTable,
                }
                .Where(x => x != null)
                .ToList();

            tables.AddRange(VertexTablesByMemoryAddress.Values);
            tables.AddRange(PolygonTablesByMemoryAddress.Values);
            tables.AddRange(AttrTablesByMemoryAddress.Values);

            if (CollisionLineIndexTablesByBlock != null)
                tables.AddRange(CollisionLineIndexTablesByBlock.Values);
            if (DataAfterInstancesTable != null)
                tables.Add(DataAfterInstancesTable);

            return tables;
        }

        public uint GetOffsetInChunk(uint memoryAddress) {
            if (Collection.IsHeaderModelCollection())
                return memoryAddress - 0x290000;
            else if (memoryAddress >= 0x60a0000)
                return memoryAddress - 0x60a0000 /* TODO: apply actual offset of chunk! */;
            else if (memoryAddress >= 0x290000)
                return memoryAddress - 0x292100 /* TODO: apply actual offset of chunk! */;
            else if (memoryAddress >= 0x252100) // SHIP2.MPD
                return memoryAddress - 0x252100;
            else
                return memoryAddress;
        }

        private IReadOnlyList<IMPD_ModelInstance> _mpdModelInstances;
        public IReadOnlyList<IMPD_ModelInstance> ModelInstances {
            get {
                if (_mpdModelInstances == null) {
                    var instances = new List<IMPD_ModelInstance>();

                    if (ModelInstanceTable != null)
                        foreach (var mi in ModelInstanceTable)
                            instances.Add(mi);
                    if (HeaderModelInstanceTable != null)
                        foreach (var mi in HeaderModelInstanceTable)
                            instances.Add(mi);

                    _mpdModelInstances = instances.ToArray();
                }

                UpdateModelInstanceIDs();
                return _mpdModelInstances;
            }
        }

        private void UpdateModelInstanceIDs() {
            var instances = _mpdModelInstances;

            Dictionary<(int ModelID, int LoD), uint> pdataAddressesByID;
            pdataAddressesByID = PDatasByMemoryAddress
                .ToDictionary(x => (ModelID: x.Value.ModelID, LoD: x.Value.LevelOfDetail), x => x.Key);

            foreach (var inst in instances) {
                var fileInst = (MPD_ModelInstanceBase) inst;
                fileInst.ModelIDToPDataMap = null;
                inst.ModelID = PDatasByMemoryAddress.TryGetValue(fileInst.PData0, out var pdata) ? pdata.ModelID : -1;
                fileInst.ModelIDToPDataMap = pdataAddressesByID;
            }
        }

        ISGL_Model ISGL_ModelCollection.GetModel(int id, int lod) => GetModel(id, lod);
        public IMPD_ModelLoD GetModel(int id, int lod)
            => PDatasByMemoryAddress.Values.FirstOrDefault(x => x.Collection == Collection && x.ModelID == id && x.LevelOfDetail == lod);

        private class ModelWithLoD : IMPD_Model {
            public ModelWithLoD(MPD_CollectionType collection, int modelID, IMPD_ModelLoD[] models) {
                Collection     = collection;
                ModelID        = modelID;
                ModelLoDs      = models;
                LevelsOfDetail = ModelLoDs.Max(x => x.LevelOfDetail) + 1;
            }

            public MPD_CollectionType Collection { get; }
            public int ModelID { get; }
            public int LevelsOfDetail { get; }
            public IReadOnlyList<IMPD_ModelLoD> ModelLoDs { get; }
        }

        private IReadOnlyList<IMPD_Model> _mpdModelsWithLoD;
        public IReadOnlyList<IMPD_Model> Models {
            get {
                if (_mpdModelsWithLoD == null) {
                    _mpdModelsWithLoD = PDataTable
                        .GroupBy(x => x.ModelID)
                        .Select(x => (IMPD_Model) new ModelWithLoD(Collection, x.Key, x.ToArray()))
                        .ToArray();
                }
                return _mpdModelsWithLoD;
            }
        }

        [BulkCopyRowName]
        public string Name { get; }
        public ScenarioType Scenario => MPD_File.Scenario;
        public IMPD_File MPD_File { get; }
        public MPD_CollectionType Collection { get; }
        public int Address { get; }
        public int? ChunkIndex { get; }

        [BulkCopyRecurse]
        public ModelsHeader ModelsHeader { get; private set; }

        [BulkCopyRecurse]
        public MPD_ModelInstanceTable ModelInstanceTable { get; private set; }

        [BulkCopyRecurse]
        public HeaderModelInstanceTable HeaderModelInstanceTable { get; private set; }

        [BulkCopyRecurse]
        public MPD_SGL_Model_PDataTable PDataTable { get; private set; }

        public Dictionary<uint, MPD_SGL_Model_PDataStruct> PDatasByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, VertexTable> VertexTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, PolygonTable> PolygonTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, AttrTable> AttrTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public CollisionLinesHeader CollisionLinesHeader { get; private set; }

        [BulkCopyRecurse]
        public CollisionPointTable CollisionPointTable { get; private set; }

        [BulkCopyRecurse]
        public CollisionLineTable CollisionLineTable { get; private set; }

        [BulkCopyRecurse]
        public CollisionBlockTable CollisionBlockTable { get; private set; } 

        [BulkCopyRecurse]
        public Dictionary<int, CollisionLineIndexTable> CollisionLineIndexTablesByBlock { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt8Table DataAfterInstancesTable { get; private set; }

        private bool _gotTextures = false;
        private IReadOnlyList<IMPD_AnimatableTexture> _textures = null;
        public IReadOnlyList<IMPD_AnimatableTexture> Textures {
            get {
                if (!_gotTextures) {
                    var textureChunks = MPD_File.TextureChunks
                        .Where(x => x.Collection == Collection)
                        .ToArray();

                    if (textureChunks.Length > 0) {
                        _textures = textureChunks
                            .SelectMany(x => x.TextureTable.Rows)
                            .Cast<IMPD_AnimatableTexture>()
                            .ToArray();
                    }
                    _gotTextures = true;
                }
                return _textures;
            }
        }

        public IReadOnlyList<byte> DataAfterInstances {
            get => DataAfterInstancesTable;
            set {}
        }

        public bool IsUnreferenced { get; set; }
        public bool HasMissingModels => false;
    }
}
