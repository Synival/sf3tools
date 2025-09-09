using SF3.ByteData;
using SF3.Models.Structs.MPD.SurfaceModel;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class TileTextureRowTable : FixedSizeTable<TileTextureRow> {
        protected TileTextureRowTable(IByteData data, string name, int address, bool hasRotation) : base(data, name, address, 64) {
            HasRotation = hasRotation;
        }

        public static TileTextureRowTable Create(IByteData data, string name, int address, bool hasRotation)
            => Create(() => new TileTextureRowTable(data, name, address, hasRotation));

        public override bool Load() {
            return Load((id, address) => {
                // Ignore address; this table is in a special order:
                // [Y:16, X:16][Y:4, X:4]
                var block = id / 4;
                var y = id % 4;
                address = Address + (block * 256 + y * 4) * 2;
                return new TileTextureRow(Data, id, "Y" + id.ToString("D2"), address, HasRotation);
            });
        }

        public bool HasRotation { get; }

        public ushort[,] Make2DTextureData() {
            var textureData = new ushort[64, 64];
            for (var y = 0; y < textureData.GetLength(1); y++) {
                var tiles = Rows[y].GetRowCopy();
                for (var x = 0; x < textureData.GetLength(0); x++)
                    textureData[x, y] = tiles[x];
            }
            return textureData;
        }
    }
}
