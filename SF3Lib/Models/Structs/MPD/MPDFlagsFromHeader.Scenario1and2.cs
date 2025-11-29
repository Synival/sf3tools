using CommonLib.Attributes;

namespace SF3.Models.Structs.MPD {
    public partial class MPDFlagsFromHeader {
        private bool IsScenario2OrEarlier => Header.IsScenario2OrEarlier;

        public bool CanSet_0x0002_Unknown => IsScenario2OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0002f, displayName: "(0x0002) Unknown (Scn1,2)", visibilityProperty: nameof(IsScenario2OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x0002_Unknown {
            get => CanSet_0x0002_Unknown && (MapFlags & 0x0002) == 0x0002;
            set {
                if (CanSet_0x0002_Unknown)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }
    }
}
