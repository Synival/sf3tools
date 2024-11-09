using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF3.MPDEditor.Controls {
    public partial class TextureControl : UserControl {
        public TextureControl() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (Data == null)
                return;

            var brushes = new Brush[256];
            for (int i = 0; i < brushes.Length; i++)
                brushes[i] = new SolidBrush(Color.FromArgb(i, i, i));

            for (int x = 0; x < Data.GetLength(0); x++) {
                for (int y = 0; y < Data.GetLength(1); y++) {
                    ushort color = Data[x, y];
                    byte a = ((color & 0x8000) == 0) ? (byte) 0 : (byte) 255;
                    byte r = (byte) (((color >>  0) & 0x1F) * 8.25);
                    byte g = (byte) (((color >>  5) & 0x1F) * 8.25);
                    byte b = (byte) (((color >> 10) & 0x1F) * 8.25);

                    var brush = new SolidBrush(Color.FromArgb(a, r, g, b));

                    var rect = new RectangleF(x * 4, y * 4, 4, 4);
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
        }

        private ushort[,] _data = null;

        public ushort[,] Data {
            get => _data;
            set {
                if (_data != value) {
                    _data = value;
                    var newSize = new Size(_data.GetLength(0) * 4, _data.GetLength(1) * 4);
                    var sizeDiff = new Point(newSize.Width - this.Size.Width, newSize.Height - this.Size.Height);
                    this.Size = newSize;
                    this.Location = new Point(this.Location.X - sizeDiff.X, this.Location.Y - sizeDiff.Y);
                    Invalidate();
                }
            }
        }
    }
}
