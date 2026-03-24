namespace CommonLib.Imaging {
    /// <summary>
    /// Non-existant data source. Using this with an instance of CachedDataInstance will result in a texture with no
    /// data source; the image data is stored only in its own cache.
    /// </summary>
    public class DummyTextureDataSource : ITextureDataSource {
        public byte[,] FetchImageData8Bit(ITextureData tex) {
            // No data to fetch -- it only exists in the buffer.
            return null;
        }

        public ushort[,] FetchImageData16Bit(ITextureData tex) {
            // No data to fetch -- it only exists in the buffer.
            return null;
        }

        public object ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, Palette palette, out int? storageSize) {
            // No data to store -- it only exists in the buffer.
            storageSize = null;
            return null;
        }

        public object ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data, out int? storageSize) {
            // No data to store -- it only exists in the buffer.
            storageSize = null;
            return null;
        }

        public void StoreImageData(ITextureData data, object storageData) {
            // No data to store -- it only exists in the buffer.
        }

        public int? StoredImageDataSize => null;
    }
}
