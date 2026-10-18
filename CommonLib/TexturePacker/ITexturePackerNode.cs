using System.Collections.Generic;
using System.Drawing;
using CommonLib.Imaging;

namespace CommonLib.TexturePacker {
    public interface ITexturePackerNode {
        IAnimatedTextureFrame Texture { get; }
        Rectangle Rect { get; }
        bool Rotated { get; }

        IEnumerable<ITexturePackerNode> Children { get; }
    }
}
