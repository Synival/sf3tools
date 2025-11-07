using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using CommonLib.Imaging;

namespace SF3.Win.Views {
    public class PaletteView : ImageView {
        public PaletteView(string name, float? imageScale = null) : base(name, imageScale) {
        }

        public void SetColors(ushort[] colors) {
            _colors = colors;

            if (colors == null) {
                PaletteBitmap = null;
                Image = null;
                if (Control != null) {
                    Control.ExportAction = null;
                    Control.ImportAction = null;
                }
                return;
            }

            var width = (int) Math.Ceiling(Math.Sqrt(colors.Length));

            var nextPow = 1;
            while (width > nextPow)
                nextPow *= 2;
            width = nextPow;

            var height = (int) Math.Ceiling(colors.Length / (float) width);

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

            if (Control != null) {
                Control.ImageScale = (int) Math.Ceiling(128.0f / PaletteBitmap.Width);
                Control.Image = PaletteBitmap;
                Control.ExportAction = ExportPaletteDialog;
                Control.ImportAction = ImportPaletteDialog;
            }
        }

        void ExportPaletteDialog() {
            if (_colors == null)
                return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Palettes|*.sf3pal";
            if (dialog.ShowDialog() == DialogResult.OK) {
                var content = JsonSerializer.Serialize(_colors, new JsonSerializerOptions() { WriteIndented = true });
                File.WriteAllText(dialog.FileName, content);
            }
        }

        void ImportPaletteDialog() {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Palettes|*.sf3pal";
            if (dialog.ShowDialog() == DialogResult.OK) {
                var text = File.ReadAllText(dialog.FileName);
                var colors = JsonSerializer.Deserialize<ushort[]>(text);
                ImportPalette?.Invoke(this, colors);
            }
        }

        private ushort[] _colors = null;
        public Bitmap PaletteBitmap { get; private set; } = null;

        public delegate void ImportPaletteEventHandler(object source, ushort[] colors);
        public event ImportPaletteEventHandler ImportPalette;
    }
}
