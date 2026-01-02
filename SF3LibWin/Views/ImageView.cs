using System;
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
            Image = image;
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

            PreImageSet();
            Image = controlImage;
            OnImageSet();

            return rval;
        }

        protected virtual void PreImageSet() {}
        protected virtual void OnImageSet() {}

        public void ExportImageDialog() {
            if (Image == null)
                throw new Exception("No image to expot");

            var dialog = new SaveFileDialog();
            dialog.Filter = "Images|*.png;*.bmp;*.gif;*.jpg;*.jpeg;*.tiff";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            ExportImage(dialog.FileName, GetImageFormatFromFilename(dialog.FileName));
        }

        public void ImportImageDialog() {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.png;*.bmp;*.gif;*.jpg;*.jpeg;*.tiff";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            ImportImage(dialog.FileName);
        }

        private ImageFormat GetImageFormatFromFilename(string filename) {
            switch (Path.GetExtension(filename).ToLower())
            {
                case ".bmp":               return ImageFormat.Bmp;
                case ".gif":               return ImageFormat.Gif;
                case ".jpg": case ".jpeg": return ImageFormat.Jpeg;
                case ".png":               return ImageFormat.Png;
                case ".tif": case ".tiff": return ImageFormat.Tiff;
                default:                   return ImageFormat.Png;
            }
        }

        public virtual void ImportImage(string filename) {
            using (var image = Image.FromFile(filename))
                OnImportImage(image, filename);
        }

        public virtual void ExportImage(string filename, ImageFormat format)
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
                    PreImageSet();
                    if (Control != null) {
                        Control.Image = value;
                        Control.ImportAction = GetImportImageAction();
                        Control.ExportAction = GetExportImageAction();
                    }
                    OnImageSet();
                }
            }
        }

        protected virtual Action GetImportImageAction() => null;
        protected virtual void OnImportImage(Image image, string filename) => throw new NotImplementedException();

        protected virtual Action GetExportImageAction()
            => (_image == null) ? null : ExportImageDialog;

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
