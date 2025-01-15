// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad
// Modified by Synival

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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
                var minWidth  = textures.Min(x => x.Width) + padding;

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
            if (node != null)
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
            using (var g = Graphics.FromImage(atlas))
                DrawPackedNodesSub(atlas, g, _rootNode);
        }

        private void DrawPackedNodesSub(Bitmap atlas, Graphics graphics, TextureAtlasNode node) {
            if (node == null)
                return;

            if (node.Texture != null) {
                var image = node.Texture.CreateBitmap();
                if (node.Rotated)
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                graphics.DrawImage(image, node.Rect);
            }

            DrawPackedNodesSub(atlas, graphics, node.Left);
            DrawPackedNodesSub(atlas, graphics, node.Right);
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