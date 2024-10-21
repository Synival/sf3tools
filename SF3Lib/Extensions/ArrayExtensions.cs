using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns a copy of an array with an additional element 'newValue' tacked on the end.
        /// </summary>
        /// <typeparam name="T">Underlying type of array.</typeparam>
        /// <param name="array">Array to expand.</param>
        /// <param name="newValue">The element to tack on the end.</param>
        /// <returns>An array with a length +1 with the new element.</returns>
        public static T[] ExpandedWith<T>(this T[] array, T newValue)
        {
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
        public static T[] ExpandedWith<T>(this T[] array, T[] newValues)
        {
            var newArray = new T[array.Length + newValues.Length];
            array.CopyTo(newArray, 0);
            newValues.CopyTo(newArray, array.Length);
            return newArray;
        }
    }
}
