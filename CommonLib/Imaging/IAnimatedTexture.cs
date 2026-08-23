namespace CommonLib.Imaging {
    /// <summary>
    /// Interface for a texture whose contents animated while retaining the same ID.
    /// </summary>
    public interface IAnimatedTexture : ITextureRef {
        /// <summary>
        /// Retrieves an animation frame based on an external frame counter.
        /// </summary>
        /// <param name="frameCounter">Curent frame number from an external frame counter.</param>
        /// <returns></returns>
        IAnimatedTextureFrame GetFrame(int frameCounter);

        /// <summary>
        /// Initial internal frame counter value of the animation.
        /// </summary>
        int FrameTimerStart { get; }

        /// <summary>
        /// Collection of all frames contained in the animation.
        /// </summary>
        IAnimatedTextureFrame[] Frames { get; }
    }
}
