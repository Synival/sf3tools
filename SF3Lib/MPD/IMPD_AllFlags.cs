namespace SF3.MPD {
    public interface IMPD_AllFlags :
        IMPD_AllScenarioFlags,
        IMPD_Scenario1Flags,
        IMPD_Scenario1and2Flags,
        IMPD_Scenario2PlusFlags,
        IMPD_Scenario3PlusFlags,
        IMPD_DerivedFlags
    {
        // Properties to indicate whether or not this flag can be set
        bool CanSet_0x0001_Unknown { get; }
        bool CanSet_0x0002_Unknown { get; }
        bool CanSet_0x0002_HasSurfaceTextureRotation { get; }
        bool CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap { get; }
        bool CanSet_0x0008_KeepTexturelessFlatTiles { get; }
        bool CanSet_0x0010_HasTileBasedForegroundImage { get; }
        bool CanSet_0x0020_Unknown { get; }
        bool CanSet_0x0040_HasBackgroundImage { get; }
        bool CanSet_0x0080_HasChunk19ModelWithChunk10Textures { get; }
        bool CanSet_0x0080_SetMSBForPalette1 { get; }
        bool CanSet_0x0100_HasModels { get; }
        bool CanSet_0x0200_HasSurfaceModel { get; }
        bool CanSet_0x0400_HasGroundImage { get; }
        bool CanSet_0x0800_Unused { get; }
        bool CanSet_0x0800_HasCutsceneSkyBox { get; }
        bool CanSet_0x1000_HasTileBasedGroundImage { get; }
        bool CanSet_0x2000_HasBattleSkyBox { get; }
        bool CanSet_0x2000_NarrowAngleBasedLightmap { get; }
        bool CanSet_0x4000_Unused { get; }
        bool CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures { get; }
        bool CanSet_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists { get; }
        bool CanSet_0x8000_Chunk20IsSurfaceModelIfExists { get; }
    }
}
