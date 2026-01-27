using CommonLib.Imaging;

namespace SF3.MPD.Interfaces {
    public interface IMPD_Settings {
        /// <summary>
        /// When set, the surface is visible with textures and meshes. When unset, surface model tile heights
        /// can be set independently of their neighbors.
        /// </summary>
        bool HasSurfaceModel { get; set; }

        /// <summary>
        /// When set, models are accessed from low memory (0x00292100) rather than high memory (0x060A0000).
        /// </summary>
        bool ForceLowMemoryModels { get; set; }

        /// <summary>
        /// When set, the lightmap index for the surface has an additional calculation step that converts the dot
        /// product of the lighting to an angle, which is used for the lightmap index using this formula:
        /// 
        ///    lightmapIndex = ((dotProduct < 0) ? 0 : atan2(dotProduct, (1 - dotProduct)^2) / PI * 128 + 64) % 32
        ///    
        /// The resulting angle-to-lightmap-index is:
        /// 
        /// Angle  Lightmap-Index
        ///  0      0
        ///  15     5
        ///  30     10
        ///  45     15
        ///  60     21
        ///  75     26
        ///  90     31
        /// --- wraps ---
        ///  105    5
        ///  120    10
        ///  135    15
        ///  150    21
        ///  165    26
        ///  180    31
        ///  
        /// To prevent wrapping, the pitch of the light direction should be pointing very close to straight down, or
        /// --- with poorer results --- directly from the side.
        /// </summary>
        bool NarrowAngleBasedLightmap { get; set; }

        /// <summary>
        /// When set, the MSB for all values for the ground palette will be turned on. This seems to be important for maps with a
        /// gradient, but seems redundant because the code that activates the gradient turns this on anyway. Could be
        /// useful for forcing it if a gradient is not immediately present.
        /// </summary>
        bool SetMSBForGroundPalette { get; set; }

        /// <summary>
        /// When set, tiles' textures can be rotated as well as flipped. The surface model should be in Chunk[2] if
        /// this is on.
        /// </summary>
        bool HasSurfaceTextureRotation { get; set; }

        /// <summary>
        /// When set, the 0x01 bit of the lightmap indices for surface model tile vertices are XOR'ed based on the value of the
        /// dot product of the tile's normal and the light direct vector. This is used for outdoor maps that need to appear
        /// noisier. Does not apply to maps with 'NarrowAngleBasedLightmap' on.
        /// </summary>
        bool AddDotProductBasedNoiseToStandardLightmap { get; set; }

        /// <summary>
        /// Normally, tiles marked as "flat" that have no texture are skipped over. When this is set, they are not skipped over,
        /// and exist for the purpose of highlights for movement or targetting during battle. When this is not set, any flat
        /// tile without a texture will not show a highlight at all during battle.
        /// </summary>
        bool KeepTexturelessFlatTiles { get; set; }

        /// <summary>
        /// Unknown MPD header flag 0x0020. Present in Scenario 2 HNSN00, HNSN01, and HNSN02.
        /// </summary>
        bool UnknownHeaderFlag { get; set; }

        /// <summary>
        /// Y-axis rotation of the entire scene (in degrees), but just the models. Always set to 0x8000, otherwise sprites are
        /// facing the wrong way.
        /// </summary>
        float ModelsYRotation { get; set; }

        /// <summary>
        /// Some sort of value that determines what models are visible. Usually 0x48, goes up to 0x90.
        /// </summary>
        ushort ModelsViewDistance { get; set; }

        /// <summary>
        /// Lower angle (in degrees) of the view area in which models appear. Usually -108.
        /// </summary>
        float ModelsViewAngleMin { get; set; }

        /// <summary>
        /// Upper angle (in degrees) of the view area in which models appear. Usually +108.
        /// </summary>
        float ModelsViewAngleMax { get; set; }

        /// <summary>
        /// Some unknown value. Seems to usually be small and negative (~ -0x30).
        /// </summary>
        short UnknownHeaderSetting { get; set; }

        /// <summary>
        /// The color here is added to every color in the light palette.
        /// Only supported in Scenario 2+. If unsupported, all channels are locked at 0x00.
        /// </summary>
        IColorAdjustRGB555 LightPaletteAdjustment { get; set; }

        /// <summary>
        /// The color here is added to every color in the ground palette.
        /// Only supported in Scenario 3+ and only selectively applied, depending on the X1???.BIN file.
        /// If unsupported, all channels are locked at 0x00.
        /// </summary>
        IColorAdjustRGB555 GroundPaletteAdjustment { get; set; }

        /// <summary>
        /// The transparency level for models tagged as transparent (tag 2000 (decimal) in any model instance).
        /// Valid range is (0x00 ... 0x1F).
        /// Only supported in Scenario 3+. If unsupported, this is locked at 0x0F.
        /// </summary>
        byte ShadowTransparency { get; set; }

        /// <summary>
        /// When set, the ground animation table exists, but is serialized with 'FF' before it and unused.
        /// </summary>
        bool AreGroundAnimationsDummiedOut { get; set; }

        /// <summary>
        /// When set, the gradient exists, but is serialized with 'FFFF' before it and unused.
        /// </summary>
        bool IsGradientDummiedOut { get; set; }

        /// <summary>
        /// When true, an Unknown2 table exists, but is dummied out with a preceeding 0xFFFF.
        /// </summary>
        bool IsUnknown2TableDummiedOut { get; set; }

        /// <summary>
        /// When set, the table with ignored textures exists, but is serialized with 'FFFF' before it and unused.
        /// </summary>
        bool AreIgnoredTexturesDummiedOut { get; set; }

        /// <summary>
        /// When set, any ground plane that exists will be ignored when exporting MPD flags.
        /// </summary>
        bool IgnoreGroundImage { get; set; }

        /// <summary>
        /// When set, any tile-based ground image present is ignored when exporting MPD flags.
        /// </summary>
        bool IgnoreGroundTiledImage { get; set; }

        /// <summary>
        /// When set, any sky plane that exists will be ignored when exporting MPD flags.
        /// </summary>
        bool IgnoreSkyImage { get; set; }

        /// <summary>
        /// When set, any background image present is ignored when exporting MPD flags.
        /// </summary>
        bool IgnoreBackgroundImage { get; set; }

        /// <summary>
        /// When set, any tile-based foreground image present is ignored when exporting MPD flags.
        /// </summary>
        bool IgnoreForegroundTiledImage { get; set; }

        /// <summary>
        /// When set, an image should be loaded when a battle occurs with this MPD. This should always be set for any
        /// MPD that is expected to have a battle. In Scenario 1, a sky plane should be present. In Scenario 2+, the
        /// sky plane is for cutscenes only and the battle background is loaded from elsewhere.
        /// </summary>
        bool HasBattleBackground { get; set; }

        /// <summary>
        /// When set, any surface model that exists will be ignored when exporting MPD flags.
        /// </summary>
        bool IgnoreSurfaceModel { get; set; }

        /// <summary>
        /// When set, an unreferenced Unknown2Table is placed after the gradient in Scenario 2+ MPDs.
        /// This occurs in some Scenario 2 maps (MUBAR2, SARA22), where they just added 0xFFF to the front
        /// and didn't bother with a gradient.
        /// </summary>
        bool IsUnknown2InLaterFileAfterGradient { get; set; }
    }
}
