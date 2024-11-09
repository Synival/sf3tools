using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.MPD.TextureChunk;
using SF3.MPDEditor.Extensions;

namespace SF3.MPDEditor.Controls {
    public partial class BattleMapControl : UserControl {
        public BattleMapControl() {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (FullImage != null)
                e.Graphics.DrawImage(FullImage, 0, 0);
        }

        public Dictionary<int, Image> UniqueImages { get; private set; } = null;
        public Image[,] Images { get; private set; } = null;
        public byte[,] Flags { get; private set; } = null;
        public Image FullImage { get; private set; } = null;

        public void UpdateTextures(ushort[,] textureData, TextureChunk[] textureChunks) {
            UniqueImages = new Dictionary<int, Image>();
            Images = new Image[textureData.GetLength(1), textureData.GetLength(0)];
            Flags = new byte[textureData.GetLength(1), textureData.GetLength(0)];

            UniqueImages.Add(0xFF, null);

            for (int y = 0; y < textureData.GetLength(1); y++) {
                for (int x = 0; x < textureData.GetLength(0); x++) {
                    var textureId = textureData[x, y] & 0xFF;
                    if (!UniqueImages.ContainsKey(textureId)) {
                        var texture = textureChunks
                            .Select(a => a.TextureTable)
                            .SelectMany(a => a.Rows)
                            .FirstOrDefault(a => a.ID == textureId);

                        var image = texture?.GetImage();
                        UniqueImages.Add(textureId, image);
                    }

                    Images[x, y] = UniqueImages[textureId];
                    Flags[x, y] = (byte) ((textureData[x, y] >> 8) & 0xFF);
                }
            }

            FullImage = new Bitmap(16 * 64, 16 * 64);
            using (var graphics = Graphics.FromImage(FullImage)) {
                for (int y = 0; y < Images.GetLength(1); y++) {
                    for (int x = 0; x < Images.GetLength(0); x++) {
                        var originalImage = Images[x, y] as Bitmap;
                        if (originalImage != null) {

                            var image = originalImage.Clone(new Rectangle(0, 0, originalImage.Width, originalImage.Height), PixelFormat.Format16bppArgb1555);

                            // TODO: WIP code to shade the image in case normals are actually working!
                            if ((Flags[x, y] & 0x80) != 0) {
                                var bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
                                unsafe {
                                    ushort* ptr = (ushort*) bmpData.Scan0;
                                    var size = image.Width * image.Height;
                                    for (int i = 0; i < size; i++) {
                                        var val = *ptr;
                                        var channel1 = (val >>  0) & 0x1F;
                                        var channel2 = (val >>  5) & 0x1F;
                                        var channel3 = (val >> 10) & 0x1F;
                                        var newVal = (ushort) ((val & 0x8000) +
                                            ((channel1 * 3 / 4) <<  0) +
                                            ((channel2 * 3 / 4) <<  5) +
                                            ((channel3 * 3 / 4) << 10));
                                        *ptr = newVal;
                                        ptr++;
                                    }
                                }
                                image.UnlockBits(bmpData);
                            }

                            if ((Flags[x, y] & 0x03) == 1)
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            else if ((Flags[x, y] & 0x03) == 2)
                                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            else if ((Flags[x, y] & 0x03) == 3)
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone);

                            bool flipHoriz = (Flags[x, y] & 0x10) != 0;
                            bool flipVert  = (Flags[x, y] & 0x20) != 0;

                            if (flipHoriz && flipVert)
                                image.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            else if (flipHoriz)
                                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            else if (flipVert)
                                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                            graphics.DrawImage(image, x * 16, y * 16, 16, 16);
                        }
                    }
                }
            }

            Invalidate();
        }
    }
}
