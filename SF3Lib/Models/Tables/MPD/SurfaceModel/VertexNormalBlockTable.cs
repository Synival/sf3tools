using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.SurfaceModel;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class VertexNormalBlockTable : FixedSizeTable<VertexNormalBlock> {
        protected VertexNormalBlockTable(IByteData data, int address) : base(data, address, 256) {
        }

        public static VertexNormalBlockTable Create(IByteData data, int address) {
            var newTable = new VertexNormalBlockTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address)
                => new VertexNormalBlock(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));
    }
}
