using System;
using CommonLib.Arrays;
using CommonLib.Utils;

namespace CommonLib.Imaging {
    public class ExternalTextureDataSource : ITextureDataSource {
        public ExternalTextureDataSource(IByteArray data, int imageDataOffset, bool isCompressed) {
            Data            = data;
            ImageDataOffset = imageDataOffset;
            IsCompressed    = isCompressed;
        }

        public byte[,] FetchImageData8Bit(ITextureData tex) {
            if (tex.BytesPerPixel != 1)
                throw new InvalidOperationException();
            if (ImageDataOffset < 0 || !IsCompressed && ImageDataOffset + tex.ImageDataSize > Data.Length)
                return null;

            var storedSize = tex.ImageDataSize;
            var inputData = IsCompressed
                ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), ImageDataOffset, null, out storedSize, out var _)
                : Data.GetDataCopyAt(ImageDataOffset, Math.Min(storedSize, Data.Length - ImageDataOffset));
            var outputData = new byte[tex.Width, tex.Height];

            var off = 0;
            for (var y = 0; y < tex.Height; y++) {
                for (var x = 0; x < tex.Width; x++) {
                    var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                    outputData[x, y] = texPixel;
                }
            }

            StoredImageDataSize = storedSize;
            return outputData;
        }

        public ushort[,] FetchImageData16Bit(ITextureData tex) {
            if (tex.BytesPerPixel != 2)
                throw new InvalidOperationException();
            if (ImageDataOffset < 0 || !IsCompressed && ImageDataOffset + tex.ImageDataSize > Data.Length)
                return null;

            var storedSize = tex.ImageDataSize;
            var inputData = (IsCompressed
                ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), ImageDataOffset, null, out storedSize, out var _)
                : Data.GetDataCopyAt(ImageDataOffset, Math.Min(storedSize, Data.Length - ImageDataOffset)))
                .ToUShorts();

            var outputData = new ushort[tex.Width, tex.Height];

            var off = 0;
            for (var y = 0; y < tex.Height; y++) {
                for (var x = 0; x < tex.Width; x++) {
                    var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                    outputData[x, y] = texPixel;
                }
            }

            StoredImageDataSize = storedSize;
            return outputData;
        }

        public IByteArray Data { get; set; }
        public int ImageDataOffset { get; set; }
        public bool IsCompressed { get; set; }
        public int StoredImageDataSize { get; set; }
    }
}
