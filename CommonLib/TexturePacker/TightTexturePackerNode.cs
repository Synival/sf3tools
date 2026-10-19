// Original code by Thomas Alexander (https://gist.github.com/ttalexander2)
// https://gist.github.com/ttalexander2/88a40eec0fd0ea5b31cc2453d6bbddad
// Modified by Synival

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.Types;

namespace CommonLib.TexturePacker {
    public class TightTexturePackerNode : ITexturePackerNode, IDisposable {
        public TightTexturePackerNode(Rectangle innerRect, Rectangle outerRect, int padding, bool tryRotate) {
            if (innerRect.Width < 0 || innerRect.Height < 0)
                throw new ArgumentOutOfRangeException(nameof(innerRect));

            InnerRect = innerRect;
            OuterRect = outerRect;
            Padding   = padding;
            TryRotate = tryRotate;
        }

        ~TightTexturePackerNode() {
            Dispose(false);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                (Left as IDisposable)?.Dispose();
                (Right as IDisposable)?.Dispose();
            }

            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ITexturePackerNode Add(IAnimatedTextureFrame tex)
            => Add(tex, false, TryRotate);

        private ITexturePackerNode Add(IAnimatedTextureFrame tex, bool rotated, bool tryRotate) {
            var padding = Padding;
            var padding2 = padding * 2;
            var innerWidth  = rotated ? tex.Height : tex.Width;
            var innerHeight = rotated ? tex.Width  : tex.Height;
            var outerWidth  = innerWidth  + padding2;
            var outerHeight = innerHeight + padding2;

            // If there's no image assigned yet, attempt to add it.
            if (Texture == null) {
                // If the image fits within the bounds of the PackedNode, add it.
                if (outerWidth <= OuterRect.Width && outerHeight <= OuterRect.Height) {
                    Texture = tex;

                    // If there's room to the right, add an empty node to the right.
                    if (OuterRect.Width - outerWidth - padding2 > 0) {
                        Right = new TightTexturePackerNode(
                            new Rectangle(
                                OuterRect.X     + outerWidth + padding,  OuterRect.Y + padding,
                                OuterRect.Width - outerWidth - padding2, outerHeight - padding2
                            ),
                            new Rectangle(OuterRect.X + outerWidth, OuterRect.Y, OuterRect.Width - outerWidth, outerHeight),
                            Padding, TryRotate
                        );
                    }

                    // If this node is all the way on the left side, add an empty node below.
                    if (OuterRect.X == 0) {
                        Left = new TightTexturePackerNode(
                            new Rectangle(
                                padding, OuterRect.Y + outerHeight + padding,
                                OuterRect.Width - padding2, outerHeight - padding2
                            ),
                            new Rectangle(0, OuterRect.Y + outerHeight, OuterRect.Width, OuterRect.Height),
                            Padding, TryRotate
                        );
                    }
                    // TODO: else, if there's remaining space below, add that as 'Left'.  we'll need a parent reference for that.

                    OuterRect = new Rectangle(OuterRect.X, OuterRect.Y, outerWidth, outerHeight);
                    InnerRect = new Rectangle(OuterRect.X + padding, OuterRect.Y + padding, outerWidth - padding2, outerHeight - padding2);
                    if (InnerRect.Width < 0 || InnerRect.Height < 0)
                        throw new InvalidOperationException($"{nameof(InnerRect)} somehow has negative size");

                    Rotation = rotated ? TextureRotateType.Rotate90CW : TextureRotateType.NoRotation;
                    return this;
                }

                // If the image couldn't be added, attempt to add it again with 90 degree rotation.
                // (Don't bother if it's a square image)
                if (tryRotate && outerWidth != outerHeight) {
                    var ret = Add(tex, true, false);
                    return ret;
                }
            }

            // An image already exists, or we couldn't add it. Attempt to add it to the right.
            if (Right != null) {
                var ret = Right.Add(tex);
                if (ret != null)
                    return ret;
            }

            // Couldn't add to the right - attempt to add to the left.
            if (Left != null) {
                var ret = Left.Add(tex);
                if (ret != null)
                    return ret;
            }

            // Not enough room for this image anywhere.
            return null;
        }

        private TightTexturePackerNode _left = null;
        private TightTexturePackerNode _right = null;

        private TightTexturePackerNode Left {
            get => _left;
            set {
                if (_left != value) {
                    _left = value;
                    Children = new List<ITexturePackerNode>() { _left, _right }.Where(x => x != null).ToArray();
                }
            }
        }

        private TightTexturePackerNode Right {
            get => _right;
            set {
                if (_right != value) {
                    _right = value;
                    Children = new List<ITexturePackerNode>() { _left, _right }.Where(x => x != null).ToArray();
                }
            }
        }

        public Rectangle InnerRect { get; private set; }
        public Rectangle OuterRect { get; private set; }
        public TextureRotateType Rotation { get; private set; } = TextureRotateType.NoRotation;
        public IAnimatedTextureFrame Texture { get; private set; } = null;

        public int Padding { get; }
        public bool TryRotate { get; }
        public IEnumerable<ITexturePackerNode> Children { get; private set; } = new ITexturePackerNode[0];

    }
}
