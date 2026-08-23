namespace CommonLib.Imaging {
    /// <summary>
    /// Interface for a texture that could potentially be replaced with an animation.
    /// </summary>
    public interface IAnimatableTexture : ITexture {
        /// <summary>
        /// Animation that replaces this texture completely, if it exists.
        /// </summary>
        IAnimatedTexture Animation { get; }
    }
}
