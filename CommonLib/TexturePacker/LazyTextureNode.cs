using System;
using System.Collections.Generic;
using System.Drawing;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public class LazyTextureNode : ITextureNode {
        public LazyTextureNode(IAnimatedTextureFrame tex, Rectangle rect) {
            Texture = tex;
            Rect    = rect;
        }

        public ITextureNode Add(IAnimatedTextureFrame tex)
            => throw new NotSupportedException();

        public IAnimatedTextureFrame Texture { get; }
        public Rectangle Rect { get; }

        public bool Rotated => false;
        public IEnumerable<ITextureNode> Children => new ITextureNode[0];

    }
}
