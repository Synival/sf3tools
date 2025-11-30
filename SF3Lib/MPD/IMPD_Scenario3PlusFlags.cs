namespace SF3.MPD {
    public interface IMPD_Scenario3PlusFlags {
        /// <summary>
        /// When set, tiles' textures can be rotated as well as flipped. The surface model should be in Chunk[2] if
        /// this is on.
        /// </summary>
        bool Bit_0x0002_HasSurfaceTextureRotation { get; set; }
    }
}
