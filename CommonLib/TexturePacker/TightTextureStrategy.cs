using System;
using System.Drawing;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public class TightTextureStrategy : ITexturePackerStrategyImpl, IDisposable {
        public TightTextureStrategy(int maxX = 0, int maxY = 0, int padding = 0, bool tryRotate = true) {
            MaxX      = maxX;
            MaxY      = maxY;
            Padding   = padding;
            TryRotate = tryRotate;
        }

        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public int Padding { get; set; }
        public bool TryRotate { get; set; }

        // Finish() is never necessary for this strategy.
        public bool NeedsFinish => false;

        public void Finish() {
            // Nothing to finish.
        }

        public ITextureNode CreateRootNode() {
            if (_rootNode != null)
                throw new InvalidOperationException("Strategy already has a root node created");
            return (_rootNode = new TightTextureNode(new Rectangle(Padding, Padding, MaxX, MaxY), Padding, TryRotate));
        }

        public ITextureNode Insert(IAnimatedTextureFrame tex)
            => _rootNode.Insert(tex);

        private TightTextureNode _rootNode = null;
        public ITextureNode RootNode => _rootNode;

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
    }
}
