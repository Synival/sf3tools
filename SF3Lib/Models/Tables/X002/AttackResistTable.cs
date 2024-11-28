using SF3.Models.Structs.X002;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X002 {
    public class AttackResistTable : Table<AttackResist> {
        public AttackResistTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new AttackResist(Editor, id, name, address));

        public override int? MaxSize => 2;
    }
}
