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
        ) : base(width, height, pixelFormat, palette, zeroIsTransparent, canSetImage) {
            _data              = data;
            _imageDataOffset   = imageDataOffset;
            _isCompressed      = isCompressed;
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

            StoredImageDataSize = storedSize;
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
                StoredImageDataSize = newStoredData.Length;
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

            StoredImageDataSize = storedSize;
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
            Width  = newWidth;
            Height = newHeight;
            Data.SetDataAtTo(ImageDataOffset, newData1D.Length, newData1D);

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                _ = _textureDataBuffer.SetImageData16Bit(newData);
                StoredImageDataSize = newStoredData.Length;
            }

            InvokeInvalidatedEvent();
        }

        private IByteArray _data;
        public IByteArray Data {
            get => _data;
            set {
                if (_data != value) {
                    _data = value;
                    Invalidate();
                }
            }
        }

        private int _imageDataOffset;
        public virtual int ImageDataOffset {
            get => _imageDataOffset;
            set {
                if (_imageDataOffset != value) {
                    _imageDataOffset = value;
                    Invalidate();
                }
            }
        }

        private bool _isCompressed;
        public bool IsCompressed {
            get => _isCompressed;
            set {
                if (_isCompressed != value) {
                    _isCompressed = value;
                    Invalidate();
                }
            }
        }

        public int StoredImageDataSize { get; private set; }
    }
}
