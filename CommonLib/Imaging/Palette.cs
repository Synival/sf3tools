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
            Colors = colors.Select(x => ABGR1555toChannels(x)).ToArray();
        }

        /// <summary>
        /// Makes a copy of an existing palette.
        /// </summary>
        /// <param name="palette">Palette to copy.</param>
        public Palette(IPalette palette) {
            Colors = palette.Colors.Clone() as PixelChannels[];
        }

        /// <summary>
        /// Creates a new palette with a copy of PixelChannels provided.
        /// </summary>
        /// <param name="channels">Color channels to copy.</param>
        public Palette(PixelChannels[] channels) {
            Colors = channels.Clone() as PixelChannels[];
        }

        /// <summary>
        /// Creates an empty palette with a set number of colors, grayscale by default.
        /// </summary>
        /// <param name="colorCount">The number of colors.</param>
        public Palette(int colorCount) {
            Colors = new PixelChannels[colorCount];
            for (var i = 0; i < colorCount; i++) {
                var color = (byte) ((255 * i) / colorCount);
                Colors[i] = new PixelChannels() { A = 255, R = color, G = color, B = color };
            }
        }

        public static Palette FromJToken(JToken token) => new Palette(token);
        private Palette(JToken token) {
            var jArray = (JArray) token;
            Colors = jArray.Select(x => PixelChannels.FromHtmlColor((string) x, (byte) 0)).ToArray();
        }

        public int ColorCount => Colors.Length;

        public PixelChannels[] Colors { get; }

        public void Replace(PixelChannels[] colors) {
            if (colors == null)
                throw new ArgumentNullException(nameof(Colors));
            if (colors.Length != Colors.Length)
                throw new ArgumentException($"Wrong length for '{nameof(Colors)}'");
            for (int i = 0; i < colors.Length; i++)
                Colors[i] = colors[i];
        }

        public PixelChannels this[int index] {
            get => Colors[index];
            set => Colors[index] = value;
        }
    }
}
