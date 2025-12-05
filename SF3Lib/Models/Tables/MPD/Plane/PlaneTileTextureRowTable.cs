using SF3.ByteData;
using SF3.Models.Structs.MPD.Plane;

namespace SF3.Models.Tables.MPD.Plane {
    public class PlaneTileTextureRowTable : FixedSizeTable<PlaneTileTextureRow> {
        protected PlaneTileTextureRowTable(IByteData data, string name, int address, int startY, int blockCountX, int rowCount)
        : base(data, name, address, rowCount) {
            StartY      = startY;
            BlockCountX = blockCountX;
            Width       = BlockCountX * 64;
            Height      = Size;
        }

        public static PlaneTileTextureRowTable Create(IByteData data, string name, int address, int startY, int blockCountX, int rowCount)
            => Create(() => new PlaneTileTextureRowTable(data, name, address, startY, blockCountX, rowCount));

        public override bool Load() {
            return Load((id, address) => {
                // Ignore address; this table is in a special order:
                //  Blocks    Tiles
                // [Y:4, X:2][Y:64, X:64]
                var block = id / 0x40;
                var y = id % 0x40;
                address = Address + (block * 0x4000 + y * 0x40) * 2;
                return
                    (BlockCountX == 4) ? new PlaneTileTextureRow256(Data, id, "Y" + (id + StartY).ToString("D3"), address) :
                    (BlockCountX == 1) ? new PlaneTileTextureRow64 (Data, id, "Y" + (id + StartY).ToString("D3"), address) :
                                         new PlaneTileTextureRow   (Data, id, "Y" + (id + StartY).ToString("D3"), address, BlockCountX);
            });
        }

        public int StartY { get; }
        public int BlockCountX { get; }

        public int Width { get; }
        public int Height { get; }

        public ushort this[int x, int y] {
            get => this[y][x];
            set => this[y][x] = value;
        }
    }
}
