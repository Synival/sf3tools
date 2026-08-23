using CommonLib.Imaging;

namespace SF3.Imaging {
    /// <summary>
    /// Interface for a texture in an MPD that could potentially be replaced with an animation.
    /// </summary>
    public interface IMPD_AnimatableTexture : IAnimatableTexture, IMPD_Texture {
        /// <summary>
        /// Animation that replaces this texture completely.
        /// </summary>
        new IMPD_Animation Animation { get; }
    }
}
