namespace SF3.Types {
    public enum GroundImageType {
        None      = 0x0000,
        Image     = 0x0400,
        TileBased = 0x1000,
        Invalid   = 0x1400
    }

    public static class GroundImageTypeExtensions {
        public const int ApplicableMapFlags = 0x1400;

        public static GroundImageType FromMapFlags(ushort mapFlags)
            => (GroundImageType) (mapFlags & ApplicableMapFlags);

        public static ushort ToMapFlags(this GroundImageType type)
            => (ushort) type;
    }
}
