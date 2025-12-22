using System;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public partial class MPD_FlagsFromHeader : IMPD_AllFlags {
        public MPD_FlagsFromHeader(MPD_Header header) {
            Header = header;
        }

        public MPD_Header Header { get; }

        private ushort MapFlags {
            get => Header.MapFlags;
            set => Header.MapFlags = value;
        }

        public ushort GetHeaderFlags(ScenarioType scenario) {
            switch (scenario) {
                case ScenarioType.Ship2:
                case ScenarioType.Other:
                case ScenarioType.Scenario1:
                    return GetScenario1HeaderFlags();

                case ScenarioType.Scenario2:
                    return GetScenario2HeaderFlags();

                case ScenarioType.Scenario3:
                    return GetScenario3HeaderFlags();

                case ScenarioType.PremiumDisk:
                    return GetPremiumDiskHeaderFlags();

                default:
                    throw new ArgumentException($"Unhandled scenario '{scenario}'");
            }
        }

        // TODO: Implement properly!!
        public ushort GetScenario1HeaderFlags() => Header.MapFlags;

        // TODO: Implement properly!!
        public ushort GetScenario2HeaderFlags() => Header.MapFlags;

        // TODO: Implement properly!!
        public ushort GetScenario3HeaderFlags() => Header.MapFlags;

        // TODO: Implement properly!!
        public ushort GetPremiumDiskHeaderFlags() => Header.MapFlags;
    }
}
