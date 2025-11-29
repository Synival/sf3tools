namespace SF3.MPD {
    public interface IMPD_Scenario2PlusFlags {
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
        bool Bit_0x2000_NarrowAngleBasedLightmap { get; set; }

        /// <summary>
        /// When set, the MSB for all values in Palette1 will be turned on. This seems to be important for maps with a
        /// gradient, but seems redundant because the code that activates the gradient turns this on anyway. Could be
        /// useful for forcing it if a gradient is not immediately present.
        /// </summary>
        bool Bit_0x0080_SetMSBForPalette1 { get; set; }

        /// <summary>
        /// When set, there are chunks with data for a skybox image that is displayed outside of battle.
        /// </summary>
        bool Bit_0x0800_HasCutsceneSkyBox { get; set; }

        /// <summary>
        /// When set, Chunk[1] is an additional model (used for the Kraken) and Chunk[21] is used for its textures.
        /// </summary>
        bool Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures { get; set; }
    }
}
