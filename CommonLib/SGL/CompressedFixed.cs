namespace CommonLib.SGL {
    public struct CompressedFIXED {
        public CompressedFIXED(short sh) {
            RawShort = sh;
        }

        /// <summary>
        /// Creates a CompressedFIXED with an input float.
        /// </summary>
        /// <param name="f">Float value of the new CompressedFIXED.</param>
        /// <param name="_">Deliberately unused, only to prevent accidental construction with the 'int' constructor.</param>
        public CompressedFIXED(float f, int _) : this((f == 1.00f) ? ((short) 32767) : (short) (f * 32768f)) { }

        public CompressedFIXED(FIXED f) : this((short) (f.RawInt / 2)) { }

        public short RawShort { get; set; }

        public float Float {
            get => RawShort / 32768f;
            set => RawShort = (short) (value * 32768f);
        }

        public override bool Equals(object obj) {
            return (obj is CompressedFIXED cf && RawShort == cf.RawShort) ||
                   (obj is FIXED f && RawShort * 2 == f.RawInt);
        }

        public override int GetHashCode()
            => RawShort.GetHashCode();

        public override string ToString()
            => Float.ToString();

        // Comparisons / operators
        public static bool operator ==(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort == rhs.RawShort;
        public static bool operator !=(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort == rhs.RawShort;
        public static bool operator <(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort < rhs.RawShort;
        public static bool operator >(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort > rhs.RawShort;
        public static bool operator <=(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort <= rhs.RawShort;
        public static bool operator >=(CompressedFIXED lhs, CompressedFIXED rhs) => lhs.RawShort >= rhs.RawShort;

        public static CompressedFIXED operator +(CompressedFIXED lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs.RawShort + rhs.RawShort));
        public static CompressedFIXED operator -(CompressedFIXED lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs.RawShort - rhs.RawShort));

        // Short comparisons / operators (RHS)
        public static bool operator ==(CompressedFIXED lhs, short rhs) => lhs.RawShort == rhs;
        public static bool operator !=(CompressedFIXED lhs, short rhs) => lhs.RawShort == rhs;
        public static bool operator <(CompressedFIXED lhs, short rhs) => lhs.RawShort < rhs;
        public static bool operator >(CompressedFIXED lhs, short rhs) => lhs.RawShort > rhs;
        public static bool operator <=(CompressedFIXED lhs, short rhs) => lhs.RawShort <= rhs;
        public static bool operator >=(CompressedFIXED lhs, short rhs) => lhs.RawShort >= rhs;

        public static CompressedFIXED operator +(CompressedFIXED lhs, short rhs) => new CompressedFIXED((short) (lhs.RawShort + rhs));
        public static CompressedFIXED operator -(CompressedFIXED lhs, short rhs) => new CompressedFIXED((short) (lhs.RawShort - rhs));

        // Short comparisons / operators (LHS)
        public static bool operator ==(short lhs, CompressedFIXED rhs) => lhs == rhs.RawShort;
        public static bool operator !=(short lhs, CompressedFIXED rhs) => lhs == rhs.RawShort;
        public static bool operator <(short lhs, CompressedFIXED rhs) => lhs < rhs.RawShort;
        public static bool operator >(short lhs, CompressedFIXED rhs) => lhs > rhs.RawShort;
        public static bool operator <=(short lhs, CompressedFIXED rhs) => lhs <= rhs.RawShort;
        public static bool operator >=(short lhs, CompressedFIXED rhs) => lhs >= rhs.RawShort;

        public static CompressedFIXED operator +(short lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs + rhs.RawShort));
        public static CompressedFIXED operator -(short lhs, CompressedFIXED rhs) => new CompressedFIXED((short) (lhs - rhs.RawShort));

        // Float comparisons / operators (RHS)
        public static bool operator ==(CompressedFIXED lhs, float rhs) => lhs.Float == rhs;
        public static bool operator !=(CompressedFIXED lhs, float rhs) => lhs.Float == rhs;
        public static bool operator <(CompressedFIXED lhs, float rhs) => lhs.Float < rhs;
        public static bool operator >(CompressedFIXED lhs, float rhs) => lhs.Float > rhs;
        public static bool operator <=(CompressedFIXED lhs, float rhs) => lhs.Float <= rhs;
        public static bool operator >=(CompressedFIXED lhs, float rhs) => lhs.Float >= rhs;

        public static CompressedFIXED operator +(CompressedFIXED lhs, float rhs) => new CompressedFIXED(lhs.Float + rhs, 0);
        public static CompressedFIXED operator -(CompressedFIXED lhs, float rhs) => new CompressedFIXED(lhs.Float - rhs, 0);

        // Float comparisons / operators (LHS)
        public static bool operator ==(float lhs, CompressedFIXED rhs) => lhs == rhs.Float;
        public static bool operator !=(float lhs, CompressedFIXED rhs) => lhs == rhs.Float;
        public static bool operator <(float lhs, CompressedFIXED rhs) => lhs < rhs.Float;
        public static bool operator >(float lhs, CompressedFIXED rhs) => lhs > rhs.Float;
        public static bool operator <=(float lhs, CompressedFIXED rhs) => lhs <= rhs.Float;
        public static bool operator >=(float lhs, CompressedFIXED rhs) => lhs >= rhs.Float;

        public static CompressedFIXED operator +(float lhs, CompressedFIXED rhs) => new CompressedFIXED(lhs + rhs.Float, 0);
        public static CompressedFIXED operator -(float lhs, CompressedFIXED rhs) => new CompressedFIXED(lhs - rhs.Float, 0);

        // Other operations
        public static CompressedFIXED operator -(CompressedFIXED f) => new CompressedFIXED((short) -f.RawShort);

    }
}
