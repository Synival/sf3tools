using SF3.RawEditors;
using SF3.Models.X002;

namespace SF3.Tables.X002 {
    public class ItemTable : Table<Item> {
        public ItemTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Item(FileEditor, id, name, address));

        public override int? MaxSize => 300;
    }
}
