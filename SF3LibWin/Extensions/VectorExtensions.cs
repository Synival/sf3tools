using CommonLib.SGL;
using OpenTK.Mathematics;

namespace SF3.Win.Extensions {
    public static class VectorExtensions {
        public static float[] ToFloatArray(this Vector2 vec)
            => [vec.X, vec.Y];

        public static float[] ToFloatArray(this Vector3 vec)
            => [vec.X, vec.Y, vec.Z];

        public static float[] ToFloatArray(this Vector4 vec)
            => [vec.X, vec.Y, vec.Z, vec.W];

        public static Vector2 ToOpenTKVector2(this System.Numerics.Vector2 vec)
            => new(vec.X, vec.Y);

        public static Vector3 ToOpenTKVector3(this System.Numerics.Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

        public static Vector4 ToOpenTKVector4(this System.Numerics.Vector4 vec)
            => new(vec.X, vec.Y, vec.Z, vec.W);

        public static System.Numerics.Vector2 ToNumericsVector2(this Vector2 vec)
            => new(vec.X, vec.Y);

        public static System.Numerics.Vector3 ToNumericsVector3(this Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

        public static System.Numerics.Vector4 ToNumericsVector4(this Vector4 vec)
            => new(vec.X, vec.Y, vec.Z, vec.W);

        public static VECTOR ToVECTOR(this Vector3 vec)
            => new(vec.X, vec.Y, vec.Z);

    }
}
