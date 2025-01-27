using System;

namespace CommonLib.Imaging {
    public struct PixelChannels {
        public static PixelChannels FromHtmlColor(string htmlColor, byte defaultAlpha) {
            if (htmlColor == null)
                throw new ArgumentNullException(nameof(htmlColor));

            if (htmlColor.Length >= 1 && htmlColor[0] == '#')
                htmlColor = htmlColor.Substring(1);
            if (htmlColor.Length != 3 && htmlColor.Length != 4 && htmlColor.Length != 6 && htmlColor.Length != 8)
                throw new ArgumentException(nameof(htmlColor));

            if (htmlColor.Length == 3 || htmlColor.Length == 4) {
                return new PixelChannels {
                    r = Convert.ToByte(new string(htmlColor[0], 2), 16),
                    g = Convert.ToByte(new string(htmlColor[1], 2), 16),
                    b = Convert.ToByte(new string(htmlColor[2], 2), 16),
                    a = (htmlColor.Length == 3) ? defaultAlpha : Convert.ToByte(new string(htmlColor[3], 2), 16),
                };
            }
            else {
                return new PixelChannels {
                    r = Convert.ToByte(htmlColor.Substring(0, 2), 16),
                    g = Convert.ToByte(htmlColor.Substring(2, 2), 16),
                    b = Convert.ToByte(htmlColor.Substring(4, 2), 16),
                    a = (htmlColor.Length == 6) ? defaultAlpha : Convert.ToByte(htmlColor.Substring(6, 2), 16),
                };
            }
        }

        public byte a, r, g, b;

        public ushort ToARGB1555()
            => (ushort) (((a >= 127) ? 0x8000 : 0x0000) |
                         ((r >> 3) << 10) |
                         ((g >> 3) << 5)  |
                         ((b >> 3) << 0));

        public ushort ToABGR1555()
            => (ushort) (((a >= 127) ? 0x8000 : 0x0000) |
                         ((b >> 3) << 10) |
                         ((g >> 3) << 5)  |
                         ((r >> 3) << 0));

        public uint ToARGB8888()
            => (uint) ((a << 24) |
                       (r << 16) |
                       (g << 8)  |
                       (b << 0));

        public uint ToABGR8888()
            => (uint) ((a << 24) |
                       (r << 0) |
                       (g << 8)  |
                       (b << 16));

        public uint ToBGRA8888()
            => (uint) ((a << 0) |
                       (r << 8) |
                       (g << 16)  |
                       (b << 24));

        public string ToHtmlColor() {
            return "#" +
                r.ToString("X2") +
                g.ToString("X2") +
                b.ToString("X2") +
                a.ToString("X2");
        }
    }
}
