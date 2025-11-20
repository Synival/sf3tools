using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_Flags {
        // Possible flags/settings for all scenarios
        bool UnknownMapFlag0x0001 { get; set; }
        bool UnknownMapFlag0x0002 { get; set; }
        bool HasSurfaceTextureRotation { get; set; }
        bool AddDotProductBasedNoiseToStandardLightmap { get; set; }
        bool KeepTexturelessFlatTiles { get; set; }
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

        // Properties to indicate whether or not this flag can be set
        bool CanSetUnknownMapFlag0x0001 { get; }
        bool CanSetUnknownMapFlag0x0002 { get; }
        bool CanSetHasSurfaceTextureRotation { get; }
        bool CanSetAddDotProductBasedNoiseToStandardLightmap { get; }
        bool CanSetKeepTexturelessFlatTiles { get; }
        bool CanSetBackgroundImageType { get; }
        bool CanSetUnknownMapFlag0x0020 { get; }
        bool CanSetHasChunk19Model { get; }
        bool CanSetModifyPalette1ForGradient { get; }
        bool CanSetHasModels { get; }
        bool CanSetHasSurfaceModel { get; }
        bool CanSetGroundImageType { get; }
        bool CanSetHasCutsceneSkyBox { get; }
        bool CanSetHasBattleSkyBox { get; }
        bool CanSetNarrowAngleBasedLightmap { get; }
        bool CanSetHasExtraChunk1ModelWithChunk21Textures { get; }
        bool CanSetChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists { get; }
        bool CanSetChunk20IsSurfaceModelIfExists { get; }

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
