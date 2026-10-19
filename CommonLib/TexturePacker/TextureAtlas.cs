// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad
// Modified by Synival

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using CommonLib.Imaging;
using CommonLib.Types;
using static CommonLib.Types.CornerTypeConsts;
using static CommonLib.Utils.MemoryUtils;

namespace CommonLib.TexturePacker {
    public class TextureAtlas : IDisposable {
        public TextureAtlas(ITexturePackerStrategyImpl strategy) {
            _strategyImpl = strategy;
            PopulateTextureDictionarySub(strategy.RootNode);
        }

        private void PopulateTextureDictionarySub(ITexturePackerNode node) {
            var tex = node.Texture;
            if (tex != null)
                _nodeByTextureIDFrame[(tex.TextureID, tex.Frame)] = node;
            foreach (var childNode in node.Children)
                PopulateTextureDictionarySub(childNode);
        }

        public ITexturePackerNode Insert(IAnimatedTextureFrame tex) {
            if (_nodeByTextureIDFrame.ContainsKey((tex.TextureID, tex.Frame)))
                throw new ArgumentException(nameof(tex));

            var node = _strategyImpl.Add(tex);
            if (node == null)
                throw new InvalidOperationException("Couldn't fit texture");

            _nodeByTextureIDFrame[(tex.TextureID, tex.Frame)] = node;
            return node;
        }

        public ITexturePackerNode[] GetAllNodes()
            => _nodeByTextureIDFrame.Values.ToArray();

        public ITexturePackerNode GetNodeByTextureIDFrame(int id, int frame) {
            var key = (id, frame);
            return _nodeByTextureIDFrame.ContainsKey(key) ? _nodeByTextureIDFrame[key] : null;
        }

        public Vector2[] GetUVCoordinatesByTextureIDFrame(int id, int frame, int width, int height, TextureRotateType rotation, TextureFlipType flip, float pixelBorder = 0.0f) {
            var node = GetNodeByTextureIDFrame(id, frame);
            if (node == null)
                throw new ArgumentException(nameof(id) + ", " + nameof(frame));

            var widthf  = (float) width;
            var heightf = (float) height;

            // Perform calculations in texture coordinates, where (0, 0) is the top-left corner.
            var x1 = (node.InnerRect.Left   + CornerType.TopLeft.GetUVDirectionX()     * pixelBorder) / widthf;
            var y1 = (node.InnerRect.Top    + CornerType.TopLeft.GetUVDirectionY()     * pixelBorder) / heightf;
            var x2 = (node.InnerRect.Right  + CornerType.BottomRight.GetUVDirectionX() * pixelBorder) / widthf;
            var y2 = (node.InnerRect.Bottom + CornerType.BottomRight.GetUVDirectionY() * pixelBorder) / heightf;

            var tl = new Vector2(x1, y1);
            var tr = new Vector2(x2, y1);
            var br = new Vector2(x2, y2);
            var bl = new Vector2(x1, y2);

            // If the node was rotated, the UV coordinates must be rotated in reverse order here.
            var cwRotateCount = node.Rotation.GetCWRotationCount();
            for (int i = 0; i < cwRotateCount; i++)
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
            var bitmapCoords = new Vector2[2, 2] {
                        /* y1, y2 */
                /* x1 */ { tl, bl },
                /* x2 */ { tr, br }
            };

            // Return the bitmap coordinates translated into UV coordinates (which may have some flipping).
            return new Vector2[] {
                bitmapCoords[Corner1UVX, Corner1UVY],
                bitmapCoords[Corner2UVX, Corner2UVY],
                bitmapCoords[Corner3UVX, Corner3UVY],
                bitmapCoords[Corner4UVX, Corner4UVY],
            };
        }

        public enum DimensionsMode {
            Compact,
            CompactEvenOnly,
            PowersOfTwo
        }

        public Bitmap CreateBitmap(bool onlyTextures = false, DimensionsMode dimensionsMode = DimensionsMode.Compact) {
            var dimensions = GetDimensions(onlyTextures, dimensionsMode);
            if (dimensions.Width == 0 || dimensions.Height == 0)
                return null;

            var atlas = new Bitmap(dimensions.Width, dimensions.Height);
            DrawPackedNodes(atlas);
            return atlas;
        }

