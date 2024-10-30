using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    public class MonsterForSlotInfo : INamedValueFromResourceForScenariosInfo {
        public MonsterForSlotInfo() {
            Dictionary<int, string> dictionaryWithSpecialSlots(Dictionary<int, string> values)
                => new Dictionary<int, string>(values) {
                    { 0xFFFD, "Unkown BTL328 Slot" },
                    { 0xFFFF, "Character Slot" }
                };

            Info = ValueNames.MonsterInfo.Info
                .ToDictionary(
                    x => x.Key,
                    x => (INamedValueInfo) new NamedValueInfo(x.Value.MinValue, x.Value.MaxValue, x.Value.FormatString, dictionaryWithSpecialSlots(x.Value.Values))
                );
        }

        public string ResourceName => ValueNames.MonsterInfo.ResourceName;
        public int MinValue => ValueNames.MonsterInfo.MinValue;
        public int MaxValue => ValueNames.MonsterInfo.MaxValue;
        public string FormatString => ValueNames.MonsterInfo.FormatString;
        public Dictionary<ScenarioType, INamedValueInfo> Info { get; }
    }
}
