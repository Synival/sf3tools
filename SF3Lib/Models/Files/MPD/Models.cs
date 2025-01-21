using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Models.Files.MPD {
    public class Models : TableFile {
        protected Models(IByteData data, INameGetterContext nameContext, int address, string name)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
        }

        public static Models Create(IByteData data, INameGetterContext nameContext, int address, string name) {
            var newFile = new Models(data, nameContext, address, name);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            ModelsHeaderTable = ModelsHeaderTable.Create(Data, "ModelsHeader", 0x0000);
            ModelTable = ModelTable.Create(Data, "Models", 0x000C, ModelsHeaderTable[0].NumModels);

            int GetFileAddr(int addr) {
                if (addr >= 0x60a0000)
                    return addr - 0x60a0000 /* TODO: apply actual offset of chunk! */;
                else if (addr >= 0x290000)
                    return addr - 0x292100 /* TODO: apply actual offset of chunk! */;
                else
                    return addr;
            }

            var pdataAddresses = ModelTable
                .SelectMany(x => x.PDatas)
                .Select(x => x.Value)
                .Where(x => x != 0)
                .Select(x => GetFileAddr(x))
                .GroupBy(x => x)
                .Select(x => new { Offset = x.Key, Count = x.Count() })
                .OrderBy(x => x.Offset)
                .ToArray();

            PDataTable = PDataTable.Create(Data, "PDATAs", pdataAddresses.Select(x => x.Offset).ToArray(), pdataAddresses.Select(x => x.Count).ToArray());

            VertexTables = PDataTable
                .Select(x => new OffsetCount { Offset = GetFileAddr(x.VerticesOffset), Count = x.VertexCount })
                .Where(x => x.Offset != 0)
                .GroupBy(x => x.GetHashCode())
                .Select(x => {
                    var first = x.First();
                    first.Refs = x.Count();
                    return first;
                })
                .OrderBy(x => x.Offset)
                .ThenBy(x => x.Count)
                .ToDictionary(
                    x => x,
                    x => VertexTable.Create(Data, "POINT[] @ 0x" + x.Offset.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", x.Offset, x.Count)
                );

            PolygonTables = PDataTable
                .Select(x => new OffsetCount { Offset = GetFileAddr(x.PolygonsOffset), Count = x.PolygonCount })
                .Where(x => x.Offset != 0)
                .GroupBy(x => x.GetHashCode())
                .Select(x => {
                    var first = x.First();
                    first.Refs = x.Count();
                    return first;
                })
                .OrderBy(x => x.Offset)
                .ThenBy(x => x.Count)
                .ToDictionary(
                    x => x,
                    x => PolygonTable.Create(Data, "POLYGON[] @ 0x" + x.Offset.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", x.Offset, x.Count)
                );

            AttrTables = PDataTable
                .Select(x => new OffsetCount { Offset = GetFileAddr(x.AttributesOffset), Count = x.PolygonCount })
                .Where(x => x.Offset != 0)
                .GroupBy(x => x.GetHashCode())
                .Select(x => {
                    var first = x.First();
                    first.Refs = x.Count();
                    return first;
                })
                .OrderBy(x => x.Offset)
                .ThenBy(x => x.Count)
                .ToDictionary(
                    x => x,
                    x => AttrTable.Create(Data, "ATTR[] @ 0x" + x.Offset.ToString("X") + " (Count=" + x.Count + ", Refs=" + x.Refs + ")", x.Offset, x.Count)
                );

            var tables = new List<ITable>() {
                ModelsHeaderTable,
                ModelTable,
                PDataTable
            };

            tables.AddRange(VertexTables.Values);
            tables.AddRange(AttrTables.Values);

            return tables;
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }

        [BulkCopyRecurse]
        public ModelsHeaderTable ModelsHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public ModelTable ModelTable { get; private set; }

        [BulkCopyRecurse]
        public PDataTable PDataTable { get; private set; }

        public struct OffsetCount {
            public int Offset;
            public int Count;
            public int Refs;

            public override int GetHashCode()
                => Offset * 0x10000 + Count;

            public override bool Equals(object rhs)
                => (rhs is OffsetCount oc) ? (Offset == oc.Offset && Count == oc.Count) : base.Equals(rhs);

            public override string ToString()
                => "0x" + Offset.ToString("X4") + " (Count = " + Count + ") (Refs = " + Refs + ")";
        }

        [BulkCopyRecurse]
        public Dictionary<OffsetCount, VertexTable> VertexTables { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<OffsetCount, PolygonTable> PolygonTables { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<OffsetCount, AttrTable> AttrTables { get; private set; }
    }
}
