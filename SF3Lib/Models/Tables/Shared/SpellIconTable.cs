using System;
using SF3.ByteData;
using SF3.Models.Structs.Shared;
using SF3.Types;

namespace SF3.Models.Tables.Shared {
    public class SpellIconTable : ResourceTable<SpellIcon> {
        protected SpellIconTable(IByteData data, string name, string resourceFile, int address, bool has16BitIconAddr, ScenarioType scenario)
        : base(data, name, resourceFile, address, 256) {
            Has16BitIconAddr = has16BitIconAddr;
            Scenario         = scenario;
            RealOffsetStart  = GetIconRealOffset(scenario);
        }

        public static int GetIconRealOffset(ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Scenario1:   return 0xFF8E;
                case ScenarioType.Scenario2:   return 0xFC86;
                case ScenarioType.Scenario3:   return 0x12A48;
                case ScenarioType.PremiumDisk: return 0x12A32;
                default:
                    throw new ArgumentException(nameof(scenario));
            }
        }

        public static SpellIconTable Create(IByteData data, string name, string resourceFile, int address, bool has16BitIconAddr, ScenarioType scenario)
            => Create(() => new SpellIconTable(data, name, resourceFile, address, has16BitIconAddr, scenario));

        public override bool Load()
            => Load((id, name, address) => new SpellIcon(Data, id, name, address, Has16BitIconAddr, RealOffsetStart));

        public bool Has16BitIconAddr { get; }
        public ScenarioType Scenario { get; }
        public int RealOffsetStart { get; }
    }
}
