using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_AllFlags : IMPD_AllScenarioFlags, IMPD_Scenario1Flags, IMPD_Scenario1and2Flags, IMPD_Scenario2PlusFlags, IMPD_Scenario3PlusFlags {
        // Possible flags/settings for all scenarios
        BackgroundImageType BackgroundImageType { get; set; }
        GroundImageType GroundImageType { get; set; }
        bool HasExtraChunk1ModelWithChunk21Textures { get; set; }
        bool Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists { get; set; }
        bool Chunk20IsSurfaceModelIfExists { get; set; }

        // Properties to indicate whether or not this flag can be set
        bool CanSet_0x0001_Unknown { get; }
        bool CanSet_0x0002_Unknown { get; }
        bool CanSet_0x0002_HasSurfaceTextureRotation { get; }
        bool CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap { get; }
        bool CanSet_0x0008_KeepTexturelessFlatTiles { get; }
        bool CanSetBackgroundImageType { get; }
        bool CanSet_0x0020_Unknown { get; }
        bool CanSet_0x0080_HasChunk19Model { get; }
        bool CanSet_0x0080_SetMSBForPalette1 { get; }
        bool CanSetHasModels { get; }
        bool CanSetHasSurfaceModel { get; }
        bool CanSetGroundImageType { get; }
        bool CanSet_0x0800_HasCutsceneSkyBox { get; }
        bool CanSet_0x2000_HasBattleSkyBox { get; }
        bool CanSet_0x2000_NarrowAngleBasedLightmap { get; }
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
