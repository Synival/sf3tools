using System.Collections.Generic;
using System.Linq;

namespace SF3.Models.Structs.CHR {
    /// <summary>
    /// Information about a particular texture read from a sprite.
    /// </summary>
    public class FrameTextureInfo {
        public FrameTextureInfo(string textureHash, string spriteName, int width, int height, string animationName) {
            TextureHash   = textureHash;
            SpriteName    = spriteName;
            AnimationName = animationName;
            Width         = width;
            Height        = height;
        }

        public string TextureHash { get; }
        public string SpriteName { get; }
        public string AnimationName { get; set; }
        public int Width { get; }
        public int Height { get; }
        public Dictionary<string, int> DirectionCounts { get; } = new Dictionary<string, int>();
        public string DirectionsString => string.Join(", ", DirectionCounts.OrderByDescending(x => x.Value).Select(x => $"{x.Key} (x{x.Value})"));
    }
}
