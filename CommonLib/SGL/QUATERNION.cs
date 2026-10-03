using System;
using Newtonsoft.Json.Linq;

namespace CommonLib.SGL {
    public struct QUATERNION {
        public QUATERNION(FIXED x, FIXED y, FIXED z, FIXED w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public QUATERNION(int x, int y, int z, int w, bool isRaw) {
            X = new FIXED(x, isRaw);
            Y = new FIXED(y, isRaw);
            Z = new FIXED(z, isRaw);
            W = new FIXED(w, isRaw);
        }

        public QUATERNION(short x, short y, short z, short w) {
            X = new FIXED(x, false);
            Y = new FIXED(y, false);
            Z = new FIXED(z, false);
            W = new FIXED(w, false);
        }

        public QUATERNION(float x, float y, float z, float w) {
            X = new FIXED(x, 0);
            Y = new FIXED(y, 0);
            Z = new FIXED(z, 0);
            W = new FIXED(w, 0);
        }

        public QUATERNION(QUATERNION original) {
            X = original.X;
            Y = original.Y;
            Z = original.Z;
            W = original.W;
        }

        public static QUATERNION FromJToken(JToken token) => new QUATERNION(token);
        private QUATERNION(JToken token) {
            var jObject = (JObject) token;
            X = new FIXED((float) jObject["X"], 0);
            Y = new FIXED((float) jObject["Y"], 0);
            Z = new FIXED((float) jObject["Z"], 0);
            W = new FIXED((float) jObject["W"], 0);
        }

        public FIXED X;
        public FIXED Y;
        public FIXED Z;
        public FIXED W;

        public override bool Equals(object obj)
            => obj is QUATERNION q && X == q.X && Y == q.Y && Z == q.Z && W == q.W;

        public override int GetHashCode() {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            "{" + X.Float + ", " + Y.Float + ", " + Z.Float + ", " + W.Float + "}";

        public static bool operator ==(QUATERNION lhs, QUATERNION rhs)
            => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z && lhs.W == rhs.W;

        public static bool operator !=(QUATERNION lhs, QUATERNION rhs)
            => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z || lhs.W != rhs.W;

        public static QUATERNION operator +(QUATERNION lhs, QUATERNION rhs)
            => new QUATERNION(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);

        public static QUATERNION operator -(QUATERNION lhs, QUATERNION rhs)
            => new QUATERNION(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);

        public static QUATERNION operator *(float mult, QUATERNION rhs) => rhs * mult;
        public static QUATERNION operator *(QUATERNION lhs, float mult) {
            return new QUATERNION(
                lhs.X.Float * mult,
                lhs.Y.Float * mult,
                lhs.Z.Float * mult,
                lhs.W.Float * mult
            );
        }

        public static QUATERNION operator /(QUATERNION lhs, float div) {
            return new QUATERNION(
                lhs.X.Float / div,
                lhs.Y.Float / div,
                lhs.Z.Float / div,
                lhs.W.Float / div
            );
        }

        public JObject ToJObject() {
            return new JObject {
                { "X", X.Float },
                { "Y", Y.Float },
                { "Z", Z.Float },
                { "W", W.Float },
            };
        }

        public void Normalize() {
            float x = X.Float, y = Y.Float, z = Z.Float, w = W.Float;
            var length = (float) Math.Sqrt(x*x + y*y + z*z + w*w);
            X.Float /= length;
            Y.Float /= length;
            Z.Float /= length;
            W.Float /= length;
        }

        public void Negate() {
            X = -X;
            Y = -Y;
            Z = -Z;
            W = -W;
        }

        public float Dot(QUATERNION q2) {
            return
                X.Float * q2.X.Float +
                Y.Float * q2.Y.Float +
                Z.Float * q2.Z.Float +
                W.Float * q2.W.Float;
        }

        public float AngularDistance(QUATERNION q2) {
            float dot = Dot(q2);
            float absDot = Math.Min(Math.Abs(dot), 1.0f);
            return 2.0f * (float) Math.Acos(absDot);
        }

        public float AngularDistanceInDegrees(QUATERNION q2)
            => AngularDistance(q2) * (180.0f / (float) Math.PI);

        public float GetLerpDist(QUATERNION rot2) {
            return (float) (
                Math.Abs(X.Float - rot2.X.Float) +
                Math.Abs(Y.Float - rot2.Y.Float) +
                Math.Abs(Z.Float - rot2.Z.Float) +
                Math.Abs(W.Float - rot2.W.Float)
            );
        }
    }
}
