using System.Linq;
using SF3.RawEditors;
using SF3.Models.MPD;

namespace SF3.Tables.MPD {
    public class TileSurfaceCharacterRowTable : Table<TileSurfaceCharacterRow> {
        public TileSurfaceCharacterRowTable(IRawEditor editor, int address) : base(editor, address) {
        }

        public override bool Load() {
            return LoadUntilMax((id, address) => {
                // Ignore address; this table is in a special order:
                // [Y:16, X:16][Y:4, X:4]
                var block = (63 - id) / 4;
                var y = (63 - id) % 4;
                address = Address + ((block * 256) + y * 4) * 2;
                return new TileSurfaceCharacterRow(Editor, id, "Y" + id.ToString("D2"), address);
            });
        }

        public override int? MaxSize => 64;

        public ushort[,] Make2DTextureData() {
            var textureData = new ushort[64, 64];
            for (int y = 0; y < textureData.GetLength(1); y++) {
                var tiles = Rows[y].Tiles;
                for (int x = 0; x < textureData.GetLength(0); x++)
                    textureData[x, y] = tiles[x];
            }
            return textureData;
        }
    }
}
