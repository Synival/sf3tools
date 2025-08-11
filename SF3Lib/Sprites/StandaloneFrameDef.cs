using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.Sprites {
    public class StandaloneFrameDef {
        public StandaloneFrameDef() { }

        public StandaloneFrameDef(Frame frame, SpriteFrameDirection direction, string name, int width, int height) {
            Hash         = frame.Hash;
            SpritesheetX = frame.SpritesheetX;
            SpritesheetY = frame.SpritesheetY;
            Direction    = direction;
            Name         = name;
            Width        = width;
            Height       = height;
        }

        public override string ToString() => Name;

        public string Name;
        public string Hash;
        public int Width;
        public int Height;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;

        public int SpritesheetX;
        public int SpritesheetY;
    }
}
