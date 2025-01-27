using System;

namespace SF3.Types {
    public enum TexturePixelFormat {
        Unknown,
        ABGR1555,
        UnknownPalette,
        Palette1,
        Palette2,
        Palette3
    }

    public static class TexturePixelFormatExtensions {
        public static int BytesPerPixel(this TexturePixelFormat format)
            => (format == TexturePixelFormat.ABGR1555) ? 2 : 1;
    }
}
