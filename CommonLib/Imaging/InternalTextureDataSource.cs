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
    }
}
