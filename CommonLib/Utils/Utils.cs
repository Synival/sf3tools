using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Utils {
    public static class Utils {
        /// <summary>
        /// Returns a string with the name in a name dictionary or, if it's not available, the value in hex format.
        /// </summary>
        /// <param name="value">Value for which a string should be generated</param>
        /// <param name="nameDict">Dictionary with possible names for 'value'</param>
        /// <param name="hexDigits">Number of digits for the hexideimcal portion of the return string</param>
        /// <returns>
        /// If no name available:
        ///     {0:X[hexDigits]}
        /// If value name available:
        ///     {name}
        /// </returns>
        public static string NameOrHexValue(int value, Dictionary<int, string> nameDict, string formatString = null) {
            return nameDict.TryGetValue(value, out string name)
                ? name
                : value.ToString(formatString ?? "X2");
        }

        /// <summary>
        /// Returns a string with a stringified hex value followed by a value name if available in a dictionary.
        /// </summary>
        /// <param name="value">Value for which a string should be generated</param>
        /// <param name="nameDict">Dictionary with possible names for 'value'</param>
        /// <param name="hexDigits">Number of digits for the hexideimcal portion of the return string</param>
        /// <returns>
        /// If no name available:
        ///     {0:X[hexDigits]}
        /// If value name available:
        ///     {0:X[hexDigits]}: {name}
        /// </returns>
        public static string HexValueWithName(int value, Dictionary<int, string> nameDict, string formatString = null) {
            var valueStr = value.ToString(formatString ?? "X2");
            return nameDict.TryGetValue(value, out string name)
                ? valueStr + ": " + name
                : valueStr;
        }
    }
}
