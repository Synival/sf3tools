using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X002 {
    public class StatBoostTable : Table<StatBoost> {
        protected StatBoostTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static StatBoostTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new StatBoostTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new StatBoost(Data, id, name, address));

        public override int? MaxSize => 300;
    }
}
