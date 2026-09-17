using CommonLib.Attributes;
using CommonLib.Types;

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

    public static class TextureFlipTypeExtensions {
        public static CommonTextureFlipType ToCommon(this TextureFlipType type) {
            switch (type) {
                case TextureFlipType.NoFlip:     return CommonTextureFlipType.NoFlip;
                case TextureFlipType.Horizontal: return CommonTextureFlipType.Horizontal;
                case TextureFlipType.Vertical:   return CommonTextureFlipType.Vertical;
                case TextureFlipType.Both:       return CommonTextureFlipType.Both;
                default: return CommonTextureFlipType.NoFlip;
            }
        }
    }
}
