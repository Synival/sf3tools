using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.ByteData;

namespace SF3.Models.Tables.X002 {
    public class WeaponSpellTable : Table<WeaponSpell> {
        protected WeaponSpellTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static WeaponSpellTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new WeaponSpellTable(data, resourceFile, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpell(Data, id, name, address));

        public override int? MaxSize => 31;
    }
}
