using SF3.Models.Structs.MPD;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.MPD.SurfaceModel {
    public class VertexNormalMeshBlockTable : Table<TileSurfaceVertexNormalMesh> {
        protected VertexNormalMeshBlockTable(IByteData data, int address) : base(data, address) {
        }

        public static VertexNormalMeshBlockTable Create(IByteData data, int address) {
            var newTable = new VertexNormalMeshBlockTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address)
                => new TileSurfaceVertexNormalMesh(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));

        public override int? MaxSize => 256;
    }
}
