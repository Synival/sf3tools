using SF3.ByteData;
using System;
using CommonLib.SGL;
using CommonLib.Types;
using System.Collections.Generic;
using static CommonLib.Utils.BlockHelpers;
using SF3.Models.Structs.MPD.Surface;

namespace SF3.Models.Tables.MPD.Surface {
    public class HeightmapRowTable : Table<HeightmapRow> {
        protected HeightmapRowTable(IByteData data, int address) : base(data, address) {
        }

        public static HeightmapRowTable Create(IByteData data, int address) {
            var newTable = new HeightmapRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new HeightmapRow(Data, 0, "", Address).Size;
            return LoadUntilMax((id, address) => new HeightmapRow(Data, id, "Y" + id.ToString("D2"), Address + (63 - id) * size));
        }

        public override int? MaxSize => 64;

        /// <summary>
        /// Calculates the "abnormal" for a tile at a given corner.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile whose vertex abnormal should be calculated.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        /// <returns>A freshly-calculated abnormal for the vertex requested.</returns>
        public VECTOR CalculateSurfaceVertexAbnormal(int tileX, int tileY, CornerType corner, bool useMoreAccurateCalculations)
            => CalculateSurfaceVertexAbnormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), useMoreAccurateCalculations);

        /// <summary>
        /// Calculates the vertex abnormal for a specific vertex of a tile.
        /// </summary>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <param name="useMoreAccurateCalculations">When 'true', math more accurate than SF3 provided will be used.</param>
        /// <returns>A freshly-calculated abnormal for the vertex requested.</returns>
        public VECTOR CalculateSurfaceVertexAbnormal(int vertexX, int vertexY, bool useMoreAccurateCalculations) {
            // Determine the normals of the 4 quads surrounding the vertex.
            var sumNormals = new List<VECTOR>();

            void TryAddQuadNormal(int vx, int vy) {
                if (vx >= 0 && vy >= 0 && vx <= 63 && vy <= 63) {
                    var heights = Rows[63 - vy].GetHeights(vx);
                    var quad = new POLYGON(new VECTOR[] {
                        new VECTOR(0.00f, heights[0], 0.00f),
                        new VECTOR(1.00f, heights[1], 0.00f),
                        new VECTOR(1.00f, heights[2], 1.00f),
                        new VECTOR(0.00f, heights[3], 1.00f)
                    });
                    sumNormals.Add(quad.GetNormal(useMoreAccurateCalculations));
                }
            }

            // Gather a list of all quad normals to use for averaging the vertex normal.
            // On the edges of maps, there are fewer adjected polys to the vertex,
            // so only add normals if they exist.
            TryAddQuadNormal(vertexX - 1, vertexY - 1);
            TryAddQuadNormal(vertexX - 0, vertexY - 1);
            TryAddQuadNormal(vertexX - 0, vertexY - 0);
            TryAddQuadNormal(vertexX - 1, vertexY - 0);

            // Return the average of each normal (normalized) for Gouraud shading.
            return VECTOR.GetAbnormalFromNormals(sumNormals.ToArray());
        }
    }
}
