﻿using SF3.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SF3.Utils
{
    /// <summary>
    /// Miscellaneous utility functions.
    /// </summary>
    public static class Utils
    {
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
        public static string NameOrHexValue(int value, Dictionary<int, string> nameDict, int hexDigits = 2)
        {
            string name;
            return nameDict.TryGetValue(value, out name)
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
        public static string HexValueWithName(int value, Dictionary<int, string> nameDict, int hexDigits = 2)
        {
            var valueStr = value.ToString("X" + hexDigits.ToString());
            string name;
            return nameDict.TryGetValue(value, out name)
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
        public static Dictionary<ScenarioType, Dictionary<NamedValue, string>> MakeNamedValueComboBoxValuesForAllScenarios(int minValue, int maxValue, Func<ScenarioType, int, NamedValue> factoryFunc)
        {
            return new Dictionary<ScenarioType, Dictionary<NamedValue, string>>()
            {
                { ScenarioType.Scenario1, MakeNamedValueComboBoxValues(minValue, maxValue, v => factoryFunc(ScenarioType.Scenario1, v)) },
                { ScenarioType.Scenario2, MakeNamedValueComboBoxValues(minValue, maxValue, v => factoryFunc(ScenarioType.Scenario2, v)) },
                { ScenarioType.Scenario3, MakeNamedValueComboBoxValues(minValue, maxValue, v => factoryFunc(ScenarioType.Scenario3, v)) },
                { ScenarioType.PremiumDisk, MakeNamedValueComboBoxValues(minValue, maxValue, v => factoryFunc(ScenarioType.PremiumDisk, v)) },
            };
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
                dict.Add(value, value.ValueName);
            }
            return dict;
        }
    }
}
