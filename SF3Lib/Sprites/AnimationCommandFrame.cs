using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class AnimationCommandFrame {
        /// <summary>
        /// Deserializes a JSON object of a AnimationCommandFrame.
        /// </summary>
        /// <param name="json">AnimationCommandFrame in JSON format as a string.</param>
        /// <returns>A new AnimationCommandFrame if deserializing was successful, or 'null' if not.</returns>
        public static AnimationCommandFrame FromJSON(string json) {
            var aniCommandFrame = new AnimationCommandFrame();
            return aniCommandFrame.AssignFromJSON_String(json) ? aniCommandFrame : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationCommandFrame.
        /// </summary>
        /// <param name="jToken">AnimationCommandFrame as a JToken.</param>
        /// <returns>A new AnimationCommandFrame if deserializing was successful, or 'null' if not.</returns>
        public static AnimationCommandFrame FromJToken(JToken jToken) {
            var aniCommandFrame = new AnimationCommandFrame();
            return aniCommandFrame.AssignFromJToken(jToken) ? aniCommandFrame : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;
                        FrameGroup = jObj.TryGetValue("Frame",     out var frame)     ? ((string) frame) : null;
                        Direction  = jObj.TryGetValue("Direction", out var direction) ? (direction.ToObject<SpriteFrameDirection>()) : SpriteFrameDirection.Unset;
                    }
                    catch {
                        return false;
                    }
                    return true;

                default:
                    return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() {
            return new JObject {
                { "Frame", new JValue(FrameGroup) },
                { "Direction", new JValue(Direction.ToString()) }
            };
        }

        public override string ToString() => $"{FrameGroup} ({Direction})";

        public string FrameGroup;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;
    }
}
