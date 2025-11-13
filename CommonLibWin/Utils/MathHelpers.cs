using System.Drawing;

namespace CommonLib.Win.Utils {
    public static class MathHelpers {
        /// <summary>
        /// Performs linear interpolation of colors 'a' and 'b' with amount 't'.
        /// </summary>
        /// <param name="a">First color.</param>
        /// <param name="b">Second color.</param>
        /// <param name="t">Amount between colors 'a' and 'b', from 0.00 (a) to 1.00 (b).</param>
        /// <returns>A linearly-interpolated Color.</returns>
        public static Color Lerp(Color a, Color b, float t) {
            return Color.FromArgb(
                CommonLib.Utils.MathHelpers.Lerp(a.A, b.A, t),
                CommonLib.Utils.MathHelpers.Lerp(a.R, b.R, t),
                CommonLib.Utils.MathHelpers.Lerp(a.G, b.G, t),
                CommonLib.Utils.MathHelpers.Lerp(a.B, b.B, t)
            );
        }
    }
}
