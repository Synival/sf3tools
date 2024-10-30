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
    }
}
