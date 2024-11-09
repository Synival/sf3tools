using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileHeightRowTable : Table<TileHeightRow> {
        public TileHeightRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load() {
            var size = new TileHeightRow(FileEditor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileHeightRow(FileEditor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
