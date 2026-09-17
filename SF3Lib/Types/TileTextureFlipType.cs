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
        public static TextureFlipType ToCommon(this TileTextureFlipType type) {
            switch (type) {
                case TileTextureFlipType.NoFlip:     return TextureFlipType.NoFlip;
                case TileTextureFlipType.Horizontal: return TextureFlipType.Horizontal;
                case TileTextureFlipType.Vertical:   return TextureFlipType.Vertical;
                case TileTextureFlipType.Both:       return TextureFlipType.Both;
                default: return TextureFlipType.NoFlip;
            }
        }
    }
}
