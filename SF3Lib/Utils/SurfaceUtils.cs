using System;
using CommonLib.SGL;
using CommonLib.Types;

namespace SF3.Utils {
    public static class SurfaceUtils {
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

        public struct TileAndCorner {
            public int X;
            public int Y;
            public CornerType Corner;
        }

        public static TileAndCorner[] GetSharedTilesAtCorner(int tx, int ty, CornerType corner) {
            var tileAndCorners = new TileAndCorner[4];
            var index = 0;

            void AddTileAndCorner(int x, int y, CornerType c) {
                if (x >= 0 && y >= 0 && x < 64 && y < 64)
                    tileAndCorners[index++] = new TileAndCorner() { X = x, Y = y, Corner = c };
            }

            switch (corner) {
                case CornerType.TopLeft:
                    AddTileAndCorner(tx,     ty,     corner);
                    AddTileAndCorner(tx - 1, ty + 1, CornerType.BottomRight);
                    AddTileAndCorner(tx - 1, ty + 0, CornerType.TopRight);
                    AddTileAndCorner(tx - 0, ty + 1, CornerType.BottomLeft);
                    break;

                case CornerType.TopRight:
                    AddTileAndCorner(tx,     ty,     corner);
                    AddTileAndCorner(tx + 1, ty + 1, CornerType.BottomLeft);
                    AddTileAndCorner(tx + 1, ty + 0, CornerType.TopLeft);
                    AddTileAndCorner(tx + 0, ty + 1, CornerType.BottomRight);
                    break;

                case CornerType.BottomRight:
                    AddTileAndCorner(tx,     ty,     corner);
                    AddTileAndCorner(tx + 1, ty - 1, CornerType.TopLeft);
                    AddTileAndCorner(tx + 1, ty - 0, CornerType.BottomLeft);
                    AddTileAndCorner(tx + 0, ty - 1, CornerType.TopRight);
                    break;

                case CornerType.BottomLeft:
                    AddTileAndCorner(tx,     ty,     corner);
                    AddTileAndCorner(tx - 1, ty - 1, CornerType.TopRight);
                    AddTileAndCorner(tx - 1, ty - 0, CornerType.BottomRight);
                    AddTileAndCorner(tx - 0, ty - 1, CornerType.TopLeft);
                    break;

                default:
                    throw new ArgumentException(nameof(corner));
            }

            if (index != 4)
                Array.Resize(ref tileAndCorners, index);
            return tileAndCorners;
        }

        /// <summary>
        /// Converts a tile X coordinate and corner to a vertex coordinate.
        /// </summary>
        /// <param name="tileX">X coordinate of the requested tile.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileX'.</param>
        /// <returns>A vertex X coordinate.</returns>
        public static int TileToVertexX(int tileX, CornerType corner)
            => tileX + corner.GetVertexOffsetX();

        /// <summary>
        /// Converts a tile Y coordinate and corner to a vertex coordinate.
        /// </summary>
        /// <param name="tileY">Y coordinate of the requested tile.</param>
        /// <param name="corner">Corner of the tile referenced by 'tileY'.</param>
        /// <returns>A vertex Y coordinate.</returns>
        public static int TileToVertexY(int tileY, CornerType corner)
            => tileY + corner.GetVertexOffsetY();
    }
}
