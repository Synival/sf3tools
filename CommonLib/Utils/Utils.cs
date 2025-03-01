using System;
using System.Collections.Generic;

namespace CommonLib.Utils {
    public static class Utils {
        /// <summary>
        /// Returns a string with the name in a name dictionary or null if it's unavailable.
        /// </summary>
        /// <param name="value">Value for which a string should be generated</param>
        /// <param name="nameDict">Dictionary with possible names for 'value'</param>
        /// <returns> The corresponding name in the dictionary or 'null'.</returns>
        public static string NameOrNull(int value, Dictionary<int, string> nameDict)
            => nameDict.TryGetValue(value, out var name) ? name : null;

        /// <summary>
        /// Returns a combination of a value (formatted) and a name, if one exists.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">Name of the value. Can be 'null'.</param>
        /// <param name="formatString"></param>
        /// <returns>A string in the format of either:
        ///     {formatted-value}
        ///     {formatted-vaule}: name
        /// </returns>
        public static string GetFullName(int value, string name, string formatString = "X2") {
            var hexName = value.ToString(formatString);
            return hexName + ((name != null) ? ": " + name : "");
        }

        /// <summary>
        /// Returns the name of the enum if it's defined or an alternative string retrieved with a function.
        /// </summary>
        /// <typeparam name="T">Type of the enum whose value you want to retrieve.</typeparam>
        /// <param name="value">Value to retrieve a name from.</param>
        /// <param name="alternate">Function to use if the value in the requested enum T is not defined.</param>
        /// <returns>The name of the enum value if it is defined or a result of alternate(value).</returns>
        public static string EnumNameOr<T>(T value, Func<T, string> alternate) where T : System.Enum
            => Enum.IsDefined(typeof(T), value) ? value.ToString() : alternate(value);
    }
}
