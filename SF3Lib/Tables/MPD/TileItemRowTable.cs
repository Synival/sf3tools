using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileItemRowTable : Table<TileItemRow> {
        public TileItemRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load() {
            var size = new TileItemRow(FileEditor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileItemRow(FileEditor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
