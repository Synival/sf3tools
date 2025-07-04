using System.Collections.Generic;
using System.Linq;
using SF3.Types;

namespace SF3.Sprites {
    /// <summary>
    /// Information about a particular texture read from a sprite.
    /// </summary>
    public class UniqueFrameInfo {
        public UniqueFrameInfo(string textureHash, string spriteName, int width, int height, string frameName, SpriteFrameDirection direction) {
            TextureHash = textureHash;
            SpriteName  = spriteName;
            Width       = width;
            Height      = height;
            FrameName   = frameName;
            Direction   = direction;
        }

        public string TextureHash;
        public string SpriteName;
        public int Width;
        public int Height;
        public string FrameName;
        public SpriteFrameDirection Direction;
        public HashSet<SpriteFrameDirection> Directions = new HashSet<SpriteFrameDirection>();
        public HashSet<string> AnimationNames = new HashSet<string>();
    }
}
