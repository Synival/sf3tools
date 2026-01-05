namespace SF3.Imaging {
    /// <summary>
    /// Interface for an animation contained in an MPD.
    /// </summary>
    public interface IMPD_Animation {
        /// <summary>
        /// Retrieves an animation frame based on an external frame counter.
        /// </summary>
        /// <param name="frameCounter">Curent frame number from an external frame counter.</param>
        /// <returns></returns>
        IMPD_AnimationFrame GetFrame(int frameCounter);

        /// <summary>
        /// ID of the texture this animation is assigned to.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Initial internal frame counter value of the animation.
        /// </summary>
        int FrameTimerStart { get; }

        /// <summary>
        /// Collection of all frames contained in the animation.
        /// </summary>
        IMPD_AnimationFrame[] Frames { get; }
    }
}
