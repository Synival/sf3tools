using System;

namespace CommonLib.Extensions {
    public static class ArrayExtensions {
        /// <summary>
        /// Returns a copy of an array with an additional element 'newValue' tacked on the end.
        /// </summary>
        /// <typeparam name="T">Underlying type of array.</typeparam>
        /// <param name="array">Array to expand.</param>
        /// <param name="newValue">The element to tack on the end.</param>
        /// <returns>An array with a length +1 with the new element.</returns>
        public static T[] ExpandedWith<T>(this T[] array, T newValue) {
            var newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[array.Length] = newValue;
            return newArray;
        }

        /// <summary>
        /// Returns a copy of an array with a additional elements 'newValues' tacked on the end.
        /// </summary>
        /// <typeparam name="T">Underlying type of array.</typeparam>
        /// <param name="array">Array to expand.</param>
        /// <param name="newValue">The elements to tack on the end.</param>
        /// <returns>An array with a length +newValues.Length with the new elements.</returns>
        public static T[] ExpandedWith<T>(this T[] array, T[] newValues) {
            var newArray = new T[array.Length + newValues.Length];
            array.CopyTo(newArray, 0);
            newValues.CopyTo(newArray, array.Length);
            return newArray;
        }

        /// <summary>
        /// Returns a copy of an array with additional uninitialized elements at the end.
        /// </summary>
        /// <typeparam name="T">Underlying type of array.</typeparam>
        /// <param name="array">Array to expand.</param>
        /// <param name="newElements">The number of new uninitialized elements tacked on the end.</param>
        /// <returns>An array with a length +newElements with new uninitialized elements.</returns>
        public static T[] Expanded<T>(this T[] array, int newElements) {
            var newArray = new T[array.Length + newElements];
            array.CopyTo(newArray, 0);
            return newArray;
        }

        /// <summary>
        /// Returns a copy of a 1D array coverted to a 2D array in row-major order.
        /// </summary>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 1D array to convert.</param>
        /// <param name="width">The width (first index) of the new 2D array.</param>
        /// <param name="height">The height (second index) of the new 2D array.</param>
        /// <returns>A new 2D array with data from the input 1D array in row-major order.</returns>
        /// <exception cref="ArgumentException">Thrown if width * height != array.Length</exception>
        public static T[,] To2DArray<T>(this T[] array, int width, int height) {
            if (width * height != array.Length)
                throw new ArgumentException(nameof(array) + ": length does not equal width x height");

            var newArray = new T[width, height];
            int pos = 0;
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    newArray[x, y] = array[pos++];

            return newArray;
        }

        /// <summary>
        /// Returns a copy of a 1D array coverted to a 2D array in column-major order.
        /// </summary>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 1D array to convert.</param>
        /// <param name="width">The width (first index) of the new 2D array.</param>
        /// <param name="height">The height (second index) of the new 2D array.</param>
        /// <returns>A new 2D array with data from the input 1D array in column-major order.</returns>
        /// <exception cref="ArgumentException">Thrown if width * height != array.Length</exception>
        public static T[,] To2DArrayColumnMajor<T>(this T[] array, int width, int height) {
            if (width * height != array.Length)
                throw new ArgumentException(nameof(array) + ": length does not equal width x height");

            var newArray = new T[width, height];
            int pos = 0;
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    newArray[x, y] = array[pos++];

            return newArray;
        }

        /// <summary>
        /// Returns a copy of a 2D array in converted to a 1D array.
        /// </summary>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 2D array to convert.</param>
        /// <returns>A new 1D array with data from the input 2D array copied.</returns>
        public static T[] To1DArray<T>(this T[,] array) {
            var len1 = array.GetLength(0);
            var len2 = array.GetLength(1);

            var newArray = new T[len1 * len2];
            int pos = 0;
            for (var i1 = 0; i1 < len1; i1++)
                for (var i2 = 0; i2 < len2; i2++)
                    newArray[pos++] = array[i1, i2];

            return newArray;
        }

        /// <summary>
        /// Returns a copy of a 2D array in converted to a 1D array with the dimensions swapped.
        /// </summary>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 2D array to convert.</param>
        /// <returns>A new 1D array with data from the input 2D array copied with the dimensions wapped.</returns>
        public static T[] To1DArrayTransposed<T>(this T[,] array) {
            var len1 = array.GetLength(0);
            var len2 = array.GetLength(1);

            var newArray = new T[len1 * len2];
            int pos = 0;
            for (var i2 = 0; i2 < len2; i2++)
                for (var i1 = 0; i1 < len1; i1++)
                    newArray[pos++] = array[i1, i2];

            return newArray;
        }

