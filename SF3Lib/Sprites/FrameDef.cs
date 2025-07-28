using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class FrameDef : IJsonResource {
        public FrameDef() { }

        public FrameDef(UniqueFrameDef frame) {
            Hash         = frame.TextureHash;
            SpritesheetX = -1;
            SpritesheetY = -1;
        }

        public FrameDef(StandaloneFrameDef frame) {
            Hash         = frame.Hash;
            SpritesheetX = frame.SpriteSheetX;
            SpritesheetY = frame.SpriteSheetY;
        }

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
                        Hash         = jObj.TryGetValue("Hash",         out var hash)         ? ((string) hash)      : null;
                        SpritesheetX = jObj.TryGetValue("SpritesheetX", out var spritesheetX) ? ((int) spritesheetX) : -1;
                        SpritesheetY = jObj.TryGetValue("SpritesheetY", out var spritesheetY) ? ((int) spritesheetY) : -1;
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
            return new JObject() {
                { "Hash", new JValue(Hash) },
                { "SpritesheetX", new JValue(SpritesheetX) },
                { "SpritesheetY", new JValue(SpritesheetY) },
            };
        }

        public override string ToString() => Hash;

        public string Hash;
        public int SpritesheetX;
        public int SpritesheetY;
    }
}
