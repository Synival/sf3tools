using System;

namespace CommonLib.Imaging {
    public class InternalTextureDataSource : ITextureDataSource {
        public byte[,] FetchImageData8Bit(ITextureData tex) {
            if (tex.BytesPerPixel != 1)
                throw new InvalidOperationException();

            // Nothing to fetch; if it's not set, it's not set.
            return null;
        }

        public ushort[,] FetchImageData16Bit(ITextureData tex) {
            if (tex.BytesPerPixel != 2)
                throw new InvalidOperationException();

            // Nothing to fetch; if it's not set, it's not set.
            return null;
        }

        public byte[] ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, Palette palette) {
            // No data to store.
            return null;
        }

        public byte[] ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data) {
            // No data to store.
            return null;
        }
    }
}
