using System.Drawing;
using System.Windows.Forms;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class ImageView : ControlView<ImagePanel> {
        public ImageView(string name, float? imageScale = null) : base(name) {
            ImageScale = imageScale ?? 0;
        }

        public ImageView(string name, Image image, float? imageScale = null) : base(name) {
            _image = image;
            ImageScale = imageScale ?? 0;
        }

        public override Control Create() {
            var rval = base.Create();

            if (ImageScale == 0)
                ImageScale = Control.ImageScale;
            else
                Control.ImageScale = ImageScale;

            Control.Image = _image;
            return rval;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            var old = Control.Image;
            Control.Image = null;
            Control.Image = old;
        }

        private Image _image = null;
        public Image Image {
            get => _image;
            set {
                if (value != _image) {
                    _image = value;
                    if (Control != null)
                        Control.Image = value;
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
                        Control.ImageScale = value;
                }
            }
        }
    }
}
