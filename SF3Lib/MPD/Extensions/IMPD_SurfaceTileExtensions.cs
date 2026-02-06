using System;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_SurfaceTileExtensions {
        /// <summary>
        /// Gets the normal vector of the tile for the specified corner.
        /// </summary>
        /// <param name="corner">The corner of the tile whose normal vector should be retrieved.</param>
        /// <returns>A normal VECTOR.</returns>
        public static VECTOR GetVertexNormal(this IMPD_SurfaceTile tile, CornerType corner) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));
            var vx = BlockHelpers.TileToVertexX(tile.X, corner);
            var vy = BlockHelpers.TileToVertexY(tile.Y, corner);
            return tile.Surface.GetVertex(vx, vy).Normal;
        }

        /// <summary>
        /// Gets normal vectors for all corners of the tile.
        /// </summary>
        /// <returns>A normal VECTOR for every corner of the tile.</returns>
        public static VECTOR[] GetVertexNormals(this IMPD_SurfaceTile tile) {
            var surface = tile.Surface;
            var x = tile.X;
            var y = tile.Y;
            return new VECTOR[] {
                surface.GetVertex(x + 1, y + 1).Normal,
                surface.GetVertex(x + 0, y + 1).Normal,
                surface.GetVertex(x + 0, y + 0).Normal,
                surface.GetVertex(x + 1, y + 0).Normal,
            };
        }

        /// <summary>
        /// Sets the normal vector of the tile for the specified corner.
        /// </summary>
        /// <param name="corner">The corner of the tile whose normal vector should be set.</param>
        /// <returns>A normal VECTOR.</returns>
        public static void SetVertexNormal(this IMPD_SurfaceTile tile, CornerType corner, VECTOR normal) {
            int cornerInt = (int) corner;
            if (cornerInt < 0 || cornerInt > 3)
                throw new ArgumentOutOfRangeException(nameof(corner));
            var vx = BlockHelpers.TileToVertexX(tile.X, corner);
            var vy = BlockHelpers.TileToVertexY(tile.Y, corner);
            tile.Surface.GetVertex(vx, vy).Normal = normal;
        }
    }
}
