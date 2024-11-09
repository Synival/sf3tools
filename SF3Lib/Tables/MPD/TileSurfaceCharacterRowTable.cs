using SF3.FileEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileSurfaceCharacterRowTable : Table<TileSurfaceCharacterRow> {
        public TileSurfaceCharacterRowTable(IByteEditor fileEditor, int address) : base(fileEditor, address) {
        }

        public override bool Load() {
            return LoadUntilMax((id, address) => {
                // Ignore address; this table is in a special order:
                // [Y:16, X:16][Y:4, X:4]
                var block = id / 4;
                var y = id % 4;
                address = Address + ((block * 256) + y * 4) * 2;
                return new TileSurfaceCharacterRow(FileEditor, id, "Y" + id, address);
            });
        }

        public override int? MaxSize => 64;
    }
}
