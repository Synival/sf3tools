using SF3.ByteData;
using SF3.Models.Structs.MPD.Model;

namespace SF3.Models.Tables.MPD.Model {
    public class VertexTable : FixedSizeTable<VertexModel> {
        protected VertexTable(IByteData data, string name, int address, int size) : base(data, name, address, size) {
        }

        public static VertexTable Create(IByteData data, string name, int address, int size)
            => CreateBase(() => new VertexTable(data, name, address, size));

        public override bool Load()
            => Load((id, address) => new VertexModel(Data, id, "VERTEX" + id.ToString("D4"), address));
    }
}
