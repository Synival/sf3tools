using CommonLib.Attributes;
using CommonLib.Types;

namespace SF3.Types {
    public enum TileTextureFlipType : byte {
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
        public static CommonTextureFlipType ToCommon(this TileTextureFlipType type) {
            switch (type) {
                case TileTextureFlipType.NoFlip:     return CommonTextureFlipType.NoFlip;
                case TileTextureFlipType.Horizontal: return CommonTextureFlipType.Horizontal;
                case TileTextureFlipType.Vertical:   return CommonTextureFlipType.Vertical;
                case TileTextureFlipType.Both:       return CommonTextureFlipType.Both;
                default: return CommonTextureFlipType.NoFlip;
            }
        }
    }
}
