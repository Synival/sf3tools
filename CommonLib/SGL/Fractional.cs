namespace CommonLib.SGL {
    public struct Fractional {
        /// <summary>
        /// Creates a CompressedFIXED with an input short.
        /// </summary>
        /// <param name="sh">RawShort value of the new CompressedFIXED.</param>
        public Fractional(short sh) {
            RawShort = sh;
        }

        /// <summary>
        /// Creates a CompressedFIXED with an input short. If 'isWeird' is 'true', then the raw value is a "weird"
        /// compressed FIXED used in Shining Force 3's surface normal vertex vectors. These "weird" values have the
        /// MSB on the *right* side, with all other bits shifted 1 to the left. Weird, huh?
        /// </summary>
        /// <param name="sh">RawShort value of the new CompressedFIXED.</param>
        /// <param name="isWierd">When 'true', interpets 'sh' as a "weird" short (see summary).</param>
        public Fractional(ushort sh, bool isWeird) {
            RawShort = (short) (isWeird ? (((sh & 0x0001) << 15) | (sh >> 1)) : sh);
        }

        /// <summary>
        /// Creates a CompressedFIXED with an input float.
        /// </summary>
        /// <param name="f">Float value of the new CompressedFIXED.</param>
        /// <param name="_">Deliberately unused, only to prevent accidental construction with the 'int' constructor.</param>
        public Fractional(float f, int _) : this((f == 1.00f) ? ((short) 0x7FFF) : (short) (f * 0x8000)) { }

        public Fractional(FIXED f) : this((short) (f.RawInt / 2)) { }

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
            return (obj is Fractional cf && RawShort == cf.RawShort) ||
                   (obj is FIXED f && RawShort * 2 == f.RawInt);
        }

        public override int GetHashCode()
            => RawShort.GetHashCode();

        public override string ToString()
            => Float.ToString();

        // Comparisons / operators
        public static bool operator ==(Fractional lhs, Fractional rhs) => lhs.RawShort == rhs.RawShort;
        public static bool operator !=(Fractional lhs, Fractional rhs) => lhs.RawShort == rhs.RawShort;
        public static bool operator <(Fractional lhs, Fractional rhs) => lhs.RawShort < rhs.RawShort;
        public static bool operator >(Fractional lhs, Fractional rhs) => lhs.RawShort > rhs.RawShort;
        public static bool operator <=(Fractional lhs, Fractional rhs) => lhs.RawShort <= rhs.RawShort;
        public static bool operator >=(Fractional lhs, Fractional rhs) => lhs.RawShort >= rhs.RawShort;

        public static Fractional operator +(Fractional lhs, Fractional rhs) => new Fractional((short) (lhs.RawShort + rhs.RawShort));
        public static Fractional operator -(Fractional lhs, Fractional rhs) => new Fractional((short) (lhs.RawShort - rhs.RawShort));

        // Short comparisons / operators (RHS)
        public static bool operator ==(Fractional lhs, short rhs) => lhs.RawShort == rhs;
        public static bool operator !=(Fractional lhs, short rhs) => lhs.RawShort == rhs;
        public static bool operator <(Fractional lhs, short rhs) => lhs.RawShort < rhs;
        public static bool operator >(Fractional lhs, short rhs) => lhs.RawShort > rhs;
        public static bool operator <=(Fractional lhs, short rhs) => lhs.RawShort <= rhs;
        public static bool operator >=(Fractional lhs, short rhs) => lhs.RawShort >= rhs;

        public static Fractional operator +(Fractional lhs, short rhs) => new Fractional((short) (lhs.RawShort + rhs));
        public static Fractional operator -(Fractional lhs, short rhs) => new Fractional((short) (lhs.RawShort - rhs));

        // Short comparisons / operators (LHS)
        public static bool operator ==(short lhs, Fractional rhs) => lhs == rhs.RawShort;
        public static bool operator !=(short lhs, Fractional rhs) => lhs == rhs.RawShort;
        public static bool operator <(short lhs, Fractional rhs) => lhs < rhs.RawShort;
        public static bool operator >(short lhs, Fractional rhs) => lhs > rhs.RawShort;
        public static bool operator <=(short lhs, Fractional rhs) => lhs <= rhs.RawShort;
        public static bool operator >=(short lhs, Fractional rhs) => lhs >= rhs.RawShort;

        public static Fractional operator +(short lhs, Fractional rhs) => new Fractional((short) (lhs + rhs.RawShort));
        public static Fractional operator -(short lhs, Fractional rhs) => new Fractional((short) (lhs - rhs.RawShort));

        // Float comparisons / operators (RHS)
        public static bool operator ==(Fractional lhs, float rhs) => lhs.Float == rhs;
        public static bool operator !=(Fractional lhs, float rhs) => lhs.Float == rhs;
        public static bool operator <(Fractional lhs, float rhs) => lhs.Float < rhs;
        public static bool operator >(Fractional lhs, float rhs) => lhs.Float > rhs;
        public static bool operator <=(Fractional lhs, float rhs) => lhs.Float <= rhs;
        public static bool operator >=(Fractional lhs, float rhs) => lhs.Float >= rhs;

        public static Fractional operator +(Fractional lhs, float rhs) => new Fractional(lhs.Float + rhs, 0);
        public static Fractional operator -(Fractional lhs, float rhs) => new Fractional(lhs.Float - rhs, 0);

        // Float comparisons / operators (LHS)
        public static bool operator ==(float lhs, Fractional rhs) => lhs == rhs.Float;
        public static bool operator !=(float lhs, Fractional rhs) => lhs == rhs.Float;
        public static bool operator <(float lhs, Fractional rhs) => lhs < rhs.Float;
        public static bool operator >(float lhs, Fractional rhs) => lhs > rhs.Float;
        public static bool operator <=(float lhs, Fractional rhs) => lhs <= rhs.Float;
        public static bool operator >=(float lhs, Fractional rhs) => lhs >= rhs.Float;

        public static Fractional operator +(float lhs, Fractional rhs) => new Fractional(lhs + rhs.Float, 0);
        public static Fractional operator -(float lhs, Fractional rhs) => new Fractional(lhs - rhs.Float, 0);

        // Other operations
        public static Fractional operator -(Fractional f) => new Fractional((short) -f.RawShort);

    }
}
