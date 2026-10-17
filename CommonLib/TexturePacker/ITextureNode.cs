using System.Collections.Generic;
using System.Drawing;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public interface ITextureNode {
        IAnimatedTextureFrame Texture { get; }
        Rectangle Rect { get; }
        bool Rotated { get; }

        IEnumerable<ITextureNode> Children { get; }
    }
}
