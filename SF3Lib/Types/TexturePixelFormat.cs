namespace SF3.Types {
    public enum TexturePixelFormat {
        ABGR1555    = 0,
        Indexed8Bit = 1
    }

    public static class TexturePixelFormatExtensions {
        public static int BytesPerPixel(this TexturePixelFormat format)
            => (format == TexturePixelFormat.ABGR1555) ? 2 : 1;
    }
}
