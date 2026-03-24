namespace CommonLib.Imaging {
    /// <summary>
    /// A source for fetching and storaging image data.
    /// </summary>
    public interface ITextureDataSource {
        byte[,] FetchImageData8Bit(ITextureData tex);
        ushort[,] FetchImageData16Bit(ITextureData tex);

        object ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, Palette palette, out int? storageSize);
        object ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data, out int? storageSize);

        void StoreImageData(ITextureData data, object storageData);

        int? StoredImageDataSize { get; }
    }
}
