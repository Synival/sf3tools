using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public class TightTexturePackerStrategy : ITexturePackerStrategyImpl, IDisposable {
        public TightTexturePackerStrategy(int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true) {
            MaxX      = maxX;
            MaxY      = maxY;
            Padding   = padding;
            TryRotate = tryRotate;
            CreateRootNode();
        }

        public TightTexturePackerStrategy(IEnumerable<ITexture> textures, int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true, bool sortBySize = true, int? minWidth = null)
        : this(textures?.Select(x => new MockAnimatedTextureFrame(x)).ToArray(), maxX, maxY, padding, tryRotate, sortBySize, minWidth)
        {}

        public TightTexturePackerStrategy(IEnumerable<IAnimatedTextureFrame> textures, int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true, bool sortBySize = true, int? minWidth = null) {
            if (textures == null)
                throw new ArgumentNullException(nameof(textures));

            if (!textures.Any()) {
                maxX = 0;
                maxY = 0;
            }
            else {
                var textureArray = sortBySize ? textures.OrderByDescending(x => x.Height).ToArray() : textures.ToArray();
                textures = textureArray;

                var totalArea = textures.Sum(x => (x.Width + padding) * (x.Height + padding));
                minWidth      = minWidth ?? textures.Max(x => x.Width) + padding;

                maxX = Math.Max((int) minWidth, (int) Math.Sqrt(totalArea));
                maxY = textures.Sum(x => x.Height + padding);
            }

            MaxX = maxX;
            MaxY = maxY;

            CreateRootNode();
            foreach (var tex in textures)
                _ = Add(tex);

            if (NeedsFinish)
                Finish();
        }

        private ITexturePackerNode CreateRootNode() {
            if (_rootNode != null)
                throw new InvalidOperationException("Strategy already has a root node created");
            return (_rootNode = new TightTexturePackerNode(new Rectangle(Padding, Padding, MaxX, MaxY), Padding, TryRotate));
        }

        ~TightTexturePackerStrategy() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                _rootNode?.Dispose();
                _rootNode = null;
                disposedValue = true;
            }
        }

        public void Finish() {
            // Nothing to finish.
        }

        public ITexturePackerNode Add(IAnimatedTextureFrame tex)
            => _rootNode.Add(tex);

        private TightTexturePackerNode _rootNode = null;
        public ITexturePackerNode RootNode => _rootNode;

        public int MaxX { get; }
        public int MaxY { get; }
        public int Padding { get; }
        public bool TryRotate { get; }

        // Finish() is never necessary for this strategy.
        public bool NeedsFinish => false;
    }
}
