using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    public class SpriteCharacterMonsterInfo : INamedValueFromResourceForScenariosInfo {
        public SpriteCharacterMonsterInfo() {
            Info = ValueNames.SpriteInfo.Info.ToDictionary(x => x.Key, x => {
                var values = new Dictionary<int, string>(x.Value.Values);

                foreach (var kv in ValueNames.CharacterInfo.Info[x.Key].Values)
                    if (!values.ContainsKey(kv.Key) || values[kv.Key] == "")
                        values[kv.Key] = kv.Value;
                foreach (var kv in ValueNames.MonsterInfo.Info[x.Key].Values)
                    if (!values.ContainsKey(kv.Key + 0xC8) || values[kv.Key + 0xC8] == "")
                        values[kv.Key + 0xC8] = kv.Value;

                return (INamedValueInfo) new NamedValueInfo(MinValue, MaxValue, x.Value.FormatString, values);
            });
        }

        public string ResourceName => ValueNames.SpriteInfo.ResourceName + " + " + ValueNames.CharacterInfo.ResourceName;
        public int MinValue => 0x000;
        public int MaxValue => 0x300;
        public string FormatString => Info[ScenarioType.Scenario1].FormatString;
        public Dictionary<ScenarioType, INamedValueInfo> Info { get; }
    }
}
