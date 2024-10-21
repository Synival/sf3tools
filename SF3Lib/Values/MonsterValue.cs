using SF3.Types;
using System.Collections.Generic;
using System.Linq;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    public class MonsterValueResourceInfo : INamedValueFromResourceInfo
    {
        public string ResourceName => "Monsters.xml";
        public int MinValue => 0;
        public int MaxValue => 0xFF;
    }

    /// <summary>
    /// Named value for Monster ID's that can be bound to an ObjectListView.
    /// </summary>
    public class MonsterValue : NamedValueFromResourceForScenarios<MonsterValue, MonsterValueResourceInfo>
    {
        public static readonly Dictionary<int, string> ValueNamesPDX044 = GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.PremiumDisk, "Monsters_X044.xml"));

        public static readonly Dictionary<NamedValue, string> _comboBoxValuesPDX044 = MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new MonsterValue(ScenarioType.PremiumDisk, true, value, false));

        // Values with FFFF
        private static Dictionary<int, string> DictionaryWithFFFF(Dictionary<int, string> dict)
        {
            return new Dictionary<int, string>(dict)
            {
                { 0xFFFF, "Character Slot" }
            };
        }

        private static Dictionary<NamedValue, string> DictionaryWithFFFF(Dictionary<NamedValue, string> dict)
        {
            var item = dict.First().Key as MonsterValue;
            var newValue = new MonsterValue(item.Scenario, item.IsX044, 0xFFFF, true);

            return new Dictionary<NamedValue, string>(
                dict.Select(x =>
                {
                    var mv = x.Key as MonsterValue;
                    return new KeyValuePair<NamedValue, string>(new MonsterValue(mv.Scenario, mv.IsX044, mv.Value, true), mv.ValueName);
                })
                .ToDictionary(x => x.Key, x => x.Value)
            )
            {
                { newValue, newValue.ValueName }
            };
        }

        public static readonly Dictionary<ScenarioType, Dictionary<int, string>> ValueNames_WithFFFF = new Dictionary<ScenarioType, Dictionary<int, string>>()
        {
            { ScenarioType.Scenario1,   DictionaryWithFFFF(ValueNames[ScenarioType.Scenario1]) },
            { ScenarioType.Scenario2,   DictionaryWithFFFF(ValueNames[ScenarioType.Scenario2]) },
            { ScenarioType.Scenario3,   DictionaryWithFFFF(ValueNames[ScenarioType.Scenario3]) },
            { ScenarioType.PremiumDisk, DictionaryWithFFFF(ValueNames[ScenarioType.PremiumDisk]) },
        };

        public static readonly Dictionary<int, string> ValueNamesPDX044_WithFFFF = DictionaryWithFFFF(ValueNamesPDX044);

        public static readonly Dictionary<ScenarioType, Dictionary<NamedValue, string>> _comboBoxValues_WithFFFF = new Dictionary<ScenarioType, Dictionary<NamedValue, string>>()
        {
            { ScenarioType.Scenario1,   DictionaryWithFFFF(_comboBoxValues[ScenarioType.Scenario1]) },
            { ScenarioType.Scenario2,   DictionaryWithFFFF(_comboBoxValues[ScenarioType.Scenario2]) },
            { ScenarioType.Scenario3,   DictionaryWithFFFF(_comboBoxValues[ScenarioType.Scenario3]) },
            { ScenarioType.PremiumDisk, DictionaryWithFFFF(_comboBoxValues[ScenarioType.PremiumDisk]) },
        };

        public static readonly Dictionary<NamedValue, string> _comboBoxValuesPDX044_WithFFFF = DictionaryWithFFFF(_comboBoxValuesPDX044);

        // --------------------------------------------------------------------------------

        public MonsterValue(ScenarioType scenario, int value) : base(scenario, value)
        { }

        public MonsterValue(ScenarioType scenario, bool isX044, int value, bool withFFFF)
        : base(
            scenario,
            NameOrHexValue(value, withFFFF
                ? (scenario == ScenarioType.PremiumDisk && isX044) ? ValueNamesPDX044_WithFFFF : ValueNames_WithFFFF[scenario]
                : (scenario == ScenarioType.PremiumDisk && isX044) ? ValueNamesPDX044 : ValueNames[scenario]),
            HexValueWithName(value, withFFFF
                ? (scenario == ScenarioType.PremiumDisk && isX044) ? ValueNamesPDX044_WithFFFF : ValueNames_WithFFFF[scenario]
                : (scenario == ScenarioType.PremiumDisk && isX044) ? ValueNamesPDX044 : ValueNames[scenario]),
            value)
        {
            IsX044 = isX044;
            WithFFFF = withFFFF;
        }

        public override Dictionary<NamedValue, string> ComboBoxValues =>
            WithFFFF
                ? (Scenario == ScenarioType.PremiumDisk && IsX044) ? _comboBoxValuesPDX044_WithFFFF : _comboBoxValues_WithFFFF[Scenario]
                : (Scenario == ScenarioType.PremiumDisk && IsX044) ? _comboBoxValuesPDX044 : _comboBoxValues[Scenario];

        public bool IsX044 { get; }

        public bool WithFFFF { get; }
    }
}
