using System.Collections.Generic;
using System.Linq;
using SF3.Types;

namespace SF3.Utils {
    /// <summary>
    /// Miscellaneous utility functions.
    /// </summary>
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
        public static string NameOrHexValue(int value, Dictionary<int, string> nameDict, int hexDigits = 2) {
            return nameDict.TryGetValue(value, out string name)
                ? name
                : value.ToString("X" + hexDigits.ToString());
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
        public static string HexValueWithName(int value, Dictionary<int, string> nameDict, int hexDigits = 2) {
            var valueStr = value.ToString("X" + hexDigits.ToString());
            return nameDict.TryGetValue(value, out string name)
                ? valueStr + ": " + name
                : valueStr;
        }

        /// <summary>
        /// Creates a dictionary of the results of MakeNamedValueComboBoxValues() for all scenarios.
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="factoryFunc">Factory function to create a NamedValue. Used for looking up 'NamedValue.Name' for all possible values.</param>
        /// <returns>A dictionary of the results of MakeNamedValueComboBoxValues() for all scenarios.</returns>
        public static Dictionary<ScenarioType, Dictionary<NamedValue, string>> MakeNamedValueComboBoxValuesForAllScenarios(Dictionary<ScenarioType, NamedValue> baseValues)
            => baseValues.ToDictionary(x => x.Key, x => MakeNamedValueComboBoxValues(x.Value));

        /// <summary>
        /// Creates a dictionary of all possible values to supply to MakeNamedValueComboBox().
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="factoryFunc">Factory function to create a NamedValue. Used for looking up 'NamedValue.Name' for all possible values.</param>
        /// <returns>A key-value dictionary of all possible values (key) and their names (value).</returns>
        public static Dictionary<NamedValue, string> MakeNamedValueComboBoxValues(NamedValue baseValue) {
            var dict = new Dictionary<NamedValue, string>();

            // Add values below MinValue...
            foreach (var kv in baseValue.PossibleValues.Where(x => x.Key < baseValue.MinValue).OrderBy(x => x.Key)) {
                var value = baseValue.MakeRelatedValue(kv.Key);
                dict.Add(value, value.ValueName);
            }

            // ...add values in range (MinValue, MaxValue)....
            for (int i = baseValue.MinValue; i <= baseValue.MaxValue; i++) {
                var value = baseValue.MakeRelatedValue(i);
                dict.Add(value, value.ValueName);
            }

            // ...and finally add values above MaxValue.
            foreach (var kv in baseValue.PossibleValues.Where(x => x.Key > baseValue.MaxValue).OrderBy(x => x.Key)) {
                var value = baseValue.MakeRelatedValue(kv.Key);
                dict.Add(value, value.ValueName);
            }

            return dict;
        }
    }
}
