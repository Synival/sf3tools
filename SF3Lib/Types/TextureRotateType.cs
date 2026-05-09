using CommonLib.Attributes;

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
}
