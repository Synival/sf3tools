using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.NamedValues;
using CommonLib.Utils;
using SF3.Models.Tables.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views {
    public class ColorTableView : ControlSpaceView {
        public ColorTableView(string name, ColorTable table, INameGetterContext nameGetterContext) : base(name) {
            Table       = table;
            TableView   = new TableView("Table", table, nameGetterContext);
            TextureView = new TextureView("Texture");
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            CreateChild(TableView, (c) => {
                var olv = (ObjectListView) c;
                olv.CellEditFinishing += (s, e) => {
                    if (!e.Cancel) {
                        e.Control.LostFocus += (s, e) => UpdateBitmap();
                    }
                };
            });

            CreateChild(TextureView, (c) => {
                var tc = (TextureControl) c;
                tc.Dock = DockStyle.Right;
                UpdateBitmap();
            }, false);

            return control;
        }

        private void UpdateBitmap() {
            var colorCount = Table.Length;
            var width = (int) Math.Ceiling(Math.Sqrt(colorCount));

            var nextPow = 1;
            while (width > nextPow)
                nextPow *= 2;
            width = nextPow;

            var height = (int) Math.Ceiling(colorCount / (float) width);

            PaletteBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            int y = 0, x = 0;
            using (var g = Graphics.FromImage(PaletteBitmap)) {
                g.Clear(Color.Transparent);
                for (var i = 0; i < colorCount; i++) {
                    var colorChannels = PixelConversion.ABGR1555toChannels(Table[i].ColorABGR1555);
                    colorChannels.a = 0xff;
                    var brush = new SolidBrush(Color.FromArgb(colorChannels.a, colorChannels.r, colorChannels.g, colorChannels.b));
                    g.FillRectangle(brush, x, y, 1, 1);

                    if (++x == width) {
                        x = 0;
                        ++y;
                    }
                }
            }

            var tc = (TextureControl) (TextureView?.Control);
            if (tc != null) {
                tc.TextureScale = (int) Math.Ceiling(128.0f / PaletteBitmap.Width);
                tc.TextureImage = PaletteBitmap;
            }
        }

        public readonly ColorTable Table = null;
        public readonly TableView TableView = null;
        public readonly TextureView TextureView = null;

        public Bitmap PaletteBitmap { get; private set; } = null;

    }
}
