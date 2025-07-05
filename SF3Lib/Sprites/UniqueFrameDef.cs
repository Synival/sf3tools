using SF3.Types;

namespace SF3.Sprites {
    public class UniqueFrameDef {
        public UniqueFrameDef(string textureHash, string spriteName, int width, int height, string frameName, SpriteFrameDirection direction) {
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
    }
}
