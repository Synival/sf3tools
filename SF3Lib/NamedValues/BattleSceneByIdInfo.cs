using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static SF3.Utils.ResourceUtils;

namespace SF3.NamedValues {
    public class BattleSceneByIdInfo : INamedValueInfo {
        public BattleSceneByIdInfo() {
            // Merge values from two XML files.
            var values      = GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.Scenario1, "BattleScenesByBattle.xml"));
            var values0x100 = GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.Scenario1, "OtherBattleScenes.xml"));
            foreach (var value in values0x100)
                values[value.Key + 0x100] = value.Value;

            MergedInfo = new NamedValueInfo(0, -1, "X3", values);
        }

        private INamedValueInfo MergedInfo { get; }

        public int MinValue => MergedInfo.MinValue;
        public int MaxValue => MergedInfo.MaxValue;
        public string FormatString => MergedInfo.FormatString;
        public Dictionary<int, string> Values => MergedInfo.Values;
        public Dictionary<int, string> ComboBoxValues => MergedInfo.ComboBoxValues;
    }
}
