namespace CommonLib.Types {
    public enum TextureRotateType {
        NoRotation,
        Rotate90CW,
        Rotate180,
        Rotate270CW
    }

    public static class TextureRotateTypeExtensions {
        public static int GetCWRotationCount(this TextureRotateType type) {
            switch (type) {
                case TextureRotateType.NoRotation:  return 0;
                case TextureRotateType.Rotate90CW:  return 1;
                case TextureRotateType.Rotate180:   return 2;
                case TextureRotateType.Rotate270CW: return 3;
                default:                            return 0;
            }
        }
    }
}
