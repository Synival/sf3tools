using CommonLib.Attributes;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPDFlagsFromHeader : IMPD_Flags {
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

        public bool CanSetUnknownMapFlag0x0001 => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0001f, displayName: "(0x0001) " + nameof(UnknownMapFlag0x0001), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0001 {
            get => (MapFlags & 0x0001) == 0x0001;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0001) : (ushort) (MapFlags & ~0x0001);
        }

        public bool CanSetUnknownMapFlag0x0002 => IsScenario2OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0002f, displayName: "(0x0002) " + nameof(UnknownMapFlag0x0002) + " (Scn1,2)", visibilityProperty: nameof(IsScenario2OrEarlier), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0002 {
            get => CanSetUnknownMapFlag0x0002 && (MapFlags & 0x0002) == 0x0002;
            set {
                if (CanSetUnknownMapFlag0x0002)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }

        public bool CanSetHasSurfaceTextureRotation => IsScenario3OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.00021f, displayName: "(0x0002) " + nameof(HasSurfaceTextureRotation) + " (Scn3+)", visibilityProperty: nameof(IsScenario3OrLater), displayGroup: "Flags")]
        public bool HasSurfaceTextureRotation {
            get => CanSetHasSurfaceTextureRotation && (MapFlags & 0x0002) == 0x0002;
            set {
                if (CanSetHasSurfaceTextureRotation)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }

        public bool CanSetAddDotProductBasedNoiseToStandardLightmap => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0004f, displayName: "(0x0004) " + nameof(AddDotProductBasedNoiseToStandardLightmap), displayGroup: "Flags")]
        public bool AddDotProductBasedNoiseToStandardLightmap {
            get => (MapFlags & 0x0004) == 0x0004;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0004) : (ushort) (MapFlags & ~0x0004);
        }

        public bool CanSetUnknownFlatTileFlag0x0008 => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0008f, displayName: "(0x0008) " + nameof(UnknownFlatTileFlag0x0008), displayGroup: "Flags")]
        public bool UnknownFlatTileFlag0x0008 {
            get => (MapFlags & 0x0008) == 0x0008;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0008) : (ushort) (MapFlags & ~0x0008);
        }

        public bool CanSetBackgroundImageType => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0010f, displayName: "(0x0010 | 40) " + nameof(BackgroundImageType), minWidth: 100, displayGroup: "Flags")]
        public BackgroundImageType BackgroundImageType {
            get => BackgroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) (MapFlags & ~BackgroundImageTypeExtensions.ApplicableMapFlags | value.ToMapFlags());
        }

        public bool CanSetUnknownMapFlag0x0020 => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0020f, displayName: "(0x0020) " + nameof(UnknownMapFlag0x0020), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0020 {
            get => (MapFlags & 0x0020) == 0x0020;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0020) : (ushort) (MapFlags & ~0x0020);
        }

        public bool CanSetHasChunk19Model => IsScenario1;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0080f, displayName: "(0x0080) " + nameof(HasChunk19Model) + " (Scn1)", visibilityProperty: nameof(IsScenario1), displayGroup: "Flags")]
        public bool HasChunk19Model {
            get => CanSetHasChunk19Model && (MapFlags & 0x0080) == 0x0080;
            set {
                if (CanSetHasChunk19Model)
                    MapFlags = (ushort) (MapFlags & ~0x0080 | (value ? 0x0080 : 0));
            }
        }

        public bool CanSetModifyPalette1ForGradient => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0081f, displayName: "(0x0080) " + nameof(ModifyPalette1ForGradient) + " (Redundant) (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool ModifyPalette1ForGradient {
            get => CanSetModifyPalette1ForGradient && (MapFlags & 0x0080) == 0x0080;
            set {
                if (CanSetModifyPalette1ForGradient)
                    MapFlags = value ? (ushort) (MapFlags | 0x0080) : (ushort) (MapFlags & ~0x0080);
            }
        }

        public bool CanSetHasModels => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0100f, displayName: "(0x0100) " + nameof(HasModels), displayGroup: "Flags")]
        public bool HasModels {
            get => (MapFlags & 0x0100) == 0x0100;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0100) : (ushort) (MapFlags & ~0x0100);
        }

        public bool CanSetHasSurfaceModel => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0200f, displayName: "(0x0200) " + nameof(HasSurfaceModel), displayGroup: "Flags")]
        public bool HasSurfaceModel {
            get => (MapFlags & 0x0200) == 0x0200;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0200) : (ushort) (MapFlags & ~0x0200);
        }

        public bool CanSetGroundImageType => true;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0400f, displayName: "(0x0400 | 1000): Ground Image Type", minWidth: 100, displayGroup: "Flags")]
        public GroundImageType GroundImageType {
            get => GroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) (MapFlags & ~GroundImageTypeExtensions.ApplicableMapFlags | value.ToMapFlags());
        }

        public bool CanSetHasCutsceneSkyBox => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.0801f, displayName: "(0x0800) " + nameof(HasCutsceneSkyBox) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HasCutsceneSkyBox {
            get => CanSetHasCutsceneSkyBox ? (MapFlags & 0x0800) == 0x0800 : false;
            set {
                if (CanSetHasCutsceneSkyBox)
                    MapFlags = value ? (ushort) (MapFlags | 0x0800) : (ushort) (MapFlags & ~0x0800);
            }
        }

        public bool CanSetHasBattleSkyBox => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.2000f, displayName: "(0x2000) " + nameof(HasBattleSkyBox) + " (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool HasBattleSkyBox {
            get => CanSetHasBattleSkyBox ? (MapFlags & 0x2000) == 0x2000 : false;
            set {
                if (CanSetHasBattleSkyBox)
                    MapFlags = value ? (ushort) (MapFlags | 0x2000) : (ushort) (MapFlags & ~0x2000);
            }
        }
 
        public bool CanSetNarrowAngleBasedLightmap => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.2001f, displayName: "(0x2000) " + nameof(NarrowAngleBasedLightmap) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool NarrowAngleBasedLightmap {
            get => CanSetNarrowAngleBasedLightmap && (MapFlags & 0x2000) == 0x2000;
            set {
                if (CanSetNarrowAngleBasedLightmap)
                    MapFlags = (ushort) (MapFlags & ~0x2000 | (value ? 0x2000 : 0));
            }
        }

        public bool CanSetHasExtraChunk1ModelWithChunk21Textures => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.4001f, displayName: "(0x4000) " + nameof(HasExtraChunk1ModelWithChunk21Textures) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HasExtraChunk1ModelWithChunk21Textures {
            get => CanSetHasExtraChunk1ModelWithChunk21Textures && (MapFlags & 0x4000) == 0x4000;
            set {
                if (CanSetHasExtraChunk1ModelWithChunk21Textures)
                    MapFlags = (ushort) (value ? MapFlags | 0x4000 : MapFlags & ~0x4000);
            }
        }

        public bool CanSetChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists => IsScenario1OrEarlier;
        [TableViewModelColumn(addressField: null, displayOrder: 0.8000f, displayName: "(0x8000) " + nameof(Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists) + " (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists {
            get => CanSetChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (CanSetChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        public bool CanSetChunk20IsSurfaceModelIfExists => IsScenario2OrLater;
        [TableViewModelColumn(addressField: null, displayOrder: 0.8001f, displayName: "(0x8000) " + nameof(Chunk20IsSurfaceModelIfExists) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsSurfaceModelIfExists {
            get => CanSetChunk20IsSurfaceModelIfExists ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (CanSetChunk20IsSurfaceModelIfExists)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8010f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromLowMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromLowMemory
            => HasModels && (IsScenario1OrEarlier ? !HasSurfaceModel || Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists : Chunk1IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8011f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromHighMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromHighMemory
            => HasModels && IsScenario1OrEarlier && HasSurfaceModel && !Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8013f, displayName: "(Derived) " + nameof(Chunk1IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk1IsModels
            => HasModels && (IsScenario1OrEarlier || !HasSurfaceModel || Chunk20IsSurfaceModelIfExists || HasExtraChunk1ModelWithChunk21Textures);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8014f, displayName: "(Derived) " + nameof(Chunk2IsSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk2IsSurfaceModel
            => HasSurfaceModel && (IsScenario1OrEarlier || !Chunk20IsSurfaceModelIfExists);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8015f, displayName: "(Derived) " + nameof(Chunk20IsSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsSurfaceModel
            => IsScenario2OrLater && HasSurfaceModel && Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8016f, displayName: "(Derived) " + nameof(Chunk20IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsModels
            => IsScenario2OrLater && HasSurfaceModel && !Chunk20IsSurfaceModelIfExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8017f, displayName: "(Derived) " + nameof(HighMemoryHasModels), displayGroup: "Flags")]
        public bool HighMemoryHasModels => HasModels && (IsScenario1OrEarlier ? Chunk1IsLoadedFromHighMemory : Chunk20IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8018f, displayName: "(Derived) " + nameof(HighMemoryHasSurfaceModel) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HighMemoryHasSurfaceModel => HasSurfaceModel && IsScenario2OrLater && Chunk20IsSurfaceModel;

        public bool HasAnySkyBox => HasCutsceneSkyBox || HasBattleSkyBox;
    }
}
