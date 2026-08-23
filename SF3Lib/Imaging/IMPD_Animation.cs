using CommonLib.Imaging;

namespace SF3.Imaging {
    /// <summary>
    /// Interface for an animation contained in an MPD.
    /// </summary>
    public interface IMPD_Animation : IAnimatedTexture {
        /// <summary>
        /// Retrieves an animation frame based on an external frame counter.
        /// </summary>
        /// <param name="frameCounter">Curent frame number from an external frame counter.</param>
        /// <returns></returns>
        new IMPD_AnimationFrame GetFrame(int frameCounter);

        /// <summary>
        /// Collection of all frames contained in the animation.
        /// </summary>
        new IMPD_AnimationFrame[] Frames { get; }

        /// <summary>
        /// When 'true', the texture for this animation is not allocated in VRAM and should not be used.
        /// </summary>
        bool IsIgnored { get; }
    }
}
