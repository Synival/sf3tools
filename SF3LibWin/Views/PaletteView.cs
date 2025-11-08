using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using CommonLib.Extensions;
using CommonLib.Imaging;

namespace SF3.Win.Views {
    public class PaletteView : ImageView {
        public PaletteView(string name, float? imageScale = null) : base(name, imageScale) {
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            Control.ImportAction = ImportImageDialog;
            return Control;
        }

        public void SetColors(ushort[] colors) {
            _colors = colors;

            if (colors == null) {
                PaletteBitmap = null;
                Image = null;
                return;
            }

            var (width, height) = GetImageDimensions(colors.Length);

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

        /// <summary>
        /// Returns the width and height of an image to be used for a color palette based on the number of colors in the palette.
        /// The image width is the square root of the color count rounded down (must be at least 1) and the height is the count
        /// necessary to meet or exceed the color count.
        /// </summary>
        /// <param name="colorCount">The number of colors in the palette for which an image will be generated.</param>
        /// <returns>A tuple with width and height.</returns>
        public (int Width, int Height) GetImageDimensions(int colorCount) {
            var width = (int) Math.Ceiling(Math.Sqrt(colorCount));

            var nextPow = 1;
            while (width > nextPow)
                nextPow *= 2;
            width = nextPow;

            var height = (int) Math.Ceiling(colorCount / (float) width);

            return (width, height);
        }

        protected override void OnLoadImage(Image image, string filename) {
            base.OnLoadImage(image, filename);
            var colors = image.GetDataABGR1555();
            for (int i = 0; i < colors.Length; i++)
                colors[i] &= 0x7FFF;

            ImportPalette?.Invoke(this, colors);
        }

        private ushort[] _colors = null;
        public Bitmap PaletteBitmap { get; private set; } = null;

        public delegate void ImportPaletteEventHandler(object source, ushort[] colors);
        public event ImportPaletteEventHandler ImportPalette;
    }
}
