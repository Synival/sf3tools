using System;
using CommonLib.Extensions;
using CommonLib.Types;
using CommonLib.Utils;

namespace CommonLib.Imaging {
    /// <summary>
    /// 8-bit indexed or 16-bit texture with a configurable data source and strategy for fetching and assigning
    /// image and palette data.
    /// </summary>
    public class CachedTextureData : CachedTextureDataBase {
        public CachedTextureData(
            int width,
            int height,
            TexturePixelFormat pixelFormat,
            IPalette palette,
            bool zeroIsTransparent,
            bool canSetImage,
            ITextureDataSource dataSource,
            IndexedColorUpdateStrategy indexedUpdateStrategy
        ) {
            _width             = width;
            _height            = height;
            _pixelFormat       = pixelFormat;
            _palette           = palette;
            _zeroIsTransparent = zeroIsTransparent;
            CanSetImage        = canSetImage;
            DataSource        = dataSource;
            IndexedColorUpdateStrategy = indexedUpdateStrategy;
        }

        protected override byte[,] FetchImageData8Bit() {
            if (BytesPerPixel != 1)
                throw new InvalidOperationException();
            return DataSource.FetchImageData8Bit(this);
        }

        protected override ushort[,] FetchImageData16Bit() {
            if (BytesPerPixel != 2)
                throw new InvalidOperationException();
            return DataSource.FetchImageData16Bit(this);
        }

        public override void SetImageData8Bit(byte[,] data, IPalette palette) {
            if (IndexedColorUpdateStrategy == IndexedColorUpdateStrategy.MatchToExistingPalette)
                data = ImageUtils.GetImageDataConformingToPalette(data, palette, Palette);

            var newStoredData = DataSource.ConvertImageDataToStorageData8Bit(this, data, palette, out var newStoredDataSize);
            var error = Validate8BitImageData(data, palette, DataSource.StoredImageDataSize, newStoredDataSize);
            if (error != null)
                throw new ArgumentException(error);

            DataSource.StoreImageData(this, newStoredData);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                PixelFormat = TexturePixelFormat.Indexed8Bit;
                Width = data.GetLength(0);
                Height = data.GetLength(1);
                _ = _textureDataCache.SetImageData8Bit(data);

                if (IndexedColorUpdateStrategy == IndexedColorUpdateStrategy.UpdateExistingPalette)
                    Palette.Replace(palette.Colors);
            }
            InvokeInvalidatedEvent();
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            data = data.Clone() as ushort[,];
            data.FixSaturnTransparency(useEndCodes: true);

            var newStoredData = DataSource.ConvertImageDataToStorageData16Bit(this, data, out var newStoredDataSize);
            var error = Validate16BitImageData(data, DataSource.StoredImageDataSize, newStoredDataSize);
            if (error != null)
                throw new ArgumentException(error);

            DataSource.StoreImageData(this, newStoredData);

            Invalidate(sendEvent: false);
            using (InvalidateGuard()) {
                PixelFormat = TexturePixelFormat.ABGR1555;
                Width = data.GetLength(0);
                Height = data.GetLength(1);
                _ = _textureDataCache.SetImageData16Bit(data);
                Palette = null;
            }
            InvokeInvalidatedEvent();
        }

        public void LoadImageData() {
            // Accessing the getter performs loading.
            if (BytesPerPixel == 1)
                _ = ImageData8Bit;
            else
                _ = ImageData16Bit;
        }

        private int _width;
        public override int Width {
            get => _width;
            set {
                if (_width != value) {
                    _width = value;
                    Invalidate();
                }
            }
        }

        private int _height;
        public override int Height {
            get => _height;
            set {
                if (_height != value) {
                    _height = value;
                    Invalidate();
                }
            }
        }

        private TexturePixelFormat _pixelFormat;
        public override TexturePixelFormat PixelFormat {
            get => _pixelFormat;
            set {
                if (_pixelFormat != value) {
                    _pixelFormat = value;
                    Invalidate();
                }
            }
        }

        private IPalette _palette;
        public override IPalette Palette {
            get => _palette;
            set {
                if (_palette != value) {
                    _palette = value;
                    Invalidate();
                }
            }
        }

        private bool _zeroIsTransparent;
        public override bool ZeroIsTransparent {
            get => _zeroIsTransparent;
            set {
                if (_zeroIsTransparent != value) {
                    _zeroIsTransparent = value;
                    Invalidate();
                }
            }
        }

        public override bool CanSetImageData8Bit => CanSetImage;
        public override bool CanSetImageData16Bit => CanSetImage;
        public virtual bool CanSetImage { get; set; }
        public IndexedColorUpdateStrategy IndexedColorUpdateStrategy { get; }

        protected ITextureDataSource DataSource { get; }
    }
}
