using CommonLib.SGL;
using OpenTK.Mathematics;

namespace SF3.Win.Extensions {
    public static class VECTOR_Extensions {
        public static Vector3 ToOpenTKVector3(this VECTOR vec)
            => new(vec.X.Float, vec.Y.Float, vec.Z.Float);

        public static Vector4 ToOpenTKVector4(this VECTOR vec)
            => new(vec.X.Float, vec.Y.Float, vec.Z.Float, 1);
    }
}
