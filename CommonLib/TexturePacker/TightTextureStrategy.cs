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
    }
}
