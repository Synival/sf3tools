using System;
using System.Numerics;

namespace CommonLib.Extensions {
    public static class QuaternionExtensions {
        public static float AngularDistance(this Quaternion q1, Quaternion q2) {
            float dot = Quaternion.Dot(q1, q2);
            float absDot = Math.Min(Math.Abs(dot), 1.0f);
            return 2.0f * (float) Math.Acos(absDot);
        }

        public static float AngularDistanceInDegrees(Quaternion q1, Quaternion q2)
            => AngularDistance(q1, q2) * (180.0f / (float) Math.PI);
    }
}
