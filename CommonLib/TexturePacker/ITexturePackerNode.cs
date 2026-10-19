using System.Collections.Generic;
using System.Drawing;
using CommonLib.Imaging;
using CommonLib.Types;

namespace CommonLib.TexturePacker {
    public interface ITexturePackerNode {
        IAnimatedTextureFrame Texture { get; }
        Rectangle InnerRect { get; }
        Rectangle OuterRect { get; }
        TextureRotateType Rotation { get; }

        IEnumerable<ITexturePackerNode> Children { get; }
    }
}
