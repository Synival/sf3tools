namespace CommonLib.Imaging {
    public abstract class TextureDataStandard : TextureDataBase {
        public TextureDataStandard(int width, int height, bool zeroIsTransparent) {
            _width  = width;
            _height = height;
            _zeroIsTransparent = zeroIsTransparent;
        }

        protected void SetDimensionsInternal(int width, int height, bool invalidate) {
            _width  = width;
            _height = height;
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
    }
}
