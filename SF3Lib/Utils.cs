using SF3.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SF3
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

        /// <summary>
        /// Copies all properties of type T tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <typeparam name="T">The type whose properties should be </typeparam>
        /// <param name="objFrom">The object whose properties should be copied from.</param>
        /// <param name="objTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A list of all properties copied.</returns>
        public static List<PropertyInfo> BulkCopyProperties<T>(T objFrom, T objTo, bool inherit = true) where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | (inherit ? 0 : BindingFlags.DeclaredOnly))
                .Where(x => x.IsDefined(typeof(BulkCopyAttribute)))
                .ToList();

            foreach (var property in properties)
            {
                property.SetValue(objTo, property.GetValue(objFrom));
            }

            return properties;
        }

        /// <summary>
        /// Copies all properties of type T in an IEnumerable<T> tagged with [BulkCopy] in 'objFrom' to 'objTo'.
        /// </summary>
        /// <typeparam name="T">The type whose properties should be </typeparam>
        /// <param name="objFrom">The object whose properties should be copied from.</param>
        /// <param name="objTo">The object whose properties should be copied to.</param>
        /// <param name="inherit">When true (default), all inherited properties are copied.</param>
        /// <returns>A list of all properties copied.</returns>
        public static List<PropertyInfo> BulkCopyProperties<T>(IEnumerable<T> listFrom, IEnumerable<T> listTo, bool inherit = true) where T : class
        {
            var properties = typeof(T).GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    (inherit ? 0 : BindingFlags.DeclaredOnly))
                .Where(x => x.IsDefined(typeof(BulkCopyAttribute)))
                .ToList();

            var arrayFrom = listFrom.ToArray();
            var arrayTo = listTo.ToArray();

            if (arrayFrom.Length != arrayTo.Length)
            {
                // TODO: make it better!!
                throw new ArgumentException("Model lists need to have the same number of elements!");
            }

            foreach (var property in properties)
            {
                for (int i = 0; i < arrayFrom.Length; i++)
                {
                    var value = property.GetValue(arrayFrom[i]);
                    property.SetValue(arrayTo[i], value);
                }
            }

            return properties;
        }
    }
}
