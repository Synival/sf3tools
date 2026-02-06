using System.Collections.Generic;
using CommonLib.Types;
using CommonLib.Utils;

namespace SF3.Models.Files.MPD {
    public partial class SurfaceTile {
        private void UpdateVertexNormals(CornerType corner, out HashSet<SurfaceTile> tilesModified) {
            var surfaceModel = MPD_File.SurfaceModelChunk;
            if (surfaceModel == null) {
                tilesModified = new HashSet<SurfaceTile>();
                return;
            }

            var vxCenter = BlockHelpers.TileToVertexX(X, corner);
            var vyCenter = BlockHelpers.TileToVertexY(Y, corner);

            // Normals need to be updated in a 3x3 grid.
            for (var x = -1; x <= 1; x++) {
                for (var y = -1; y <= 1; y++) {
                    var vx = x + vxCenter;
                    var vy = y + vyCenter;
                    if (vx >= 0 && vy >= 0 && vx < 65 && vy < 65)
                        surfaceModel.UpdateVertexNormal(vx, vy, Surface);
                }
            }

            // Updating vertex normals in a 3x3 grid means tiles need to be re-rendered in a 4x4 grid.
            tilesModified = new HashSet<SurfaceTile>();
            for (var x = -2; x <= 1; x++) {
                for (var y = -2; y <= 1; y++) {
                    var tx = x + vxCenter;
                    var ty = y + vyCenter;
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64) {
                        var tile = MPD_File.Surface.GetTile(tx, ty) as SurfaceTile;
                        tilesModified.Add(tile);
                    }
                }
            }
        }
    }
}
