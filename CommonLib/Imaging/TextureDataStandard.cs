using CommonLib.Types;

namespace CommonLib.Imaging {
    public abstract class TextureDataStandard : TextureDataBase {
        public TextureDataStandard(
            int width,
            int height,
            TexturePixelFormat pixelFormat,
            Palette palette,
            bool zeroIsTransparent,
            bool canSetImage,
            ITextureDataSource dataSource
        ) {
            _width             = width;
            _height            = height;
            _pixelFormat       = pixelFormat;
            _palette           = palette;
            _zeroIsTransparent = zeroIsTransparent;
            CanSetImage        = canSetImage;
            DataSource        = dataSource;
        }

        protected override byte[,] FetchImageData8Bit() => DataSource.FetchImageData8Bit(this);
        protected override ushort[,] FetchImageData16Bit() => DataSource.FetchImageData16Bit(this);

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

        protected ITextureDataSource DataSource { get; }
    }
}
