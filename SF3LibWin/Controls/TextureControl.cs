using System;
using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public partial class TextureControl : UserControl {
        public TextureControl() {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode    = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            e.Graphics.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.None;

            if (_textureImage != null)
                e.Graphics.DrawImage(TextureImage, 0, 0, _textureImage.Width * 4, _textureImage.Height * 4);
        }

        private Image _textureImage = null;

        public Image TextureImage {
            get => _textureImage;
            set {
                if (_textureImage != value) {
                    _textureImage = value;
                    if (_textureImage != null) {
                        var newSize = new Size(value.Width * 4, value.Height * 4);
                        var sizeDiff = new Point(newSize.Width - this.Size.Width, newSize.Height - this.Size.Height);

                        // Forcing the size to 0 clears the render buffer.
                        this.Size = new Size(0, 0);
                        this.Size = newSize;

                        var widthMagnitude  = Anchor.HasFlag(AnchorStyles.Right)  ? 1.00 : Anchor.HasFlag(AnchorStyles.Left) ? 0.00 : 0.50;
                        var heightMagnitude = Anchor.HasFlag(AnchorStyles.Bottom) ? 1.00 : Anchor.HasFlag(AnchorStyles.Top)  ? 0.00 : 0.50;

                        this.Location = new Point(
                            (int) (this.Location.X - sizeDiff.X * widthMagnitude),
                            (int) (this.Location.Y - sizeDiff.Y * heightMagnitude)
                        );
                    }
                    else {
                        this.Size = new Size(0, 0);
                        this.Size = new Size(64, 64);
                    }
                    Invalidate();
                }
            }
        }
    }
}
