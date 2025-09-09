using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionPointTable : FixedSizeTable<CollisionPoint> {
        protected CollisionPointTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static CollisionPointTable Create(IByteData data, string name, int address, int size)
            => Create(() => new CollisionPointTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new CollisionPoint(Data, id, "Point" + id.ToString("D3"), address));
    }
}
