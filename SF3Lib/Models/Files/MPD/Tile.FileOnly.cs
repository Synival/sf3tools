using System;
using CommonLib.Types;

namespace SF3.Models.Files.MPD {
    public partial class Tile {
        public float GetSurfaceDataVertexHeight(CornerType corner)
            => MPD_File.SurfaceData?.HeightmapRowTable?[Y]?.GetHeight(X, corner) ?? 0.0f;
        public float GetSurfaceModelVertexHeight(CornerType corner) {
            var bl = _blockVertexLocations[corner];
            return ((MPD_File.SurfaceModel?.VertexHeightBlockTable?[bl.Num]?[bl.X, bl.Y]) ?? 0) / 16f;
        }
    }
}
