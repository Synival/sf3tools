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
        public static AnimationFrameDirectionDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a AnimationFrameDirectionDef.
        /// </summary>
        /// <param name="jToken">AnimationFrameDirectionDef as a JToken.</param>
        /// <returns>A new AnimationFrameDirectionDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationFrameDirectionDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new AnimationFrameDirectionDef();

                newDef.Frame     = jObj.TryGetValue("Frame",     out var frame)     ? ((string) frame) : null;
                newDef.Direction = jObj.TryGetValue("Direction", out var direction) ? (direction.ToObject<SpriteFrameDirection>()) : SpriteFrameDirection.Unset;

                return newDef;
            }
            catch {
                return null;
            }
        }

        public string Frame;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;
    }
}
