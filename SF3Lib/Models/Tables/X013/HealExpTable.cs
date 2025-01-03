using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X013 {
    public class HealExpTable : Table<HealExp> {
        protected HealExpTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static HealExpTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new HealExpTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new HealExp(Data, id, name, address));

        public override int? MaxSize => 2;
    }
}
