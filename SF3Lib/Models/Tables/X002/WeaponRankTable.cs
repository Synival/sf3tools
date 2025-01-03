using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X002 {
    public class WeaponRankTable : Table<WeaponRank> {
        protected WeaponRankTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static WeaponRankTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponRankTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponRank(Data, id, name, address));

        public override int? MaxSize => 5;
    }
}
