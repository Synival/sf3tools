using System;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;

namespace CommonLib.Imaging {
    public class InPlaceTextureData : TextureDataStandard, ITextureData {
        public InPlaceTextureData(
            IByteArray data, int imageDataOffset,
            int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool isCompressed, bool zeroIsTransparent, bool canSetImage
        ) : base(
            width,
            height,
            pixelFormat,
            palette,
            zeroIsTransparent,
            canSetImage,
            new ExternalTextureDataSource(data, imageDataOffset, isCompressed)
        ) {
        }

        protected override byte[,] FetchImageData8Bit() {
            if (BytesPerPixel != 1)
                throw new InvalidOperationException();
            if (ImageDataOffset < 0 || !IsCompressed && ImageDataOffset + ImageDataSize > Data.Length)
                return null;

            var storedSize = ImageDataSize;
            var inputData = IsCompressed
                ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), ImageDataOffset, null, out storedSize, out var _)
                : Data.GetDataCopyAt(ImageDataOffset, Math.Min(storedSize, Data.Length - ImageDataOffset));
            var outputData = new byte[Width, Height];

            var off = 0;
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                    outputData[x, y] = texPixel;
                }
            }

            DataSource.StoredImageDataSize = storedSize;
            return outputData;
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var off = 0;
            var newWidth = data.GetLength(0);
            var newHeight = data.GetLength(1);
            var newData = new byte[newWidth * newHeight];
            for (var y = 0; y < newHeight; y++)
                for (var x = 0; x < newWidth; x++)
                    newData[off++] = data[x, y];

            var newStoredData = IsCompressed ? Compression.CompressLZSS(newData) : newData;

            var error = Validate8BitImageData(data, palette, StoredImageDataSize, newStoredData.Length);
            if (error != null)
                throw new ArgumentException(error);

            SetPixelFormatInternal(TexturePixelFormat.Indexed8Bit, invalidate: false);
            SetDimensionsInternal(newWidth, newHeight, invalidate: false);
            Data.SetDataAtTo(ImageDataOffset, newStoredData.Length, newStoredData);

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                _ = _textureDataBuffer.SetImageData8Bit(data);
                Palette = palette;
                DataSource.StoredImageDataSize = newStoredData.Length;
            }

            InvokeInvalidatedEvent();
        }

        protected override ushort[,] FetchImageData16Bit() {
            if (BytesPerPixel != 2)
                throw new InvalidOperationException();
            if (ImageDataOffset < 0 || !IsCompressed && ImageDataOffset + ImageDataSize > Data.Length)
                return null;

            var storedSize = ImageDataSize;
            var inputData = (IsCompressed
                ? Compression.DecompressLZSS(Data.GetDataCopyOrReference(), ImageDataOffset, null, out storedSize, out var _)
                : Data.GetDataCopyAt(ImageDataOffset, Math.Min(storedSize, Data.Length - ImageDataOffset)))
                .ToUShorts();

            var outputData = new ushort[Width, Height];

            var off = 0;
            for (var y = 0; y < Height; y++) {
                for (var x = 0; x < Width; x++) {
                    var texPixel = off < inputData.Length ? inputData[off++] : (byte) 0;
                    outputData[x, y] = texPixel;
                }
            }

            DataSource.StoredImageDataSize = storedSize;
            return outputData;
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            var off = 0;
            var newWidth = data.GetLength(0);
            var newHeight = data.GetLength(1);

            var newData = data.Clone() as ushort[,];
            newData.FixSaturnTransparency(useEndCodes: true);
            var newData1D = new byte[newWidth * newHeight * 2];

            for (var y = 0; y < newHeight; y++) {
                for (var x = 0; x < newWidth; x++) {
                    var val = newData[x, y];
                    newData1D[off++] = (byte) (val >> 8);
                    newData1D[off++] = (byte) val;
                }
            }

            var newStoredData = IsCompressed ? Compression.CompressLZSS(newData1D) : newData1D;

            var error = Validate16BitImageData(data, StoredImageDataSize, newStoredData.Length);
            if (error != null)
                throw new ArgumentException(error);

            PixelFormat = TexturePixelFormat.ABGR1555;
            SetDimensionsInternal(newWidth, newHeight, invalidate: false);
            Data.SetDataAtTo(ImageDataOffset, newData1D.Length, newData1D);

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                _ = _textureDataBuffer.SetImageData16Bit(newData);
                DataSource.StoredImageDataSize = newStoredData.Length;
            }

            InvokeInvalidatedEvent();
        }

        public IByteArray Data {
            get => DataSource.Data;
            set {
                if (DataSource.Data != value) {
                    DataSource.Data = value;
                    Invalidate();
                }
            }
        }

        public virtual int ImageDataOffset {
            get => DataSource.ImageDataOffset;
            set {
                if (DataSource.ImageDataOffset != value) {
                    DataSource.ImageDataOffset = value;
                    Invalidate();
                }
            }
        }

        public bool IsCompressed {
            get => DataSource.IsCompressed;
            set {
                if (DataSource.IsCompressed != value) {
                    DataSource.IsCompressed = value;
                    Invalidate();
                }
            }
        }

        public int StoredImageDataSize => DataSource.StoredImageDataSize;

        protected new ExternalTextureDataSource DataSource => (ExternalTextureDataSource) base.DataSource;
    }
}
