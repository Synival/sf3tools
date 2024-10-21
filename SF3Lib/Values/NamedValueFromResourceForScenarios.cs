using SF3.Types;
using System;
using System.Collections.Generic;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public class NamedValueFromResourceForScenarios<TSelf, TResourceInfo> : NamedValue
        where TSelf : NamedValue
        where TResourceInfo : INamedValueFromResourceInfo, new()
    {
        private static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public static readonly int MinValue = ResourceInfo.MinValue;
        public static readonly int MaxValue = ResourceInfo.MaxValue;

        public static readonly Dictionary<ScenarioType, Dictionary<int, string>> ValueNames = GetValueNameDictionaryForAllScenariosFromXML(ResourceInfo.ResourceName);

        public static readonly Dictionary<ScenarioType, Dictionary<NamedValue, string>> _comboBoxValues =
            MakeNamedValueComboBoxValuesForAllScenarios(MinValue, MaxValue, (s, v) => (TSelf)Activator.CreateInstance(typeof(TSelf), s, v));

        public NamedValueFromResourceForScenarios(ScenarioType scenario, int value)
        : base(
            NameOrHexValue(value, ValueNames[scenario]),
            HexValueWithName(value, ValueNames[scenario]),
            value
        )
        {
            Scenario = scenario;
        }

        public NamedValueFromResourceForScenarios(ScenarioType scenario, string name, string valueName, int value)
        : base(name, valueName, value)
        {
            Scenario = scenario;
        }

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues[Scenario];

        public ScenarioType Scenario { get; }
    }
}
