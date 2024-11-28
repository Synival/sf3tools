using SF3.RawEditors;
using SF3.Structs.X033_X031;

namespace SF3.TableModels.X033_X031 {
    public class WeaponLevelTable : Table<WeaponLevel> {

        public WeaponLevelTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponLevel(Editor, id, name, address));

        public override int? MaxSize => 2;
    }
}
