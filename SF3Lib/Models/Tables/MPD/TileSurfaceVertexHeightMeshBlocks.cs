using SF3.Models.Structs.MPD;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class TileSurfaceVertexHeightMeshBlocks : Table<TileSurfaceVertexHeightMesh> {
        protected TileSurfaceVertexHeightMeshBlocks(IByteData data, int address) : base(data, address) {
        }

        public static TileSurfaceVertexHeightMeshBlocks Create(IByteData data, int address) {
            var newTable = new TileSurfaceVertexHeightMeshBlocks(data, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address)
                => new TileSurfaceVertexHeightMesh(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));

        public override int? MaxSize => 256;
    }
}
