using System.Linq;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.OpenGL.MPD_File;
using static CommonLib.Types.CornerTypeConsts;

namespace SF3.Win.Extensions {
    public static class TileExtensions {
        public static Vector3[] GetVector3Vertices(this Tile tile, float scale = 1.00f) {
            var xzOff = scale * 0.5f;

            var heights = tile.GetVisualVertexHeights();
            if (scale != 1.00f) {
                var centerHeight = heights.Average();
                for (var i = 0; i < 4; i++)
                    heights[i] = (heights[i] - centerHeight) * scale + centerHeight;
            }

            const int corner1X = (Corner1X * 2) - 1;
            const int corner2X = (Corner2X * 2) - 1;
            const int corner3X = (Corner3X * 2) - 1;
            const int corner4X = (Corner4X * 2) - 1;

            const int corner1Z = (Corner1Z * 2) - 1;
            const int corner2Z = (Corner2Z * 2) - 1;
            const int corner3Z = (Corner3Z * 2) - 1;
            const int corner4Z = (Corner4Z * 2) - 1;

            const float modelOffX = GeneralResources.ModelOffsetX + 0.5f;
            const float modelOffZ = GeneralResources.ModelOffsetZ + 0.5f;

            return [
                (tile.X + (xzOff * corner1X) + modelOffX, heights[0], (63 - tile.Y) + (xzOff * corner1Z) + modelOffZ),
                (tile.X + (xzOff * corner2X) + modelOffX, heights[1], (63 - tile.Y) + (xzOff * corner2Z) + modelOffZ),
                (tile.X + (xzOff * corner3X) + modelOffX, heights[2], (63 - tile.Y) + (xzOff * corner3Z) + modelOffZ),
                (tile.X + (xzOff * corner4X) + modelOffX, heights[3], (63 - tile.Y) + (xzOff * corner4Z) + modelOffZ),
            ];
        }

        public static Vector3 GetVector3Normal(this Tile tile, CornerType corner)
            => tile.GetVertexNormal(corner).ToVector3();

        public static Vector3[] GetVector3Normals(this Tile tile)
            => tile.GetVertexNormals().Select(x => x.ToVector3()).ToArray();
    }
}