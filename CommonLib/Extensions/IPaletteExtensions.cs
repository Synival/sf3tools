using System;
using System.Linq;
using CommonLib.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class IPaletteExtensions {
        public static int GetHighestScoringIndex(this IPalette palette, bool ignoreColorZero, Func<PixelChannels, int> scoreFunc) {
            int bestIndex = -1;
            int bestScore = 0;

            var colors = palette.Colors;
            var max = colors.Length;
            for (int i = ignoreColorZero ? 1 : 0; i < max; i++) {
                var color = colors[i];
                var score = scoreFunc(color);

                if (bestIndex == -1 || score > bestScore) {
                    bestIndex = i;
                    bestScore = score;
                }
            }

            return bestIndex;
        }

        public static int GetDarkestIndex(this IPalette palette, bool ignoreColorZero)
            => palette.GetHighestScoringIndex(ignoreColorZero, color => 0x200 - (int) (0.25 * color.R + 0.625 * color.G + 0.125 * color.B));

        public static int GetLightestIndex(this IPalette palette, bool ignoreColorZero)
            => palette.GetHighestScoringIndex(ignoreColorZero, color => (int) (0.25 * color.R + 0.625 * color.G + 0.125 * color.B));

        public static int GetClosestIndex(this IPalette palette, bool ignoreColorZero, PixelChannels matchToColor) {
            return palette.GetHighestScoringIndex(ignoreColorZero, color =>
                0x1000 - (int) (
                    0.25  * Math.Abs(matchToColor.R - color.R) +
                    0.625 * Math.Abs(matchToColor.G - color.G) +
                    0.125 * Math.Abs(matchToColor.B - color.B)
                )
            );
        }

        public static string ToJSON_String(this IPalette palette)
            => palette.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IPalette palette) => palette.ToJArray();
        public static JArray ToJArray(this IPalette palette) => new JArray(palette.Colors.Select(x => x.ToHtmlColor()).ToArray());
    }
}
