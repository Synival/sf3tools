using System.Collections.Generic;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.MPD.Interfaces;
using SF3.Utils;

namespace SF3.MPD.Extensions {
    public static class IMPD_SurfaceVertexExtensions {
        /// <summary>
        /// Returns a 3x3 set of heights used for calculating normals for a given vertex. Only non-flat tiles can be
        /// used to determine a height for the normal map because flat tiles are not part of the mesh. If no non-flat
        /// tiles can be found for a vertex, the height is set to 'null'.
        /// </summary>
        /// <param name="vertex">Surface vertex to operate on.</param>
        /// <returns>A byte?[3, 3] with heights necessary for normal calculation.</returns>
        public static byte?[,] GetHeightMeshForNormalCalculation(this IMPD_SurfaceVertex vertex) {
            var vx = vertex.X;
            var vy = vertex.Y;
            return vertex.Surface.GetVertexHeightMeshForNormalCalculation(vx, vy, vx, vy);
        }

        /// <summary>
        /// Calculates the vertex normal for a specific vertex of a tile.
        /// </summary>
        /// <param name="vertex">Vertex to operate on.</param>
        /// <returns>A freshly-calculated normal for the vertex requested.</returns>
        public static VECTOR CalculateNormal(this IMPD_SurfaceVertex vertex) {
            var vx = vertex.X;
            var vy = vertex.Y;
            return vertex.CalculateNormal(vertex.GetHeightMeshForNormalCalculation(), vx - 1, vy - 1);
        }

        /// <summary>
        /// Calculates the vertex normal for a specific vertex of a tile.
        /// </summary>
        /// <param name="vertex">Surface vertex to operate on.</param>
        /// <param name="heightsStartVX">X component of the vertex coordinate represented by heights[0, 0].</param>
        /// <param name="heightsStartVY">Y component of the vertex coordinate represented by heights[0, 0].</param>
        /// <returns>A freshly-calculated normal for the vertex requested.</returns>
        public static VECTOR CalculateNormal(this IMPD_SurfaceVertex vertex, byte?[,] heights, int heightsStartVX, int heightsStartVY) {
            var settings = vertex.Surface.NormalSettings;

            var quadHeightFactor = ((settings.HalfHeight || vertex.Surface.MPD.Settings.NarrowAngleBasedLightmap) ? 0.5f : 1.0f) / 16.0f;
            var sumNormals = new List<VECTOR>();
            void TryAddQuadNormal(int tx, int ty, CornerType corner) {
                if (tx >= 0 && tx < 64 && ty >= 0 && ty < 64) {
                    var tile = vertex.Surface.GetTile(tx, ty);
                    if (tile.IsFlat) {
                        sumNormals.Add(new VECTOR(0, -1, 0));
                        return;
                    }
                    if (settings.IgnoreBlankTiles && tile.TextureID == 0xFF)
                        return;
                }

                var vx = tx - heightsStartVX;
                var vy = ty - heightsStartVY;

                if (!heights[vx, vy].HasValue || !heights[vx + 1, vy].HasValue || !heights[vx, vy + 1].HasValue || !heights[vx + 1, vy + 1].HasValue)
                    return;

                // Normal calculation methods requie a quad starting from the bottom-right corner in clockwise order.
                // Heights are stored with higher values being higher, but SF3's 3D space is the opposite, so flip the height.
                var quad = new POLYGON(new VECTOR[] {
                    new VECTOR(1, heights[vx + 1, vy + 0].Value * -quadHeightFactor, 0),
                    new VECTOR(0, heights[vx + 0, vy + 0].Value * -quadHeightFactor, 0),
                    new VECTOR(0, heights[vx + 0, vy + 1].Value * -quadHeightFactor, 1),
                    new VECTOR(1, heights[vx + 1, vy + 1].Value * -quadHeightFactor, 1),
                });

                sumNormals.Add(quad.GetMeshNormalComponent(corner, settings.CalculationMethod));
            }

            // Gather a list of all quad normals to use for averaging the vertex normal.
            // On the edges of maps, there are fewer adjected polys to the vertex,
            // so only add normals if they exist.
            var vertexX = vertex.X;
            var vertexY = vertex.Y;
            TryAddQuadNormal(vertexX + 0, vertexY - 1, CornerType.TopLeft);
            TryAddQuadNormal(vertexX - 1, vertexY - 1, CornerType.TopRight);
            TryAddQuadNormal(vertexX - 1, vertexY + 0, CornerType.BottomRight);
            TryAddQuadNormal(vertexX + 0, vertexY + 0, CornerType.BottomLeft);

            if (sumNormals.Count == 0)
                return new VECTOR(0, -1, 0);

            // Average all the quad normals and normalize them. This appears to be the calculation that Camelot used
            // for normal calculations and produces a near-identical result. (They aren't identical due to some
            // unsolved floating-point math differences.)
            var components = new int[3];
            foreach (var normal in sumNormals) {
                components[0] += normal.X.RawInt;
                components[1] += normal.Y.RawInt;
                components[2] += normal.Z.RawInt;
            }

            var vec = new VECTOR(components[0], components[1], components[2], isRaw: true).Normalized();

            // Normal vectors are actually CompressedFIXED, so account for the lower decimal resolution.
            vec.X.RawInt = (vec.X.RawInt / 2) * 2;
            vec.Y.RawInt = (vec.Y.RawInt / 2) * 2;
            vec.Z.RawInt = (vec.Z.RawInt / 2) * 2;

            // Correct for some vanilla bugs if set.
            if (settings.FixOverflowUnderflowErrors)
                vec = SurfaceUtils.FixVertexNormalOverflowUnderflowErrors(vec);

            return vec;
        }

        /// <summary>
        /// Updates all vertex normals whose calculation depends on a given vertex.
        /// </summary>
        /// <param name="vertex">Vertex to operate on.</param>
        public static void UpdateNormalsInvolvingVertex(this IMPD_SurfaceVertex vertex) {
            var vx = vertex.X;
            var vy = vertex.Y;
            vertex.Surface.UpdateVertexNormalsInvolvingVertices(vx, vy, vx, vy);
        }
    }
}
