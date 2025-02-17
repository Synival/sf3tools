namespace SF3.Types {
    public enum BackgroundImageType {
        None          = 0x0000,
        Tiled         = 0x0010,
        Still         = 0x0040,
        StillAndTiled = 0x0050
    }

    public static class BackgroundImageTypeExtensions {
        public const int ApplicableMapFlags = 0x0050;

        public static BackgroundImageType FromMapFlags(ushort mapFlags)
            => (BackgroundImageType) (mapFlags & ApplicableMapFlags);

        public static ushort ToMapFlags(this BackgroundImageType type)
            => (ushort) type;
    }
}
