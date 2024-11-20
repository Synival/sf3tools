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
        /// Returns a copy of a 1D array coverted to a 2D array in row major order.
        /// </summary>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 1D array to convert.</param>
        /// <param name="width">The width (first index) of the new 2D array.</param>
        /// <param name="height">The height (second index) of the new 2D array.</param>
        /// <returns>A new 2D array with data from the input 1D array in row major order.</returns>
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
        /// Returns a copy of a 2D array in row major order converted to a 1D arrray.
        /// </summary>
        /// <typeparam name="T">Type of array to convert.</typeparam>
        /// <param name="array">The 2D array to convert.</param>
        /// <returns>A new 1D array with data from the input 2D array copied in row major order.</returns>
        public static T[] To1DArray<T>(this T[,] array) {
            var width = array.GetLength(0);
            var height = array.GetLength(1);

            var newArray = new T[width * height];
            int pos = 0;
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    newArray[pos++] = array[x, y];

            return newArray;
        }
    }
}
