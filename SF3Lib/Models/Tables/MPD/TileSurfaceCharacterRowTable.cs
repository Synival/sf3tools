using System.Linq;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class TileSurfaceCharacterRowTable : Table<TileSurfaceCharacterRow> {
        public TileSurfaceCharacterRowTable(IRawData data, int address) : base(data, address) {
        }

        public override bool Load() {
            return LoadUntilMax((id, address) => {
                // Ignore address; this table is in a special order:
                // [Y:16, X:16][Y:4, X:4]
                var block = (63 - id) / 4;
                var y = (63 - id) % 4;
                address = Address + (block * 256 + y * 4) * 2;
                return new TileSurfaceCharacterRow(Data, id, "Y" + id.ToString("D2"), address);
            });
        }

        public override int? MaxSize => 64;

        public ushort[,] Make2DTextureData() {
            var textureData = new ushort[64, 64];
            for (var y = 0; y < textureData.GetLength(1); y++) {
                var tiles = Rows[y].Tiles;
                for (var x = 0; x < textureData.GetLength(0); x++)
                    textureData[x, y] = tiles[x];
            }
            return textureData;
        }
    }
}
