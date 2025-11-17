using CommonLib.Attributes;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPDFlags {
        public MPDFlags(MPDHeaderModel header) {
            Header = header;
        }

        public MPDHeaderModel Header { get; }

        public ScenarioType Scenario => Header.Scenario;

        public bool IsScenario1             => Header.IsScenario1;
        public bool IsScenario1OrEarlier    => Header.IsScenario1OrEarlier;
        public bool IsScenario1OrLater      => Header.IsScenario1OrLater;
        public bool IsScenario2OrEarlier    => Header.IsScenario2OrEarlier;
        public bool IsScenario2OrLater      => Header.IsScenario2OrLater;
        public bool IsScenario3OrLater      => Header.IsScenario3OrLater;

        public bool HasUnknown1Table        => Header.HasUnknown1Table;
        public bool HasLightAdjustmentTable => Header.HasLightAdjustmentTable;
        public bool HasUnknown2Table        => Header.HasUnknown2Table;
        public bool HasGradientTable        => Header.HasGradientTable;
        public bool HasMesh3                => Header.HasMesh3;
        public bool HasModelsInfo           => Header.HasModelsInfo;
        public bool HasPalette3             => Header.HasPalette3;
        public bool HasIndexedTextures      => Header.HasIndexedTextures;

        private ushort MapFlags {
            get => Header.MapFlags;
            set => Header.MapFlags = value;
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0001f, displayName: "(0x0001) " + nameof(UnknownMapFlag0x0001), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0001 {
            get => (MapFlags & 0x0001) == 0x0001;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0001) : (ushort) (MapFlags & ~0x0001);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0002f, displayName: "(0x0002) " + nameof(UnknownMapFlag0x0002) + " (Scn1,2)", visibilityProperty: nameof(IsScenario2OrEarlier), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0002 {
            get => Scenario < ScenarioType.Scenario3 && (MapFlags & 0x0002) == 0x0002;
            set {
                if (Scenario < ScenarioType.Scenario3)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.00021f, displayName: "(0x0002) " + nameof(HasSurfaceTextureRotation) + " (Scn3+)", visibilityProperty: nameof(IsScenario3OrLater), displayGroup: "Flags")]
        public bool HasSurfaceTextureRotation {
            get => Scenario >= ScenarioType.Scenario3 && (MapFlags & 0x0002) == 0x0002;
            set {
                if (Scenario >= ScenarioType.Scenario3)
                    MapFlags = value ? (ushort) (MapFlags | 0x0002) : (ushort) (MapFlags & ~0x0002);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0004f, displayName: "(0x0004) " + nameof(AddDotProductBasedNoiseToStandardLightmap), displayGroup: "Flags")]
        public bool AddDotProductBasedNoiseToStandardLightmap {
            get => (MapFlags & 0x0004) == 0x0004;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0004) : (ushort) (MapFlags & ~0x0004);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0008f, displayName: "(0x0008) " + nameof(UnknownFlatTileFlag0x0008), displayGroup: "Flags")]
        public bool UnknownFlatTileFlag0x0008 {
            get => (MapFlags & 0x0008) == 0x0008;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0008) : (ushort) (MapFlags & ~0x0008);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0010f, displayName: "(0x0010 | 40) " + nameof(BackgroundImageType), minWidth: 100, displayGroup: "Flags")]
        public BackgroundImageType BackgroundImageType {
            get => BackgroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) ((MapFlags & ~BackgroundImageTypeExtensions.ApplicableMapFlags) | value.ToMapFlags());
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0020f, displayName: "(0x0020) " + nameof(UnknownMapFlag0x0020), displayGroup: "Flags")]
        public bool UnknownMapFlag0x0020 {
            get => (MapFlags & 0x0020) == 0x0020;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0020) : (ushort) (MapFlags & ~0x0020);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0080f, displayName: "(0x0080) " + nameof(HasChunk19Model) + " (Scn1)", visibilityProperty: nameof(IsScenario1), displayGroup: "Flags")]
        public bool HasChunk19Model {
            get => IsScenario1 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (IsScenario1)
                    MapFlags = (ushort) ((MapFlags & ~0x0080) | (value ? 0x0080 : 0));
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0081f, displayName: "(0x0080) " + nameof(ModifyPalette1ForGradient) + " (Redundant) (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool ModifyPalette1ForGradient {
            get => Scenario >= ScenarioType.Scenario2 && (MapFlags & 0x0080) == 0x0080;
            set {
                if (Scenario >= ScenarioType.Scenario2)
                    MapFlags = value ? (ushort) (MapFlags | 0x0080) : (ushort) (MapFlags & ~0x0080);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0100f, displayName: "(0x0100) " + nameof(HasModels), displayGroup: "Flags")]
        public bool HasModels {
            get => (MapFlags & 0x0100) == 0x0100;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0100) : (ushort) (MapFlags & ~0x0100);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0200f, displayName: "(0x0200) " + nameof(HasSurfaceModel), displayGroup: "Flags")]
        public bool HasSurfaceModel {
            get => (MapFlags & 0x0200) == 0x0200;
            set => MapFlags = value ? (ushort) (MapFlags | 0x0200) : (ushort) (MapFlags & ~0x0200);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0400f, displayName: "(0x0400 | 1000): Ground Image Type", minWidth: 100, displayGroup: "Flags")]
        public GroundImageType GroundImageType {
            get => GroundImageTypeExtensions.FromMapFlags(MapFlags);
            set => MapFlags = (ushort) ((MapFlags & ~GroundImageTypeExtensions.ApplicableMapFlags) | value.ToMapFlags());
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.0801f, displayName: "(0x0800) " + nameof(HasCutsceneSkyBox) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HasCutsceneSkyBox {
            get => IsScenario2OrLater ? (MapFlags & 0x0800) == 0x0800 : false;
            set {
                if (IsScenario2OrLater)
                    MapFlags = value ? (ushort) (MapFlags | 0x0800) : (ushort) (MapFlags & ~0x0800);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.2000f, displayName: "(0x2000) " + nameof(HasBattleSkyBox) + " (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool HasBattleSkyBox {
            get => IsScenario1OrEarlier ? (MapFlags & 0x2000) == 0x2000 : false;
            set {
                if (IsScenario1OrEarlier)
                    MapFlags = value ? (ushort) (MapFlags | 0x2000) : (ushort) (MapFlags & ~0x2000);
            }
        }
 
        [TableViewModelColumn(addressField: null, displayOrder: 0.2001f, displayName: "(0x2000) " + nameof(NarrowAngleBasedLightmap) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool NarrowAngleBasedLightmap {
            get => IsScenario2OrLater && (MapFlags & 0x2000) == 0x2000;
            set {
                if (IsScenario2OrLater)
                    MapFlags = (ushort) ((MapFlags & ~0x2000) | (value ? 0x2000 : 0));
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.4001f, displayName: "(0x4000) " + nameof(HasExtraChunk1ModelWithChunk21Textures) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool HasExtraChunk1ModelWithChunk21Textures {
            get => IsScenario2OrLater && (MapFlags & 0x4000) == 0x4000;
            set {
                if (IsScenario2OrLater)
                    MapFlags = (ushort) (value ? (MapFlags | 0x4000) : (MapFlags & ~0x4000));
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8000f, displayName: "(0x8000) " + nameof(Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists) + " (Scn1)", visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists {
            get => IsScenario1OrEarlier ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (IsScenario1OrEarlier)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8010f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromLowMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromLowMemory
            => HasModels && (IsScenario1OrEarlier ? (!HasSurfaceModel || Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists) : Chunk1IsModels);

        [TableViewModelColumn(addressField: null, displayOrder: 0.8011f, displayName: "(Derived) " + nameof(Chunk1IsLoadedFromHighMemory), visibilityProperty: nameof(IsScenario1OrEarlier), displayGroup: "Flags")]
        public bool Chunk1IsLoadedFromHighMemory
            => HasModels && IsScenario1OrEarlier && HasSurfaceModel && !Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists;

        [TableViewModelColumn(addressField: null, displayOrder: 0.8012f, displayName: "(0x8000) " + nameof(Chunk20IsSurfaceModelIfExists) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk20IsSurfaceModelIfExists {
            get => IsScenario2OrLater ? (MapFlags & 0x8000) == 0x8000 : false;
            set {
                if (IsScenario2OrLater)
                    MapFlags = value ? (ushort) (MapFlags | 0x8000) : (ushort) (MapFlags & ~0x8000);
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 0.8013f, displayName: "(Derived) " + nameof(Chunk1IsModels) + " (Scn2+)", visibilityProperty: nameof(IsScenario2OrLater), displayGroup: "Flags")]
        public bool Chunk1IsModels
            => HasModels && (IsScenario1OrEarlier || (!HasSurfaceModel || Chunk20IsSurfaceModelIfExists || HasExtraChunk1ModelWithChunk21Textures));

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
