using System;
using System.Linq;
using CommonLib.SGL;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.MPD.Interfaces;
using static SF3.Utils.SurfaceUtils;

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

        /// <summary>
        /// Updated the 'IsFlat' flat of a tile and simultaneously changes/fixes its heights.
        /// </summary>
        /// <param name="tile">Tile to modify.</param>
        /// <param name="value">New 'IsFlat' value.</param>
        /// <param name="valueSetter">Callback to set the internal 'IsFlat' value.</param>
        public static void SetFlatAndUpdateHeights(this IMPD_SurfaceTile tile, bool value, Action<bool> valueSetter) {
            // If flattening the tile, set heights to the lowest value.
            if (value) {
                var minHeight = tile.GetVertexHeights().Min();
                valueSetter(true);
                tile.SetVertexHeights(new byte[] { minHeight, minHeight, minHeight, minHeight });
                return;
            }

            // If unflattening the tile, update its heights to its neighbors.
            // The bottom-right corner of the heightmap table determines the flattened height.
            // We're going to use that as a fallback if there's no non-flat tile to fetch here.
            var brHeight = tile.GetVertexHeight(CornerType.BottomRight);

            valueSetter(false);

            // Steal heights from now-shared vertices.
            var allCorners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            var allSharedHeights = allCorners.Select(x => tile.GetSharedVerticesAtCorner(x)).ToArray();

            var newVertexHeights = new byte[4];
            foreach (var corner in allCorners) {
                var sharedHeights = allSharedHeights[(int) corner];
                var vertex = sharedHeights.Length >= 2 ? sharedHeights[1] : (TileAndCorner?) null;
                newVertexHeights[(int) corner] = vertex.HasValue ? tile.Surface.GetTile(vertex.Value.X, vertex.Value.Y).GetVertexHeight(vertex.Value.Corner) : brHeight;
            }

            tile.SetVertexHeights(newVertexHeights);
        }
    }
}
