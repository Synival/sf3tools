using SF3.Win.Extensions;

namespace SF3.Win.Views {
    public class TextureView : ImageView {
        public TextureView(string name, float? imageScale = null) : base(name, imageScale) {}

        public TextureView(string name, ITexture texture, float? imageScale = null) : base(name, texture?.CreateBitmapARGB1555(), imageScale) {
            _texture = texture;
        }

        private ITexture _texture = null;
        public ITexture Texture {
            get => _texture;
            set {
                if (value != _texture) {
                    _texture = value;
                    Image = value?.CreateBitmapARGB1555(AppState.RetrieveAppState().HighlightEndCodesInTextureView);
                }
            }
        }
    }
}
