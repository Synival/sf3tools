// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad
// Modified by Synival

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using CommonLib.Utils;
using OpenTK.Mathematics;
using SF3.Types;
using SF3.Win.Extensions;
using SF3.Win.ThirdParty.TexturePacker.Extensions;

namespace SF3.Win.ThirdParty.TexturePacker {
    public class TextureAtlas : IDisposable {
        public TextureAtlas(int maxX, int maxY, int padding, bool tryRotate) {
            MaxX      = maxX;
            MaxY      = maxY;
            Padding   = padding;
            TryRotate = tryRotate;
            _rootNode = new TextureAtlasNode(new Rectangle(Padding, Padding, MaxX, MaxY), Padding, TryRotate);
        }

        public TextureAtlas(IEnumerable<ITexture> textures, int padding, bool tryRotate) {
            if (textures == null)
                throw new ArgumentNullException(nameof(textures));

            if (!textures.Any()) {
                MaxX = 0;
                MaxY = 0;
            }
            else {
                var textureArray = textures.OrderByDescending(x => x.Height).ToArray();
                textures = textureArray;

                var totalArea = textures.Sum(x => (x.Width + padding) * (x.Height + padding));
                var minWidth  = textures.Max(x => x.Width) + padding;

                MaxX = Math.Max(minWidth, (int) Math.Sqrt(totalArea));
                MaxY = textures.Sum(x => x.Height + padding);
            }

            Padding   = padding;
            TryRotate = tryRotate;
            _rootNode = new TextureAtlasNode(new Rectangle(Padding, Padding, MaxX, MaxY), Padding, TryRotate);

            foreach (var tex in textures)
                _ = Insert(tex);
        }

        public TextureAtlasNode Insert(ITexture tex) {
            if (_nodeByTextureIDFrame.ContainsKey((tex.ID, tex.Frame)))
                throw new ArgumentException(nameof(tex));

            var node = _rootNode.Insert(tex);
            if (node == null)
                throw new InvalidOperationException("Couldn't fit texture");

            _nodeByTextureIDFrame[(tex.ID, tex.Frame)] = node;
            return node;
        }

        public TextureAtlasNode GetNodeByTextureIDFrame(int id, int frame) {
            var key = (id, frame);
            return _nodeByTextureIDFrame.ContainsKey(key) ? _nodeByTextureIDFrame[key] : null;
        }

        public Vector2[] GetUVCoordinatesByTextureIDFrame(int id, int frame, int width, int height, TextureRotateType rotation, TextureFlipType flip) {
            var node = GetNodeByTextureIDFrame(id, frame);
            if (node == null)
                throw new ArgumentException(nameof(id) + ", " + nameof(frame));

            var widthf  = (float) width;
            var heightf = (float) height;

            var x1 = node.Rect.Left   / widthf;
            var y1 = node.Rect.Top    / heightf;
            var x2 = node.Rect.Right  / widthf;
            var y2 = node.Rect.Bottom / heightf;

            var tl = new Vector2(x1, y2);
            var tr = new Vector2(x2, y2);
            var br = new Vector2(x2, y1);
            var bl = new Vector2(x1, y1);

            // Flip UV coordinates for rotated textures in the TextureAtlas.
            if (node.Rotated)
                (tl, tr, br, bl) = (tr, br, bl, tl);

            // Rotation is applied first...
            if (rotation == TextureRotateType.Rotate270CW)
                (tl, tr, br, bl) = (tr, br, bl, tl);
            else if (rotation == TextureRotateType.Rotate180)
                (tl, tr, br, bl) = (br, bl, tl, tr);
            else if (rotation == TextureRotateType.Rotate90CW)
                (tl, tr, br, bl) = (bl, tl, tr, br);

            // ... then flipping.
            if (flip == TextureFlipType.Horizontal)
                (tl, tr, br, bl) = (tr, tl, bl, br);
            else if (flip == TextureFlipType.Vertical)
                (tl, tr, br, bl) = (bl, br, tr, tl);
            else if (flip == TextureFlipType.Both)
                (tl, tr, br, bl) = (br, bl, tl, tr);

            return [bl, br, tr, tl];
        }

