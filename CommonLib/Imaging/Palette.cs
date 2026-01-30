using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public readonly PixelChannels[] Channels = null;

        public PixelChannels this[int index] {
            get => Channels[index];
            set => Channels[index] = value;
        }

        public int GetHighestScoringIndex(bool ignoreColorZero, Func<PixelChannels, int> scoreFunc) {
            int bestIndex = -1;
            int bestScore = 0;

            var max = Channels.Length;
            for (int i = ignoreColorZero ? 1 : 0; i < max; i++) {
                var color = Channels[i];
                var score = scoreFunc(color);

                if (bestIndex == -1 || score > bestScore) {
                    bestIndex = i;
                    bestScore = score;
                }
            }

            return bestIndex;
        }

        public int GetDarkestIndex(bool ignoreColorZero)
            => GetHighestScoringIndex(ignoreColorZero, color => 0x100 - Math.Max(color.R, Math.Max(color.G, color.B)));

        public int GetLightestIndex(bool ignoreColorZero)
            => GetHighestScoringIndex(ignoreColorZero, color => Math.Max(color.R, Math.Max(color.G, color.B)));

        public int GetClosestIndex(bool ignoreColorZero, PixelChannels matchToColor) {
            return GetHighestScoringIndex(ignoreColorZero, color => {
                return -(Math.Abs(matchToColor.R - color.R) + Math.Abs(matchToColor.G - color.G) + Math.Abs(matchToColor.B - color.B));
            });
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() => ToJObject();
        public JObject ToJObject() {
            return null;
        }
    }
}
