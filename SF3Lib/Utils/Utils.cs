using SF3.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SF3.Utils
{
    /// <summary>
    /// Miscellaneous utility functions.
    /// </summary>
    public static class Utils
    {
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
        ///     {0:X[hexDigits]: [name]
        /// </returns>
        public static string HexValueWithName(int value, Dictionary<int, string> nameDict, int hexDigits = 2)
        {
            var valueStr = value.ToString("X" + hexDigits.ToString());
            string className;
            return nameDict.TryGetValue(value, out className)
                ? valueStr + ": " + className
                : valueStr;
        }

        /// <summary>
        /// Creates a dictionary of all possible values to supply to MakeNamedValueComboBox().
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="factoryFunc">Factory function to create a NamedValue. Used for looking up 'NamedValue.Name' for all possible values.</param>
        /// <returns>A key-value dictionary of all possible values (key) and their names (value).</returns>
        public static Dictionary<NamedValue, string> MakeNamedValueComboBoxValues(int minValue, int maxValue, Func<int, NamedValue> factoryFunc)
        {
            var dict = new Dictionary<NamedValue, string>();
            for (int i = minValue; i < maxValue; i++)
            {
                var value = factoryFunc(i);
                dict.Add(value, value.Name);
            }
            return dict;
        }
    }
}
