using SF3.MPD.Interfaces.Flags;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Main {
    public partial class MPD_FlagsFromHeader : IMPD_EditableFlags {
        public MPD_FlagsFromHeader(MPD_Header header) {
            Header = header;
        }

        public MPD_Header Header { get; }

        private ushort MapFlags {
            get => Header.MapFlags;
            set => Header.MapFlags = value;
        }

        private ScenarioType Scenario => Header.Scenario;
    }
}
