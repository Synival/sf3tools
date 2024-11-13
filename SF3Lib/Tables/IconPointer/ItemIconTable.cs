using SF3.RawEditors;
using SF3.Models.IconPointer;

namespace SF3.Tables.IconPointer {
    public class ItemIconTable : Table<ItemIcon> {
        public ItemIconTable(IRawEditor fileEditor, string resourceFile, int address, bool has16BitIconAddr) : base(fileEditor, resourceFile, address) {
            Has16BitIconAddr = has16BitIconAddr;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ItemIcon(FileEditor, id, name, address, Has16BitIconAddr));

        public override int? MaxSize => 300;

        public bool Has16BitIconAddr { get; }
    }
}
