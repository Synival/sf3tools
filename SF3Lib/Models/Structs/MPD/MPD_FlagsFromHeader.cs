using CommonLib.Attributes;
using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public class MPDFlagsFromHeader : IMPD_AllFlags {
        public MPDFlagsFromHeader(MPDHeaderModel header) {
            Header = header;
        }

        public MPDHeaderModel Header { get; }

        private bool IsScenario1          => Header.IsScenario1;
        private bool IsScenario1OrEarlier => Header.IsScenario1OrEarlier;
        private bool IsScenario2OrEarlier => Header.IsScenario2OrEarlier;
        private bool IsScenario2OrLater   => Header.IsScenario2OrLater;
        private bool IsScenario3OrLater   => Header.IsScenario3OrLater;

        private ushort MapFlags {
            get => Header.MapFlags;
            set => Header.MapFlags = value;
        }

        public bool CanSet_0x0001_Unknown => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0001f, displayName: "(0x0001) Unknown", displayGroup: "Flags")]
        public bool Bit_0x0001_Unknown {
            get => (MapFlags & 0x0001) == 0x0001;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0001) : (ushort) (MapFlags & ~0x0001);
        }

        public bool CanSet_0x0002_Unknown => IsScenario2OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0002f, displayName: "(0x0002) Unknown (Scn1,2)", visibilityProperty: nameof(IsScenario2OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x0002_Unknown {
            get => CanSet_0x0002_Unknown && (MapFlags & 0x0002) == 0x0002;
            set {
                if (CanSet_0x0002_Unknown)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }

        public bool CanSet_0x0002_HasSurfaceTextureRotation => IsScenario3OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.00021f, displayName: "(0x0002) HasSurfaceTextureRotation (Scn3+)", visibilityProperty: nameof(IsScenario3OrLater), displayGroup: "Flags")]
        public bool Bit_0x0002_HasSurfaceTextureRotation {
            get => CanSet_0x0002_HasSurfaceTextureRotation && (MapFlags & 0x0002) == 0x0002;
            set {
                if (CanSet_0x0002_HasSurfaceTextureRotation)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }

        public bool CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0004f, displayName: "(0x0004) AddDotProductBasedNoiseToStandardLightmap", displayGroup: "Flags")]
        public bool Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap {
            get => (MapFlags & 0x0004) == 0x0004;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0004) : (ushort) (MapFlags & ~0x0004);
        }

        public bool CanSet_0x0008_KeepTexturelessFlatTiles => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0008f, displayName: "(0x0008) KeepTexturelessFlatTiles", displayGroup: "Flags")]
        public bool Bit_0x0008_KeepTexturelessFlatTiles {
            get => (MapFlags & 0x0008) == 0x0008;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0008) : (ushort) (MapFlags & ~0x0008);
        }

        public bool CanSet_0x0010_HasTileBasedForegroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0010f, displayName: "(0x0010) HasTileBasedForegroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x0010_HasTileBasedForegroundImage {
            get => (MapFlags & 0x0010) == 0x0010;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0010) : (ushort) (MapFlags & ~0x0010);
        }

        public bool CanSet_0x0020_Unknown => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0020f, displayName: "(0x0020) Unknown", displayGroup: "Flags")]
        public bool Bit_0x0020_Unknown {
            get => (MapFlags & 0x0020) == 0x0020;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0020) : (ushort) (MapFlags & ~0x0020);
        }

        public bool CanSet_0x0040_HasBackgroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0040f, displayName: "(0x0040) HasBackgroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x0040_HasBackgroundImage {
            get => (MapFlags & 0x0040) == 0x0040;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0040) : (ushort) (MapFlags & ~0x0040);
        }

        public bool CanSet_0x0080_HasChunk19ModelWithChunk10Textures => IsScenario1;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0080f, displayName: "(0x0080) HasChunk19ModelWithChunk10Textures (Scn1)", visibilityProperty: nameof(IsScenario1), displayGroup: "Flags")]
        public bool Bit_0x0080_HasChunk19ModelWithChunk10Textures {
            get => CanSet_0x0080_HasChunk19ModelWithChunk10Textures && (MapFlags & 0x0080) == 0x0080;
            set {
                if (CanSet_0x0080_HasChunk19ModelWithChunk10Textures)
                    MapFlags = (ushort) (MapFlags & ~0x0080 | (value ? 0x0080 : 0));
            }
        }

        public bool CanSet_0x0080_SetMSBForPalette1 => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0081f, displayName: "(0x0080) SetMSBForPalette1 (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Bit_0x0080_SetMSBForPalette1 {
            get => CanSet_0x0080_SetMSBForPalette1 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (CanSet_0x0080_SetMSBForPalette1)
                    MapFlags = value ? (ushort) (MapFlags | 0x0080) : (ushort) (MapFlags & ~0x0080);
            }
        }

        public bool CanSet_0x0100_HasModels => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0100f, displayName: "(0x0100) HasModels", displayGroup: "Flags")]
        public bool Bit_0x0100_HasModels {
            get => (MapFlags & 0x0100) == 0x0100;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0100) : (ushort) (MapFlags & ~0x0100);
        }

        public bool CanSet_0x0200_HasSurfaceModel => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0200f, displayName: "(0x0200) HasSurfaceModel", displayGroup: "Flags")]
        public bool Bit_0x0200_HasSurfaceModel {
            get => (MapFlags & 0x0200) == 0x0200;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0200) : (ushort) (MapFlags & ~0x0200);
        }

        public bool CanSet_0x0400_HasGroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0400f, displayName: "(0x0400) HasGroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x0400_HasGroundImage {
            get => (MapFlags & 0x0400) == 0x0400;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0400) : (ushort) (MapFlags & ~0x0400);
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

        public bool CanSet_0x1000_HasTileBasedGroundImage => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.1000f, displayName: "(0x1000) HasTileBasedGroundImage", minWidth: 100, displayGroup: "Flags")]
        public bool Bit_0x1000_HasTileBasedGroundImage {
            get => (MapFlags & 0x1000) == 0x1000;
            set => MapFlags = value ? (ushort) (MapFlags | 0x1000) : (ushort) (MapFlags & ~0x1000);
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

        public bool CanSet_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.8000f, displayName: "(0x8000) Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists {
            get => CanSet_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (CanSet_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        public bool CanSet_0x8000_Chunk20IsSurfaceModelIfExists => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.8001f, displayName: "(0x8000) Chunk20IsSurfaceModelIfExists (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Bit_0x8000_Chunk20IsSurfaceModelIfExists {
            get => CanSet_0x8000_Chunk20IsSurfaceModelIfExists ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (CanSet_0x8000_Chunk20IsSurfaceModelIfExists)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8010f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromLowMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromLowMemory
            => Bit_0x0100_HasModels && (IsScenario1OrEarlier ? !Bit_0x0200_HasSurfaceModel || Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists : Chunk1IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8011f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromHighMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromHighMemory
            => Bit_0x0100_HasModels && IsScenario1OrEarlier && Bit_0x0200_HasSurfaceModel && !Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8013f, displayName: "(Derived) " + nameof(Chunk1IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk1IsModels
            => Bit_0x0100_HasModels && (IsScenario1OrEarlier || !Bit_0x0200_HasSurfaceModel || Bit_0x8000_Chunk20IsSurfaceModelIfExists || Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8014f, displayName: "(Derived) " + nameof(Chunk2IsSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk2IsSurfaceModel
            => Bit_0x0200_HasSurfaceModel && (IsScenario1OrEarlier || !Bit_0x8000_Chunk20IsSurfaceModelIfExists);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8015f, displayName: "(Derived) " + nameof(Chunk20IsSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsSurfaceModel
            => IsScenario2OrLater && Bit_0x0200_HasSurfaceModel && Bit_0x8000_Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8016f, displayName: "(Derived) " + nameof(Chunk20IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsModels
            => IsScenario2OrLater && Bit_0x0200_HasSurfaceModel && !Bit_0x8000_Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8017f, displayName: "(Derived) " + nameof(HighMemoryHasModels), displayGroup: "Flags")]
        public bool HighMemoryHasModels => Bit_0x0100_HasModels && (IsScenario1OrEarlier ? Chunk1IsLoadedFromHighMemory : Chunk20IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8018f, displayName: "(Derived) " + nameof(HighMemoryHasSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HighMemoryHasSurfaceModel => Bit_0x0200_HasSurfaceModel && IsScenario2OrLater && Chunk20IsSurfaceModel;

        public bool HasAnySkyBox => Bit_0x0800_HasCutsceneSkyBox || Bit_0x2000_HasBattleSkyBox;
    }
}
