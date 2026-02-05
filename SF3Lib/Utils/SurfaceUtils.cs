using System;
using CommonLib.SGL;

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
    }
}
