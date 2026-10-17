namespace CommonLib.TexturePacker {
    public interface ITexturePackerStrategy {
        void Finish();
        bool NeedsFinish { get; }
    }
}
