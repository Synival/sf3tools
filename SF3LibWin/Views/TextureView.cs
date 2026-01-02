using System;
using System.Drawing;
using System.Drawing.Imaging;
using CommonLib.Logging;
using CommonLib.Win.Utils;
using CommonLib.Extensions;
using SF3.Win.Extensions;
using SF3.Images;

namespace SF3.Win.Views {
    public class TextureView : ImageView {
        public TextureView(string name, float? imageScale = null) : base(name, imageScale) {}

        public TextureView(string name, ITextureData texture, float? imageScale = null) : base(name, texture?.CreateBitmapARGB1555(), imageScale) {
            Texture = texture;
        }

        public override void ExportImage(string filename, ImageFormat format)
            => _texture?.CreateBitmap()?.Save(filename, format);

        public virtual void ReloadImage()
            => SetImageFromTexture();

        private void SetImageFromTexture()
            => Image = Texture?.CreateBitmap(AppState.RetrieveAppState().HighlightEndCodesInTextureView);

        private ITextureData _texture = null;
        public ITextureData Texture {
            get => _texture;
            set {
                if (value != _texture) {
                    _texture = value;
                    SetImageFromTexture();
                }
            }
        }

        protected override void PreImageSet() {
            if (Control != null)
                Control.ZeroIsTransparent = _texture?.ZeroIsTransparent ?? false;
        }

        protected override Action GetImportImageAction() {
            var canImport = (_texture != null) && (_texture.CanSetImageData8Bit || _texture.CanSetImageData16Bit);
            return canImport ? ImportImageDialog : null;
        }

        protected override void OnImportImage(Image image, string filename) {
            if (image == null)
                return;

            var canReplaceTexture8Bit  = _texture.CanSetImageData8Bit;
            var canReplaceTexture16Bit = _texture.CanSetImageData16Bit;

            if (image.PixelFormat == PixelFormat.Format8bppIndexed && canReplaceTexture8Bit) {
                var bitmap  = image.CreateIndexedBitmap();
                var data    = bitmap.Get2DDataIndexed();
                var palette = bitmap.GetPalette();

                try {
                    _texture.SetImageData8Bit(data, palette);
                }
                catch (Exception e) {
                    Logger.LogException(e);
                    MessageUtils.ErrorMessage(e.Message);
                    return;
                }
            }
            else if (canReplaceTexture16Bit) {
                var data = image.Get2DDataABGR1555();

                try {
                    _texture.ImageData16Bit = data;
                }
                catch (Exception e) {
                    Logger.LogException(e);
                    MessageUtils.ErrorMessage(e.Message);
                    return;
                }
            }
            else {
                if (canReplaceTexture16Bit)
                    MessageUtils.ErrorMessage("Image is not compatible with 16-bit ABGR format");
                else if (canReplaceTexture8Bit)
                    MessageUtils.ErrorMessage("Image must be in 8-bit indexed format");
                else
                    MessageUtils.ErrorMessage("Image format is invalid");
                return;
            }

            Image = _texture?.CreateBitmap(AppState.RetrieveAppState().HighlightEndCodesInTextureView);
        }
    }
}
