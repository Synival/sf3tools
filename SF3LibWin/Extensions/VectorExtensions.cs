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

        public static Vector4 ToVector4(this VECTOR vec)
            => new(vec.X.Float, vec.Y.Float, vec.Z.Float, 1);

        public static Vector2 ToOpenTKVector(this System.Numerics.Vector2 vec)
            => new(vec.X, vec.Y);

        public static Vector3 ToOpenTKVector(this System.Numerics.Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

        public static Vector4 ToOpenTKVector(this System.Numerics.Vector4 vec)
            => new(vec.X, vec.Y, vec.Z, vec.W);

        public static System.Numerics.Vector2 ToNumericsVector(this Vector2 vec)
            => new(vec.X, vec.Y);

        public static System.Numerics.Vector3 ToNumericsVector(this Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

        public static System.Numerics.Vector4 ToNumericsVector(this Vector4 vec)
            => new(vec.X, vec.Y, vec.Z, vec.W);

        public static VECTOR ToVECTOR(this System.Numerics.Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

        public static VECTOR ToVECTOR(this Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

    }
}
