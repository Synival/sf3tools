using SF3.RawEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class AttackResistTable : Table<AttackResist> {
        public AttackResistTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AttackResist(Editor, id, name, address));

        public override int? MaxSize => 2;
    }
}
