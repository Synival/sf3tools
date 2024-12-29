using CommonLib.SGL;
using OpenTK.Mathematics;

namespace SF3.Win.Extensions {
    public static class VectorExtensions {
        public static float[] ToFloatArray(this Vector2 vec2)
            => [vec2.X, vec2.Y];

        public static float[] ToFloatArray(this Vector3 vec3)
            => [vec3.X, vec3.Y, vec3.Z];

        public static float[] ToFloatArray(this Vector4 vec4)
            => [vec4.X, vec4.Y, vec4.Z, vec4.W];

        public static Vector3 ToVector3(this VECTOR vec)
            => new(vec.X.Float, vec.Y.Float, vec.Z.Float);
    }
}
