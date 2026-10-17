using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public class TightTextureStrategy : ITexturePackerStrategyImpl, IDisposable {
        public TightTextureStrategy(int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true) {
            MaxX      = maxX;
            MaxY      = maxY;
            Padding   = padding;
            TryRotate = tryRotate;
            CreateRootNode();
        }

        public TightTextureStrategy(IEnumerable<ITexture> textures, int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true, bool sortBySize = true, int? minWidth = null)
        : this(textures?.Select(x => new MockAnimatedTextureFrame(x)).ToArray(), maxX, maxY, padding, tryRotate, sortBySize, minWidth)
        {}

        public TightTextureStrategy(IEnumerable<IAnimatedTextureFrame> textures, int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true, bool sortBySize = true, int? minWidth = null) {
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

        private ITextureNode CreateRootNode() {
            if (_rootNode != null)
                throw new InvalidOperationException("Strategy already has a root node created");
            return (_rootNode = new TightTextureNode(new Rectangle(Padding, Padding, MaxX, MaxY), Padding, TryRotate));
        }

        ~TightTextureStrategy() {
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

        public ITextureNode Add(IAnimatedTextureFrame tex)
            => _rootNode.Add(tex);

        private TightTextureNode _rootNode = null;
        public ITextureNode RootNode => _rootNode;

        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int Padding { get; set; }
        public bool TryRotate { get; set; }

        // Finish() is never necessary for this strategy.
        public bool NeedsFinish => false;
    }
}
