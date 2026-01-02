using System;
using System.Linq;
using static CommonLib.Imaging.PixelConversion;

namespace CommonLib.Imaging {
    public class Palette {
        /// <summary>
        /// Converts a palette of colors in ABGR1555 format to an array of PixelChannels[].
        /// </summary>
        /// <param name="colors">An array of colors in ABGR1555 format.</param>
        public Palette(ushort[] colors) {
            Channels = colors.Select(x => ABGR1555toChannels(x)).ToArray();
        }

        /// <summary>
        /// Creates an empty palette with a set number of colors, grayscale by default.
        /// </summary>
        /// <param name="colorCount">The number of colors.</param>
        public Palette(int colorCount) {
            Channels = new PixelChannels[colorCount];
            for (var i = 0; i < colorCount; i++) {
                var color = (byte) ((255 * i) / colorCount);
                Channels[i] = new PixelChannels() { a = 255, r = color, g = color, b = color };
            }
        }

        public readonly PixelChannels[] Channels = null;

        public PixelChannels this[int index] {
            get => Channels[index];
            set => Channels[index] = value;
        }

        public int GetDarkestIndex(bool zeroIsTransparent) {
            int darkestIndex = 0;
            int darkestValue = -1;

            var max = Channels.Length;
            for (int i = zeroIsTransparent ? 1 : 0; i < max; i++) {
                var color = Channels[i];
                var value = Math.Max(color.r, Math.Max(color.g, color.b));

                // Stop if we've found true black.
                if (value == 0)
                    return i;

                if (darkestIndex == 0 || value < darkestValue) {
                    darkestValue = value;
                    darkestIndex = i;
                }
            }

            return darkestIndex;
        }

        public int GetLightestIndex(bool zeroIsTransparent) {
            int lightestIndex = 0;
            int lightestValue = -1;

            var max = Channels.Length;
            for (int i = zeroIsTransparent ? 1 : 0; i < max; i++) {
                var color = Channels[i];
                var value = Math.Max(color.r, Math.Max(color.g, color.b));

                // Stop if we've found true white.
                if (value == 0xFF)
                    return i;

                if (lightestIndex == 0 || value > lightestValue) {
                    lightestValue = value;
                    lightestIndex = i;
                }
            }

            return lightestIndex;
        }
    }
}
