using SF3.Types;
using System;
using System.Collections.Generic;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    public interface INamedValueFromResourceForScenariosInfo
    {
        string ResourceName { get; }
        int MinValue { get; }
        int MaxValue { get; }
        Dictionary<ScenarioType, Dictionary<int, string>> PossibleValues { get; }
        Dictionary<ScenarioType, Dictionary<NamedValue, string>> ComboBoxValues { get; }
    };

    public class NamedValueFromResourceForScenariosInfo : INamedValueFromResourceForScenariosInfo
    {
        public NamedValueFromResourceForScenariosInfo(string resourceName)
        {
            ResourceName = resourceName;
            PossibleValues = GetValueNameDictionaryForAllScenariosFromXML(ResourceName);
        }

        public string ResourceName { get; }
        public virtual int MinValue => 0x00;
        public virtual int MaxValue => 0xFF;
        public Dictionary<ScenarioType, Dictionary<int, string>> PossibleValues { get; }
        public Dictionary<ScenarioType, Dictionary<NamedValue, string>> ComboBoxValues { get; } = new Dictionary<ScenarioType, Dictionary<NamedValue, string>>();
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public abstract class NamedValueFromResourceForScenarios<TResourceInfo> : NamedValue
        where TResourceInfo : INamedValueFromResourceForScenariosInfo, new()
    {
        public NamedValueFromResourceForScenarios(ScenarioType scenario, int value)
        : base(
            NameOrHexValue(value, ResourceInfo.PossibleValues[scenario]),
            HexValueWithName(value, ResourceInfo.PossibleValues[scenario]),
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

        public static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public override int MinValue => ResourceInfo.MinValue;
        public override int MaxValue => ResourceInfo.MaxValue;

        public override Dictionary<int, string> PossibleValues => ResourceInfo.PossibleValues[Scenario];

        public override Dictionary<NamedValue, string> ComboBoxValues
        {
            get
            {
                if (!ResourceInfo.ComboBoxValues.ContainsKey(Scenario))
                {
                    ResourceInfo.ComboBoxValues.Add(Scenario, MakeNamedValueComboBoxValues(this));
                }
                return ResourceInfo.ComboBoxValues[Scenario];
            }
        }

        public ScenarioType Scenario { get; }
    }
}
