using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.Utils;

namespace SF3.Win.Views {
    public class PaletteView : ImageView {
        public PaletteView(string name, float? imageScale = null) : base(name, imageScale) {
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;
            return Control;
        }

        public void SetColors(ushort[] colors) {
            if (colors == null) {
                PaletteBitmap = null;
                Image = null;
                return;
            }

            var (width, height) = ImageUtils.GetPaletteImageDimensions(colors.Length);

            PaletteBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            int y = 0, x = 0;
            using (var g = Graphics.FromImage(PaletteBitmap)) {
                g.Clear(Color.Transparent);
                for (var i = 0; i < colors.Length; i++) {
                    var colorChannels = PixelConversion.ABGR1555toChannels(colors[i]);
                    colorChannels.a = 0xff;
                    var brush = new SolidBrush(Color.FromArgb(colorChannels.a, colorChannels.r, colorChannels.g, colorChannels.b));
                    g.FillRectangle(brush, x, y, 1, 1);

                    if (++x == width) {
                        x = 0;
                        ++y;
                    }
                }
            }

            ImageScale = (int) Math.Ceiling(128.0f / PaletteBitmap.Width);
            Image = PaletteBitmap;
        }

        protected override Action GetImportImageAction() => ImportImageDialog;

        public override void ImportImage(string filename) {
            using (var image = Image.FromFile(filename)) {
                var colors = image.Get1DDataABGR1555();
                for (int i = 0; i < colors.Length; i++)
                    colors[i] &= 0x7FFF;

                ImportPalette?.Invoke(this, colors);
            }
        }

        public Bitmap PaletteBitmap { get; private set; } = null;

        public delegate void ImportPaletteEventHandler(object source, ushort[] colors);
        public event ImportPaletteEventHandler ImportPalette;
    }
}
