using System;
using CommonLib.Types;

namespace CommonLib.Imaging {
    public class TextureData : TextureDataBase, ITextureData {
        public TextureData(byte[,] data, Palette palette, bool zeroIsTransparent, bool canSetImage) {
            if (data != null) {
                _width  = data.GetLength(0);
                _height = data.GetLength(1);
                _textureDataBuffer.SetImageData8Bit(data);
            }

            _pixelFormat       = TexturePixelFormat.Indexed8Bit;
            _palette           = palette;
            _zeroIsTransparent = zeroIsTransparent;
            CanSetImage        = canSetImage;
        }

        public TextureData(ushort[,] data, bool canSetImage) {
            if (data != null) {
                _width       = data.GetLength(0);
                _height      = data.GetLength(1);
                _textureDataBuffer.SetImageData16Bit(data);
            }

            _pixelFormat       = TexturePixelFormat.ABGR1555;
            CanSetImage        = canSetImage;
        }

        public TextureData(int width, int height, TexturePixelFormat pixelFormat, Palette palette, bool zeroIsTransparent, bool canSetImage) {
            _width             = width;
            _height            = height;
            _pixelFormat       = pixelFormat;
            _palette           = palette;
            _zeroIsTransparent = zeroIsTransparent;
            CanSetImage        = canSetImage;
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

        private Palette _palette;
        public override Palette Palette {
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

        protected override byte[,] FetchImageData8Bit() {
            if (BytesPerPixel != 1)
                throw new InvalidOperationException();

            // Nothing to fetch; if it's not set, it's not set.
            return null;
        }

        public override void SetImageData8Bit(byte[,] data, Palette palette) {
            var newWidth = data.GetLength(0);
            var newHeight = data.GetLength(1);

            var error = Validate8BitImageData(data, palette, ImageDataSize, newWidth * newHeight);
            if (error != null)
                throw new ArgumentException(error);

            _pixelFormat = TexturePixelFormat.Indexed8Bit;
            _width  = newWidth;
            _height = newHeight;

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--)) {
                _ = _textureDataBuffer.SetImageData8Bit(data);
                Palette = palette;
            }

            InvokeInvalidatedEvent();
        }

        protected override ushort[,] FetchImageData16Bit() {
            if (BytesPerPixel != 2)
                throw new InvalidOperationException();

            // Nothing to fetch; if it's not set, it's not set.
            return null;
        }

        protected override void SetImageData16Bit(ushort[,] data) {
            var newWidth = data.GetLength(0);
            var newHeight = data.GetLength(1);

            var error = Validate16BitImageData(data, ImageDataSize, newWidth * newHeight * 2);
            if (error != null)
                throw new ArgumentException(error);

            PixelFormat = TexturePixelFormat.ABGR1555;
            Width  = newWidth;
            Height = newHeight;

            Invalidate(sendEvent: false);
            using (new ScopeGuard(() => _invalidateGuard++, () => _invalidateGuard--))
                _ = _textureDataBuffer.SetImageData16Bit(data);

            InvokeInvalidatedEvent();
        }
    }
}
