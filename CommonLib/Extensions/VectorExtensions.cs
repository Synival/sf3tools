using System.Numerics;
using CommonLib.SGL;

namespace CommonLib.Extensions {
    public static class VectorExtensions {
        public static float[] ToFloatArray(this Vector2 vec2)
            => new float[] { vec2.X, vec2.Y };

        public static float[] ToFloatArray(this Vector3 vec3)
            => new float[] { vec3.X, vec3.Y, vec3.Z };

        public static float[] ToFloatArray(this Vector4 vec4)
            => new float[] { vec4.X, vec4.Y, vec4.Z, vec4.W };

        public static VECTOR ToVECTOR(this Vector3 vec)
            => new VECTOR(vec.X, vec.Y, vec.Z);

        public static Vector3 ToSwappedYZ(this Vector3 vec)
            => new Vector3(vec.X, -vec.Y, -vec.Z);

        public static Vector4 ToSwappedYZ(this Vector4 vec)
            => new Vector4(vec.X, -vec.Y, -vec.Z, vec.W);
    }
}
