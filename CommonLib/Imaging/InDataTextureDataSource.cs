using System;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Utils;

namespace CommonLib.Imaging {
    /// <summary>
    /// Data source that comes directly from an IByteArray.
    /// </summary>
    public class InDataTextureDataSource : ITextureDataSource {
        public InDataTextureDataSource(IByteArray data, int imageDataOffset, bool isCompressed) {
            Data            = data;
            ImageDataOffset = imageDataOffset;
            IsCompressed    = isCompressed;
        }

        public byte[,] FetchImageData8Bit(ITextureData tex) {
            if (ImageDataOffset < 0 || (!IsCompressed && ImageDataOffset + tex.ImageDataSize > Data.Length)) {
                StoredImageDataSize = null;
                return null;
            }

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
            if (ImageDataOffset < 0 || (!IsCompressed && ImageDataOffset + tex.ImageDataSize > Data.Length)) {
                StoredImageDataSize = null;
                return null;
            }

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

        public object ConvertImageDataToStorageData8Bit(ITextureData tex, byte[,] data, IPalette palette, out int? storageSize) {
            var newData = data.To1DArrayTransposed();
            newData = IsCompressed ? Compression.CompressLZSS(newData) : newData;
            storageSize = newData.Length;
            return newData;
        }

        public object ConvertImageDataToStorageData16Bit(ITextureData tex, ushort[,] data, out int? storageSize) {
            var newWidth  = data.GetLength(0);
            var newHeight = data.GetLength(1);
            var newData = new byte[newWidth * newHeight * 2];

            int off = 0;
            for (var y = 0; y < newHeight; y++) {
                for (var x = 0; x < newWidth; x++) {
                    var val = data[x, y];
                    newData[off++] = (byte) (val >> 8);
                    newData[off++] = (byte) val;
                }
            }

            newData = IsCompressed ? Compression.CompressLZSS(newData) : newData;
            storageSize = newData.Length;
            return newData;
        }

        public void StoreImageData(ITextureData tex, object storageData) {
            var storageDataBytes = (byte[]) storageData;
            Data.SetDataAtTo(ImageDataOffset, storageDataBytes.Length, storageDataBytes);
            StoredImageDataSize = storageDataBytes.Length;
        }

        public IByteArray Data { get; set; }
        public int ImageDataOffset { get; set; }
        public bool IsCompressed { get; set; }
        public int? StoredImageDataSize { get; private set; }
    }
}
