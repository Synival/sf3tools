using System.Windows.Forms;
using SF3.Win.Controls;
using SF3.Win.Extensions;

namespace SF3.Win.Views {
    public class TextureView : ControlView<TextureControl> {
        public TextureView(string name, float textureScale = 0) : base(name) {
            ImageScale = textureScale;
        }

        public TextureView(string name, ITexture texture, float textureScale = 0) : base(name) {
            _texture = texture;
            ImageScale = textureScale;
        }

        public override Control Create() {
            var rval = base.Create();

            if (ImageScale == 0)
                ImageScale = TextureControl.TextureScale;
            else
                TextureControl.TextureScale = ImageScale;

            TextureControl.TextureImage = _texture?.CreateBitmapARGB1555();
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            var old = TextureControl.TextureImage;
            TextureControl.TextureImage = null;
            TextureControl.TextureImage = old;
        }

        public TextureControl TextureControl => (TextureControl) Control;

        private ITexture _texture = null;
        public ITexture Image {
            get => _texture;
            set {
                if (value != _texture) {
                    _texture = value;
                    if (TextureControl != null)
                        TextureControl.TextureImage = value?.CreateBitmapARGB1555();
                }
            }
        }

        private float _imageScale = 0;
        public float ImageScale {
            get => _imageScale;
            set {
                if (value != _imageScale) {
                    _imageScale = value;
                    if (TextureControl != null)
                        TextureControl.TextureScale = value;
                }
            }
        }
    }
}
