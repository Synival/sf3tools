using System.Collections.Generic;
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
            ModelsHeaderTable = ModelsHeaderTable.Create(Data, 0x0000);
            return new List<ITable>() {
                ModelsHeaderTable,
                (ModelTable = ModelTable.Create(Data, 0x000C, ModelsHeaderTable.Rows[0].NumModels)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }

        [BulkCopyRecurse]
        public ModelsHeaderTable ModelsHeaderTable { get; private set; }

        [BulkCopyRecurse]
        public ModelTable ModelTable { get; private set; }
    }
}
