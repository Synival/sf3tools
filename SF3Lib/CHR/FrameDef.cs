using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.CHR {
    public class FrameDef {
        /// <summary>
        /// Deserializes a JSON object of a FrameDef.
        /// </summary>
        /// <param name="json">FrameDef in JSON format as a string.</param>
        /// <returns>A new FrameDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a FrameDef.
        /// </summary>
        /// <param name="jToken">FrameDef as a JToken.</param>
        /// <returns>A new FrameDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new FrameDef();

                newDef.Direction    = jObj.TryGetValue("Direction",    out var direction)    ? direction.ToObject<SpriteFrameDirection>() : SpriteFrameDirection.Unset;
                newDef.DuplicateKey = jObj.TryGetValue("DuplicateKey", out var duplicateKey) ? ((string) duplicateKey) : null;

                return newDef;
            }
            catch {
                return null;
            }
        }

        public override string ToString()
            => Direction.ToString() + (DuplicateKey != null ? $", {DuplicateKey}" : "");

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteFrameDirection Direction;

        public string DuplicateKey;
    }
}
