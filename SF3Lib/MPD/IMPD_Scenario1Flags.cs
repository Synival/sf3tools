namespace SF3.MPD {
    public interface IMPD_Scenario1Flags : IMPD_AllScenarioFlags, IMPD_Scenario1and2Flags, IMPD_DerivedFlags {
        /// <summary>
        /// Builds the flags for an MPD file belonging to Scenario 1.
        /// </summary>
        /// <returns></returns>
        ushort GetScenario1HeaderFlags();

        /// <summary>
        /// When set, Chunk[19] is an additional model (used for the Titan in Z_AS.MPD) and texture Chunk[10] is for
        /// that model rather than the models in Chunk[1] (models) and Chunk[2] (surface model).
        /// </summary>
        bool Bit_0x0080_HasChunk19ModelWithChunk10Textures { get; set; }

        /// <summary>
        /// Unused flag.
        /// </summary>
        bool Bit_0x0800_Unused { get; set; }

        /// <summary>
        /// When set, there are chunks with data for a skybox image that is displayed during battle.
        /// </summary>
        bool Bit_0x2000_HasBattleSkyBox { get; set; }

        /// <summary>
        /// Unused flag.
        /// </summary>
        bool Bit_0x4000_Unused { get; set; }
    }
}
