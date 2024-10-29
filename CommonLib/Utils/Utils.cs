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
    }
}
