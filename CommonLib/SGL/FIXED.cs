namespace CommonLib.SGL {
    public struct FIXED {
        /// <summary>
        /// Creates a new FIXED with either raw int input (as read from a byte stream), or a specific int value for retrieval.
        /// </summary>
        /// <param name="i">The int value, which can be either raw int input (as read from a byte stream), or a specific int value for retrieval.</param>
        /// <param name="isRaw">If 'true', then 'i' is a raw int value. If 'false', then 'i' is a specific int value for retrieval.</param>
        public FIXED(int i, bool isRaw) {
            RawInt = isRaw ? i : (i * 65536);
        }

        /// <summary>
        /// Creates a FIXED with an input float.
        /// </summary>
        /// <param name="f">Float value of the new FIXED.</param>
        /// <param name="_">Deliberately unused, only to prevent accidental construction with the 'int' constructor.</param>
        public FIXED(float f, int _) {
            RawInt = (int) (f * 65536f);
        }

        public FIXED(CompressedFIXED cf) : this(cf.RawShort * 2, true) { }

        public int RawInt { get; set; }

        public int Int {
            get => (int) (RawInt / 65536f);
            set => RawInt = (int) (value * 65536f);
        }

        public float Float {
            get => RawInt / 65536f;
            set => RawInt = (int) (value * 65536f);
        }

        public override bool Equals(object obj) {
            return (obj is CompressedFIXED cf && RawInt == cf.RawShort) ||
                   (obj is FIXED f && RawInt == f.RawInt);
        }

        public override int GetHashCode()
            => RawInt.GetHashCode();

        public override string ToString()
            => Float.ToString();

        // Comparisons / operators
        public static bool operator ==(FIXED lhs, FIXED rhs) => lhs.RawInt == rhs.RawInt;
        public static bool operator !=(FIXED lhs, FIXED rhs) => lhs.RawInt == rhs.RawInt;
        public static bool operator <(FIXED lhs, FIXED rhs) => lhs.RawInt < rhs.RawInt;
        public static bool operator >(FIXED lhs, FIXED rhs) => lhs.RawInt > rhs.RawInt;
        public static bool operator <=(FIXED lhs, FIXED rhs) => lhs.RawInt <= rhs.RawInt;
        public static bool operator >=(FIXED lhs, FIXED rhs) => lhs.RawInt >= rhs.RawInt;

        public static FIXED operator +(FIXED lhs, FIXED rhs) => new FIXED(lhs.RawInt + rhs.RawInt, true);
        public static FIXED operator -(FIXED lhs, FIXED rhs) => new FIXED(lhs.RawInt - rhs.RawInt, true);

        // Short comparisons / operators (RHS)
        public static bool operator ==(FIXED lhs, short rhs) => lhs.RawInt == rhs;
        public static bool operator !=(FIXED lhs, short rhs) => lhs.RawInt == rhs;
        public static bool operator <(FIXED lhs, short rhs) => lhs.RawInt < rhs;
        public static bool operator >(FIXED lhs, short rhs) => lhs.RawInt > rhs;
        public static bool operator <=(FIXED lhs, short rhs) => lhs.RawInt <= rhs;
        public static bool operator >=(FIXED lhs, short rhs) => lhs.RawInt >= rhs;

        public static FIXED operator +(FIXED lhs, short rhs) => new FIXED(lhs.RawInt + rhs, true);
        public static FIXED operator -(FIXED lhs, short rhs) => new FIXED(lhs.RawInt - rhs, true);

        // Short comparisons / operators (LHS)
        public static bool operator ==(short lhs, FIXED rhs) => lhs == rhs.RawInt;
        public static bool operator !=(short lhs, FIXED rhs) => lhs == rhs.RawInt;
        public static bool operator <(short lhs, FIXED rhs) => lhs < rhs.RawInt;
        public static bool operator >(short lhs, FIXED rhs) => lhs > rhs.RawInt;
        public static bool operator <=(short lhs, FIXED rhs) => lhs <= rhs.RawInt;
        public static bool operator >=(short lhs, FIXED rhs) => lhs >= rhs.RawInt;

        public static FIXED operator +(short lhs, FIXED rhs) => new FIXED(lhs + rhs.RawInt, true);
        public static FIXED operator -(short lhs, FIXED rhs) => new FIXED(lhs - rhs.RawInt, true);

        // Float comparisons / operators (RHS)
        public static bool operator ==(FIXED lhs, float rhs) => lhs.Float == rhs;
        public static bool operator !=(FIXED lhs, float rhs) => lhs.Float == rhs;
        public static bool operator <(FIXED lhs, float rhs) => lhs.Float < rhs;
        public static bool operator >(FIXED lhs, float rhs) => lhs.Float > rhs;
        public static bool operator <=(FIXED lhs, float rhs) => lhs.Float <= rhs;
        public static bool operator >=(FIXED lhs, float rhs) => lhs.Float >= rhs;

        public static FIXED operator +(FIXED lhs, float rhs) => new FIXED(lhs.Float + rhs, 0);
        public static FIXED operator -(FIXED lhs, float rhs) => new FIXED(lhs.Float - rhs, 0);

        // Float comparisons / operators (LHS)
        public static bool operator ==(float lhs, FIXED rhs) => lhs == rhs.Float;
        public static bool operator !=(float lhs, FIXED rhs) => lhs == rhs.Float;
        public static bool operator <(float lhs, FIXED rhs) => lhs < rhs.Float;
        public static bool operator >(float lhs, FIXED rhs) => lhs > rhs.Float;
        public static bool operator <=(float lhs, FIXED rhs) => lhs <= rhs.Float;
        public static bool operator >=(float lhs, FIXED rhs) => lhs >= rhs.Float;

        public static FIXED operator +(float lhs, FIXED rhs) => new FIXED(lhs + rhs.Float, 0);
        public static FIXED operator -(float lhs, FIXED rhs) => new FIXED(lhs - rhs.Float, 0);

        // Other operations
        public static FIXED operator -(FIXED f) => new FIXED(-f.RawInt, true);
    }
}
