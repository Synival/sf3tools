using System;
using System.Collections.Generic;
using System.Drawing;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public class LazyTexturePackerNode : ITexturePackerNode {
        public LazyTexturePackerNode(IAnimatedTextureFrame tex, Rectangle rect) {
            Texture = tex;
            Rect    = rect;
        }

        public ITexturePackerNode Add(IAnimatedTextureFrame tex)
            => throw new NotSupportedException();

        public IAnimatedTextureFrame Texture { get; }
        public Rectangle Rect { get; }

        public bool Rotated => false;
        public IEnumerable<ITexturePackerNode> Children => new ITexturePackerNode[0];

    }
}
