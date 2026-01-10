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
                    R = Convert.ToByte(new string(htmlColor[0], 2), 16),
                    G = Convert.ToByte(new string(htmlColor[1], 2), 16),
                    B = Convert.ToByte(new string(htmlColor[2], 2), 16),
                    A = (htmlColor.Length == 3) ? defaultAlpha : Convert.ToByte(new string(htmlColor[3], 2), 16),
                };
            }
            else {
                return new PixelChannels {
                    R = Convert.ToByte(htmlColor.Substring(0, 2), 16),
                    G = Convert.ToByte(htmlColor.Substring(2, 2), 16),
                    B = Convert.ToByte(htmlColor.Substring(4, 2), 16),
                    A = (htmlColor.Length == 6) ? defaultAlpha : Convert.ToByte(htmlColor.Substring(6, 2), 16),
                };
            }
        }

        public byte A, R, G, B;

        public ushort ToARGB1555()
            => (ushort) (((A >= 127) ? 0x8000 : 0x0000) |
                         ((R >> 3) << 10) |
                         ((G >> 3) << 5)  |
                         ((B >> 3) << 0));

        public ushort ToABGR1555()
            => (ushort) (((A >= 127) ? 0x8000 : 0x0000) |
                         ((B >> 3) << 10) |
                         ((G >> 3) << 5)  |
                         ((R >> 3) << 0));

        public uint ToARGB8888()
            => (uint) ((A << 24) |
                       (R << 16) |
                       (G << 8)  |
                       (B << 0));

        public uint ToABGR8888()
            => (uint) ((A << 24) |
                       (R << 0) |
                       (G << 8)  |
                       (B << 16));

        public uint ToBGRA8888()
            => (uint) ((A << 0) |
                       (R << 8) |
                       (G << 16)  |
                       (B << 24));

        public string ToHtmlColor() {
            return "#" +
                R.ToString("X2") +
                G.ToString("X2") +
                B.ToString("X2") +
                A.ToString("X2");
        }
    }
}
