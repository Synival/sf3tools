using System.Collections.Generic;
using System.Linq;
using static CommonLib.Utils.Utils;

namespace CommonLib.NamedValues {
    /// <summary>
    /// Information about a named value type.
    /// </summary>
    public class NamedValueInfo : INamedValueInfo {
        public NamedValueInfo(int minValue, int maxValue, string formatString, Dictionary<int, string> possibleValues) {
            MinValue = minValue;
            MaxValue = maxValue;
            FormatString = formatString;
            Values = possibleValues;
            ComboBoxValues = MakeNamedValueComboBoxValues();
        }

        /// <summary>
        /// Creates a dictionary of all possible values to supply to MakeNamedValueComboBox().
        /// </summary>
        /// <returns>A key-value dictionary of all possible values (key) and their names (value).</returns>
        private Dictionary<int, string> MakeNamedValueComboBoxValues() {
            var dict = new Dictionary<int, string>();
            string getFullName(int v) => GetFullName(v, NameOrNull(v, Values), FormatString);

            // Add values from (MinValue...MaxValue)
            for (var i = MinValue; i <= MaxValue; i++)
                dict.Add(i, getFullName(i));

            // And any missing values in info.Values
            foreach (var kv in Values)
                if (!dict.ContainsKey(kv.Key))
                    dict.Add(kv.Key, getFullName(kv.Key));

            return dict
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public int MinValue { get; }
        public int MaxValue { get; }
        public string FormatString { get; }
        public Dictionary<int, string> Values { get; }
        public Dictionary<int, string> ComboBoxValues { get; }
    }
}