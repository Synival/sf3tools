using SF3.Types;
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

        /// <summary>
        /// Parses an XML file and returns a Dictionary of values (attribute "value") with their names (attribute "name").
        /// </summary>
        /// <param name="filename">XML file to parse.</param>
        /// <returns>a Dictionary of values (attribute "value") with their names (attribute "name").</returns>
        public static Dictionary<int, string> GetValueNameDictionaryFromXML(string filename)
        {
            var stream = new FileStream(filename, FileMode.Open);

            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            var xml = XmlReader.Create(stream, settings);
            xml.Read();

            var nameDict = new Dictionary<int, string>();
            while (!xml.EOF)
            {
                xml.Read();
                if (xml.HasAttributes)
                {
                    string valueStr = xml.GetAttribute("value");
                    string name = xml.GetAttribute("name");

                    if (valueStr != null && name != null)
                    {
                        nameDict.Add(Convert.ToInt32(valueStr, 16), name);
                    }
                }
            }

            return nameDict;
        }

        /// <summary>
        /// Returns the relative path of a resource file that is different depending on the scenario.
        /// </summary>
        /// <param name="scenario">The scenario for the resource.</param>
        /// <param name="resourceName">The name of the resource file without its path.</param>
        /// <returns>A string with a relative path with file for the resource.</returns>
        /// <exception cref="ArgumentException">Thrown if the scenario is invalid.</exception>
        public static string ResourceFileForScenario(ScenarioType scenario, string resourceName)
        {
            switch (scenario)
            {
                case ScenarioType.Scenario1:
                    return "Resources/S1/" + resourceName;
                case ScenarioType.Scenario2:
                    return "Resources/S2/" + resourceName;
                case ScenarioType.Scenario3:
                    return "Resources/S3/" + resourceName;
                case ScenarioType.PremiumDisk:
                    return "Resources/PD/" + resourceName;
                default:
                    throw new ArgumentException(MethodBase.GetCurrentMethod().Name + ": Handled scenario for " + nameof(scenario));
            }
        }
    }
}
