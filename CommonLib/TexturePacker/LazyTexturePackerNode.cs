using System;
using System.Collections.Generic;
using System.Drawing;
using CommonLib.Imaging;
using CommonLib.Types;

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

        public TextureRotateType Rotation => TextureRotateType.NoRotation;
        public IEnumerable<ITexturePackerNode> Children => new ITexturePackerNode[0];

    }
}
