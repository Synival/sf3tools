namespace CommonLib.SGL {
    public struct CompressedFIXED {
        /// <summary>
        /// Creates a CompressedFIXED with an input short.
        /// </summary>
        /// <param name="sh">RawShort value of the new CompressedFIXED.</param>
        public CompressedFIXED(short sh) {
            RawShort = sh;
        }

        /// <summary>
        /// Creates a CompressedFIXED with an input short. If 'isWeird' is 'true', then the raw value is a "weird"
        /// compressed FIXED used in Shining Force 3's surface normal vertex vectors. These "weird" values have the
        /// MSB on the *right* side, with all other bits shifted 1 to the left. Weird, huh?
        /// </summary>
        /// <param name="sh">RawShort value of the new CompressedFIXED.</param>
        /// <param name="isWierd">When 'true', interpets 'sh' as a "weird" short (see summary).</param>
        public CompressedFIXED(ushort sh, bool isWeird) {
            RawShort = (short) (isWeird ? (((sh & 0x0001) << 15) | (sh >> 1)) : sh);
        }

        /// <summary>
        /// Creates a CompressedFIXED with an input float.
        /// </summary>
        /// <param name="f">Float value of the new CompressedFIXED.</param>
        /// <param name="_">Deliberately unused, only to prevent accidental construction with the 'int' constructor.</param>
        public CompressedFIXED(float f, int _) : this((f == 1.00f) ? ((short) 0x7FFF) : (short) (f * 0x8000)) { }

        public CompressedFIXED(FIXED f) : this((short) (f.RawInt / 2)) { }

        public short RawShort { get; set; }

        /// <summary>
        /// Shining Force 3's surface model vertex normals use this "weird" version of a compressed FIXED where the MSB
        /// (-0x8000) is stored at the *right* end, and every other bit is shifted left by 1 bit. Weird, huh?
        /// </summary>
        public ushort WeirdRawShort => (ushort) (((((ushort) RawShort) & 0x8000) >> 15) | (((ushort) RawShort) << 1));

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
