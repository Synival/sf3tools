namespace DFRLib.Extensions {
    public static class ArrayExtensions {
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
    }
}
