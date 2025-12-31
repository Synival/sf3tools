using System;
using CommonLib.Imaging;

namespace CommonLib.Utils {
    public static class ImageUtils {
        /// <summary>
        /// For 8-bit indexed image data where zero is a transparent pixel, this returns new data where all zeroes are
        /// replaced with the darkest color available in the palette (determined by value).
        /// </summary>
        /// <param name="data">Input data whose transpency needs to be removed.</param>
        /// <param name="palette">The color palette used for the 8-bit indexed image data.</param>
        /// <returns>A new byte[,] with a copy of input 'data' without any transparent pixels.</returns>
        public static byte[,] Create8BitImageDataWithoutTransparency(byte[,] data, Palette palette) {
            byte darkestIndex = 0;
            int darkestValue = -1;

            for (int i = 1; i < 0x100; i++) {
                var color = palette.Channels[i];
                var value = Math.Max(color.r, Math.Max(color.g, color.b));
                if (darkestIndex == 0 || value < darkestValue) {
                    darkestValue = value;
                    darkestIndex = (byte) i;
                }

                // Stop if we've found true black.
                if (value == 0)
                    break;
            }

            var width = data.GetLength(0);
            var height = data.GetLength(1);

            var newData = new byte[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    newData[x, y] = (data[x, y] == 0) ? darkestIndex : data[x, y];

            return newData;
        }
    }
}
