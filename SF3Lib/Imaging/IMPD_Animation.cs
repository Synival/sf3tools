using System;

namespace SF3.Imaging {
    /// <summary>
    /// Interface for an animation contained in an MPD.
    /// </summary>
    public interface IMPD_Animation : IDisposable {
        /// <summary>
        /// Retrieves an animation frame based on an external frame counter.
        /// </summary>
        /// <param name="frameCounter">Curent frame number from an external frame counter.</param>
        /// <returns></returns>
        IMPD_AnimationFrame GetFrame(int frameCounter);

        /// <summary>
        /// Initial internal frame counter value of the animation.
        /// </summary>
        int FrameTimerStart { get; }

        /// <summary>
        /// Collection of all frames contained in the animation.
        /// </summary>
        IMPD_AnimationFrame[] Frames { get; }

        /// <summary>
        /// When 'true', the texture for this animation is not allocated in VRAM and should not be used.
        /// </summary>
        bool IsIgnored { get; }
    }
}
