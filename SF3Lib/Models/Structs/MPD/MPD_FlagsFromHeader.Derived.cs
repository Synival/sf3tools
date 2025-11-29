using CommonLib.Attributes;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public partial class MPD_FlagsFromHeader {
        [TableViewModelColumn(addressField: null, displayOrder: 1.0000f, displayName: "(Derived) " + nameof(ModelsChunkIndex), displayGroup: "Flags")]
        public int? ModelsChunkIndex {
            get {
                if (!Bit_0x0100_HasModels)
                    return null;

                return
                    (Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures ||
                     (IsScenario2OrLater && Bit_0x0200_HasSurfaceModel && !Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel))
                    ? 20 : 1;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0001f, displayName: "(Derived) " + nameof(SurfaceModelChunkIndex), displayGroup: "Flags")]
        public int? SurfaceModelChunkIndex {
            get {
                if (!Bit_0x0200_HasSurfaceModel)
                    return null;

                return (IsScenario2OrLater && (!Bit_0x0100_HasModels || !Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures) && Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel && !Bit_0x0002_HasSurfaceTextureRotation)
                    ? 20 : 2;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0002f, displayName: "(Derived) " + nameof(HasAnySkyBox), displayGroup: "Flags")]
        public bool HasAnySkyBox => Bit_0x0800_HasCutsceneSkyBox || Bit_0x2000_HasBattleSkyBox;

        [TableViewModelColumn(addressField: null, displayOrder: 1.0003f, displayName: "(Derived) " + nameof(Chunk1Type), displayGroup: "Flags")]
        public ChunkType Chunk1Type {
            get {
                if (Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures)
                    return ChunkType.Models;
                return Bit_0x0100_HasModels && (IsScenario1OrEarlier || !Bit_0x0200_HasSurfaceModel || Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                    ? ChunkType.Models
                    : ChunkType.Unset;
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
                return Bit_0x0200_HasSurfaceModel && (!Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel || Bit_0x0002_HasSurfaceTextureRotation || IsScenario1OrEarlier)
                    ? ChunkType.SurfaceModel
                    : ChunkType.Unset;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0006f, displayName: "(Derived) " + nameof(Chunk20Type) + " (Scn2+)", displayGroup: "Flags")]
        public ChunkType Chunk20Type {
            get {
                if (!IsScenario2OrLater)
                    return ChunkType.Unset;

                if (!Bit_0x0200_HasSurfaceModel)
                    return (Bit_0x0100_HasModels && Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures) ? ChunkType.Models : ChunkType.Unset;

                if (Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                    return Bit_0x0002_HasSurfaceTextureRotation ? ChunkType.Unset : ChunkType.SurfaceModel;

                return Bit_0x0100_HasModels ? ChunkType.Models : ChunkType.Unset;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0007f, displayName: "(Derived) " + nameof(ModelsMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType? ModelsMemoryLocation {
            get {
                var mci = ModelsChunkIndex;
                return (mci.HasValue)
                    ? ((mci.Value == 1) ? Chunk1PointersMemoryLocation : MemoryLocationType.HighMemory)
                    : null;
            }
        }

        [TableViewModelColumn(addressField: null, displayOrder: 1.0008f, displayName: "(Derived) " + nameof(SurfaceModelMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType? SurfaceModelMemoryLocation {
            get {
                return Bit_0x0200_HasSurfaceModel
                    ? ((IsScenario2OrLater && Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel && !Bit_0x0002_HasSurfaceTextureRotation) ? MemoryLocationType.HighMemory : MemoryLocationType.LowMemory)
                    : (MemoryLocationType?) null;
            }
        }
    }
}
