using CommonLib.Types;

namespace CommonLib.Imaging {
    public abstract class TextureDataStandard : TextureDataBase {
        public TextureDataStandard(
            int width,
            int height,
            TexturePixelFormat pixelFormat,
            Palette palette,
            bool zeroIsTransparent,
            bool canSetImage
        ) {
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

        protected void SetPixelFormatInternal(TexturePixelFormat pixelFormat, bool invalidate) {
            _pixelFormat = pixelFormat;
            if (invalidate)
                Invalidate();
        }

        protected void SetDimensionsInternal(int width, int height, bool invalidate) {
            _width  = width;
            _height = height;
            if (invalidate)
                Invalidate();
        }

        protected void SetPaletteInternal(Palette palette, bool invalidate) {
            if (_palette == palette)
                return;
            _palette = palette;

            if (invalidate)
                Invalidate();
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
            set => SetPaletteInternal(value, true);
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
    }
}
