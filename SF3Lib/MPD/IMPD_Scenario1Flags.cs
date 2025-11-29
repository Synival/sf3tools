namespace SF3.MPD {
    public interface IMPD_Scenario1Flags {
        /// <summary>
        /// When set, Chunk[19] is an additional model (used for the Titan in Z_AS.MPD) and texture Chunk[10] is for
        /// that model rather than the models in Chunk[1] (models) and Chunk[2] (surface model).
        /// </summary>
        bool Bit_0x0080_HasChunk19ModelWithChunk10Textures { get; set; }

        /// <summary>
        /// When set, there are chunks with data for a skybox image that is displayed during battle.
        /// </summary>
        bool Bit_0x2000_HasBattleSkyBox { get; set; }

        /// <summary>
        /// When set, if the surface model bit (0x0200) is set, then the models in Chunk[1] are in low memory rather
        /// than high memory. The surface chunk is stored in high memory in this case.
        /// </summary>
        bool Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists { get; set; }
    }
}
