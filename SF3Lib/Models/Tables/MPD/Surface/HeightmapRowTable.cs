using System;
using System.Collections.Generic;
using CommonLib.SGL;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Surface;
using static CommonLib.Utils.BlockHelpers;

namespace SF3.Models.Tables.MPD.Surface {
    public class HeightmapRowTable : FixedSizeTable<HeightmapRow> {
        protected HeightmapRowTable(IByteData data, int address) : base(data, address, 64) {
        }

        public static HeightmapRowTable Create(IByteData data, int address) {
            var newTable = new HeightmapRowTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            var size = new HeightmapRow(Data, 0, "", Address).Size;
            return Load((id, address) => new HeightmapRow(Data, id, "Y" + id.ToString("D2"), Address + id * size));
        }

        /// <summary>
        /// Calculates the "normal" for a tile at a given corner.
        /// </summary>
        /// <param name="tileX">X coordinate of the tile.</param>
        /// <param name="tileY">Y coordinate of the tile.</param>
        /// <param name="corner">Corner of the tile whose vertex normal should be calculated.</param>
        /// <param name="calculationMethod">The calculations used for determining the normal for each part of the heightmap.</param>
        /// <returns>A freshly-calculated normal for the vertex requested.</returns>
        public VECTOR CalculateVertexNormal(int tileX, int tileY, CornerType corner, POLYGON_NormalCalculationMethod calculationMethod)
            => CalculateVertexNormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), calculationMethod);

        /// <summary>
        /// Calculates the vertex normal for a specific vertex of a tile.
        /// </summary>
        /// <param name="vertexX">X coordinate of the vertex.</param>
        /// <param name="vertexY">Y coordinate of the vertex.</param>
        /// <param name="calculationMethod">The calculations used for determining the normal for each part of the heightmap.</param>
        /// <returns>A freshly-calculated normal for the vertex requested.</returns>
        public VECTOR CalculateVertexNormal(int vertexX, int vertexY, POLYGON_NormalCalculationMethod calculationMethod) {
            // Determine the normals of the 4 quads surrounding the vertex.
            var sumNormals = new List<VECTOR>();

            void TryAddQuadNormal(int vx, int vy) {
                if (vx >= 0 && vy >= 0 && vx <= 63 && vy <= 63) {
                    var heights = Rows[vy].GetQuadHeights(vx);
                    // SF3 intentionally cuts quad height by half when calculating surface vertex normals.
                    var quad = new POLYGON(new VECTOR[] {
                        new VECTOR(0.00f, heights[0] / 2, 1.00f),
                        new VECTOR(1.00f, heights[1] / 2, 1.00f),
                        new VECTOR(1.00f, heights[2] / 2, 0.00f),
                        new VECTOR(0.00f, heights[3] / 2, 0.00f)
                    });
                    sumNormals.Add(quad.GetNormal(calculationMethod));
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
            var components = new float[3];
            foreach (var normal in sumNormals) {
                components[0] += normal.X.Float;
                components[1] += normal.Y.Float;
                components[2] += normal.Z.Float;
            }

            var count = sumNormals.Count;
            var vec = new VECTOR(
                components[0] / count,
                components[1] / count,
                components[2] / count
            ).Normalized();

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
    }
}
