using System.Drawing;
using System.Windows.Forms;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class ImageView : ControlView<TextureControl> {
        public ImageView(string name, float? textureScale = null) : base(name) {
            ImageScale = textureScale ?? 0;
        }

        public ImageView(string name, Image image, float? textureScale = null) : base(name) {
            _image = image;
            ImageScale = textureScale ?? 0;
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
