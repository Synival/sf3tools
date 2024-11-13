using SF3.StreamEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class WeaponSpellTable : Table<WeaponSpell> {
        public WeaponSpellTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpell(FileEditor, id, name, address));

        public override int? MaxSize => 31;
    }
}
