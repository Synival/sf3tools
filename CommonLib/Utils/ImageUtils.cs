using System;
using System.Linq;
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
            var transparentColor = palette.Channels[0];
            byte closestToTransparentIndex = (byte) palette.GetHighestScoringIndex(
                ignoreColorZero: true,
                color => {
                    var rDiff = Math.Abs(color.r - transparentColor.r);
                    var gDiff = Math.Abs(color.g - transparentColor.g);
                    var bDiff = Math.Abs(color.b - transparentColor.b);
                    return 0x300 - (rDiff + gDiff + bDiff);
                }
            );

            var width = data.GetLength(0);
            var height = data.GetLength(1);

            var newData = new byte[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    newData[x, y] = (data[x, y] == 0) ? closestToTransparentIndex : data[x, y];

            return newData;
        }

        /// <summary>
        /// Returns the width and height of an image to be used for a color palette based on the number of colors in the palette.
        /// The image width is the square root of the color count rounded down (must be at least 1) and the height is the count
        /// necessary to meet or exceed the color count.
        /// </summary>
        /// <param name="colorCount">The number of colors in the palette for which an image will be generated.</param>
        /// <returns>A tuple with width and height.</returns>
        public static (int Width, int Height) GetPaletteImageDimensions(int colorCount) {
            var width = (int) Math.Ceiling(Math.Sqrt(colorCount));

            var nextPow = 1;
            while (width > nextPow)
                nextPow *= 2;
            width = nextPow;

            var height = (int) Math.Ceiling(colorCount / (float) width);

            return (width, height);
        }

        /// <summary>
        /// Converts 8-bit indexed color data to conform to a different palette and returns it as new data.
        /// Colors are matched using a distance check between two RGB vectors.
        /// </summary>
        /// <param name="newData">8-bit indexed color data to convert.</param>
        /// <param name="newPalette">Original palette belonging to the 8-bit indexed color data.</param>
        /// <param name="toPalette">Color palette that the 8-bit indexed color data should be updated to conform to.</param>
        /// <returns>A new byte[,] with 8-bit indexed color data.</returns>
        public static byte[,] GetImageDataConformingToPalette(byte[,] newData, Palette newPalette, Palette toPalette) {
            // For each color in newPalette, find the closest match in toPalette.
            var conversionMap = newPalette.Channels
                .Select(x => (byte) toPalette.GetClosestIndex(ignoreColorZero: false, x))
                .ToArray();

            // Create new data with the updated colors.
            var updatedData = new byte[newData.GetLength(0), newData.GetLength(1)];
            for (int y = 0; y < newData.GetLength(1); y++)
                for (int x = 0; x < newData.GetLength(0); x++)
                    updatedData[x, y] = conversionMap[newData[x, y]];

            return updatedData;
        }
    }
}
