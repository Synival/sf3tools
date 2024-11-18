using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using SF3.Editors.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.Controls {
    public partial class SurfaceMapControl : UserControl {
        public SurfaceMapControl() {
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

        public void UpdateTextures(ushort[,] textureData, TextureChunkEditor[] textureChunks) {
            if (textureData == null || textureChunks == null) {
                UniqueImages = null;
                Images = null;
                Flags = null;
                if (FullImage != null) {
                    FullImage = null;
                    Invalidate();
                }
                return;
            }

            UniqueImages = new Dictionary<int, Image>();
            Images = new Image[textureData.GetLength(1), textureData.GetLength(0)];
            Flags = new byte[textureData.GetLength(1), textureData.GetLength(0)];

            for (int y = 0; y < textureData.GetLength(1); y++) {
                for (int x = 0; x < textureData.GetLength(0); x++) {
                    var key = textureData[x, y];
                    var textureId = textureData[x, y] & 0xFF;
                    var textureFlags = (byte) ((textureData[x, y] >> 8) & 0xFF);

                    if (textureId != 0xFF && !UniqueImages.ContainsKey(key)) {
                        var texture = textureChunks
                            .Select(a => a.TextureTable)
                            .SelectMany(a => a.Rows)
                            .FirstOrDefault(a => a.ID == textureId);

                        // Generate a new bitmap for this texture.
                        var image = texture?.CreateBitmap();
                        if (image != null) {
                            // Texture flag 0x80 will darken the texture.
                            if ((textureFlags & 0x80) != 0) {
                                if (image.PixelFormat == PixelFormat.Format16bppArgb1555) {
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
                            }

                            // Bits 0x03 determine rotation (before flipping).
                            var rotateFlag = textureFlags & 0x03;
                            if (rotateFlag == 1)
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            else if (rotateFlag == 2)
                                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            else if (rotateFlag == 3)
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone);

                            // Bits 0x30 determine flipping (after rotation).
                            bool flipHoriz = (textureFlags & 0x10) != 0;
                            bool flipVert  = (textureFlags & 0x20) != 0;
                            if (flipHoriz && flipVert)
                                image.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            else if (flipHoriz)
                                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            else if (flipVert)
                                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

#if DEBUG
                            // Anything we missed?
                            var unhandledBits = textureFlags & ~0xB3;
                            if (unhandledBits != 0x00) {
                                // Let's just let the debugger handle this.
                                try {
                                    throw new NotImplementedException("Unhandled battle map texture bits: " + unhandledBits.ToString("X2"));
                                }
                                catch { }
                            }
#endif

                            UniqueImages.Add(key, image);
                        }
                    }

                    Images[x, y] = UniqueImages.ContainsKey(key) ? UniqueImages[key] : null;
                    Flags[x, y]  = textureFlags;
                }
            }

            FullImage = new Bitmap(16 * 64, 16 * 64);
            using (var graphics = Graphics.FromImage(FullImage)) {
                for (int y = 0; y < Images.GetLength(1); y++) {
                    for (int x = 0; x < Images.GetLength(0); x++) {
                        var image = Images[x, y];
                        if (image != null)
                            graphics.DrawImage(image, x * 16, y * 16, 16, 16);
                    }
                }
            }

            Invalidate();
        }
    }
}
