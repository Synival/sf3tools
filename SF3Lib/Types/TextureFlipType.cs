using CommonLib.Attributes;

namespace SF3.Types {
    public enum TextureFlipType : byte {
        [EnumDisplayName("No Flip")]
        NoFlip     = 0x00,

        [EnumDisplayName("Horizontal")]
        Horizontal = 0x10,

        [EnumDisplayName("Vertical")]
        Vertical   = 0x20,

        [EnumDisplayName("Both")]
        Both       = 0x30,
    }
}
