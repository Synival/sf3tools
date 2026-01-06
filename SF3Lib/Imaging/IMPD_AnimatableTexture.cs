using System.Collections.Generic;
using CommonLib.Imaging;
using SF3.Types;

namespace SF3.Imaging {
    /// <summary>
    /// Interface for a texture in an MPD that could potentially be replaced with an animation.
    /// </summary>
    public interface IMPD_AnimatableTexture : IMPD_Texture {
        /// <summary>
        /// Animation that replaces this texture completely.
        /// </summary>
        IMPD_Animation Animation { get; }
    }
}
