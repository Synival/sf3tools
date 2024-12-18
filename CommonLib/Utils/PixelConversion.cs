using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Utils {
    public static class PixelConversion {
        public struct PixelChannels {
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
                => (uint ) ((a << 24) |
                            (r << 16) |
                            (g << 8)  |
                            (b << 0));
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

        public static PixelChannels ARGB8888ToChannels(uint input) {
            return new PixelChannels {
                a = (byte) ((input >> 24) & 0xFF),
                r = (byte) ((input >> 16) & 0xFF),
                g = (byte) ((input >> 8)  & 0xFF),
                b = (byte) ((input >> 0)  & 0xFF),
            };
        }
    }
}
