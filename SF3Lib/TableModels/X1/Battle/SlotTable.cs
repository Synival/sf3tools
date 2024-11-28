using SF3.RawEditors;
using SF3.Structs.X1.Battle;

namespace SF3.TableModels.X1.Battle {
    public class SlotTable : Table<Slot> {
        public SlotTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Slot(Editor, id, name, address));

        public override int? MaxSize => 256;

    }
}
