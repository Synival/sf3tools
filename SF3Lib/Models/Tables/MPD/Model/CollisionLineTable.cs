using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLineTable : FixedSizeTable<CollisionLine> {
        protected CollisionLineTable(IByteData data, string name, int address, int size, CollisionPointTable pointTable)
        : base(data, name, address, size) {
            PointTable = pointTable;
        }

        public static CollisionLineTable Create(IByteData data, string name, int address, int size, CollisionPointTable pointTable)
            => Create(() => new CollisionLineTable(data, name, address, size, pointTable));

        public override bool Load()
            => Load((id, address) => new CollisionLine(Data, id, "Line" + id.ToString("D3"), address, PointTable));

        public CollisionPointTable PointTable { get; }
    }
}
