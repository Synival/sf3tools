using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileHeightRowTable : Table<TileHeightRow> {
        public TileHeightRowTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            var size = new TileHeightRow(FileEditor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileHeightRow(FileEditor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
