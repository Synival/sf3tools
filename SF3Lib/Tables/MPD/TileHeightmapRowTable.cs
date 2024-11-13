using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileHeightmapRowTable : Table<TileHeightmapRow> {
        public TileHeightmapRowTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            var size = new TileHeightmapRow(Editor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileHeightmapRow(Editor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
