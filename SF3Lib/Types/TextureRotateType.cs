using CommonLib.Attributes;
using CommonLib.Types;

namespace SF3.Types {
    public enum TextureRotateType : byte {
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
        public static CommonTextureRotateType ToCommon(this TextureRotateType type) {
            switch (type) {
                case TextureRotateType.NoRotation:  return CommonTextureRotateType.NoRotation;
                case TextureRotateType.Rotate90CW:  return CommonTextureRotateType.Rotate90CW;
                case TextureRotateType.Rotate180:   return CommonTextureRotateType.Rotate180;
                case TextureRotateType.Rotate270CW: return CommonTextureRotateType.Rotate270CW;
                default: return CommonTextureRotateType.NoRotation;
            }
        }
    }
}
