using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Win.Extensions;

namespace SF3.Win.Controls {
    public partial class SurfaceMapControl : UserControl {
        public const int WidthInTiles = 64;
        public const int HeightInTiles = 64;
        public const int TileResolution = 16;

        public SurfaceMapControl() {
            SuspendLayout();
            InitializeComponent();
            Size = MaximumSize = new Size(WidthInTiles  * TileResolution, HeightInTiles * TileResolution);
            ResumeLayout();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            if (FullImage != null) {
                var rect = e.ClipRectangle;
                e.Graphics.DrawImage(FullImage, rect.X, rect.Y, rect, GraphicsUnit.Pixel);
            }
            else {
                var rect = e.ClipRectangle;
                e.Graphics.FillRectangle(Brushes.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
            }

            if (_tileX.HasValue && _tileY.HasValue) {
                var rect = new Rectangle(_tileX.Value * TileResolution, _tileY.Value * TileResolution, TileResolution, TileResolution);
                if (rect.IntersectsWith(e.ClipRectangle))
                    e.Graphics.DrawRectangle(Pens.White, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }
        }

        public Dictionary<int, Image> UniqueImages { get; private set; } = null;
        public Image[,] Images { get; private set; } = null;
        public byte[,] Flags { get; private set; } = null;
        public Image FullImage { get; private set; } = null;

        public void UpdateTextures(ushort[,] textureData, MPD_FileTextureChunk[] textureChunks) {
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
                            .FirstOrDefault(a => a.ID == textureId)?.Texture;

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

                            // Indicate unidentified textures.
                            var expectedTag = new TagKey(textureFlags);
                            if (!texture.Tags.ContainsKey(expectedTag)) {
                                // NOTE: Graphics.FromImage() throws an OutOfMemoryException due to a bad GDI+ implementation,
                                // so we have to do it this way.
                                using var questionMark = new Bitmap(image.Width / 2, image.Height / 2);
                                using (var g = Graphics.FromImage(questionMark)) {
                                    g.Clear(Color.Black);
                                    g.DrawString("?", new Font(new FontFamily("Consolas"), (int) (questionMark.Width * 0.75)), Brushes.White, 0, 0);
                                    g.Flush();
                                }

                                var posX = image.Width  - questionMark.Width;
                                var posY = image.Height - questionMark.Height;
                                image.SafeDrawImage(questionMark, posX, posY);
                            }

                            UniqueImages.Add(key, image);
                        }
                    }

                    Images[x, y] = UniqueImages.ContainsKey(key) ? UniqueImages[key] : null;
                    Flags[x, y]  = textureFlags;
                }
            }

            FullImage = new Bitmap(TileResolution * WidthInTiles, TileResolution * HeightInTiles);
            using (var graphics = Graphics.FromImage(FullImage)) {
                for (int y = 0; y < Images.GetLength(1); y++) {
                    for (int x = 0; x < Images.GetLength(0); x++) {
                        var image = Images[x, y];
                        if (image != null)
                            graphics.DrawImage(image, x * TileResolution, y * TileResolution, TileResolution, TileResolution);
                    }
                }
            }

            Invalidate();
        }

        private int? _tileX = null;
        private int? _tileY = null;

        private void UpdateTilePosition(int? x, int? y) {
            // All invalid tile values should be -1, -1.
            if (x < 0 || y < 0 || x >= WidthInTiles || y >= HeightInTiles) {
                x = null;
                y = null;
            }

            // Early exit if no change is necessary.
            if (_tileX == x && _tileY == y)
                return;

            if (_tileX.HasValue && _tileY.HasValue)
                Invalidate(new Rectangle(_tileX.Value * TileResolution, _tileY.Value * TileResolution, TileResolution, TileResolution));

            _tileX = x;
            _tileY = y;

            if (_tileX.HasValue && _tileY.HasValue)
                Invalidate(new Rectangle(_tileX.Value * TileResolution, _tileY.Value * TileResolution, TileResolution, TileResolution));
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            UpdateTilePosition(null, null);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            UpdateTilePosition(e.Location.X / TileResolution, e.Location.Y / TileResolution);
        }
    }
}
