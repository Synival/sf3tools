using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class ItemTileRowTable : Table<ItemTileRow> {
        public ItemTileRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new ItemTileRow(FileEditor, id, "Y" + (63 - id), address));

        public override int? MaxSize => 64;
    }
}