        public Rectangle GetDimensions(bool onlyTextures = false, DimensionsMode dimensionsMode = DimensionsMode.Compact) {
            var width = 0;
            var height = 0;
            GetDimensionsSub(_strategyImpl.RootNode, ref width, ref height, onlyTextures);

            switch (dimensionsMode) {
                case DimensionsMode.Compact:
                    // Nothing to do here.
                    break;

                case DimensionsMode.CompactEvenOnly:
                    width = (width + 1) / 2 * 2;
                    break;

                case DimensionsMode.PowersOfTwo: {
                    if (width >= 2) {
                        var oldWidth = width;
                        width = 1;
                        while (width < oldWidth)
                            width <<= 1;
                    }

                    if (height >= 2) {
                        var oldHeight = height;
                        height = 1;
                        while (height < oldHeight)
                            height <<= 1;
                    }

                    break;
                }
            }

            return new Rectangle(0, 0, width, height);
        }

        private void GetDimensionsSub(ITexturePackerNode node, ref int width, ref int height, bool onlyTextures) {
            if (node == null)
                return;
            if (!onlyTextures || node.Texture != null) {
                if (node.OuterRect.Right > width)
                    width = node.OuterRect.Right;
                if (node.OuterRect.Bottom > height)
                    height = node.OuterRect.Bottom;
            }
            foreach (var childNode in node.Children)
                GetDimensionsSub(childNode, ref width, ref height, onlyTextures);
        }

        public void DrawPackedNodes(Bitmap bitmap)
            => DrawPackedNodesSub(bitmap, _strategyImpl.RootNode);

        private void DrawPackedNodesSub(Bitmap atlas, ITexturePackerNode node) {
            if (node == null)
                return;

            if (node.Texture != null && node.Texture.BitmapDataARGB8888 != null) {
                void CopyImage() {
                    // Should never happen, but in case it does, fail instead of crashing really hard due to memcpy().
                    if (atlas.PixelFormat != PixelFormat.Format32bppArgb)
                        throw new InvalidOperationException();

                    var imageData = node.Texture.BitmapDataARGB8888;

                    // Scan for transparent pixels. If there are any, turn the flag on.
                    if (!HasTransparency) {
                        for (var i = 3; i < imageData.Length; i += 4) {
                            if (imageData[i] != 255) {
                                HasTransparency = true;
                                break;
                            }
                        }
                    }

                    // Rotate the byte array clockwise if 'node.Rotated' is on.
                    // TODO: What about 180 and 270?
                    if (node.Rotation == TextureRotateType.Rotate90CW) {
                        var newImageData = new byte[imageData.Length];
                        var posIn = 0;
                        for (var inY = 0; inY < node.Texture.Height; inY++) {
                            var outX = node.InnerRect.Width - inY - 1;
                            for (var inX = 0; inX < node.Texture.Width; inX++) {
                                var outY = inX;
                                var posOut = (outY * node.InnerRect.Width + outX) * 4;
                                for (var c = 0; c < 4; c++)
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
                    var strideFrom = node.InnerRect.Width * 4;

                    // Start writing, row by row.
                    var posTo = strideTo * node.InnerRect.Top + node.InnerRect.Left * bpp;
                    unsafe {
                        fixed (byte* imageDataPtr = imageData) {
                            var posFrom = 0;
                            for (var row = 0; row < node.InnerRect.Height; row++) {
                                _ = MemCpyUnsafe(bitmapToData.Scan0 + posTo, (IntPtr) (imageDataPtr + posFrom), strideFrom);
                                posTo += strideTo;
                                posFrom += strideFrom;
                            }
                        }
                    }

                    atlas.UnlockBits(bitmapToData);
                }
                CopyImage();
            }

            foreach (var childNode in node.Children)
                DrawPackedNodesSub(atlas, childNode);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                (_strategyImpl as IDisposable)?.Dispose();
                _strategyImpl = null;
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextureAtlas() {
            Dispose(false);
        }

        private ITexturePackerStrategyImpl _strategyImpl = null;
        public ITexturePackerStrategy Strategy => _strategyImpl;

        public bool HasTransparency { get; private set; } = false;
        private readonly Dictionary<(int, int), ITexturePackerNode> _nodeByTextureIDFrame = new Dictionary<(int, int), ITexturePackerNode>();
    }
}