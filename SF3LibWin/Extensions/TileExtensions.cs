using System.Drawing;
using System.Linq;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Extensions {
    public static class TileExtensions {
        public static Vector3[] GetSurfaceModelVertices(this Tile tile) {
            var heights = tile.GetSurfaceModelVertexHeights();
            return [
                (tile.X + 0 + WorldResources.ModelOffsetX, heights[0], tile.Y + 0 + WorldResources.ModelOffsetZ),
                (tile.X + 1 + WorldResources.ModelOffsetX, heights[1], tile.Y + 0 + WorldResources.ModelOffsetZ),
                (tile.X + 1 + WorldResources.ModelOffsetX, heights[2], tile.Y + 1 + WorldResources.ModelOffsetZ),
                (tile.X + 0 + WorldResources.ModelOffsetX, heights[3], tile.Y + 1 + WorldResources.ModelOffsetZ)
            ];
        }

        public static Vector3 GetVertex3Abnormal(this Tile tile, CornerType corner)
            => tile.GetVertexAbnormal(corner).ToVector3();

        public static Vector3[] GetVertex3Abnormals(this Tile tile)
            => tile.GetVertexAbnormals().Select(x => x.ToVector3()).ToArray();
    }
}