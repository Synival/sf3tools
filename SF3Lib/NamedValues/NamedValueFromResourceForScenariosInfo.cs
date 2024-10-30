using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Types;
using static SF3.Utils.Resources;

namespace SF3.NamedValues {
    /// <summary>
    /// Named value info from a resource that is specific to each scenario.
    /// </summary>
    public class NamedValueFromResourceForScenariosInfo : INamedValueFromResourceForScenariosInfo {
        public NamedValueFromResourceForScenariosInfo(string resourceName, int minValue = 0x00, int maxValue = 0xFF, string formatString = "X2")
        {
            ResourceName = resourceName;
            MinValue = minValue;
            MaxValue = maxValue;
            FormatString = formatString;
            var possibleValues = GetValueNameDictionaryForAllScenariosFromXML(ResourceName);

            Info = possibleValues
                .ToDictionary(
                    x => x.Key,
                    x => (INamedValueInfo) new NamedValueInfo(MinValue, MaxValue, FormatString, x.Value)
                );
        }

        public string ResourceName { get; }
        public virtual int MinValue { get; }
        public virtual int MaxValue { get; }
        public virtual string FormatString { get; }

        public Dictionary<ScenarioType, INamedValueInfo> Info { get; }
    };
}
