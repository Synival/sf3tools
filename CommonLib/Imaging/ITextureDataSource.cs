namespace CommonLib.Imaging {
    public interface ITextureDataSource {
        byte[,] FetchImageData8Bit(ITextureData tex);
        ushort[,] FetchImageData16Bit(ITextureData tex);

        byte[] ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, Palette palette);
        byte[] ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data);

        void StoreImageData(ITextureData data, byte[] storageData);

        int? StoredImageDataSize { get; }
    }
}
