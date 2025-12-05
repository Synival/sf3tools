using SF3.ByteData;
using SF3.Models.Structs.MPD.Plane;

namespace SF3.Models.Tables.MPD.Plane {
    public class PlaneTileTextureRowTable : FixedSizeTable<PlaneTileTextureRow> {
        protected PlaneTileTextureRowTable(IByteData data, string name, int address, int startY) : base(data, name, address, 64) {
            StartY = startY;
        }

        public static PlaneTileTextureRowTable Create(IByteData data, string name, int address, int startY)
            => Create(() => new PlaneTileTextureRowTable(data, name, address, startY));

        public override bool Load() {
            return Load((id, address) => {
                // Ignore address; this table is in a special order:
                // [Y:16, X:16][Y:4, X:4]
                var block = id / 4;
                var y = id % 4;
                address = Address + (block * 256 + y * 4) * 2;
                return new PlaneTileTextureRow(Data, id, "Y" + id.ToString("D2"), address);
            });
        }

        public int StartY { get; }

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
