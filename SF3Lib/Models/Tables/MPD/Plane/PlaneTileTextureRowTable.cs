using SF3.ByteData;
using SF3.Models.Structs.MPD.Plane;

namespace SF3.Models.Tables.MPD.Plane {
    public class PlaneTileTextureRowTable : FixedSizeTable<PlaneTileTextureRow> {
        protected PlaneTileTextureRowTable(IByteData data, string name, int address, int startY)
        : base(data, name, address, 128) {
            StartY = startY;
        }

        public static PlaneTileTextureRowTable Create(IByteData data, string name, int address, int startY)
            => Create(() => new PlaneTileTextureRowTable(data, name, address, startY));

        public override bool Load() {
            return Load((id, address) => {
                // Ignore address; this table is in a special order:
                //  Blocks    Tiles
                // [Y:4, X:2][Y:64, X:64]
                var block = id / 0x40;
                var y = id % 0x40;
                address = Address + (block * 0x4000 + y * 0x40) * 2;
                return new PlaneTileTextureRow(Data, id, "Y" + (id + StartY).ToString("D3"), address);
            });
        }

        public int StartY { get; }
    }
}
