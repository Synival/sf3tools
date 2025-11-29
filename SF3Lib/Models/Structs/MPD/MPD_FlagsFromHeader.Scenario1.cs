using CommonLib.Attributes;

namespace SF3.Models.Structs.MPD {
    public partial class MPD_FlagsFromHeader {
        private bool IsScenario1OrEarlier => Header.IsScenario1OrEarlier;

        public bool CanSet_0x0080_HasChunk19ModelWithChunk10Textures => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0080f, displayName: "(0x0080) HasChunk19ModelWithChunk10Textures (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x0080_HasChunk19ModelWithChunk10Textures {
            get => CanSet_0x0080_HasChunk19ModelWithChunk10Textures && (MapFlags & 0x0080) == 0x0080;
            set {
                if (CanSet_0x0080_HasChunk19ModelWithChunk10Textures)
                    MapFlags = (ushort) (MapFlags & ~0x0080 | (value ? 0x0080 : 0));
            }
        }

        public bool CanSet_0x0800_Unused => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0800f, displayName: "(0x0800) Unused (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x0800_Unused {
            get => CanSet_0x0800_Unused ? (MapFlags & 0x0800) == 0x0800 : false;
            set {
                if (CanSet_0x0800_Unused)
                    MapFlags = value ? (ushort) (MapFlags | 0x0800) : (ushort) (MapFlags & ~0x0800);
            }
        }

        public bool CanSet_0x2000_HasBattleSkyBox => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.2000f, displayName: "(0x2000) HasBattleSkyBox (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x2000_HasBattleSkyBox {
            get => CanSet_0x2000_HasBattleSkyBox ? (MapFlags & 0x2000) == 0x2000 : false;
            set {
                if (CanSet_0x2000_HasBattleSkyBox)
                    MapFlags = value ? (ushort) (MapFlags | 0x2000) : (ushort) (MapFlags & ~0x2000);
            }
        }

        public bool CanSet_0x4000_Unused => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.4000f, displayName: "(0x4000) Unused (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x4000_Unused {
            get => CanSet_0x4000_Unused ? (MapFlags & 0x4000) == 0x4000 : false;
            set {
                if (CanSet_0x4000_Unused)
                    MapFlags = value ? (ushort) (MapFlags | 0x4000) : (ushort) (MapFlags & ~0x0400);
            }
        }
    }
}
