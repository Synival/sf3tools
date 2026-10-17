using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public interface ITexturePackerStrategyImpl : ITexturePackerStrategy {
        ITextureNode Add(IAnimatedTextureFrame tex);
        ITextureNode RootNode { get; }
    }
}
