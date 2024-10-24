using System.Collections.Generic;
using System.Linq;
using SF3.Types;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values {
    public class MonsterValueResourceInfo : NamedValueFromResourceForScenariosInfo {
        public MonsterValueResourceInfo() : base("Monsters.xml") {
            PossibleValuesPDX044 = GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.PremiumDisk, "Monsters_X044.xml"));

            PossibleValues_WithFFFF = PossibleValues.ToDictionary(x => x.Key, x => DictionaryWithFFFF(x.Value));
            PossibleValuesPDX044_WithFFFF = DictionaryWithFFFF(PossibleValuesPDX044);

            // Scary (and stupid) matrix will all possible ComboBoxValues.
            ExpandedComboBoxValues = new Dictionary<bool, Dictionary<bool, Dictionary<ScenarioType, Dictionary<NamedValue, string>>>>()
            {
                { false, new Dictionary<bool, Dictionary<ScenarioType, Dictionary<NamedValue, string>>>()
                    {
                        { false, ComboBoxValues },
                        { true, new Dictionary<ScenarioType, Dictionary<NamedValue, string>>() }
                    }
                },
                { true, new Dictionary<bool, Dictionary<ScenarioType, Dictionary<NamedValue, string>>>()
                    {
                        { false, new Dictionary<ScenarioType, Dictionary<NamedValue, string>>() },
                        { true, new Dictionary<ScenarioType, Dictionary<NamedValue, string>>() }
                    }
                }
            };
        }

        public Dictionary<ScenarioType, Dictionary<int, string>> PossibleValues_WithFFFF;

        public Dictionary<int, string> PossibleValuesPDX044 { get; }
        public Dictionary<int, string> PossibleValuesPDX044_WithFFFF { get; }

        // ExpandedComboboxValues[IsPDX044][WithFFFF][Scenario]
        public Dictionary<bool, Dictionary<bool, Dictionary<ScenarioType, Dictionary<NamedValue, string>>>> ExpandedComboBoxValues { get; }

        private static Dictionary<int, string> DictionaryWithFFFF(Dictionary<int, string> dict)
            => new Dictionary<int, string>(dict) { { 0xFFFF, "Character Slot" } };
    }

    /// <summary>
    /// Named value for Monster ID's that can be bound to an ObjectListView.
    /// </summary>
    public class MonsterValue : NamedValueFromResourceForScenarios<MonsterValueResourceInfo> {
        public MonsterValue(ScenarioType scenario, int value) : base(scenario, value) {
        }

        public MonsterValue(ScenarioType scenario, bool isX044, bool withFFFF, int value)
        : base(
            scenario,
            NameOrHexValue(value, GetPossibleValues(scenario, isX044, withFFFF)),
            HexValueWithName(value, GetPossibleValues(scenario, isX044, withFFFF)),
            value
        ) {
            IsX044 = isX044;
            WithFFFF = withFFFF;
        }

        private static Dictionary<int, string> GetPossibleValues(ScenarioType scenario, bool isX044, bool withFFFF) => withFFFF
            ? ((scenario == ScenarioType.PremiumDisk && isX044) ? ResourceInfo.PossibleValuesPDX044_WithFFFF : ResourceInfo.PossibleValues_WithFFFF[scenario])
            : ((scenario == ScenarioType.PremiumDisk && isX044) ? ResourceInfo.PossibleValuesPDX044 : ResourceInfo.PossibleValues[scenario]);

        public override NamedValue MakeRelatedValue(int value) => new MonsterValue(Scenario, IsX044, WithFFFF, value);

        public override Dictionary<int, string> PossibleValues => GetPossibleValues(Scenario, IsPDX044, WithFFFF);

        public override Dictionary<NamedValue, string> ComboBoxValues {
            get {
                if (!ResourceInfo.ExpandedComboBoxValues[IsPDX044][WithFFFF].ContainsKey(Scenario))
                    ResourceInfo.ExpandedComboBoxValues[IsPDX044][WithFFFF].Add(Scenario, MakeNamedValueComboBoxValues(this));
                return ResourceInfo.ExpandedComboBoxValues[IsPDX044][WithFFFF][Scenario];
            }
        }

        public bool IsX044 { get; }

        public bool IsPDX044 => Scenario == ScenarioType.PremiumDisk && IsPDX044;

        public bool WithFFFF { get; }
    }
}
