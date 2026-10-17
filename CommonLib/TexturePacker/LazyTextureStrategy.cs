using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public class LazyTextureStrategy : ITexturePackerStrategyImpl, ITextureNode {
        public LazyTextureStrategy(int padding) {
            Width   = 0;
            Padding = padding;
        }

        public LazyTextureStrategy(IEnumerable<ITexture> textures, int padding = 0)
        : this(textures?.Select(x => new MockAnimatedTextureFrame(x)).ToArray(), padding)
        {}

        public LazyTextureStrategy(IEnumerable<IAnimatedTextureFrame> textures, int padding = 0) {
            if (textures == null)
                throw new ArgumentNullException(nameof(textures));

            foreach (var tex in textures)
                _ = Add(tex);

            if (NeedsFinish)
                Finish();
        }

        public void Finish() {
            // Nothing to finish.
        }

        public ITextureNode Add(IAnimatedTextureFrame tex) {
            var padding = Padding;
            var width   = tex.Width;
            var height  = tex.Height;

            var newChild = new LazyTextureNode(tex, new Rectangle(Width + padding, padding, width, height));
            _childList.Add(newChild);
            Width += width + Padding * 2;
            Height = Math.Max(Height, height + Padding * 2);
            Rect   = new Rectangle(0, 0, Width, Height);

            return newChild;
        }

        public ITextureNode RootNode => this;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Padding { get; }

        // Finish() is never necessary for this strategy.
        public bool NeedsFinish => false;

        public IAnimatedTextureFrame Texture => null;
        public bool Rotated => false;

        public Rectangle Rect { get; private set; } = new Rectangle(0, 0, 0, 0);
        public List<ITextureNode> _childList = new List<ITextureNode>();
        public IEnumerable<ITextureNode> Children => _childList;
    }
}
