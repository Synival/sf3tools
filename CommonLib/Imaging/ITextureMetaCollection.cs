using System.Collections.Generic;

namespace CommonLib.Imaging {
    public interface ITextureMetaCollection {
        /// <summary>
        /// Returns a dictionary ot IAnimatableTexture's, keyed by their TextureID.
        /// The textures returned should be usable with the ModelCollectionID provided.
        /// </summary>
        /// <param name="mcId">Corresponding ModelCollectionID that textures may be associated with.</param>
        /// <returns>A non-null collection of textures, keyed by their TextureID.</returns>
        Dictionary<int, IAnimatableTexture> GetAnimatableTexturesByModelCollectionID(int mcId);
    }
}
