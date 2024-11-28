using SF3.RawEditors;
using SF3.Structs.MPD;

namespace SF3.TableModels.MPD {
    public class TileSurfaceHeightmapRowTable : Table<TileSurfaceHeightmapRow> {
        public TileSurfaceHeightmapRowTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            var size = new TileSurfaceHeightmapRow(Editor, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new TileSurfaceHeightmapRow(Editor, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;
    }
}
