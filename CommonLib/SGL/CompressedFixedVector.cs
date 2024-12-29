namespace CommonLib.SGL {
    public struct CompressedFixedVector {
        public CompressedFixedVector(CompressedFixed x, CompressedFixed y, CompressedFixed z) {
            X = x;
            Y = y;
            Z = z;
        }

        public CompressedFixed X;
        public CompressedFixed Y;
        public CompressedFixed Z;

        public override bool Equals(object obj)
            => obj is CompressedFixedVector vector && X == vector.X && Y == vector.Y && Z == vector.Z;

        public override int GetHashCode() {
            var hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            "{" + X.Float + ", " + Y.Float + ", " + Z.Float + "}";

        public static bool operator ==(CompressedFixedVector lhs, CompressedFixedVector rhs)
            => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

        public static bool operator !=(CompressedFixedVector lhs, CompressedFixedVector rhs)
            => lhs.X != rhs.X || lhs.Y != rhs.Y || lhs.Z != rhs.Z;
    }
}
