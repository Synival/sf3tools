using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            // Set _image with the setter so we get the side-effects.
            var controlImage = _image;
            _image = null;
            Image = controlImage;

            return rval;
        }

        public void ExportTextureDialog() {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Images|*.png;*.bmp;*.gif;*.jpg;*.jpeg;*.tiff";

            if (dialog.ShowDialog() == DialogResult.OK) {
                string filename = dialog.FileName;
                ImageFormat format = ImageFormat.Png;
                switch (Path.GetExtension(dialog.FileName).ToLower())
                {
                    case ".bmp":               format = ImageFormat.Bmp; break;
                    case ".gif":               format = ImageFormat.Gif; break;
                    case ".jpg": case ".jpeg": format = ImageFormat.Jpeg; break;
                    case ".png":               format = ImageFormat.Png; break;
                    case ".tif": case ".tiff": format = ImageFormat.Tiff; break;
                }

                SaveImage(dialog.FileName, format);
            }
        }

        public virtual void SaveImage(string filename, ImageFormat format)
            => Image.Save(filename, format);

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
                    if (Control != null) {
                        Control.Image = value;
                        Control.ExportAction = ExportTextureDialog;
                    }
                    else
                        Control.ExportAction = null;
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
