using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileTerrainRowTable : Table<TileTerrainRow> {
        public TileTerrainRowTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            var size = new TileTerrainRow(FileEditor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileTerrainRow(FileEditor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
