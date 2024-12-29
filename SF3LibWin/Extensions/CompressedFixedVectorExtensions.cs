using CommonLib.SGL;
using OpenTK.Mathematics;

namespace SF3.Win.Extensions.CompressedFixedVectorExtensions {
    public static class CompressedFixedVectorExtensions {
        public static Vector3 ToVector3(this VECTOR vec)
            => new(vec.X.Float, vec.Y.Float, vec.Z.Float);
    }
}