        public Bitmap CreateBitmap() {
            if (MaxX <= 0 || MaxY <= 0)
                return null;

            // TODO: just get the correct dimensinos for this thing...
            Bitmap atlas = new Bitmap(MaxX, MaxY);
            DrawPackedNodes(atlas);
            return atlas.Trim(true, true) ?? new Bitmap(16, 16, PixelFormat.Format32bppArgb);
        }

        public void DrawPackedNodes(Bitmap atlas) {
            DrawPackedNodesSub(atlas, _rootNode);
        }

        [DllImport("msvcrt.dll",  SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        private void DrawPackedNodesSub(Bitmap atlas, TextureAtlasNode node) {
            if (node == null)
                return;

            if (node.Texture != null) {
                void CopyImage() {
                    // Should never happen, but in case it does, fail instead of crashing really hard due to memcpy().
                    if (atlas.PixelFormat != PixelFormat.Format32bppArgb)
                        throw new InvalidOperationException();

                    // TODO: support palettes!
                    if (node.Texture.BytesPerPixel == 1)
                        return;

                    // Get image data and convert to the format we need for the bitmap buffer.
                    byte[] imageData = null;
                    if (node.Texture.BytesPerPixel == 2) {
                        var imageData1 = node.Texture.CreateImageData();
                        // TODO: WHAT IS THIS CONVERSION???
                        imageData = PixelConversion.ImageDataToSomething(node.Texture.CreateImageData());
                    }
                    else if (node.Texture.BytesPerPixel == 4)
                        imageData = node.Texture.CreateImageData();

                    // Rotate the byte array clockwise if 'node.Rotated' is on.
                    if (node.Rotated) {
                        var newImageData = new byte[imageData.Length];
                        var posIn = 0;
                        for (int inY = 0; inY < node.Texture.Height; inY++) {
                            var outX = node.Rect.Width - inY - 1;
                            for (int inX = 0; inX < node.Texture.Width; inX++) {
                                var outY = inX;
                                int posOut = (outY * node.Rect.Width + outX) * 4;
                                for (int c = 0; c < 4; c++)
                                    newImageData[posOut++] = imageData[posIn++];
                            }
                        }
                        imageData = newImageData;
                    }

                    // Write to the texture atlas!
                    var bitmapToData = atlas.LockBits(new Rectangle(0, 0, atlas.Width, atlas.Height), ImageLockMode.WriteOnly, atlas.PixelFormat);

                    // Determine the stride for the output data (the amount of bytes to move per row).
                    const int bpp = 4;
                    var strideTo = Math.Abs(bitmapToData.Stride);
                    if (strideTo != bpp * bitmapToData.Width)
                        throw new InvalidOperationException();

                    // Determine the stride for the input data.
                    var strideFrom = node.Rect.Width * 4;

                    // Start writing, row by row.
                    var posTo = strideTo * node.Rect.Top + (node.Rect.Left * bpp);
                    unsafe {
                        fixed (byte* imageDataPtr = imageData) {
                            nint posFrom = 0;
                            for (var row = 0; row < node.Rect.Height; row++) {
                                _ = memcpy(bitmapToData.Scan0 + posTo, (nint) imageDataPtr + posFrom, strideFrom);
                                posTo += strideTo;
                                posFrom += strideFrom;
                            }
                        }
                    }

                    atlas.UnlockBits(bitmapToData);
                }
                CopyImage();
            }

            DrawPackedNodesSub(atlas, node.Left);
            DrawPackedNodesSub(atlas, node.Right);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing)
                _rootNode.Dispose();

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextureAtlas() {
            Dispose(false);
        }

        public int MaxX { get; }
        public int MaxY { get; }
        public int Padding { get; }
        public bool TryRotate { get; }

        private readonly TextureAtlasNode _rootNode;
        private readonly Dictionary<(int, int), TextureAtlasNode> _nodeByTextureIDFrame = [];
    }
}