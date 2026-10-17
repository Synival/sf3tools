using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public interface ITexturePackerStrategyImpl : ITexturePackerStrategy {
        ITextureNode Insert(IAnimatedTextureFrame tex);

        new int MaxX { get; set; }
        new int MaxY { get; set; }
        new int Padding { get; set; }
        ITextureNode RootNode { get; }
    }
}
