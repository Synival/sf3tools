using System.Drawing;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public partial class ImageControl : UserControl {
        public ImageControl() {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode  = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode    = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.None;

            if (_image != null) {
                var imageScale = ImageScale;
                var w = (int) (_image.Width * imageScale);
                var h = (int) (_image.Height * imageScale);
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(1, 1, w + 1, h + 1));
                e.Graphics.DrawImage(Image, 1, 1, w, h);
            }
        }

        public float ImageScale { get; set; } = 4;

        private Image _image = null;
        public Image Image {
            get => _image;
            set {
                if (_image != value) {
                    _image = value;
                    if (_image != null) {
                        var imageScale = ImageScale;
                        var newSize = new Size((int) (value.Width * imageScale) + 2, (int) (value.Height * imageScale) + 2);
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
