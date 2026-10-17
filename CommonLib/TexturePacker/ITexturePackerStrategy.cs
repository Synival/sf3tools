namespace CommonLib.TexturePacker {
    public interface ITexturePackerStrategy {
        ITextureNode CreateRootNode();
        void Finish();

        int MaxX { get; }
        int MaxY { get; }
        bool NeedsFinish { get; }
    }
}
