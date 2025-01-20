using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.SurfaceModel;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class VertexHeightBlockTable : FixedSizeTable<VertexHeightBlock> {
        protected VertexHeightBlockTable(IByteData data, int address) : base(data, address, 256) {
        }

        public static VertexHeightBlockTable Create(IByteData data, int address) {
            var newTable = new VertexHeightBlockTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address)
                => new VertexHeightBlock(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));
    }
}
