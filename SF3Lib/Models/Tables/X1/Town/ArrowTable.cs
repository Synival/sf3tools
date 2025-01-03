using SF3.Models.Structs.X1.Town;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X1.Town {
    public class ArrowTable : Table<Arrow> {
        protected ArrowTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static ArrowTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new ArrowTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Arrow(Data, id, name, address),
                (rows, model) => model.ArrowUnknown0 != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
