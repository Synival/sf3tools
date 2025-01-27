// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad
// Modified by Synival

using System;
using System.Drawing;

namespace SF3.Win.ThirdParty.TexturePacker {
    public class TextureAtlasNode : IDisposable {
        public TextureAtlasNode(Rectangle rect, int padding, bool tryRotate) {
            Rect      = rect;
            Padding   = padding;
            TryRotate = tryRotate;
        }

        public TextureAtlasNode Insert(ITexture tex)
            => Insert(tex, false, TryRotate);

        private TextureAtlasNode Insert(ITexture tex, bool rotated, bool tryRotate) {
            var width  = rotated ? tex.Height : tex.Width;
            var height = rotated ? tex.Width  : tex.Height;

            // If there's no image assigned yet, attempt to add it.
            if (Texture == null) {
                // If the image fits within the bounds of the PackedNode, add it.
                if (width + Padding <= Rect.Width && height + Padding <= Rect.Height) {
                    Texture = tex;

                    // If there's room to the right, add an empty node to the right.
                    if (Rect.Width - width > 0)
                        Right = new TextureAtlasNode(new Rectangle(Rect.X + width + Padding, Rect.Y, Rect.Width - width - Padding, height), Padding, TryRotate);

                    // If this node is all the way on the left side, add an empty node below.
                    if (Rect.X <= Padding)
                        Left = new TextureAtlasNode(new Rectangle(Padding, Rect.Y + height + Padding, Rect.Width, Rect.Height), Padding, TryRotate);
                    // TODO: else, if there's remaining space below, add that as 'Left'.  we'll need a parent reference for that.

                    Rect = new Rectangle(Rect.X, Rect.Y, width, height);
                    Rotated = rotated;
                    return this;
                }

                // If the image couldn't be added, attempt to add it again with 90 degree rotation.
                // (Don't bother if it's a square image)
                if (tryRotate && width != height) {
                    TextureAtlasNode ret = Insert(tex, true, false);
                    return ret;
                }
            }

            // An image already exists, or we couldn't add it. Attempt to add it to the right.
            if (Right != null) {
                TextureAtlasNode ret = Right.Insert(tex);
                if (ret != null)
                    return ret;
            }

            // Couldn't add to the right - attempt to add to the left.
            if (Left != null) {
                TextureAtlasNode ret = Left.Insert(tex);
                if (ret != null)
                    return ret;
            }

            // Not enough room for this image anywhere.
            return null;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                Left?.Dispose();
                Right?.Dispose();
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TextureAtlasNode() {
            Dispose(false);
        }

        public TextureAtlasNode Left { get; private set; } = null;
        public TextureAtlasNode Right { get; private set; } = null;
        public Rectangle Rect { get; private set; }
        public bool Rotated { get; private set; } = false;
        public ITexture Texture { get; private set; } = null;

        public int Padding { get; }
        public bool TryRotate { get; }
    }
}
