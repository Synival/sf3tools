using SF3.ByteData;
using SF3.Models.Structs.MPD.SurfaceModel;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class VertexHeightBlockTable : FixedSizeTable<VertexHeightBlock> {
        protected VertexHeightBlockTable(IByteData data, string name, int address) : base(data, name, address, 256) {
        }

        public static VertexHeightBlockTable Create(IByteData data, string name, int address)
            => CreateBase(() => new VertexHeightBlockTable(data, name, address));

        public override bool Load()
            => Load((id, address)
                => new VertexHeightBlock(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));
    }
}
