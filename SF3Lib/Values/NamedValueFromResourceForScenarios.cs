using System.Collections.Generic;
using System.Linq;
using CommonLib;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using SF3.Types;
using static CommonLib.Utils.ControlUtils;
using static CommonLib.Utils.Utils;
using static SF3.Utils.Resources;

namespace SF3.Values {
    public interface INamedValueFromResourceForScenariosInfo {
        string ResourceName { get; }
        int MinValue { get; }
        int MaxValue { get; }
        string FormatString { get; }
        Dictionary<ScenarioType, INamedValueInfo> Info { get; }
        Dictionary<ScenarioType, Dictionary<NamedValue, string>> ComboBoxValues { get; }
    };

    public class NamedValueFromResourceForScenariosInfo : INamedValueFromResourceForScenariosInfo {
        public NamedValueFromResourceForScenariosInfo(string resourceName)
        {
            ResourceName = resourceName;
            var possibleValues = GetValueNameDictionaryForAllScenariosFromXML(ResourceName);

            Info = possibleValues
                .ToDictionary(
                    x => x.Key,
                    x => (INamedValueInfo) new NamedValueInfo(MinValue, MaxValue, FormatString, x.Value)
                );
        }

        public string ResourceName { get; }
        public virtual int MinValue => 0x00;
        public virtual int MaxValue => 0xFF;
        public virtual string FormatString => "X2";
        public Dictionary<ScenarioType, Dictionary<NamedValue, string>> ComboBoxValues { get; } = new Dictionary<ScenarioType, Dictionary<NamedValue, string>>();

        public Dictionary<ScenarioType, INamedValueInfo> Info { get; }
    };

    /// <summary>
    /// Named value with values from a resource file that can be bound to an ObjectListView.
    /// </summary>
    public abstract class NamedValueFromResourceForScenarios<TResourceInfo> : NamedValue
        where TResourceInfo : INamedValueFromResourceForScenariosInfo, new() {
        public NamedValueFromResourceForScenarios(ScenarioType scenario, int value)
        : base(
            NameOrNull(value, ResourceInfo.Info[scenario].Values),
            value.ToStringHex(ResourceInfo.FormatString, ""),
            value
        ) {
            Scenario = scenario;
        }

        public NamedValueFromResourceForScenarios(ScenarioType scenario, string name, string valueName, int value)
        : base(name, valueName, value) {
            Scenario = scenario;
        }

        public static readonly TResourceInfo ResourceInfo = new TResourceInfo();

        public override int MinValue => ResourceInfo.MinValue;
        public override int MaxValue => ResourceInfo.MaxValue;

        public override Dictionary<int, string> PossibleValues => ResourceInfo.Info[Scenario].Values;

        public override Dictionary<NamedValue, string> ComboBoxValues {
            get {
                if (!ResourceInfo.ComboBoxValues.ContainsKey(Scenario))
                    ResourceInfo.ComboBoxValues.Add(Scenario, MakeNamedValueComboBoxValues(this));
                return ResourceInfo.ComboBoxValues[Scenario];
            }
        }

        public ScenarioType Scenario { get; }
    }
}
