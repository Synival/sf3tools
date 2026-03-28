namespace CommonLib.Types {
    public enum ImageDataCanSet {
        Never          = 0,
        CanSet8Bit     = 0x1,
        CanSet16Bit    = 0x2,
        CanSet8Or16Bit = CanSet8Bit | CanSet16Bit
    }
}
