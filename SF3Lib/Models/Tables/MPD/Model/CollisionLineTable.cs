using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class CollisionLineTable : FixedSizeTable<CollisionLine> {
        protected CollisionLineTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static CollisionLineTable Create(IByteData data, string name, int address, int size)
            => Create(() => new CollisionLineTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new CollisionLine(Data, id, "Line" + id.ToString("D3"), address));
    }
}
