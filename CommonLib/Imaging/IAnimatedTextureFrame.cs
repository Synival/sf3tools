namespace CommonLib.Imaging {
    /// <summary>
    /// Interface for a frame to be used with IAnimatedTexture.
    /// </summary>
    public interface IAnimatedTextureFrame : ITexture {
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
