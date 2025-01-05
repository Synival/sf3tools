using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public partial class TextureControl : UserControl {
        public TextureControl() {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode    = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.None;

            if (_textureImage != null) {
                var textureScale = TextureScale;
                var w = _textureImage.Width * textureScale;
                var h = _textureImage.Height * textureScale;
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(1, 1, w + 1, h + 1));
                e.Graphics.DrawImage(TextureImage, 1, 1, w, h);
            }
        }

        private Image _textureImage = null;

        public int TextureScale { get; set; } = 4;

        public Image TextureImage {
            get => _textureImage;
            set {
                if (_textureImage != value) {
                    _textureImage = value;
                    if (_textureImage != null) {
                        var textureScale = TextureScale;
                        var newSize = new Size(value.Width * textureScale + 2, value.Height * textureScale + 2);
                        var sizeDiff = new Point(newSize.Width - this.Size.Width, newSize.Height - this.Size.Height);
                        this.Size = newSize;

                        var widthMagnitude  = Anchor.HasFlag(AnchorStyles.Right)  ? 1.00 : Anchor.HasFlag(AnchorStyles.Left) ? 0.00 : 0.50;
                        var heightMagnitude = Anchor.HasFlag(AnchorStyles.Bottom) ? 1.00 : Anchor.HasFlag(AnchorStyles.Top)  ? 0.00 : 0.50;

                        this.Location = new Point(
                            (int) (this.Location.X - sizeDiff.X * widthMagnitude),
                            (int) (this.Location.Y - sizeDiff.Y * heightMagnitude)
                        );
                    }
                    else
                        this.Size = new Size(64, 64);

                    Refresh();
                }
            }
        }
    }
}
