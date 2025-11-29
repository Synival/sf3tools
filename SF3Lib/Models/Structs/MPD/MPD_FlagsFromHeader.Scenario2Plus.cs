using CommonLib.Attributes;

namespace SF3.Models.Structs.MPD {
    public partial class MPD_FlagsFromHeader {
        private bool IsScenario2OrLater   => Header.IsScenario2OrLater;

        public bool CanSet_0x0080_SetMSBForPalette1 => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0081f, displayName: "(0x0080) SetMSBForPalette1 (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Bit_0x0080_SetMSBForPalette1 {
            get => CanSet_0x0080_SetMSBForPalette1 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (CanSet_0x0080_SetMSBForPalette1)
                    MapFlags = value ? (ushort) (MapFlags | 0x0080) : (ushort) (MapFlags & ~0x0080);
            }
        }

        public bool CanSet_0x0800_HasCutsceneSkyBox => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0801f, displayName: "(0x0800) HasCutsceneSkyBox (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Bit_0x0800_HasCutsceneSkyBox {
            get => CanSet_0x0800_HasCutsceneSkyBox ? (MapFlags & 0x0800) == 0x0800 : false;
            set {
                if (CanSet_0x0800_HasCutsceneSkyBox)
                    MapFlags = value ? (ushort) (MapFlags | 0x0800) : (ushort) (MapFlags & ~0x0800);
            }
        }

        public bool CanSet_0x2000_NarrowAngleBasedLightmap => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.2001f, displayName: "(0x2000) Bit_0x2000_NarrowAngleBasedLightmap (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Bit_0x2000_NarrowAngleBasedLightmap {
            get => CanSet_0x2000_NarrowAngleBasedLightmap && (MapFlags & 0x2000) == 0x2000;
            set {
                if (CanSet_0x2000_NarrowAngleBasedLightmap)
                    MapFlags = (ushort) (MapFlags & ~0x2000 | (value ? 0x2000 : 0));
            }
        }

        public bool CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.4001f, displayName: "(0x4000) HasExtraChunk1ModelWithChunk21Textures (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures {
            get => CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures && (MapFlags & 0x4000) == 0x4000;
            set {
                if (CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures)
                    MapFlags = (ushort) (value ? MapFlags | 0x4000 : MapFlags & ~0x4000);
            }
        }
    }
}
