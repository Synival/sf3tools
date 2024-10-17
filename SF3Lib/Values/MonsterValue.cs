using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    /// <summary>
    /// Named value for Monster ID's that can be bound to an ObjectListView.
    /// </summary>
    public class MonsterValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        // Values without FFFF
        public static readonly Dictionary<ScenarioType, Dictionary<int, string>> ValueNames = new Dictionary<ScenarioType, Dictionary<int, string>>()
        {
            { ScenarioType.Scenario1,   GetValueNameDictionaryFromXML("Resources/S1/Monsters.xml") },
            { ScenarioType.Scenario2,   GetValueNameDictionaryFromXML("Resources/S2/Monsters.xml") },
            { ScenarioType.Scenario3,   GetValueNameDictionaryFromXML("Resources/S3/Monsters.xml") },
            { ScenarioType.PremiumDisk, GetValueNameDictionaryFromXML("Resources/PD/Monsters.xml") },
        };

        public static readonly Dictionary<int, string> ValueNamesPDX044 = GetValueNameDictionaryFromXML("Resources/PD/Monsters_X044.xml");

        public static readonly Dictionary<ScenarioType, Dictionary<NamedValue, string>> _comboBoxValues = new Dictionary<ScenarioType, Dictionary<NamedValue, string>>()
        {
            { ScenarioType.Scenario1, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new MonsterValue(ScenarioType.Scenario1, false, value, false)) },
            { ScenarioType.Scenario2, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new MonsterValue(ScenarioType.Scenario2, false, value, false)) },
            { ScenarioType.Scenario3, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new MonsterValue(ScenarioType.Scenario3, false, value, false)) },
            { ScenarioType.PremiumDisk, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new MonsterValue(ScenarioType.PremiumDisk, false, value, false)) },
        };

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
                dict.Select(x => {
                    var mv = x.Key as MonsterValue;
                    return new KeyValuePair<NamedValue, string>(new MonsterValue(mv.Scenario, mv.IsX044, mv.Value, true), mv.Name);
                })
                .ToDictionary(x => x.Key, x => x.Value)
            )
            {
                { newValue, newValue.Name }
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

        public MonsterValue(ScenarioType scenario, bool isX044, int value, bool withFFFF)
        : base(HexValueWithName(
            value,
            withFFFF
                ? (scenario == ScenarioType.PremiumDisk && isX044) ? ValueNamesPDX044_WithFFFF : ValueNames_WithFFFF[scenario]
                : (scenario == ScenarioType.PremiumDisk && isX044) ? ValueNamesPDX044 : ValueNames[scenario]), value)
        {
            Scenario = scenario;
            IsX044 = isX044;
            WithFFFF = withFFFF;
        }

        public override Dictionary<NamedValue, string> ComboBoxValues =>
            WithFFFF
                ? (Scenario == ScenarioType.PremiumDisk && IsX044) ? _comboBoxValuesPDX044_WithFFFF : _comboBoxValues_WithFFFF[Scenario]
                : (Scenario == ScenarioType.PremiumDisk && IsX044) ? _comboBoxValuesPDX044 : _comboBoxValues[Scenario];

        private ScenarioType Scenario { get; }

        private bool IsX044 { get; }

        private bool WithFFFF { get; }
    }
}
