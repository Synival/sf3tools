using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileTerrainRowTable : Table<TileTerrainRow> {
        public TileTerrainRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new TileTerrainRow(FileEditor, id, "Y" + id, address));

        public override int? MaxSize => 64;
    }
}
