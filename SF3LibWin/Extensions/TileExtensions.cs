using System.Drawing;
using System.Linq;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Extensions {
    public static class TileExtensions {
        public static Vector3[] GetSurfaceModelVertices(this Tile tile, float scale = 1.00f) {
            var xzOff = scale * 0.5f;

            var heights = tile.GetSurfaceModelVertexHeights();
            if (scale != 1.00f) {
                // The center height is just an average between these two heighs (not all four)
                // because of the way quads are drawn as two triangles.
                var centerHeight = (heights[0] + heights[2]) / 2;
                for (var i = 0; i < 4; i++)
                    heights[i] = (heights[i] - centerHeight) * scale + centerHeight;
            }

            return [
                (tile.X - xzOff + WorldResources.ModelOffsetX, heights[0], -(tile.Y - xzOff + WorldResources.ModelOffsetZ)),
                (tile.X + xzOff + WorldResources.ModelOffsetX, heights[1], -(tile.Y - xzOff + WorldResources.ModelOffsetZ)),
                (tile.X + xzOff + WorldResources.ModelOffsetX, heights[2], -(tile.Y + xzOff + WorldResources.ModelOffsetZ)),
                (tile.X - xzOff + WorldResources.ModelOffsetX, heights[3], -(tile.Y + xzOff + WorldResources.ModelOffsetZ))
            ];
        }

        public static Vector3 GetVertex3Abnormal(this Tile tile, CornerType corner)
            => tile.GetVertexAbnormal(corner).ToVector3();

        public static Vector3[] GetVertex3Abnormals(this Tile tile)
            => tile.GetVertexAbnormals().Select(x => x.ToVector3()).ToArray();
    }
}