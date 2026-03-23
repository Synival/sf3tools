namespace CommonLib.Imaging {
    public abstract class TextureDataStandard : TextureDataBase {
        public TextureDataStandard(int width, int height) {
            _width  = width;
            _height = height;
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

        protected void SetDimensionsInternal(int width, int height, bool invalidate) {
            _width  = width;
            _height = height;
            if (invalidate)
                Invalidate();
        }
    }
}
