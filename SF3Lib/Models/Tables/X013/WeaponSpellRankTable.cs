using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class WeaponSpellRankTable : ResourceTable<WeaponSpellRank> {
        protected WeaponSpellRankTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 4) {
        }

        public static WeaponSpellRankTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new WeaponSpellRankTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new WeaponSpellRank(Data, id, name, address));
    }
}
