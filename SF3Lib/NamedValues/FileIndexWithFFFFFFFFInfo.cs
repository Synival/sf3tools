using System.Collections.Generic;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Types;

namespace SF3.NamedValues {
    public class FileIndexWithFFFFFFFFInfo : INamedValueFromResourceForScenariosInfo {
        public FileIndexWithFFFFFFFFInfo() {
            Dictionary<int, string> dictionaryWithSpecialSlots(Dictionary<int, string> values)
                => new Dictionary<int, string>(values.Where(x => x.Key != 0).ToDictionary(x => x.Key, x => x.Value)) {
                    { -1, "--" },
                    { 0, "Unused" },
                };

            Info = ValueNames.FileIndexInfo.Info
                .ToDictionary(
                    x => x.Key,
                    x => (INamedValueInfo) new NamedValueInfo(x.Value.MinValue, x.Value.MaxValue, x.Value.FormatString, dictionaryWithSpecialSlots(x.Value.Values))
                );
        }

        public string ResourceName => ValueNames.FileIndexInfo.ResourceName;
        public int MinValue => ValueNames.FileIndexInfo.MinValue;
        public int MaxValue => ValueNames.FileIndexInfo.MaxValue;
        public string FormatString => ValueNames.FileIndexInfo.FormatString;
        public Dictionary<ScenarioType, INamedValueInfo> Info { get; }
    }
}
