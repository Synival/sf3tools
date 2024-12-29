namespace CommonLib.SGL {
    public struct CompressedFixed {
        public CompressedFixed(short sh) {
            Short = sh;
        }

        public CompressedFixed(float f) {
            Short = (short) (f * 32768f);
        }

        public short Short { get; set; }

        public float Float {
            get => Short / 32768f;
            set => Short = (short) (value * 32768f);
        }

        public override bool Equals(object obj)
            => obj is CompressedFixed cf && Short == cf.Short && Float == cf.Float;

        public override int GetHashCode()
            => Short.GetHashCode();

        public override string ToString()
            => Float.ToString();

        public static bool operator ==(CompressedFixed lhs, CompressedFixed rhs) => lhs.Short == rhs.Short;
        public static bool operator !=(CompressedFixed lhs, CompressedFixed rhs) => lhs.Short == rhs.Short;
        public static bool operator <(CompressedFixed lhs, CompressedFixed rhs) => lhs.Short < rhs.Short;
        public static bool operator >(CompressedFixed lhs, CompressedFixed rhs) => lhs.Short > rhs.Short;
        public static bool operator <=(CompressedFixed lhs, CompressedFixed rhs) => lhs.Short <= rhs.Short;
        public static bool operator >=(CompressedFixed lhs, CompressedFixed rhs) => lhs.Short >= rhs.Short;
    }
}