        /// <summary>
        /// Converts an array of bytes divided into tile chunks into image data in column-major order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 1D array to convert.</param>
        /// <param name="width">The width (first index) of the new 2D array.</param>
        /// <param name="height">The height (second index) of the new 2D array.</param>
        /// <param name="tileWidth">Width of each tile in the input array.</param>
        /// <param name="tileHeight">Height of each tile in the input array.</param>
        /// <returns>A new 2D array with data from the input 1D array in column-major order.</returns>
        /// <exception cref="ArgumentException">Thrown if width * height != array.Length, or width or height is not divisible tile width or height.</exception>
        public static T[,] ToTiles<T>(this T[] array, int width, int height, int tileWidth, int tileHeight) {
            if (width * height != array.Length)
                throw new ArgumentException(nameof(array) + ": length does not equal width x height");
            if (width % tileWidth != 0)
                throw new ArgumentException($"{nameof(width)} ({width}) is not divisible by {nameof(tileWidth)} ({tileWidth})");
            if (height % tileHeight != 0)
                throw new ArgumentException($"{nameof(height)} ({height}) is not divisible by {nameof(tileHeight)} ({tileHeight})");

            var newArray = new T[width, height];

            var tileXCount = width  / tileWidth;
            var tileYCount = height / tileHeight;
            var tileCount = tileXCount * tileYCount;

            int pos = 0;
            for (var tile = 0; tile < tileCount; tile++) {
                int tileX = (tile % tileXCount) * tileWidth;
                int tileY = (tile / tileXCount) * tileHeight;
                for (var y = 0; y < tileHeight; y++)
                    for (var x = 0; x < tileWidth; x++)
                        newArray[tileX + x, tileY + y] = array[pos++];
            }

            return newArray;
        }

        /// <summary>
        /// Checks an array for a sub-array and returns the index if found, or -1 if not.
        /// If 'allowExceedingSize' is 'true', then this method will search for 'needle' beyond the end of 'haystack',
        /// using 'null' for values beyond the end of 'haystack'.
        /// </summary>
        /// <typeparam name="T">Type of array to search through.</typeparam>
        /// <param name="haystack">Array of elements T to search through.</param>
        /// <param name="needle">Array of elements T whose index we want to find.</param>
        /// <param name="allowExceedingSize">When 'true', this method will search for 'needle' beyond the end of 'haystack',
        /// using 'null' for values beyond the end of 'haystack'.</param>
        /// <returns></returns>
        public static int GetFirstIndexOf<T>(this T[] haystack, T[] needle, bool allowExceedingSize) where T : class {
            bool GetFirstIndexOfSub(int haystackIndex, int needleIndex) {
                if (needleIndex == needle.Length)
                    return true;

                var needleValue = needle[needleIndex];
                var haystackValue = (haystackIndex < haystack.Length) ? haystack[haystackIndex] : null;

                var equal = (needleValue == null && haystackValue == null) ||
                            (needleValue != null && haystackValue != null && haystackValue.Equals(needleValue));

                return equal && GetFirstIndexOfSub(haystackIndex + 1, needleIndex + 1);
            }

            // Use a recursive algorithm to find it!
            var endPos = haystack.Length + 1 - (allowExceedingSize ? 0 : needle.Length);
            for (int i = 0; i < endPos; i++) {
                if (GetFirstIndexOfSub(i, 0))
                    return i;
            }

            // Not found
            return -1;
        }

        /// <summary>
        /// Converts an array of bytes to an array of ushorts.
        /// </summary>
        /// <param name="bytes">Input array to convert.</param>
        /// <returns>An array of ushort[] with half the length of 'bytes'.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static ushort[] ToUShorts(this byte[] bytes) {
            if (bytes.Length % 2 != 0)
                throw new ArgumentException($"'{nameof(bytes)}.Length' must be divisible by 2");

            var shorts = new ushort[bytes.Length / 2];
            int pos = 0;
            for (int i = 0; i < shorts.Length; i++) {
                shorts[i] = (ushort) (bytes[pos++] << 8);
                shorts[i] += bytes[pos++];
            }

            return shorts;
        }
    }
}
