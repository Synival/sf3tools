using SF3.Models.Structs.X013;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X013 {
    public class WeaponSpellRankTable : Table<WeaponSpellRank> {
        public WeaponSpellRankTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new WeaponSpellRank(Editor, id, name, address));

        public override int? MaxSize => 4;
    }
}
