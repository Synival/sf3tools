using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileTerrainRowTable : Table<TileTerrainRow> {
        public TileTerrainRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load() {
            var size = new TileTerrainRow(FileEditor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileTerrainRow(FileEditor, id, "Y" + id, Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
