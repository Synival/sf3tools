using System;

namespace CommonLib.Utils {
    public static class PixelConversion {
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

        public static bool IsValidHtmlColor(string htmlColor) {
            if (htmlColor == null || htmlColor.Length == 0)
                return false;
            if (htmlColor[0] == '#')
                htmlColor = htmlColor.Substring(1);
            return htmlColor.Length == 3 || htmlColor.Length == 4 || htmlColor.Length == 6 || htmlColor.Length == 8;
        }

        public static ushort ABGR1555toARGB1555(ushort input)
            => ARGB1555toABGR1555(input); // Exact same operation.

        public static ushort ARGB1555toABGR1555(ushort input) {
            // Return 'input' with swapped red and blue channels.
            var lowerChannel = input & 0x001F;
            var upperChannel = input & 0x7F00;
            return (ushort) ((input & ~0x7C1F) | (lowerChannel << 10) | (upperChannel >> 10));
        }

        public static PixelChannels ABGR1555toChannels(ushort input) {
            return new PixelChannels {
                a = ((input & 0x8000) != 0) ? (byte) 255 : (byte) 0,
                r = (byte) (((input >> 0)  & 0x1F) << 3),
                g = (byte) (((input >> 5)  & 0x1F) << 3),
                b = (byte) (((input >> 10) & 0x1F) << 3),
            };
        }

        public static PixelChannels ARGB1555toChannels(ushort input) {
            return new PixelChannels {
                a = ((input & 0x8000) != 0) ? (byte) 255 : (byte) 0,
                r = (byte) (((input >> 10) & 0x1F) << 3),
                g = (byte) (((input >> 5)  & 0x1F) << 3),
                b = (byte) (((input >> 0)  & 0x1F) << 3),
            };
        }

        public static PixelChannels ABGR8888toChannels(uint input) {
            return new PixelChannels {
                a = (byte) ((input >> 24) & 0xFF),
                r = (byte) ((input >> 0)  & 0xFF),
                g = (byte) ((input >> 16) & 0xFF),
                b = (byte) ((input >> 24) & 0xFF),
            };
        }

        public static PixelChannels ARGB8888toChannels(uint input) {
            return new PixelChannels {
                a = (byte) ((input >> 24) & 0xFF),
                r = (byte) ((input >> 16) & 0xFF),
                g = (byte) ((input >> 8)  & 0xFF),
                b = (byte) ((input >> 0)  & 0xFF),
            };
        }

        public static PixelChannels BGRA8888toChannels(uint input) {
            return new PixelChannels {
                a = (byte) ((input >> 0)  & 0xFF),
                r = (byte) ((input >> 8)  & 0xFF),
                g = (byte) ((input >> 16) & 0xFF),
                b = (byte) ((input >> 24) & 0xFF),
            };
        }

        public static uint ABGR1555toABGR8888(ushort input) => ABGR1555toChannels(input).ToABGR8888();
        public static uint ABGR1555toARGB8888(ushort input) => ABGR1555toChannels(input).ToARGB8888();
        public static uint ABGR1555toBGRA8888(ushort input) => ABGR1555toChannels(input).ToBGRA8888();

        public static uint ARGB1555toABGR8888(ushort input) => ARGB1555toChannels(input).ToABGR8888();
        public static uint ARGB1555toARGB8888(ushort input) => ARGB1555toChannels(input).ToARGB8888();
        public static uint ARGB1555toBGRA8888(ushort input) => ARGB1555toChannels(input).ToBGRA8888();

        public static uint ABGR8888toABGR1555(uint input) => ABGR8888toChannels(input).ToABGR1555();
        public static uint ABGR8888toARGB1555(uint input) => ABGR8888toChannels(input).ToARGB1555();
        public static uint ABGR8888toARGB8888(uint input) => ABGR8888toChannels(input).ToARGB8888();
        public static uint ABGR8888toBGRA8888(uint input) => ABGR8888toChannels(input).ToBGRA8888();

        public static uint ARGB8888toABGR1555(uint input) => ARGB8888toChannels(input).ToABGR1555();
        public static uint ARGB8888toARGB1555(uint input) => ARGB8888toChannels(input).ToARGB1555();
        public static uint ARGB8888toABGR8888(uint input) => ARGB8888toChannels(input).ToABGR8888();
        public static uint ARGB8888toBGRA8888(uint input) => ARGB8888toChannels(input).ToBGRA8888();

        public static uint BGRA8888toABGR1555(uint input) => BGRA8888toChannels(input).ToABGR1555();
        public static uint BGRA8888toARGB1555(uint input) => BGRA8888toChannels(input).ToARGB1555();
        public static uint BGRA8888toARGB8888(uint input) => BGRA8888toChannels(input).ToARGB8888();
        public static uint BGRA8888toABGR8888(uint input) => BGRA8888toChannels(input).ToBGRA8888();

        // TODO: This is used for TextureAtlases. Please figure out exactly what's going on here!!!
        public static byte[] ImageDataToSomething(byte[] input) {
            if (input.Length % 2 != 0)
                throw new ArgumentException(nameof(input));

            var output = new byte[input.Length * 2];
            int posIn = 0, posOut = 0;
            while (posIn < input.Length) {
                ushort inputPixel = 0;
                inputPixel |= input[posIn++];
                inputPixel |= (ushort) (input[posIn++] << 8);

                var outputPixel = ARGB1555toBGRA8888(inputPixel);

                output[posOut++] = (byte) ((outputPixel >> 24) & 0xFF);
                output[posOut++] = (byte) ((outputPixel >> 16) & 0xFF);
                output[posOut++] = (byte) ((outputPixel >> 8)  & 0xFF);
                output[posOut++] = (byte) ((outputPixel >> 0)  & 0xFF);
            }
            return output;
        }
    }
}
