using CommonLib.Attributes;

namespace SF3.Models.Structs.MPD {
    public partial class MPDFlagsFromHeader {
        private bool IsScenario3OrLater   => Header.IsScenario3OrLater;

        public bool CanSet_0x0002_HasSurfaceTextureRotation => IsScenario3OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.00021f, displayName: "(0x0002) HasSurfaceTextureRotation (Scn3+)", visibilityProperty: nameof(IsScenario3OrLater), displayGroup: "Flags")]
        public bool Bit_0x0002_HasSurfaceTextureRotation {
            get => CanSet_0x0002_HasSurfaceTextureRotation && (MapFlags & 0x0002) == 0x0002;
            set {
                if (CanSet_0x0002_HasSurfaceTextureRotation)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }
    }
}
