using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.Sprites {
    public class AnimationFrameDirectionDef {
        public string Frame;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;
    }
}
