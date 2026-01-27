using CommonLib.Attributes;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Main {
    public partial class MPD_FlagsFromHeader {
        [TableViewModelColumn(addressField: null, displayOrder: 1.0000f, displayName: "(Derived) " + nameof(ModelsChunkIndex), displayGroup: "Flags")]
        public int ModelsChunkIndex => MPD_ChunkLogic.GetModelsChunkIndex(this, Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0001f, displayName: "(Derived) " + nameof(SurfaceModelChunkIndex), displayGroup: "Flags")]
        public int SurfaceModelChunkIndex {
            get {
                if (IsScenario1OrEarlier)
                    return 2;
                else if (Bit_0x0002_HasSurfaceTextureRotation)
                    return 2;
                else if (ModelsChunkIndex == 20)
                    return 2;
                else
                    return 20;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0002f, displayName: "(Derived) " + nameof(HasAnySky), displayGroup: "Flags")]
        public bool HasAnySky => Bit_0x0800_HasCutsceneSky || Bit_0x2000_HasBattleSky;

        [TableViewModelColumn(addressField: null, displayOrder: 1.0003f, displayName: "(Derived) " + nameof(Chunk1Type), displayGroup: "Flags")]
        public ChunkType Chunk1Type {
            get {
                return Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures || (Bit_0x0100_HasModels && ModelsChunkIndex == 1)
                    ? ChunkType.Models : ChunkType.Unset;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0004f, displayName: "(Derived) " + nameof(Chunk1PointersMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType? Chunk1PointersMemoryLocation {
            get {
                if (Chunk1Type != ChunkType.Models)
                    return null;
                return (IsScenario2OrLater || !Bit_0x0200_HasSurfaceModel || Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                    ? MemoryLocationType.LowMemory
                    : MemoryLocationType.HighMemory;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0005f, displayName: "(Derived) " + nameof(Chunk2Type), displayGroup: "Flags")]
        public ChunkType Chunk2Type {
            get {
                return Bit_0x0200_HasSurfaceModel && SurfaceModelChunkIndex == 2
                    ? ChunkType.SurfaceModel
                    : ChunkType.Unset;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0006f, displayName: "(Derived) " + nameof(Chunk20Type) + " (Scn2+)", displayGroup: "Flags")]
        public ChunkType Chunk20Type {
            get {
                if (!IsScenario2OrLater)
                    return ChunkType.Unset;
                else if (Bit_0x0100_HasModels && ModelsChunkIndex == 20)
                    return ChunkType.Models;
                else if (Bit_0x0200_HasSurfaceModel && SurfaceModelChunkIndex == 20)
                    return ChunkType.SurfaceModel;
                else
                    return ChunkType.Unset;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0007f, displayName: "(Derived) " + nameof(ModelsMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType ModelsMemoryLocation => (ModelsChunkIndex == 1) ? Chunk1PointersMemoryLocation.Value : MemoryLocationType.HighMemory;

        [TableViewModelColumn(addressField: null, displayOrder: 1.0008f, displayName: "(Derived) " + nameof(SurfaceModelMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType SurfaceModelMemoryLocation
            => (IsScenario2OrLater && Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel && !Bit_0x0002_HasSurfaceTextureRotation) ? MemoryLocationType.HighMemory : MemoryLocationType.LowMemory;
}
}
