using SF3.Types;

namespace SF3.Sprites {
    public class FrameDef {
        public FrameDef() { }

        public FrameDef(UniqueFrameDef frame) {
            Hash         = frame.TextureHash;
            Direction    = frame.Direction;
            SpriteSheetX = -1;
            SpriteSheetY = -1;
        }

        public FrameDef(StandaloneFrameDef frame) {
            Hash         = frame.Hash;
            Direction    = frame.Direction;
            SpriteSheetX = frame.SpriteSheetX;
            SpriteSheetY = frame.SpriteSheetY;
        }

        public override string ToString() => Direction.ToString();

        public string Hash;
        public SpriteFrameDirection Direction;
        public int SpriteSheetX;
        public int SpriteSheetY;
    }
}
