using SF3.Types;

namespace SF3.Sprites {
    public class FrameDef {
        public FrameDef() { }

        public FrameDef(UniqueFrameDef uniqueFrameDef) {
            Name      = uniqueFrameDef.FrameName;
            Hash      = uniqueFrameDef.TextureHash;
            Width     = uniqueFrameDef.Width;
            Height    = uniqueFrameDef.Height;
            Direction = uniqueFrameDef.Direction;
            SpriteSheetX = -1;
            SpriteSheetY = -1;
        }

        public override string ToString() => Name;

        public string Name;
        public string Hash;
        public int Width;
        public int Height;
        public SpriteFrameDirection Direction;
        public int SpriteSheetX;
        public int SpriteSheetY;
    }
}
