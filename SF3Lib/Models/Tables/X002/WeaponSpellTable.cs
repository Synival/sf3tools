using SF3.ByteData;
using SF3.Models.Structs.X002;

namespace SF3.Models.Tables.X002 {
    public class WeaponSpellTable : ResourceTable<WeaponSpell> {
        protected WeaponSpellTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 31) {
        }

        public static WeaponSpellTable Create(IByteData data, string name, string resourceFile, int address)
            => CreateBase(() => new WeaponSpellTable(data, name, resourceFile, address));

        public override bool Load()
            => Load((id, name, address) => new WeaponSpell(Data, id, name, address));
    }
}
