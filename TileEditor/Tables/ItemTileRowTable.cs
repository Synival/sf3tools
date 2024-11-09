using SF3.FileEditors;
using SF3.Tables;
using SF3.TileEditor.Models;

namespace SF3.TileEditor.Tables {
    public class ItemTileRowTable : Table<ItemTileRow> {
        public ItemTileRowTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new ItemTileRow(FileEditor, id, name, address));

        public override int? MaxSize => 64;
    }
}
