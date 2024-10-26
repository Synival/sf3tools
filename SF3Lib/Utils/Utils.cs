using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Types;
using static CommonLib.Utils.ControlUtils;

namespace SF3.Utils {
    /// <summary>
    /// Miscellaneous utility functions.
    /// </summary>
    public static class Utils {
        /// <summary>
        /// Creates a dictionary of the results of MakeNamedValueComboBoxValues() for all scenarios.
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <param name="factoryFunc">Factory function to create a NamedValue. Used for looking up 'NamedValue.Name' for all possible values.</param>
        /// <returns>A dictionary of the results of MakeNamedValueComboBoxValues() for all scenarios.</returns>
        public static Dictionary<ScenarioType, Dictionary<NamedValue, string>> MakeNamedValueComboBoxValuesForAllScenarios(Dictionary<ScenarioType, NamedValue> baseValues)
            => baseValues.ToDictionary(x => x.Key, x => MakeNamedValueComboBoxValues(x.Value));
    }
}
