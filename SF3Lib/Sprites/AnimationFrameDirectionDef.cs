using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class AnimationFrameDirectionDef {
        /// <summary>
        /// Deserializes a JSON object of a AnimationFrameDirectionDef.
        /// </summary>
        /// <param name="json">AnimationFrameDirectionDef in JSON format as a string.</param>
        /// <returns>A new AnimationFrameDirectionDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationFrameDirectionDef FromJSON(string json) {
            var animationFrameDirectionDef = new AnimationFrameDirectionDef();
            return animationFrameDirectionDef.AssignFromJSON_String(json) ? animationFrameDirectionDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationFrameDirectionDef.
        /// </summary>
        /// <param name="jToken">AnimationFrameDirectionDef as a JToken.</param>
        /// <returns>A new AnimationFrameDirectionDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationFrameDirectionDef FromJToken(JToken jToken) {
            var animationFrameDirectionDef = new AnimationFrameDirectionDef();
            return animationFrameDirectionDef.AssignFromJToken(jToken) ? animationFrameDirectionDef : null;
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
                        Frame     = jObj.TryGetValue("Frame",     out var frame)     ? ((string) frame) : null;
                        Direction = jObj.TryGetValue("Direction", out var direction) ? (direction.ToObject<SpriteFrameDirection>()) : SpriteFrameDirection.Unset;
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
                { "Frame", new JValue(Frame) },
                { "Direction", new JValue(Direction.ToString()) }
            };
        }

        public override string ToString() => $"{Frame} ({Direction})";

        public string Frame;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;
    }
}
