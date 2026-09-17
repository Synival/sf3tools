using CommonLib.Attributes;
using CommonLib.Types;

namespace SF3.Types {
    public enum TileTextureRotateType : byte {
        [EnumDisplayName("No Rotation")]
        NoRotation   = 0x00,

        [EnumDisplayName("Rotate +90 (CW)")]
        Rotate90CW   = 0x03,

        [EnumDisplayName("Rotate 180")]
        Rotate180    = 0x02,

        [EnumDisplayName("Rotate -90 (CCW)")]
        Rotate270CW  = 0x01,
    }

    public static class TextureRotateTypeExtensions {
        public static CommonTextureRotateType ToCommon(this TileTextureRotateType type) {
            switch (type) {
                case TileTextureRotateType.NoRotation:  return CommonTextureRotateType.NoRotation;
                case TileTextureRotateType.Rotate90CW:  return CommonTextureRotateType.Rotate90CW;
                case TileTextureRotateType.Rotate180:   return CommonTextureRotateType.Rotate180;
                case TileTextureRotateType.Rotate270CW: return CommonTextureRotateType.Rotate270CW;
                default: return CommonTextureRotateType.NoRotation;
            }
        }
    }
}
