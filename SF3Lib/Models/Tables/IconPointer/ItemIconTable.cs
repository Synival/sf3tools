using SF3.Models.Structs.IconPointer;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.IconPointer {
    public class ItemIconTable : Table<ItemIcon> {
        public ItemIconTable(IRawEditor editor, string resourceFile, int address, bool has16BitIconAddr) : base(editor, resourceFile, address) {
            Has16BitIconAddr = has16BitIconAddr;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ItemIcon(Editor, id, name, address, Has16BitIconAddr));

        public override int? MaxSize => 300;

        public bool Has16BitIconAddr { get; }
    }
}
