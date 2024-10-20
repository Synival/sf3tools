using SF3.Types;
using System.Collections.Generic;
using static SF3.Utils.Resources;
using static SF3.Utils.Utils;

namespace SF3.Values
{
    /// <summary>
    /// Named value for Special Attack ID's that can be bound to an ObjectListView.
    /// </summary>
    public class SpecialValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        public static readonly Dictionary<ScenarioType, Dictionary<int, string>> ValueNames = GetValueNameDictionaryForAllScenariosFromXML("Specials.xml");

        public static readonly Dictionary<ScenarioType, Dictionary<NamedValue, string>> _comboBoxValues = new Dictionary<ScenarioType, Dictionary<NamedValue, string>>()
        {
            { ScenarioType.Scenario1, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpecialValue(ScenarioType.Scenario1, value)) },
            { ScenarioType.Scenario2, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpecialValue(ScenarioType.Scenario2, value)) },
            { ScenarioType.Scenario3, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpecialValue(ScenarioType.Scenario3, value)) },
            { ScenarioType.PremiumDisk, MakeNamedValueComboBoxValues(MinValue, MaxValue, (int value) => new SpecialValue(ScenarioType.PremiumDisk, value)) },
        };

        public SpecialValue(ScenarioType scenario, int value) : base(NameOrHexValue(value, ValueNames[scenario]), HexValueWithName(value, ValueNames[scenario]), value)
        {
            Scenario = scenario;
        }

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues[Scenario];

        private ScenarioType Scenario { get; }
    }
}
