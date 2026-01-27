namespace SF3.MPD.Interfaces.Flags {
    public interface IMPD_Flags_Scenario3Plus {
        /// <summary>
        /// When set, tiles' textures can be rotated as well as flipped. The surface model should be in Chunk[2] if
        /// this is on.
        /// </summary>
        bool Bit_0x0002_HasSurfaceTextureRotation { get; set; }
    }
}
