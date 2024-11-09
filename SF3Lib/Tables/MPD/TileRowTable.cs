using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileRowTable : Table<TileRow> {
        public TileRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new TileRow(FileEditor, id, "Y" + (63 - id), address));

        public override int? MaxSize => 64;
    }
}
