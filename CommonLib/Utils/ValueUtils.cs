using System.Collections.Generic;

namespace CommonLib.Utils {
    public static class ValueUtils {
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
        /// Returns a string in the format "0xXXXX" or "-0xXXXX" depending on the signedness of the value.
        /// </summary>
        /// <param name="value">The value to convert to a signed hex value string.</param>
        /// <param name="format">GetString() format of the string after the '0x' porition. Should probably be something like "X2".</param>
        /// <returns>A string starting with a sign (if negative), '0x', then the value formatted according to 'format'.</returns>
        public static string SignedHexStr(int value, string format)
            => SignedHexStr((uint) value, format);

        /// <summary>
        /// Returns a string in the format "0xXXXX" or "-0xXXXX" depending on the signedness of the value.
        /// </summary>
        /// <param name="value">The value to convert to a signed hex value string.</param>
        /// <param name="format">GetString() format of the string after the '0x' porition. Should probably be something like "X2".</param>
        /// <returns>A string starting with a sign (if negative), '0x', then the value formatted according to 'format'.</returns>
        public static string SignedHexStr(uint value, string format) {
            if ((value & 0x80000000u) != 0)
                return "-0x" + (0x80000000u - (value & 0x7FFFFFFFu)).ToString(format);
            else
                return "0x" + value.ToString(format);
        }
    }
}
