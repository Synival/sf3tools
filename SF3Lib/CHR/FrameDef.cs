using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SF3.Types;

namespace SF3.CHR {
    public class FrameDef {
        public override string ToString()
            => Direction.ToString() + (DuplicateKey != null ? $", {DuplicateKey}" : "");

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;

        public string DuplicateKey;
    }
}
