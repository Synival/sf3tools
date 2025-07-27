using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.CHR {
    public class FrameDef : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a FrameDef.
        /// </summary>
        /// <param name="json">FrameDef in JSON format as a string.</param>
        /// <returns>A new FrameDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameDef FromJSON(string json) {
            var frameDef = new FrameDef();
            return frameDef.AssignFromJSON_String(json) ? frameDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a FrameDef.
        /// </summary>
        /// <param name="jToken">FrameDef as a JToken.</param>
        /// <returns>A new FrameDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameDef FromJToken(JToken jToken) {
            var frameDef = new FrameDef();
            return frameDef.AssignFromJToken(jToken) ? frameDef : null;
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
                        Direction    = jObj.TryGetValue("Direction",    out var direction)    ? direction.ToObject<SpriteFrameDirection>() : SpriteFrameDirection.Unset;
                        DuplicateKey = jObj.TryGetValue("DuplicateKey", out var duplicateKey) ? ((string) duplicateKey) : null;
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
            if (DuplicateKey == null)
                return new JValue(Direction.ToString());

            var jObj = new JObject();
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            jObj.Add("Direction", new JValue(Direction.ToString()));
            if (DuplicateKey != null)
                jObj.Add("DuplicateKey", new JValue(DuplicateKey));

            return jObj;
        }

        public override string ToString()
            => Direction.ToString() + (DuplicateKey != null ? $", {DuplicateKey}" : "");

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;

        public string DuplicateKey;
    }
}
