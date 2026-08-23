namespace CommonLib.Imaging {
    public interface ITexture : ITextureData {
        /// <summary>
        /// "Meta" ID of collection this texture belongs to.
        /// </summary>
        int TextureCollectionID { get; }

        /// <summary>
        /// ID for texture.
        /// </summary>
        int TextureID { get; }
    }
}
