using System;

namespace CommonLib.Imaging {
    public class NullTextureDataSource : ITextureDataSource {
        public byte[,] FetchImageData8Bit(ITextureData tex) {
            // No data to fetch -- it only exists in the buffer.
            return null;
        }

        public ushort[,] FetchImageData16Bit(ITextureData tex) {
            // No data to fetch -- it only exists in the buffer.
            return null;
        }

        public byte[] ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, Palette palette) {
            // No data to store -- it only exists in the buffer.
            return null;
        }

        public byte[] ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data) {
            // No data to store -- it only exists in the buffer.
            return null;
        }

        public void StoreImageData(ITextureData data, byte[] storageData) {
            // No data to store -- it only exists in the buffer.
        }

        public int? StoredImageDataSize => null;
    }
}
