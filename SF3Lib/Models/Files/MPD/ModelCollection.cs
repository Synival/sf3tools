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
        protected ModelCollection(IByteData data, INameGetterContext nameContext, int address, string name, TextureCollectionType textureCollection, int? chunkIndex)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            TextureCollection = textureCollection;
            ChunkIndex = chunkIndex;
        }

        public static ModelCollection Create(IByteData data, INameGetterContext nameContext, int address, string name, TextureCollectionType textureCollection, int? chunkIndex) {
            var newFile = new ModelCollection(data, nameContext, address, name, textureCollection, chunkIndex);
            newFile.Init();
            return newFile;
        }

        private struct ModelElementKey {
            public int AddressInMemory;
            public int Count;
            public int Refs;
        }

        public override IEnumerable<ITable> MakeTables() {
            ModelsHeaderTable = ModelsHeaderTable.Create(Data, "ModelsHeader", 0x0000);
            ModelTable = ModelTable.Create(Data, "Models", 0x000C, ModelsHeaderTable[0].NumModels);

            var pdataAddresses = ModelTable
                .SelectMany(x => x.PDatas)
                .Select(x => x.Value)
                .Where(x => x != 0)
                .GroupBy(x => x)
                .Select(x => new { AddressInMemory = x.Key, Count = x.Count() })
                .OrderBy(x => x.AddressInMemory)
                .ToArray();

            PDataTable = PDataTable.Create(Data, "PDATAs", pdataAddresses.Select(x => GetOffsetInChunk(x.AddressInMemory)).ToArray(), pdataAddresses.Select(x => x.Count).ToArray());
            PDatasByMemoryAddress = PDataTable
                .Select((PData, i) => new { PData, pdataAddresses[i].AddressInMemory })
                .ToDictionary(x => x.AddressInMemory, x => x.PData);

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
                    x => VertexTable.Create(Data, "POINTs @ 0x" + x.AddressInMemory.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", GetOffsetInChunk(x.AddressInMemory), x.Count)
                );

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
                    x => PolygonTable.Create(Data, "POLYGONs @ 0x" + x.AddressInMemory.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", GetOffsetInChunk(x.AddressInMemory), x.Count)
                );

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
                    x => AttrTable.Create(Data, "ATTRs @ 0x" + x.AddressInMemory.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", GetOffsetInChunk(x.AddressInMemory), x.Count)
                );

            var tables = new List<ITable>() {
                ModelsHeaderTable,
                ModelTable,
                PDataTable
            };

            tables.AddRange(VertexTablesByMemoryAddress.Values);
            tables.AddRange(AttrTablesByMemoryAddress.Values);

            return tables;
        }

        public int GetOffsetInChunk(int memoryAddress) {
            if (memoryAddress >= 0x60a0000)
                return memoryAddress - 0x60a0000 /* TODO: apply actual offset of chunk! */;
            else if (memoryAddress >= 0x290000)
                return memoryAddress - 0x292100 /* TODO: apply actual offset of chunk! */;
            else
                return memoryAddress;
        }

        [BulkCopyRowName]
        public string Name { get; }
        public TextureCollectionType TextureCollection { get; }
        public int Address { get; }
        public int? ChunkIndex { get; }

        [BulkCopyRecurse]
        public ModelsHeaderTable ModelsHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public ModelTable ModelTable { get; private set; }

        [BulkCopyRecurse]
        public PDataTable PDataTable { get; private set; }

        public Dictionary<int, PDataModel> PDatasByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<int, VertexTable> VertexTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<int, PolygonTable> PolygonTablesByMemoryAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<int, AttrTable> AttrTablesByMemoryAddress { get; private set; }
    }
}
