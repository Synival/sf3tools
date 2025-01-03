using SF3.Models.Structs.MPD;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class VertexHeightMeshBlockTable : Table<TileSurfaceVertexHeightMesh> {
        protected VertexHeightMeshBlockTable(IByteData data, int address) : base(data, address) {
        }

        public static VertexHeightMeshBlockTable Create(IByteData data, int address) {
            var newTable = new VertexHeightMeshBlockTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address)
                => new TileSurfaceVertexHeightMesh(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));

        public override int? MaxSize => 256;
    }
}
