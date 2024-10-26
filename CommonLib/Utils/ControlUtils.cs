﻿using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;

namespace CommonLib.Utils {
    public static class ControlUtils {

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
