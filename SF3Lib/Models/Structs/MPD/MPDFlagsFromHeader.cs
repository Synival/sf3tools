using CommonLib.Attributes;
using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public partial class MPDFlagsFromHeader : IMPD_AllFlags {
        public MPDFlagsFromHeader(MPDHeaderModel header) {
            Header = header;
        }

        public MPDHeaderModel Header { get; }

        private ushort MapFlags {
            get => Header.MapFlags;
            set => Header.MapFlags = value;
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
