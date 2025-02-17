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
    }
}
