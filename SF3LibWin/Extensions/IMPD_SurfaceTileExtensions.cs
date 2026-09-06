using System.Linq;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Win.OpenGL.GLResources.MPD;
using static CommonLib.Types.CornerTypeConsts;

namespace SF3.Win.Extensions {
    public static class IMPD_SurfaceTileExtensions {
        public static Vector3[] GetVector3Vertices(this IMPD_SurfaceTile tile) {
            const float xzOff = 16f;

            const int corner1X = (Corner1X * 2) - 1;
            const int corner2X = (Corner2X * 2) - 1;
            const int corner3X = (Corner3X * 2) - 1;
            const int corner4X = (Corner4X * 2) - 1;

            const int corner1Z = (Corner1Z * 2) - 1;
            const int corner2Z = (Corner2Z * 2) - 1;
            const int corner3Z = (Corner3Z * 2) - 1;
            const int corner4Z = (Corner4Z * 2) - 1;

            const float modelOffX = MPD_ModelResources.ModelOffsetX + xzOff;
            const float modelOffZ = MPD_ModelResources.ModelOffsetZ + xzOff;

            var heights = tile.GetVertexHeights().Select(x => x * 2.0f).ToArray();

            return [
                ((tile.X * 32.0f + (xzOff * corner1X)) + modelOffX, heights[0], ((63 - tile.Y) * 32.0f + (xzOff * corner1Z)) + modelOffZ),
                ((tile.X * 32.0f + (xzOff * corner2X)) + modelOffX, heights[1], ((63 - tile.Y) * 32.0f + (xzOff * corner2Z)) + modelOffZ),
                ((tile.X * 32.0f + (xzOff * corner3X)) + modelOffX, heights[2], ((63 - tile.Y) * 32.0f + (xzOff * corner3Z)) + modelOffZ),
                ((tile.X * 32.0f + (xzOff * corner4X)) + modelOffX, heights[3], ((63 - tile.Y) * 32.0f + (xzOff * corner4Z)) + modelOffZ),
            ];
        }

        public static Vector3 GetVector3Normal(this IMPD_SurfaceTile tile, CornerType corner)
            => tile.GetVertexNormal(corner).ToVector3();

        public static Vector3[] GetVector3Normals(this IMPD_SurfaceTile tile)
            => tile.GetVertexNormals().Select(x => x.ToVector3()).ToArray();
    }
}