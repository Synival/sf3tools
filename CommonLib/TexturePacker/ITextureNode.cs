using System.Drawing;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public interface ITextureNode {
        ITextureNode Insert(IAnimatedTextureFrame tex);

        IAnimatedTextureFrame Texture { get; }
        Rectangle Rect { get; }
        ITextureNode Left { get; }
        ITextureNode Right { get; }
        bool Rotated { get; }
    }
}
