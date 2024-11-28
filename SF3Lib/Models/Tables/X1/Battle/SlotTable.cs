using SF3.Models.Structs.X1.Battle;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.X1.Battle {
    public class SlotTable : Table<Slot> {
        public SlotTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Slot(Editor, id, name, address));

        public override int? MaxSize => 256;

    }
}
