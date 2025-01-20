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

        public override IEnumerable<IBaseTable> MakeTables() {
            ModelsHeaderTable = ModelsHeaderTable.Create(Data, 0x0000);
            ModelTable = ModelTable.Create(Data, 0x000C, ModelsHeaderTable[0].NumModels);

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
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            PDataTable = PDataTable.Create(Data, pdataAddresses);

            var verticesAddresses = PDataTable
                .Select(x => x.VerticesOffset)
                .Where(x => x != 0)
                .Select(x => GetFileAddr(x))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            VertexTable = VertexTable.Create(Data, verticesAddresses);

            var attrAddresses = PDataTable
                .Select(x => x.AttributesOffset)
                .Where(x => x != 0)
                .Select(x => GetFileAddr(x))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            AttrTable = AttrTable.Create(Data, attrAddresses);

            return new List<IBaseTable>() {
                ModelsHeaderTable,
                ModelTable,
                PDataTable,
                VertexTable,
                AttrTable
            };
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

        [BulkCopyRecurse]
        public VertexTable VertexTable { get; private set; }

        [BulkCopyRecurse]
        public AttrTable AttrTable { get; private set; }
    }
}
