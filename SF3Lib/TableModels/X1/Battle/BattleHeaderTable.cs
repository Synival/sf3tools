using SF3.RawEditors;
using SF3.Structs.X1.Battle;

namespace SF3.TableModels.X1.Battle {
    public class BattleHeaderTable : Table<BattleHeader> {
        public BattleHeaderTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new BattleHeader(Editor, id, name, address));

        public override int? MaxSize => 31;
    }
}
