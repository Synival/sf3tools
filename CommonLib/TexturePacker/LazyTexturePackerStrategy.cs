using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.Types;

namespace CommonLib.TexturePacker {
    public class LazyTexturePackerStrategy : ITexturePackerStrategyImpl, ITexturePackerNode {
        public LazyTexturePackerStrategy(int padding) {
            Padding = padding;
        }

        public LazyTexturePackerStrategy(IEnumerable<ITexture> textures, int padding = 0)
        : this(textures?.Select(x => new MockAnimatedTextureFrame(x)).ToArray(), padding)
        {}

        public LazyTexturePackerStrategy(IEnumerable<IAnimatedTextureFrame> textures, int padding = 0) {
            if (textures == null)
                throw new ArgumentNullException(nameof(textures));

            Padding = padding;
            foreach (var tex in textures)
                _ = Add(tex);

            if (NeedsFinish)
                Finish();
        }

        public void Finish() {
            // Nothing to finish.
        }

        public ITexturePackerNode Add(IAnimatedTextureFrame tex) {
            var padding     = Padding;
            var innerWidth  = tex.Width;
            var innerHeight = tex.Height;
            var outerWidth  = innerWidth  + padding * 2;
            var outerHeight = innerHeight + padding * 2;

            var newChild = new LazyTexturePackerNode(
                tex,
                new Rectangle(OuterRect.Width + padding, padding, innerWidth, innerHeight),
                new Rectangle(OuterRect.Width, 0, outerWidth, outerHeight)
            );

            _childList.Add(newChild);
            outerHeight = Math.Max(OuterRect.Height, newChild.OuterRect.Height);
            OuterRect = new Rectangle(0, 0, newChild.OuterRect.Right, outerHeight);
            InnerRect = new Rectangle(padding, padding, OuterRect.Width - padding * 2, OuterRect.Height - padding * 2);

            return newChild;
        }

        public ITexturePackerNode RootNode => this;

        public int Padding { get; }

        // Finish() is never necessary for this strategy.
        public bool NeedsFinish => false;

        public IAnimatedTextureFrame Texture => null;
        public TextureRotateType Rotation => TextureRotateType.NoRotation;

        public Rectangle InnerRect { get; private set; } = new Rectangle(0, 0, 0, 0);
        public Rectangle OuterRect { get; private set; } = new Rectangle(0, 0, 0, 0);
        public List<ITexturePackerNode> _childList = new List<ITexturePackerNode>();
        public IEnumerable<ITexturePackerNode> Children => _childList;
    }
}
