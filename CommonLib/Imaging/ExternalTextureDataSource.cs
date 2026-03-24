using CommonLib.Arrays;

namespace CommonLib.Imaging {
    public class ExternalTextureDataSource : ITextureDataSource {
        public ExternalTextureDataSource(IByteArray data, int imageDataOffset, bool isCompressed) {
            Data            = data;
            ImageDataOffset = imageDataOffset;
            IsCompressed    = isCompressed;
        }

        public IByteArray Data { get; set; }
        public int ImageDataOffset { get; set; }
        public bool IsCompressed { get; set; }
        public int StoredImageDataSize { get; set; }
    }
}
