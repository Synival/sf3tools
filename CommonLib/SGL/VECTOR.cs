using System;
using static CommonLib.Utils.Abnormals;

namespace CommonLib.SGL {
    public struct VECTOR {
        public VECTOR(FIXED x, FIXED y, FIXED z) {
            X = x;
            Y = y;
            Z = z;
        }

        public VECTOR(short x, short y, short z) {
            X = new FIXED(x, false);
            Y = new FIXED(y, false);
            Z = new FIXED(z, false);
        }

        public VECTOR(float x, float y, float z) {
            X = new FIXED(x, 0);
            Y = new FIXED(y, 0);
            Z = new FIXED(z, 0);
        }

        public FIXED X;
        public FIXED Y;
        public FIXED Z;

        public override bool Equals(object obj)
            => obj is VECTOR vector && X == vector.X && Y == vector.Y && Z == vector.Z;

        public override int GetHashCode() {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            "{" + X.Float + ", " + Y.Float + ", " + Z.Float + "}";

        public static bool operator ==(VECTOR lhs, VECTOR rhs)
            => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

        public static bool operator !=(VECTOR lhs, VECTOR rhs)
            => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;

        public static VECTOR operator +(VECTOR lhs, VECTOR rhs)
            => new VECTOR(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);

        public static VECTOR operator -(VECTOR lhs, VECTOR rhs)
            => new VECTOR(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);

        public static VECTOR operator *(float mult, VECTOR rhs) => rhs * mult;
        public static VECTOR operator *(VECTOR lhs, float mult) {
            return new VECTOR(
                lhs.X.Float * mult,
                lhs.Y.Float * mult,
                lhs.Z.Float * mult
            );
        }

        public static VECTOR operator /(VECTOR lhs, float div) {
            return new VECTOR(
                lhs.X.Float / div,
                lhs.Y.Float / div,
                lhs.Z.Float / div
            );
        }

        public static VECTOR Cross(VECTOR lhs, VECTOR rhs) {
            return new VECTOR(
                lhs.Y.Float * rhs.Z.Float - lhs.Z.Float * rhs.Y.Float,
                lhs.Z.Float * rhs.X.Float - lhs.X.Float * rhs.Z.Float,
                lhs.X.Float * rhs.Y.Float - lhs.Y.Float * rhs.X.Float
            );
        }

        public float GetLength() => (float) Math.Sqrt(X.Float * X.Float + Y.Float * Y.Float + Z.Float * Z.Float);

        public VECTOR Normalized() {
            var length = GetLength();
            return (length == 0) ? this : this / length;
        }

        public VECTOR WeightedForAbnormalBlending() {
            var nvec = Normalized();
            return nvec * (float) NormalWeightForAbnormalBlending(nvec.Y.Float);
        }

        public static VECTOR GetAbnormalFromNormals(VECTOR[] normals) {
            VECTOR sum = new VECTOR(0, 0, 0);
            foreach (var n in normals)
                sum += n.WeightedForAbnormalBlending();
            return sum.Normalized().GetAbnormalFromNormal();
        }

        public VECTOR GetAbnormalFromNormal() {
            if (X == 0 && Z == 0)
                return new VECTOR(new FIXED(0, true), new FIXED(1, true), new FIXED(0, true));

            var xzTheta    = (float) Math.Atan2(Z.Float, X.Float);
            var xzMag      = Math.Max(-1.00, Math.Min(1.00, (float) Math.Sqrt(X.Float * X.Float + Z.Float * Z.Float)));
            var xzAbnormal = (float) NormalXzToAbnormalXz(xzMag);
            var yAbnormal  = (float) NormalYToAbnormalY(Y.Float);

            var vec = new VECTOR(
                xzAbnormal * (float) Math.Cos(xzTheta),
                yAbnormal,
                xzAbnormal * (float) -Math.Sin(xzTheta)
            );

            return vec;
        }
    }
}
