using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_Flags {
        // Possible flags/settings for all scenarios
        bool UnknownMapFlag0x0001 { get; set; }
        bool UnknownMapFlag0x0002 { get; set; }
        bool HasSurfaceTextureRotation { get; set; }
        bool AddDotProductBasedNoiseToStandardLightmap { get; set; }
        bool UnknownFlatTileFlag0x0008 { get; set; }
        BackgroundImageType BackgroundImageType { get; set; }
        bool UnknownMapFlag0x0020 { get; set; }
        bool HasChunk19Model { get; set; }
        bool ModifyPalette1ForGradient { get; set; }
        bool HasModels { get; set; }
        bool HasSurfaceModel { get; set; }
        GroundImageType GroundImageType { get; set; }
        bool HasCutsceneSkyBox { get; set; }
        bool HasBattleSkyBox { get; set; }
        bool NarrowAngleBasedLightmap { get; set; }
        bool HasExtraChunk1ModelWithChunk21Textures { get; set; }
        bool Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists { get; set; }
        bool Chunk20IsSurfaceModelIfExists { get; set; }

        // Derived flags based on other flags
        bool Chunk1IsLoadedFromLowMemory { get; }
        bool Chunk1IsLoadedFromHighMemory { get; }
        bool Chunk1IsModels { get; }
        bool Chunk2IsSurfaceModel { get; }
        bool Chunk20IsSurfaceModel { get; }
        bool Chunk20IsModels { get; }
        bool HighMemoryHasModels { get; }
        bool HighMemoryHasSurfaceModel { get; }
        bool HasAnySkyBox { get; }
    }
}
