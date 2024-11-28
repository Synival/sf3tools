using SF3.RawEditors;
using SF3.Models.X002;

namespace SF3.TableModels.X002 {
    public class WeaponSpellTable : Table<WeaponSpell> {
        public WeaponSpellTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpell(Editor, id, name, address));

        public override int? MaxSize => 31;
    }
}
