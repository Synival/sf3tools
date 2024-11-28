using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawEditors;

namespace SF3.Models.Tables.MPD {
    public class TileItemRowTable : Table<TileItemRow> {
        public TileItemRowTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            var size = new TileItemRow(Editor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileItemRow(Editor, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
