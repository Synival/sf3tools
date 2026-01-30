using CommonLib.Attributes;
using SF3.MPD.Extensions;
using SF3.Types;

namespace SF3.Models.Structs.MPD.Main {
    public partial class MPD_FlagsFromHeader {
        [TableViewModelColumn(addressField: null, displayOrder: 1.0000f, displayName: "(Derived) " + nameof(ModelsChunkIndex), displayGroup: "Flags")]
        public int ModelsChunkIndex => this.GetModelsChunkIndex(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0001f, displayName: "(Derived) " + nameof(SurfaceModelChunkIndex), displayGroup: "Flags")]
        public int SurfaceModelChunkIndex => this.GetSurfaceModelChunkIndex(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0002f, displayName: "(Derived) " + nameof(HasAnySky), displayGroup: "Flags")]
        public bool HasAnySky => Bit_0x0800_HasCutsceneSky || Bit_0x2000_HasBattleSky;

        [TableViewModelColumn(addressField: null, displayOrder: 1.00025f, displayName: "(Derived) " + nameof(HasExtraModel), displayGroup: "Flags")]
        public bool HasExtraModel => Bit_0x0080_HasChunk19ModelWithChunk10Textures || Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures;

        [TableViewModelColumn(addressField: null, displayOrder: 1.0003f, displayName: "(Derived) " + nameof(Chunk1Type), displayGroup: "Flags")]
        public ChunkType Chunk1Type => this.GetChunk1Type(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0004f, displayName: "(Derived) " + nameof(Chunk1PointersMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType? Chunk1PointersMemoryLocation => this.GetChunk1PointersMemoryLocation(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0005f, displayName: "(Derived) " + nameof(Chunk2Type), displayGroup: "Flags")]
        public ChunkType Chunk2Type => this.GetChunk2Type(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0006f, displayName: "(Derived) " + nameof(Chunk20Type) + " (Scn2+)", displayGroup: "Flags")]
        public ChunkType Chunk20Type => this.GetChunk20Type(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0007f, displayName: "(Derived) " + nameof(ModelsMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType ModelsMemoryLocation => this.GetModelsMemoryLocation(Scenario);

        [TableViewModelColumn(addressField: null, displayOrder: 1.0008f, displayName: "(Derived) " + nameof(SurfaceModelMemoryLocation), displayGroup: "Flags")]
        public MemoryLocationType SurfaceModelMemoryLocation => this.GetSurfaceModelMemoryLocation(Scenario);
    }
}
