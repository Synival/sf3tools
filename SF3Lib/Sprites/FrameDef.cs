using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.Sprites {
    public class FrameDef {
        public FrameDef() { }

        public FrameDef(UniqueFrameDef frame) {
            Hash         = frame.TextureHash;
            SpriteSheetX = -1;
            SpriteSheetY = -1;
        }

        public FrameDef(StandaloneFrameDef frame) {
            Hash         = frame.Hash;
            SpriteSheetX = frame.SpriteSheetX;
            SpriteSheetY = frame.SpriteSheetY;
        }

        public override string ToString() => Hash;

        public string Hash;
        public int SpriteSheetX;
        public int SpriteSheetY;
    }
}
