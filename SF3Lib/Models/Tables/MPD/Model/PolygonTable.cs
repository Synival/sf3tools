using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class PolygonTable : FixedSizeTable<PolygonStruct> {
        protected PolygonTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static PolygonTable Create(IByteData data, string name, int address, int size)
            => Create(() => new PolygonTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new PolygonStruct(Data, id, "POLYGON" + id.ToString("D4"), address));
    }
}
