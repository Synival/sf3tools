using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using static CommonLib.Imaging.PixelConversion;

namespace CommonLib.Imaging {
    public class Palette : IPalette {
        /// <summary>
        /// Converts a palette of colors in ABGR1555 format to an array of PixelChannels[].
        /// </summary>
        /// <param name="colors">An array of colors in ABGR1555 format.</param>
        public Palette(ushort[] colors) {
            Channels = colors.Select(x => ABGR1555toChannels(x)).ToArray();
        }

        /// <summary>
        /// Makes a copy of an existing palette.
        /// </summary>
        /// <param name="palette">Palette to copy.</param>
        public Palette(Palette palette) {
            Channels = palette.Channels.Clone() as PixelChannels[];
        }

        /// <summary>
        /// Creates a new palette with a copy of pixel channels provided.
        /// </summary>
        /// <param name="channels">Color channels to copy.</param>
        public Palette(PixelChannels[] channels) {
            Channels = channels.Clone() as PixelChannels[];
        }

        /// <summary>
        /// Creates an empty palette with a set number of colors, grayscale by default.
        /// </summary>
        /// <param name="colorCount">The number of colors.</param>
        public Palette(int colorCount) {
            Channels = new PixelChannels[colorCount];
            for (var i = 0; i < colorCount; i++) {
                var color = (byte) ((255 * i) / colorCount);
                Channels[i] = new PixelChannels() { A = 255, R = color, G = color, B = color };
            }
        }

        public static Palette FromJToken(JToken token) => new Palette(token);
        private Palette(JToken token) {
            var jArray = (JArray) token;
            Channels = jArray.Select(x => PixelChannels.FromHtmlColor((string) x, (byte) 0)).ToArray();
        }

        public int ColorCount => Channels.Length;

        public PixelChannels[] Channels { get; }

        public void Replace(PixelChannels[] colors) {
            if (colors == null)
                throw new ArgumentNullException(nameof(Channels));
            if (colors.Length != Channels.Length)
                throw new ArgumentException($"Wrong length for '{nameof(Channels)}'");
            for (int i = 0; i < colors.Length; i++)
                Channels[i] = colors[i];
        }

        public PixelChannels this[int index] {
            get => Channels[index];
            set => Channels[index] = value;
        }
    }
}
