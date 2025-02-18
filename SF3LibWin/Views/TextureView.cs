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
                ImageScale = Control.TextureScale;
            else
                Control.TextureScale = ImageScale;

            Control.TextureImage = _texture?.CreateBitmapARGB1555();
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            var old = Control.TextureImage;
            Control.TextureImage = null;
            Control.TextureImage = old;
        }

        private ITexture _texture = null;
        public ITexture Image {
            get => _texture;
            set {
                if (value != _texture) {
                    _texture = value;
                    if (Control != null)
                        Control.TextureImage = value?.CreateBitmapARGB1555();
                }
            }
        }

        private float _imageScale = 0;
        public float ImageScale {
            get => _imageScale;
            set {
                if (value != _imageScale) {
                    _imageScale = value;
                    if (Control != null)
                        Control.TextureScale = value;
                }
            }
        }
    }
}
