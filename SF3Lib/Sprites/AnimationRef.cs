using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.Sprites {
    public class AnimationRef {
        public string AnimationHash;
        public string SpriteName;
        public int FrameWidth;
        public int FrameHeight;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteDirectionCountType Directions;

        public string AnimationName;

        public override string ToString() => $"{SpriteName} ({FrameWidth}x{FrameHeight}x{Directions}).{AnimationName}";
    }
}
