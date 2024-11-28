using SF3.RawEditors;
using SF3.Structs.X002;

namespace SF3.TableModels.X002 {
    public class ItemTable : Table<Item> {
        public ItemTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Item(Editor, id, name, address));

        public override int? MaxSize => 300;
    }
}
