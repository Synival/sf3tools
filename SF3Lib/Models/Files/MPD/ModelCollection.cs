using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Model;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public class ModelCollection : TableFile {
        protected ModelCollection(
            IByteData data, INameGetterContext nameContext, int address, string name,
            ScenarioType scenario, int? chunkIndex, ModelCollectionType collectionType)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            Scenario   = scenario;
            CollectionType = collectionType;
            ChunkIndex = chunkIndex;
            MovableModelsIndex = null;
        }

        protected ModelCollection(IByteData data, INameGetterContext nameContext, int address, string name, int movableModelsIndex)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            Scenario   = (ScenarioType) (-1);

            CollectionType =
                (movableModelsIndex == 0) ? ModelCollectionType.MovableModels1 :
                (movableModelsIndex == 1) ? ModelCollectionType.MovableModels2 :
                (movableModelsIndex == 2) ? ModelCollectionType.MovableModels3 :
                throw new ArgumentException(nameof(movableModelsIndex));

            ChunkIndex = null;
            MovableModelsIndex = movableModelsIndex;
        }

        public static ModelCollection Create(
            IByteData data, INameGetterContext nameContext, int address, string name,
            ScenarioType scenario, int? chunkIndex, ModelCollectionType modelCollection
        ) {
            var newFile = new ModelCollection(data, nameContext, address, name, scenario, chunkIndex, modelCollection);
            newFile.Init();
            return newFile;
        }

        public static ModelCollection Create(IByteData data, INameGetterContext nameContext, int address, string name, int movableModelsIndex) {
            var newFile = new ModelCollection(data, nameContext, address, name, movableModelsIndex);
            newFile.Init();
            return newFile;
        }

        private struct ModelElementKey {
            public uint AddressInMemory;
            public int Count;
            public int Refs;
        }

        public override IEnumerable<ITable> MakeTables() {
            if (MovableModelsIndex.HasValue) {
                MovableModelTable = MovableModelTable.Create(Data, "MovableModelsHeader", Address);
            }
            else {
                ModelsHeaderTable = ModelsHeaderTable.Create(Data, "ModelsHeader", 0x0000);
                ModelTable = ModelTable.Create(Data, "Models", 0x000C, ModelsHeaderTable[0].NumModels, Scenario >= ScenarioType.Other);
            }

            var pdataAddressesPre =
                (ModelTable != null) ? ModelTable
                    .SelectMany(x => x.PDatas.Select((y, i) => new { PDataAddress = y.Value, Index = i }))
                : MovableModelTable
                    .Select(x => new { PDataAddress = x.PData0, Index = 0 });

            var pdataAddresses = pdataAddressesPre
                .Where(x => x.PDataAddress != 0)
                .GroupBy(x => x.PDataAddress)
                .Select(x => new { AddressInMemory = x.Key, x.First().Index, Count = x.Count() })
                .OrderBy(x => x.AddressInMemory)
                .ToArray();

            var pdataRefs = pdataAddresses
                .Select(x => new PDataTable.PDataRef() {
                    Address    = (int) GetOffsetInChunk(x.AddressInMemory),
                    Collection = CollectionType,
                    ChunkIndex = ChunkIndex,
                    Index      = x.Index,
                    RefCount   = x.Count
                })
                .ToArray();

            PDataTable = PDataTable.Create(Data, "PDATAs", pdataRefs);

            try {
                PDatasByMemoryAddress = PDataTable
                    .Select((PData, i) => new { PData, pdataAddresses[i].AddressInMemory })
                    .ToDictionary(x => x.AddressInMemory, x => x.PData);
                foreach (var pdata in PDatasByMemoryAddress)
                    pdata.Value.RamAddress = pdata.Key;
            }
            catch {
                // TODO: what to do on error??
                PDatasByMemoryAddress = new Dictionary<uint, PDataModel>();
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
                    .Select(x => new ModelElementKey { AddressInMemory = x.PolygonsOffset, Count = x.PolygonCount })
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
                    .Select(x => new ModelElementKey { AddressInMemory = x.AttributesOffset, Count = x.PolygonCount })
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

            var tables =
                new List<ITable>() {
                    ModelsHeaderTable,
                    ModelTable,
                    MovableModelTable,
                    PDataTable
                }
                .Where(x => x != null)
                .ToList();

            tables.AddRange(VertexTablesByMemoryAddress.Values);
            tables.AddRange(AttrTablesByMemoryAddress.Values);

            return tables;
        }

        public uint GetOffsetInChunk(uint memoryAddress) {
            if (MovableModelsIndex.HasValue)
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

        [BulkCopyRowName]
        public string Name { get; }
        public ScenarioType Scenario { get; }
        public ModelCollectionType CollectionType { get; }
        public int Address { get; }
        public int? ChunkIndex { get; }
        public int? MovableModelsIndex { get; }

        [BulkCopyRecurse]
        public ModelsHeaderTable ModelsHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public ModelTable ModelTable { get; private set; }

        [BulkCopyRecurse]
        public MovableModelTable MovableModelTable { get; private set; }

        [BulkCopyRecurse]
        public PDataTable PDataTable { get; private set; }

        public Dictionary<uint, PDataModel> PDatasByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, VertexTable> VertexTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, PolygonTable> PolygonTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, AttrTable> AttrTablesByMemoryAddress { get; private set; }
    }
}
