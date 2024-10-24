namespace SF3.Extensions {
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
    }
}
