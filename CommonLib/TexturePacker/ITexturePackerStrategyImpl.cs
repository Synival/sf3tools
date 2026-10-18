using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public interface ITexturePackerStrategyImpl : ITexturePackerStrategy {
        ITexturePackerNode Add(IAnimatedTextureFrame tex);
        ITexturePackerNode RootNode { get; }
    }
}
