using SF3.Types;

namespace SF3.Sprites {
    public class StandaloneFrameDef {
        public StandaloneFrameDef() { }

        public StandaloneFrameDef(UniqueFrameDef uniqueFrameDef) {
            Name      = uniqueFrameDef.FrameName;
            Hash      = uniqueFrameDef.TextureHash;
            Width     = uniqueFrameDef.Width;
            Height    = uniqueFrameDef.Height;
            Direction = uniqueFrameDef.Direction;
            SpriteSheetX = -1;
            SpriteSheetY = -1;
        }

        public StandaloneFrameDef(FrameDef frame, string name, int width, int height) {
            Hash         = frame.Hash;
            Direction    = frame.Direction;
            SpriteSheetX = frame.SpriteSheetX;
            SpriteSheetY = frame.SpriteSheetY;
            Name         = name;
            Width        = width;
            Height       = height;
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
