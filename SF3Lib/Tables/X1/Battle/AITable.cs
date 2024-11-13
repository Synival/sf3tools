using SF3.RawEditors;
using SF3.Models.X1.Battle;

namespace SF3.Tables.X1.Battle {
    public class AITable : Table<AI> {
        public AITable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AI(FileEditor, id, name, address));

        public override int? MaxSize => 130;
    }
}
