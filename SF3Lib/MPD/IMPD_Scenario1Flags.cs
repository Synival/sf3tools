namespace SF3.MPD {
    public interface IMPD_Scenario1Flags {
        /// <summary>
        /// When set, Chunk[19] is an additional model (used for the Titan in Z_AS.MPD) and texture Chunk[10] is for
        /// that model rather than the models in Chunk[1] (models) and Chunk[2] (surface model).
        /// </summary>
        bool Bit_0x0080_HasChunk19Model { get; set; }
    }
}
