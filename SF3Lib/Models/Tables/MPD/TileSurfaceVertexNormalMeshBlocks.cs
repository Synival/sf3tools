using SF3.Models.Structs.MPD;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class TileSurfaceVertexNormalMeshBlocks : Table<TileSurfaceVertexNormalMesh> {
        public TileSurfaceVertexNormalMeshBlocks(IByteData data, int address) : base(data, address) { }

        public override bool Load()
            => LoadUntilMax((id, address)
                => new TileSurfaceVertexNormalMesh(Data, id, "Block" + id.ToString("D2"), address, id % 16, id / 16));

        public override int? MaxSize => 256;
    }
}
