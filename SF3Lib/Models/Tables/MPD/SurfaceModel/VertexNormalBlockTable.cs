using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD.SurfaceModel;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class VertexNormalBlockTable : Table<VertexNormalBlock> {
        protected VertexNormalBlockTable(IByteData data, int address) : base(data, address) {
        }

        public static VertexNormalBlockTable Create(IByteData data, int address) {
            var newTable = new VertexNormalBlockTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address)
                => new VertexNormalBlock(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));

        public override int? MaxSize => 256;
    }
}
