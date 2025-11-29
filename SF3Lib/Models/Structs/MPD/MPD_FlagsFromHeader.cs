using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public partial class MPD_FlagsFromHeader : IMPD_AllFlags {
        public MPD_FlagsFromHeader(MPD_HeaderModel header) {
            Header = header;
        }

        public MPD_HeaderModel Header { get; }

        private ushort MapFlags {
            get => Header.MapFlags;
            set => Header.MapFlags = value;
        }
    }
}
