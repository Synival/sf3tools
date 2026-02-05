using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib.SGL;
using CommonLib.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;
using static CommonLib.Types.CornerTypeConsts;

namespace SF3.MPD.Extensions {
    public static class IMPD_SurfaceExtensions {
        public static string ToJSON_String(this IMPD_Surface surface)
            => surface.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Surface surface) => surface.ToJObject();
        public static JObject ToJObject(this IMPD_Surface surface) {
            JArray MakeByteTable(Func<int, int, byte> fetcher, int blankValue) {
                var width  = surface.Width;
                var height = surface.Height;

                var jArray = new JArray();
                for (int y = 0; y < height; y++) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var value = fetcher(x, y);
                        str.Append(value == blankValue ? "  " : value.ToString("X2"));
                    }
                    jArray.Add(str.ToString());
                }

                return jArray;
            };

            JArray MakeHeightTable(Func<int, int, byte[]> fetcher) {
                var width  = surface.Width;
                var height = surface.Height;

                var jArray = new JArray();
                for (int y = 0; y < height; y++) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var values = fetcher(x, y);
                        str.Append(((x != 0) ? " " : "") + values[0].ToString("X2") + values[1].ToString("X2") + values[2].ToString("X2") + values[3].ToString("X2"));
                    }
                    jArray.Add(str.ToString());
                }

                return jArray;
            };

            JArray MakeNormalTable(Func<int, int, VECTOR> fetcher) {
                var width  = surface.Width + 1;
                var height = surface.Height + 1;

                string VectorsToBase64(VECTOR vec) {
                    if (vec.X.RawInt == 0 && vec.Y.RawInt == -65536 && vec.Z.RawInt == 0)
                        return "        ";

                    var bytes = new byte[6];
                    var pos = 0;

                    var x = (ushort) (((uint) vec.X.RawInt) / 2);
                    var y = (ushort) (((uint) vec.Y.RawInt) / 2);
                    var z = (ushort) (((uint) vec.Z.RawInt) / 2);

                    bytes[pos++] = (byte) (x >> 8);
                    bytes[pos++] = (byte) (x >> 0);
                    bytes[pos++] = (byte) (y >> 8);
                    bytes[pos++] = (byte) (y >> 0);
                    bytes[pos++] = (byte) (z >> 8);
                    bytes[pos++] = (byte) (z >> 0);

                    return Convert.ToBase64String(bytes);
                }

                var jArray = new JArray();
                for (int y = 0; y < height; y++) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var value = fetcher(x, y);
                        str.Append(((x != 0) ? " " : "") + VectorsToBase64(value));
                    }
                    jArray.Add(str.ToString());
                }

                return jArray;
            };

            return new JObject {
                { "TextureIDs", MakeByteTable((x, y) => surface.GetTile(x, y).TextureID, 0xFF) },
                { "EventIDs",   MakeByteTable((x, y) => surface.GetTile(x, y).EventID, 0) },
                { "Heights",    MakeHeightTable((x, y) => surface.GetTile(x, y).GetVertexHeights()) },
                { "Normals",    MakeNormalTable((x, y) => surface.GetVertexNormal(x, y)) },
            };
        }

        /// <summary>
        /// Returns a 3x3 set of heights used for calculating normals for a given vertex. Only non-flat tiles can be
        /// used to determine a height for the normal map because flat tiles are not part of the mesh. If no non-flat
        /// tiles can be found for a vertex, the height is set to 'null'.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx">X coordinate of the vertex that needs a heightmap.</param>
        /// <param name="vy">Y coordinate of the vertex that needs a heightmap.</param>
        /// <returns>A byte?[3, 3] with heights necessary for normal calculation.</returns>
        public static byte?[,] GetVertexHeightMeshForNormalCalculation(this IMPD_Surface surface, int vx, int vy)
            => surface.GetVertexHeightMeshForNormalCalculation(vx, vy, vx, vy);

        /// <summary>
        /// Returns a set of heights used for calculating normals for a given set of vertices. Only non-flat tiles can be
        /// used to determine a height for the normal map because flat tiles are not part of the mesh. If no non-flat
        /// tiles can be found for a vertex, the height is set to 'null'.
        /// 
        /// The dimensions of the heightmap are:
        ///     [abs(vx2-vx1)+2, abs(vy2-vy1)+2]
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx1">Lower X coordinate of the vertices that need a heightmap.</param>
        /// <param name="vy1">Lower Y coordinate of the vertices that need a heightmap.</param>
        /// <returns>A byte?[3, 3] with heights necessary for normal calculation.</returns>
        public static byte?[,] GetVertexHeightMeshForNormalCalculation(this IMPD_Surface surface, int vx1, int vy1, int vx2, int vy2) {
            // Ensure that vx1 < vx2 and vy1 < vy2.
            if (vx1 > vx2)
                (vx1, vx2) = (vx2, vx1);
            if (vy1 > vy2)
                (vy1, vy2) = (vy2, vy1);

            // Determine the size of the heightmap to return.
            var heightmapWidth  = vx2 - vx1 + 3;
            var heightmapHeight = vy2 - vy1 + 3;

            // Get all the tiles necessary for fetching vertex heights.
            // Each normal needs a 3x3 grid of vertices to calculate, each of which can come from a specific corner
            // of tiles in a 2x2 grid. For the best chances of fetching a valid vertex, we want a grid of 4x4 tiles
            // for each normal we want to calculate.
            int surfaceWidth  = surface.Width;
            int surfaceHeight = surface.Height;
            IMPD_SurfaceTile GetTileIfExists(int tx, int ty)
                => (tx >= 0 && ty >= 0 && tx < surfaceWidth && ty < surfaceHeight) ? surface.GetTile(tx, ty) : null;

            var tilemapWidth  = heightmapWidth + 1;
            var tilemapHeight = heightmapHeight + 1;
            var tiles = new IMPD_SurfaceTile[tilemapWidth, tilemapHeight];
            for (int y = 0; y < tilemapWidth; y++)
                for (int x = 0; x < tilemapHeight; x++)
                    tiles[x, y] = GetTileIfExists(vx1 + x - 2, vy1 + y - 2);

            // Function to fetch a set of tiles to consider for fetching vertex heights for a given vertex.
            (IMPD_SurfaceTile Tile, CornerType Corner)[] GetPossibleTiles(int vx, int vy) {
                // Function to add a tile if it's considerable for vertex normal calculation.
                // It must be a tile that exists 
                var possibleTiles = new List<(IMPD_SurfaceTile Tile, CornerType Corner)>();
                void AddTileAndCornerIfValid(int tx, int ty, CornerType corner) {
                    if (tx < 0 || ty < 0 || tx >= tilemapWidth || ty >= tilemapHeight)
                        throw new InvalidOperationException("Internal error; we shouldn't call this function on this condition!");

                    var tile = tiles[tx, ty];
                    if (tile == null || tile.IsFlat)
                        return;
                    possibleTiles.Add((tile, corner));
                }

                AddTileAndCornerIfValid(vx + 1, vy + 1, CornerType.BottomLeft);
                AddTileAndCornerIfValid(vx + 0, vy + 1, CornerType.BottomRight);
                AddTileAndCornerIfValid(vx + 0, vy + 0, CornerType.TopRight);
                AddTileAndCornerIfValid(vx + 1, vy + 0, CornerType.TopLeft);

                return possibleTiles.ToArray();
            }

            var heights = new byte?[heightmapWidth, heightmapHeight];
            for (int vy = 0; vy < heightmapHeight; vy++) {
                for (int vx = 0; vx < heightmapWidth; vx++) {
                    var possibleTiles = GetPossibleTiles(vx, vy).Where(x => x.Tile != null).ToArray();
                    if (possibleTiles.Length > 0) {
                        var validHeights = possibleTiles.Select(x => x.Tile.GetVertexHeight(x.Corner)).ToArray();
                        heights[vx, vy] = (byte) validHeights.Select(x => (int) x).Average();
                    }
                }
            }

            return heights;
        }

        /// <summary>
        /// Calculates the vertex normal for a specific vertex of a tile.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <returns>A freshly-calculated normal for the vertex requested.</returns>
        public static VECTOR CalculateVertexNormal(this IMPD_Surface surface, int vertexX, int vertexY)
            => CalculateVertexNormal(surface, vertexX, vertexY, surface.GetVertexHeightMeshForNormalCalculation(vertexX, vertexY), vertexX - 1, vertexY - 1);

        /// <summary>
        /// Calculates the vertex normal for a specific vertex of a tile.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <returns>A freshly-calculated normal for the vertex requested.</returns>
        public static VECTOR CalculateVertexNormal(this IMPD_Surface surface, int vertexX, int vertexY, byte?[,] heights, int heightsStartVX, int heightsStartVY) {
            var settings = surface.NormalSettings;

            var quadHeightFactor = (settings.HalfHeight ? 0.5f : 1.0f) / 16.0f;
            var sumNormals = new List<VECTOR>();
            void TryAddQuadNormal(int tx, int ty) {
                var vx = tx - heightsStartVX;
                var vy = ty - heightsStartVY;

                if (!heights[vx, vy].HasValue || !heights[vx + 1, vy].HasValue || !heights[vx, vy + 1].HasValue || !heights[vx + 1, vy + 1].HasValue)
                    return;

                var quad = new POLYGON(new VECTOR[] {
                    new VECTOR(Corner1X, heights[vx + 1, vy + 1].Value * quadHeightFactor, Corner1Z),
                    new VECTOR(Corner2X, heights[vx + 0, vy + 1].Value * quadHeightFactor, Corner2Z),
                    new VECTOR(Corner3X, heights[vx + 0, vy + 0].Value * quadHeightFactor, Corner3Z),
                    new VECTOR(Corner4X, heights[vx + 1, vy + 0].Value * quadHeightFactor, Corner4Z)
                });

                sumNormals.Add(quad.GetNormal(settings.CalculationMethod));
            }

            // Gather a list of all quad normals to use for averaging the vertex normal.
            // On the edges of maps, there are fewer adjected polys to the vertex,
            // so only add normals if they exist.
            TryAddQuadNormal(vertexX + 0, vertexY + 0);
            TryAddQuadNormal(vertexX - 1, vertexY + 0);
            TryAddQuadNormal(vertexX - 1, vertexY - 1);
            TryAddQuadNormal(vertexX + 0, vertexY - 1);

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
                vec = FixVertexNormalOverflowUnderflowErrors(vec);

            return vec;
        }

        /// <summary>
        /// Corrects a vertex normal to avoid a bug in Shining Force 3 where VECTOR components of normals will over/
        /// underflow when they are +/- 0.50. This effectively clamps each component to the (-0.495, 0.495) range.
        /// </summary>
        /// <param name="vec">VECTOR to correct.</param>
        /// <returns>A corrected VECTOR that shouldn't over/underflow visually in Shining Force 3.</returns>
        public static VECTOR FixVertexNormalOverflowUnderflowErrors(VECTOR vec) {
            // Clamp X and Z components to [-0.50, 0.50]. There appears to be a bug in SF3
            // where this can be interpreted as an overflow, resulting in very out of place shadows.
            // TODO: enforce a maximum slope for Scn2 surface lighting.
            const float maxFloat =  0.495f;
            const float minFloat = -0.495f;

            // Reduce the X/Z components together, then adjust the Y component to keep it normalized.
            if (vec.X.Float < minFloat || vec.X.Float > maxFloat || vec.Z.Float < minFloat || vec.Z.Float > maxFloat) {
                // X component needs more correction
                if (Math.Abs(vec.X.Float) > Math.Abs(vec.Z.Float)) {
                    if (vec.X.Float < minFloat) {
                        var ratio = minFloat / vec.X.Float;
                        vec.X.Float = minFloat;
                        vec.Z.Float *= ratio;
                    }
                    else if (vec.X.Float > maxFloat) {
                        var ratio = maxFloat / vec.X.Float;
                        vec.X.Float = maxFloat;
                        vec.Z.Float *= ratio;
                    }
                    else
                        throw new InvalidOperationException("Condition should be unreachable!");
                }
                // Z component needs more correction
                else {
                    if (vec.Z.Float < minFloat) {
                        var ratio = minFloat / vec.Z.Float;
                        vec.Z.Float = minFloat;
                        vec.X.Float *= ratio;
                    }
                    else if (vec.Z.Float > maxFloat) {
                        var ratio = maxFloat / vec.Z.Float;
                        vec.Z.Float = maxFloat;
                        vec.X.Float *= ratio;
                    }
                    else
                        throw new InvalidOperationException("Condition should be unreachable!");
                }

                // Recalculate Y component so the vector remains normalized
                var oldY = vec.Y.Float;
                vec.Y.Float = (float) Math.Sqrt(1.0f - (vec.X.Float * vec.X.Float + vec.Z.Float * vec.Z.Float));
                if (oldY < 0.0f)
                    vec.Y.Float = -vec.Y.Float;
            }

            return vec;
        }

        /// <summary>
        /// Updates all vertex normals whose calculation depends on a given vertex.
        /// </summary>
        /// <param name="vx">X coordinate of the vertex involved in calculations.</param>
        /// <param name="vy">Y coordinate of the vertex involved in calculations.</param>
        public static void UpdateVertexNormalsInvolvingVertex(this IMPD_Surface surface, int vx, int vy)
            => UpdateVertexNormalsInvolvingVertices(surface, vx, vy, vx, vy);

        /// <summary>
        /// Updates all vertex normals whose calculation depends on a given range of vertices.
        /// </summary>
        /// <param name="vx1">Lowest X coordinate of the vertices involved in calculations.</param>
        /// <param name="vy1">Lowest Y coordinate of the vertices involved in calculations.</param>
        /// <param name="vx2">Highest X coordinate of the vertices involved in calculations.</param>
        /// <param name="vy2">Highest Y coordinate of the vertices involved in calculations.</param>
        public static void UpdateVertexNormalsInvolvingVertices(this IMPD_Surface surface, int vx1, int vy1, int vx2, int vy2) {
            // Ensure that vx1 < vx2 and vy1 < vy2
            if (vx1 > vx2)
                (vx1, vx2) = (vx2, vx1);
            if (vy1 > vy2)
                (vy1, vy2) = (vy2, vy1);

            surface.UpdateVertexNormals(vx1 - 1, vy1 - 1, vx2 + 1, vy2 + 1);
        }

        /// <summary>
        /// Updates all vertex normals for a surface.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        public static void UpdateVertexNormals(this IMPD_Surface surface)
            => surface.UpdateVertexNormals(0, 0, surface.Width, surface.Height);
    }
}
