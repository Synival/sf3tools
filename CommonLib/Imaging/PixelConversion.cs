using System;

namespace CommonLib.Imaging {
    public static class PixelConversion {
        public static bool IsValidHtmlColor(string htmlColor) {
            if (htmlColor == null || htmlColor.Length == 0)
                return false;
            if (htmlColor[0] == '#')
                htmlColor = htmlColor.Substring(1);
            return htmlColor.Length == 3 || htmlColor.Length == 4 || htmlColor.Length == 6 || htmlColor.Length == 8;
        }

        public static PixelChannels ABGR1555toChannels(ushort input) {
            return new PixelChannels {
                A = (input & 0x8000) != 0 ? (byte) 255 : (byte) 0,
                R = (byte) ((input >> 0  & 0x1F) * 255 / 31),
                G = (byte) ((input >> 5  & 0x1F) * 255 / 31),
                B = (byte) ((input >> 10 & 0x1F) * 255 / 31),
            };
        }

        public static PixelChannels ARGB1555toChannels(ushort input) {
            return new PixelChannels {
                A = (input & 0x8000) != 0 ? (byte) 255 : (byte) 0,
                R = (byte) ((input >> 10 & 0x1F) * 255 / 31),
                G = (byte) ((input >> 5  & 0x1F) * 255 / 31),
                B = (byte) ((input >> 0  & 0x1F) * 255 / 31),
            };
        }

        public static PixelChannels ABGR8888toChannels(uint input) {
            return new PixelChannels {
                A = (byte) (input >> 24 & 0xFF),
                R = (byte) (input >> 0  & 0xFF),
                G = (byte) (input >> 16 & 0xFF),
                B = (byte) (input >> 24 & 0xFF),
            };
        }

        public static PixelChannels ARGB8888toChannels(uint input) {
            return new PixelChannels {
                A = (byte) (input >> 24 & 0xFF),
                R = (byte) (input >> 16 & 0xFF),
                G = (byte) (input >> 8  & 0xFF),
                B = (byte) (input >> 0  & 0xFF),
            };
        }

        public static PixelChannels BGRA8888toChannels(uint input) {
            return new PixelChannels {
                A = (byte) (input >> 0  & 0xFF),
                R = (byte) (input >> 8  & 0xFF),
                G = (byte) (input >> 16 & 0xFF),
                B = (byte) (input >> 24 & 0xFF),
            };
        }

        public static PixelChannels IndexedToChannels(byte input, Palette palette, bool zeroIsTransparent) {
            var color = palette[input];
            return new PixelChannels {
                A = (byte) ((zeroIsTransparent && input == 0) ? 0 : 255),
                R = color.R,
                G = color.G,
                B = color.B
            };
        }

        public static ushort ABGR1555toARGB1555(ushort input) {
            // Return 'input' with swapped red and blue channels.
            var lowerChannel = input & 0x001F;
            var upperChannel = input & 0x7F00;
            return (ushort) (input & ~0x7C1F | lowerChannel << 10 | upperChannel >> 10);
        }
        public static uint ABGR1555toABGR8888(ushort input) => ABGR1555toChannels(input).ToABGR8888();
        public static uint ABGR1555toARGB8888(ushort input) => ABGR1555toChannels(input).ToARGB8888();
        public static uint ABGR1555toBGRA8888(ushort input) => ABGR1555toChannels(input).ToBGRA8888();

        public static ushort ARGB1555toABGR1555(ushort input) => ABGR1555toARGB1555(input); // Exact same operation.
        public static uint ARGB1555toABGR8888(ushort input) => ARGB1555toChannels(input).ToABGR8888();
        public static uint ARGB1555toARGB8888(ushort input) => ARGB1555toChannels(input).ToARGB8888();
        public static uint ARGB1555toBGRA8888(ushort input) => ARGB1555toChannels(input).ToBGRA8888();

        public static ushort ABGR8888toABGR1555(uint input) => ABGR8888toChannels(input).ToABGR1555();
        public static ushort ABGR8888toARGB1555(uint input) => ABGR8888toChannels(input).ToARGB1555();
        public static uint ABGR8888toARGB8888(uint input) => ABGR8888toChannels(input).ToARGB8888();
        public static uint ABGR8888toBGRA8888(uint input) => ABGR8888toChannels(input).ToBGRA8888();

        public static ushort ARGB8888toABGR1555(uint input) => ARGB8888toChannels(input).ToABGR1555();
        public static ushort ARGB8888toARGB1555(uint input) => ARGB8888toChannels(input).ToARGB1555();
        public static uint ARGB8888toABGR8888(uint input) => ARGB8888toChannels(input).ToABGR8888();
        public static uint ARGB8888toBGRA8888(uint input) => ARGB8888toChannels(input).ToBGRA8888();

        public static ushort BGRA8888toABGR1555(uint input) => BGRA8888toChannels(input).ToABGR1555();
        public static ushort BGRA8888toARGB1555(uint input) => BGRA8888toChannels(input).ToARGB1555();
        public static uint BGRA8888toARGB8888(uint input) => BGRA8888toChannels(input).ToARGB8888();
        public static uint BGRA8888toABGR8888(uint input) => BGRA8888toChannels(input).ToABGR8888();

        public static ushort IndexedToABGR1555(byte input, Palette palette, bool zeroIsTransparent) => IndexedToChannels(input, palette, zeroIsTransparent).ToABGR1555();
        public static uint IndexedToABGR8888(byte input, Palette palette, bool zeroIsTransparent) => IndexedToChannels(input, palette, zeroIsTransparent).ToABGR8888();
        public static ushort IndexedToARGB1555(byte input, Palette palette, bool zeroIsTransparent) => IndexedToChannels(input, palette, zeroIsTransparent).ToARGB1555();
        public static uint IndexedToARGB8888(byte input, Palette palette, bool zeroIsTransparent) => IndexedToChannels(input, palette, zeroIsTransparent).ToARGB8888();
    }
}
