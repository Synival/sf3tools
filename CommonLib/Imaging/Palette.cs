using System.Linq;
using static CommonLib.Imaging.PixelConversion;

namespace CommonLib.Imaging {
    public class Palette {
        // TODO: Supply pixel format!!
        /// <summary>
        /// 
        /// Converts a palette of colors in ABGR1555 format to an array of PixelChannels[].
        /// </summary>
        /// <param name="colors">An array of colors in ABGR1555 format.</param>
        public Palette(ushort[] colors) {
            Channels = colors.Select(x => ARGB1555toChannels(x)).ToArray();
        }

        public readonly PixelChannels[] Channels = null;

        public PixelChannels this[int index] {
            get => Channels[index];
            set => Channels[index] = value;
        }
    }
}
