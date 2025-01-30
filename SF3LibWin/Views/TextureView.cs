using System.Drawing;
using System.Windows.Forms;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class TextureView : ControlView<TextureControl> {
        public TextureView(string name, int textureScale = 0) : base(name) {
            ImageScale = textureScale;
        }

        public TextureView(string name, Image image, int textureScale = 0) : base(name) {
            _image = image;
            ImageScale = textureScale;
        }

        public override Control Create() {
            var rval = base.Create();

            if (ImageScale == 0)
                ImageScale = TextureControl.TextureScale;
            else
                TextureControl.TextureScale = ImageScale;

            TextureControl.TextureImage = _image;
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

        private Image _image = null;
        public Image Image {
            get => _image;
            set {
                if (value != _image) {
                    _image = value;
                    if (TextureControl != null)
                        TextureControl.TextureImage = value;
                }
            }
        }

        private int _imageScale = 0;
        public int ImageScale {
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
