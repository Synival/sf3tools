using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Utils {
    public static class PixelConversion {
        public struct PixelChannels {
            public byte a, r, g, b;
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

        public static PixelChannels ARGB88888oChannels(uint input) {
            return new PixelChannels {
                a = (byte) ((input >> 24) & 0xFF),
                r = (byte) ((input >> 16) & 0xFF),
                g = (byte) ((input >> 8)  & 0xFF),
                b = (byte) ((input >> 0)  & 0xFF),
            };
        }
    }
}
