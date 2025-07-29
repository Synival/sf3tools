using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.CHR {
    public class Frame : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a Frame.
        /// </summary>
        /// <param name="json">Frame in JSON format as a string.</param>
        /// <returns>A new Frame if deserializing was successful, or 'null' if not.</returns>
        public static Frame FromJSON(string json) {
            var frame = new Frame();
            return frame.AssignFromJSON_String(json) ? frame : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a Frame.
        /// </summary>
        /// <param name="jToken">Frame as a JToken.</param>
        /// <returns>A new Frame if deserializing was successful, or 'null' if not.</returns>
        public static Frame FromJToken(JToken jToken) {
            var frame = new Frame();
            return frame.AssignFromJToken(jToken) ? frame : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.String:
                    Direction = jToken.ToObject<SpriteFrameDirection>();
                    return true;

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
