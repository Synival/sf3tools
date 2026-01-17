using CommonLib.Imaging;

namespace SF3.MPD {
    public interface IMPD_Settings {
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
        /// Technical setting. When 'true', an empty animation table is written as 'FFFF' instead of the typical
        /// longer value. (This is likely a holdover from older MPDs.)
        /// </summary>
        bool ShortEmptyAnimationTable { get; set; }

        /// <summary>
        /// Technical setting. When 'true', an empty "ignored texture" table is written as 'FFFFFFFF' instead of the
        /// typical 'FFFF'. (This is likely a holdover from older MPDs.)
        /// </summary>
        bool LongEmptyIgnoredTextureTable { get; set; }

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
        /// Technical flag. When set, Scenario 3 MPDs will have a truncated LightAdjustment struct without the
        /// extra Scenario 3 fields. This is present in a lot of Scenario 3 MPDs and may be a bug.
        /// </summary>
        bool PaletteAdjustmentIsTruncated { get; set; }
    }
}
