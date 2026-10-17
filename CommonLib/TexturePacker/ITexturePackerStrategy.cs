namespace CommonLib.TexturePacker {
    public interface ITexturePackerStrategy {
        void Finish();

        int MaxX { get; }
        int MaxY { get; }
        int Padding { get; }
        bool NeedsFinish { get; }
    }
}
