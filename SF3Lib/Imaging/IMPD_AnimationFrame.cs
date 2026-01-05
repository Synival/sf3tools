namespace SF3.Imaging {
    /// <summary>
    /// Interface for a frame to be used with IMPD_Animation.
    /// </summary>
    public interface IMPD_AnimationFrame : IMPD_Texture {
        /// <summary>
        /// Frame index of this texture.
        /// </summary>
        int Frame { get; }

        /// <summary>
        /// Length of time in 1/30 seconds that this frame is active.
        /// </summary>
        int Duration { get; }
    }
}
