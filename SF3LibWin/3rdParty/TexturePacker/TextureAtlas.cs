// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad
// Modified by Synival

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Types;
using static CommonLib.Types.CornerTypeConsts;

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

        public Vector2[] GetUVCoordinatesByTextureIDFrame(int id, int frame, int width, int height, TextureRotateType rotation, TextureFlipType flip, float borderUVWidth = 0.00f, float borderUVHeight = 0.00f) {
            var node = GetNodeByTextureIDFrame(id, frame);
            if (node == null)
                throw new ArgumentException(nameof(id) + ", " + nameof(frame));

            var widthf  = (float) width;
            var heightf = (float) height;

            // Perform calculations in texture coordinates, where (0, 0) is the top-left corner.
            var x1 = node.Rect.Left   / widthf  + CornerType.TopLeft.GetUVDirectionX() * borderUVWidth;
            var y1 = node.Rect.Top    / heightf + CornerType.TopLeft.GetUVDirectionY() * borderUVHeight;
            var x2 = node.Rect.Right  / widthf  + CornerType.BottomRight.GetUVDirectionX() * borderUVWidth;
            var y2 = node.Rect.Bottom / heightf + CornerType.BottomRight.GetUVDirectionY() * borderUVHeight;

            var tl = new Vector2(x1, y1);
            var tr = new Vector2(x2, y1);
            var br = new Vector2(x2, y2);
            var bl = new Vector2(x1, y2);

            // If the node was rotated clockwise, the UV coordinates must be rotated counter-clockwise to reverse it.
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

            // Convert coordinates to a grid so we can more easily flip them based on some (0 or 1) constants.
            Vector2[,] bitmapCoords = new Vector2[2, 2] {
                        /* y1, y2 */
                /* x1 */ { tl, bl },
                /* x2 */ { tr, br }
            };

            // Return the bitmap coordinates translated into UV coordinates (which may have some flipping).
            return [
                bitmapCoords[Corner1UVX, Corner1UVY],
                bitmapCoords[Corner2UVX, Corner2UVY],
                bitmapCoords[Corner3UVX, Corner3UVY],
                bitmapCoords[Corner4UVX, Corner4UVY],
            ];
        }

        public Bitmap CreateBitmap() {
            if (MaxX <= 0 || MaxY <= 0)
                return null;

            var dimensions = GetDimensions();
            Bitmap atlas = new Bitmap(dimensions.Width, dimensions.Height);
            DrawPackedNodes(atlas);
            return atlas;
        }

        public Rectangle GetDimensions() {
            int width = 0;
            int height = 0;
            GetDimensionsSub(_rootNode, ref width, ref height);
            return new Rectangle(0, 0, width, height);
        }

        private void GetDimensionsSub(TextureAtlasNode node, ref int width, ref int height) {
            if (node == null)
                return;
            if (node.Rect.Right > width)
                width = node.Rect.Right;
            if (node.Rect.Bottom > height)
                height = node.Rect.Bottom;
            GetDimensionsSub(node.Left, ref width, ref height);
            GetDimensionsSub(node.Right, ref width, ref height);
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

                    var imageData = node.Texture.BitmapDataARGB8888;

                    // Scan for transparent pixels. If there are any, turn the flag on.
                    if (!HasTransparency) {
                        for (int i = 3; i < imageData.Length; i += 4) {
                            if (imageData[i] != 255) {
                                HasTransparency = true;
                                break;
                            }
                        }
                    }

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
        public bool HasTransparency { get; private set; } = false;

        private readonly TextureAtlasNode _rootNode;
        private readonly Dictionary<(int, int), TextureAtlasNode> _nodeByTextureIDFrame = [];
    }
}