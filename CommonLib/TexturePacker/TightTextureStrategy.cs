using System.Drawing;

namespace CommonLib.TexturePacker {
    public class TightTextureStrategy : ITexturePackerStrategy {
        public TightTextureStrategy(int maxX, int maxY, int padding = 0, bool tryRotate = true) {
            MaxX      = maxX;
            MaxY      = maxY;
            Padding   = padding;
            TryRotate = tryRotate;
        }

        public int MaxX { get; }
        public int MaxY { get; }
        public int Padding { get; }
        public bool TryRotate { get; }

        // Finish() is never necessary for this strategy.
        public bool NeedsFinish => false;

        public void Finish() {
            // Nothing to finish.
        }

        public ITextureNode CreateRootNode() =>
            new  TightTextureNode(new Rectangle(Padding, Padding, MaxX, MaxY), Padding, TryRotate);
    }
}
