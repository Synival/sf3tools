namespace CommonLib.SGL {
    public struct CompressedFIXED {
        /// <summary>
        /// Creates a CompressedFIXED with an input short.
        /// </summary>
        /// <param name="rawShort">RawShort value of the new CompressedFIXED.</param>
        /// <param name="fracBits">Number of bits dedicated to the fractional component.</param>
        public CompressedFIXED(short rawShort, int fracBits) {
            RawShort = rawShort;
            _fracBits = fracBits;
            _one = (short) (1 << fracBits);
        }

        /// <summary>
        /// Creates a CompressedFIXED with an input float.
        /// </summary>
        /// <param name="f">Float value of the new CompressedFIXED.</param>
        /// <param name="fracBits">Number of bits dedicated to the fractional component.</param>
        /// <param name="_">Deliberately unused, only to prevent accidental construction with the 'int' constructor.</param>
        public CompressedFIXED(float f, int fracBits, int _) : this((short) (f * (1 << fracBits)), fracBits) { }

        public CompressedFIXED(FIXED f, int fracBits) : this((short) (f.RawInt >> (16 - fracBits)), fracBits) { }

        public short RawShort { get; set; }
        private short _one;

        private int _fracBits;
        public int FracBits {
            get => _fracBits;
            set {
                _fracBits = value;
                _one = (short) (1 << value);
            }
        }

        public float Float {
            get => RawShort / (float) _one;
            set => RawShort = (short) (value * (float) _one);
        }

        public override bool Equals(object obj) {
            return (obj is CompressedFIXED cf && RawShort == cf.RawShort) ||
                   (obj is FIXED f && RawShort << (16 - FracBits) == f.RawInt);
        }

        public override int GetHashCode()
            => RawShort.GetHashCode();

        public override string ToString()
            => Float.ToString();

        // Comparisons / operators
        // TODO: Check IntBits!!!
        public static bool operator ==(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort == rhs.RawShort;
        public static bool operator !=(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort == rhs.RawShort;
        public static bool operator <(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort < rhs.RawShort;
        public static bool operator >(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort > rhs.RawShort;
        public static bool operator <=(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort <= rhs.RawShort;
        public static bool operator >=(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort >= rhs.RawShort;

        // TODO: Check IntBits!!!
        public static CompressedFIXED operator +(CompressedFIXED lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs.RawShort + rhs.RawShort), lhs.FracBits);
        public static CompressedFIXED operator -(CompressedFIXED lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs.RawShort - rhs.RawShort), lhs.FracBits);

        // Short comparisons / operators (RHS)
        // TODO: Check IntBits!!!
        public static bool operator ==(CompressedFIXED lhs, short rhs) => lhs.RawShort == rhs;
        public static bool operator !=(CompressedFIXED lhs, short rhs) => lhs.RawShort == rhs;
        public static bool operator <(CompressedFIXED lhs, short rhs) => lhs.RawShort < rhs;
        public static bool operator >(CompressedFIXED lhs, short rhs) => lhs.RawShort > rhs;
        public static bool operator <=(CompressedFIXED lhs, short rhs) => lhs.RawShort <= rhs;
        public static bool operator >=(CompressedFIXED lhs, short rhs) => lhs.RawShort >= rhs;

        // TODO: Check IntBits!!!
        public static CompressedFIXED operator +(CompressedFIXED lhs, short rhs) => new CompressedFIXED((short) (lhs.RawShort + rhs), lhs.FracBits);
        public static CompressedFIXED operator -(CompressedFIXED lhs, short rhs) => new CompressedFIXED((short) (lhs.RawShort - rhs), lhs.FracBits);

        // Short comparisons / operators (LHS)
        // TODO: Check IntBits!!!
        public static bool operator ==(short lhs, CompressedFIXED rhs) => lhs == rhs.RawShort;
        public static bool operator !=(short lhs, CompressedFIXED rhs) => lhs == rhs.RawShort;
        public static bool operator <(short lhs, CompressedFIXED rhs) => lhs < rhs.RawShort;
        public static bool operator >(short lhs, CompressedFIXED rhs) => lhs > rhs.RawShort;
        public static bool operator <=(short lhs, CompressedFIXED rhs) => lhs <= rhs.RawShort;
        public static bool operator >=(short lhs, CompressedFIXED rhs) => lhs >= rhs.RawShort;

        // TODO: Check IntBits!!!
        public static CompressedFIXED operator +(short lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs + rhs.RawShort), rhs.FracBits);
        public static CompressedFIXED operator -(short lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs - rhs.RawShort), rhs.FracBits);

        // Float comparisons / operators (RHS)
        // TODO: Check IntBits!!!
        public static bool operator ==(CompressedFIXED lhs, float rhs) => lhs.Float == rhs;
        public static bool operator !=(CompressedFIXED lhs, float rhs) => lhs.Float == rhs;
        public static bool operator <(CompressedFIXED lhs, float rhs) => lhs.Float < rhs;
        public static bool operator >(CompressedFIXED lhs, float rhs) => lhs.Float > rhs;
        public static bool operator <=(CompressedFIXED lhs, float rhs) => lhs.Float <= rhs;
        public static bool operator >=(CompressedFIXED lhs, float rhs) => lhs.Float >= rhs;

        // TODO: Check IntBits!!!
        public static CompressedFIXED operator +(CompressedFIXED lhs, float rhs) => new CompressedFIXED(lhs.Float + rhs, lhs.FracBits, 0);
        public static CompressedFIXED operator -(CompressedFIXED lhs, float rhs) => new CompressedFIXED(lhs.Float - rhs, lhs.FracBits, 0);

        // Float comparisons / operators (LHS)
        // TODO: Check IntBits!!!
        public static bool operator ==(float lhs, CompressedFIXED rhs) => lhs == rhs.Float;
        public static bool operator !=(float lhs, CompressedFIXED rhs) => lhs == rhs.Float;
        public static bool operator <(float lhs, CompressedFIXED rhs) => lhs < rhs.Float;
        public static bool operator >(float lhs, CompressedFIXED rhs) => lhs > rhs.Float;
        public static bool operator <=(float lhs, CompressedFIXED rhs) => lhs <= rhs.Float;
        public static bool operator >=(float lhs, CompressedFIXED rhs) => lhs >= rhs.Float;

        // TODO: Check IntBits!!!
        public static CompressedFIXED operator +(float lhs, CompressedFIXED rhs) => new CompressedFIXED(lhs + rhs.Float, rhs.FracBits, 0);
        public static CompressedFIXED operator -(float lhs, CompressedFIXED rhs) => new CompressedFIXED(lhs - rhs.Float, rhs.FracBits, 0);

        // Other operations
        // TODO: Check IntBits!!!
        public static CompressedFIXED operator -(CompressedFIXED f) => new CompressedFIXED((short) -f.RawShort, f.FracBits);
    }
}
