using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CommonLib.SGL;
using CommonLib.Types;
using CommonLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

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
                int count = 0;
                for (int y = height - 1; y >= 0; y--) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var value = fetcher(x, y);
                        bool isDefault = (value == blankValue);
                        str.Append(isDefault ? "  " : value.ToString("X2"));
                        count += isDefault ? 0 : 1;
                    }
                    jArray.Add(str.ToString());
                }

                return count == 0 ? null : jArray;
            };

            JArray MakeHeightTable(Func<int, int, byte[]> fetcher) {
                var width  = surface.Width;
                var height = surface.Height;

                var jArray = new JArray();
                var count = 0;
                for (int y = height - 1; y >= 0; y--) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var values = fetcher(x, y);
                        var isDefault = (values[0] == 0 && values[1] == 0 && values[2] == 0 && values[3] == 0);
                        str.Append(((x != 0) ? " " : "") + values[0].ToString("X2") + values[1].ToString("X2") + values[2].ToString("X2") + values[3].ToString("X2"));
                        count += isDefault ? 0 : 1;
                    }
                    jArray.Add(str.ToString());
                }

                return count == 0 ? null : jArray;
            };

            JArray MakeNormalTable(Func<int, int, VECTOR> fetcher) {
                var width  = surface.Width + 1;
                var height = surface.Height + 1;

                var count = 0;
                string VectorsToBase64(VECTOR vec) {
                    if (vec.X.RawInt == 0 && vec.Y.RawInt == -65536 && vec.Z.RawInt == 0)
                        return "        ";

                    var bytes = new byte[6];
                    var pos = 0;

                    var x = (ushort) ((short) (vec.X.RawInt / 2));
                    var y = (ushort) ((short) (vec.Y.RawInt / 2));
                    var z = (ushort) ((short) (vec.Z.RawInt / 2));

                    bytes[pos++] = (byte) (x >> 8);
                    bytes[pos++] = (byte) (x >> 0);
                    bytes[pos++] = (byte) (y >> 8);
                    bytes[pos++] = (byte) (y >> 0);
                    bytes[pos++] = (byte) (z >> 8);
                    bytes[pos++] = (byte) (z >> 0);

                    count++;
                    return Convert.ToBase64String(bytes);
                }

                var jArray = new JArray();
                for (int y = height - 1; y >= 0; y--) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var value = fetcher(x, y);
                        str.Append(((x != 0) ? " " : "") + VectorsToBase64(value));
                    }
                    jArray.Add(str.ToString());
                }

                return count == 0 ? null : jArray;
            };

            return new JObject {
                { "TextureIDs",   MakeByteTable  ((x, y) => surface.GetTile(x, y).TextureID,    0xFF) },
                { "TextureFlags", MakeByteTable  ((x, y) => surface.GetTile(x, y).TextureFlags, 0x00) },
                { "UnknownTextureFlags", MakeByteTable  ((x, y) => surface.GetTile(x, y).UnknownTextureFlags, 0x00) },
                { "EventIDs",     MakeByteTable  ((x, y) => surface.GetTile(x, y).EventID,      0x00) },
                { "Terrain",      MakeByteTable(
                    (x, y) => {
                        var tile = surface.GetTile(x, y);
                        return (byte) ((byte) (tile.TerrainType) | ((byte) (tile.TerrainFlags) << 4));
                    },
                    0x00)
                },
                { "Heights",      MakeHeightTable((x, y) => surface.GetTile(x, y).GetVertexHeights()) },
                { "Normals",      MakeNormalTable((x, y) => surface.GetVertex(x, y).Normal) },
            };
        }

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
        /// Gets a range of vertices that are clamped to (0, 0, Width, Height).
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="rectangle">Range of vertices to clamp.</param>
        /// <returns>A Rectangle with an inclusive range of vertices.</returns>
        public static Rectangle GetClampedVertexRange(this IMPD_Surface surface, Rectangle rectangle) {
            var vx1 = Math.Max(0, Math.Min(rectangle.Left,   surface.Width));
            var vy1 = Math.Max(0, Math.Min(rectangle.Top,    surface.Height));
            var vx2 = Math.Max(0, Math.Min(rectangle.Right,  surface.Width));
            var vy2 = Math.Max(0, Math.Min(rectangle.Bottom, surface.Height));

            return new Rectangle(vx1, vy1, vx2 - vx1, vy2 - vy1);
        }

        /// <summary>
        /// Gets a range of vertices that are clamped to (0, 0, Width, Height).
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx1">Lowest X coordinate of the vertex range.</param>
        /// <param name="vy1">Lowest Y coordinate of the vertex range.</param>
        /// <param name="vx2">Highest X coordinate of the vertex range.</param>
        /// <param name="vy2">Highest Y coordinate of the vertex range.</param>
        /// <returns>A Rectangle with an inclusive range of vertices.</returns>
        public static Rectangle GetClampedVertexRange(this IMPD_Surface surface, int vx1, int vy1, int vx2, int vy2) {
            // Ensure that vx1 < vx2 and vy1 < vy2
            if (vx1 > vx2)
                (vx1, vx2) = (vx2, vx1);
            if (vy1 > vy2)
                (vy1, vy2) = (vy2, vy1);

            vx1 = Math.Max(0, Math.Min(vx1, surface.Width));
            vy1 = Math.Max(0, Math.Min(vy1, surface.Height));
            vx2 = Math.Max(0, Math.Min(vx2, surface.Width));
            vy2 = Math.Max(0, Math.Min(vy2, surface.Height));

            return new Rectangle(vx1, vy1, vx2 - vx1, vy2 - vy1);
        }

        /// <summary>
        /// Gets a range of tiles that are clamped to (0, 0, Width - 1, Height - 1).
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="rectangle">Range of tiles to clamp.</param>
        /// <returns>A Rectangle with an inclusive range of tiles.</returns>
        public static Rectangle GetClampedTileRange(this IMPD_Surface surface, Rectangle rectangle) {
            var vx1 = Math.Max(0, Math.Min(rectangle.Left,   surface.Width  - 1));
            var vy1 = Math.Max(0, Math.Min(rectangle.Top,    surface.Height - 1));
            var vx2 = Math.Max(0, Math.Min(rectangle.Right,  surface.Width  - 1));
            var vy2 = Math.Max(0, Math.Min(rectangle.Bottom, surface.Height - 1));

            return new Rectangle(vx1, vy1, vx2 - vx1, vy2 - vy1);
        }

        /// <summary>
        /// Gets a range of tiles that are clamped to (0, 0, Width - 1, Height - 1).
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx1">Lowest X coordinate of the tiles range.</param>
        /// <param name="vy1">Lowest Y coordinate of the tiles range.</param>
        /// <param name="vx2">Highest X coordinate of the tiles range.</param>
        /// <param name="vy2">Highest Y coordinate of the tiles range.</param>
        /// <returns>A Rectangle with an inclusive range of tiles.</returns>
        public static Rectangle GetClampedTileRange(this IMPD_Surface surface, int vx1, int vy1, int vx2, int vy2) {
            // Ensure that vx1 < vx2 and vy1 < vy2
            if (vx1 > vx2)
                (vx1, vx2) = (vx2, vx1);
            if (vy1 > vy2)
                (vy1, vy2) = (vy2, vy1);

            vx1 = Math.Max(0, Math.Min(vx1, surface.Width  - 1));
            vy1 = Math.Max(0, Math.Min(vy1, surface.Height - 1));
            vx2 = Math.Max(0, Math.Min(vx2, surface.Width  - 1));
            vy2 = Math.Max(0, Math.Min(vy2, surface.Height - 1));

            return new Rectangle(vx1, vy1, vx2 - vx1, vy2 - vy1);
        }

        /// <summary>
        /// Gets all vertices whose normals need to be updated when a specified vertex's height is updated.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="tx">Tile X coordinate.</param>
        /// <param name="ty">Tile Y coordinate.</param>
        /// <param name="corner">Corner of the tile whose vertex is relevant to the normals to recalculate.</param>
        public static Rectangle GetNormalVertexRangeAffectedByHeightOf(this IMPD_Surface surface, int tx, int ty, CornerType corner) {
            var vx = BlockHelpers.TileToVertexX(tx, corner);
            var vy = BlockHelpers.TileToVertexY(ty, corner);
            return surface.GetClampedVertexRange(vx - 1, vy - 1, vx + 1, vy + 1);
        }

        /// <summary>
        /// Gets all vertices whose normals need to be updated when a specified vertex's height is updated.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx">Vertex X coordinate relevant to the normals that require an update.</param>
        /// <param name="vy">Vertex Y coordinate relevant to the normals that require an update.</param>
        public static Rectangle GetNormalVertexRangeAffectedByHeightOf(this IMPD_Surface surface, int vx, int vy)
            => surface.GetClampedVertexRange(vx - 1, vy - 1, vx + 1, vy + 1);

        /// <summary>
        /// Gets all vertices whose normals need to be updated when a specified set of vertices' height is updated.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx1">Lowest X coordinate of the vertices involved in calculations.</param>
        /// <param name="vy1">Lowest Y coordinate of the vertices involved in calculations.</param>
        /// <param name="vx2">Highest X coordinate of the vertices involved in calculations.</param>
        /// <param name="vy2">Highest Y coordinate of the vertices involved in calculations.</param>
        public static Rectangle GetNormalVertexRangeAffectedByHeightsOf(this IMPD_Surface surface, int vx1, int vy1, int vx2, int vy2) {
            // Ensure that vx1 < vx2 and vy1 < vy2
            if (vx1 > vx2)
                (vx1, vx2) = (vx2, vx1);
            if (vy1 > vy2)
                (vy1, vy2) = (vy2, vy1);

            return surface.GetClampedVertexRange(vx1 - 1, vy1 - 1, vx2 + 1, vy2 + 1);
        }

        /// <summary>
        /// Gets all vertices whose normals need to be updated when a specified set of vertices' height is updated.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="rectangle">Rectangle containing the coordinates of vertices whose heights would be modified.</param>
        public static Rectangle GetNormalVertexRangeAffectedByHeightsOf(this IMPD_Surface surface, Rectangle rectangle)
            => surface.GetClampedVertexRange(rectangle.Left - 1, rectangle.Top - 1, rectangle.Right + 1, rectangle.Bottom + 1);

        /// <summary>
        /// Gets a tile range that contain the vertices in a vertex range.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="rectangle">Rectangle containing the vertex range to get a tile range from.</param>
        /// <returns>A Rectangle that is an inclusive tile range.</returns>
        public static Rectangle GetTileRangeContainingVertexRange(this IMPD_Surface surface, Rectangle rectangle)
            => surface.GetClampedTileRange(rectangle.Left - 1, rectangle.Top - 1, rectangle.Right, rectangle.Bottom);

        /// <summary>
        /// Gets a tile range that contain the vertices in a vertex range.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="vx1">Lowest X coordinate of the vertex range contained in the tile range to fetch.</param>
        /// <param name="vy1">Lowest Y coordinate of the vertex range contained in the tile range to fetch.</param>
        /// <param name="vx2">Highest X coordinate of the vertex range contained in the tile range to fetch.</param>
        /// <param name="vy2">Highest Y coordinate of the vertex range contained in the tile range to fetch.</param>
        /// <returns>A Rectangle that is an inclusive tile range.</returns>
        public static Rectangle GetTileRangeContainingVertexRange(this IMPD_Surface surface, int vx1, int vy1, int vx2, int vy2)
            => surface.GetClampedTileRange(vx1 - 1, vy1 - 1, vx2, vy2);

        /// <summary>
        /// Updates all vertex normals for a surface.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        public static void UpdateVertexNormals(this IMPD_Surface surface)
            => surface.UpdateVertexNormals(0, 0, surface.Width, surface.Height);

        /// <summary>
        /// Updates vertex numbers in an inclusive range.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="rectangle">Range of vertices to update normals for.</param>
        public static void UpdateVertexNormals(this IMPD_Surface surface, Rectangle rectangle)
            => surface.UpdateVertexNormals(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

        /// <summary>
        /// Fetches the heightmap position at a coordinate in range [0, 2048) x [0, 2048).
        /// Input coordinates are mod'ed to simulate in-game behavior.
        /// </summary>
        /// <param name="surface">Surface to operate on.</param>
        /// <param name="x">X coordinate of height to fetch, in range [0, 2048).</param>
        /// <param name="y">Y coordinate of height to fetch, in range [0, 2048).</param>
        /// <returns></returns>
        public static float GetHeightAt(this IMPD_Surface surface, float x, float y) {
            x = MathHelpers.ActualMod(x, 2048);
            y = MathHelpers.ActualMod(y, 2048);

            var tileX = (int) (x / 32);
            var tileY = (int) (y / 32);

            var tile = surface.GetTile(tileX, tileY);

            var xt = MathHelpers.ActualMod(x, 32) / 32.0f;
            var yt = MathHelpers.ActualMod(y, 32) / 32.0f;

            var hBL = tile.GetVertexHeight(CornerType.BottomLeft);
            var hBR = tile.GetVertexHeight(CornerType.BottomRight);
            var hTL = tile.GetVertexHeight(CornerType.TopLeft);
            var hTR = tile.GetVertexHeight(CornerType.TopRight);

            var hT = (1 - xt) * hBL + xt * hBR;
            var hB = (1 - xt) * hTL + xt * hTR;
            var h  = (1 - yt) * hB  + yt * hT;

            return h;
        }
    }
}
