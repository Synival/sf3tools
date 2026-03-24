using System;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Types;

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

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var newStoredData = DataSource.ConvertImageDataToStorageData8Bit(this, data, palette);
            var error = Validate8BitImageData(data, palette, StoredImageDataSize, newStoredData.Length);
            if (error != null)
                throw new ArgumentException(error);

            DataSource.StoreImageData(this, newStoredData);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                PixelFormat = TexturePixelFormat.Indexed8Bit;
                Width = data.GetLength(0);
                Height = data.GetLength(1);
                _ = _textureDataBuffer.SetImageData8Bit(data);
                Palette = palette;
            }
            InvokeInvalidatedEvent();
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            data = data.Clone() as ushort[,];
            data.FixSaturnTransparency(useEndCodes: true);

            var newStoredData = DataSource.ConvertImageDataToStorageData16Bit(this, data);
            var error = Validate16BitImageData(data, StoredImageDataSize, newStoredData.Length);
            if (error != null)
                throw new ArgumentException(error);

            DataSource.StoreImageData(this, newStoredData);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                PixelFormat = TexturePixelFormat.ABGR1555;
                Width = data.GetLength(0);
                Height = data.GetLength(1);
                _ = _textureDataBuffer.SetImageData16Bit(data);
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
