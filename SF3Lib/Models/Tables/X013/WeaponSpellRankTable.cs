using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X013 {
    public class WeaponSpellRankTable : Table<WeaponSpellRank> {
        protected WeaponSpellRankTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static WeaponSpellRankTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponSpellRankTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpellRank(Data, id, name, address));

        public override int? MaxSize => 4;
    }
}
