﻿using SF3.Types;
using System.Collections.Generic;
using static SF3.Utils.Utils;
using static SF3.Utils.Resources;

namespace SF3.Values
{
    /// <summary>
    /// Named value for Item ID's that can be bound to an ObjectListView.
    /// </summary>
    public class ItemValue : NamedValue
    {
        public const int MinValue = 0;
        public const int MaxValue = 0xFF;

        public static readonly Dictionary<ScenarioType, Dictionary<int, string>> ValueNames = GetValueNameDictionaryForAllScenariosFromXML("Items.xml");

        public static readonly Dictionary<ScenarioType, Dictionary<NamedValue, string>> _comboBoxValues =
            MakeNamedValueComboBoxValuesForAllScenarios(MinValue, MaxValue, (s, v) => new ItemValue(s, v));

        public ItemValue(ScenarioType scenario, int value) : base(NameOrHexValue(value, ValueNames[scenario]), HexValueWithName(value, ValueNames[scenario]), value)
        {
            Scenario = scenario;
        }

        public override Dictionary<NamedValue, string> ComboBoxValues => _comboBoxValues[Scenario];

        private ScenarioType Scenario { get; }
    }
}
