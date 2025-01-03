using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X013 {
    public class ExpLimitTable : Table<ExpLimit> {
        protected ExpLimitTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static ExpLimitTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new ExpLimitTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ExpLimit(Data, id, name, address));

        public override int? MaxSize => 2;
    }
}
