namespace SF3.MPD {
    public interface IMPD_Scenario2PlusFlags {
        /// <summary>
        /// When set, the MSB for all values in Palette1 will be turned on.
        /// (See 'IMPD_Settings.NarrowAngleBasedLightmap' for details.)
        /// </summary>
        bool Bit_0x0080_SetMSBForPalette1 { get; set; }

        /// <summary>
        /// When set, there are chunks with data for a skybox image that is displayed outside of battle.
        /// </summary>
        bool Bit_0x0800_HasCutsceneSkyBox { get; set; }

        /// <summary>
        /// When set, the lightmap index for the surface has an additional calculation step that converts the dot
        /// product of the lighting to an angle.
        /// (See 'IMPD_Settings.NarrowAngleBasedLightmap' for details.)
        /// </summary>
        bool Bit_0x2000_NarrowAngleBasedLightmap { get; set; }

        /// <summary>
        /// When set, Chunk[1] is an additional model (used for the Kraken) and Chunk[21] is used for its textures.
        /// </summary>
        bool Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures { get; set; }
    }
}
