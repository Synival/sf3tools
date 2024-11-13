using SF3.StreamEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileHeightmapRowTable : Table<TileHeightmapRow> {
        public TileHeightmapRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load() {
            var size = new TileHeightmapRow(FileEditor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileHeightmapRow(FileEditor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
