using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class FrameDef {
        public FrameDef() { }

        public FrameDef(UniqueFrameDef frame) {
            Hash         = frame.TextureHash;
            SpriteSheetX = -1;
            SpriteSheetY = -1;
        }

        public FrameDef(StandaloneFrameDef frame) {
            Hash         = frame.Hash;
            SpriteSheetX = frame.SpriteSheetX;
            SpriteSheetY = frame.SpriteSheetY;
        }

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

                newDef.Hash         = jObj.TryGetValue("Hash",         out var hash)         ? ((string) hash)      : null;
                newDef.SpriteSheetX = jObj.TryGetValue("SpriteSheetX", out var spritesheetX) ? ((int) spritesheetX) : -1;
                newDef.SpriteSheetY = jObj.TryGetValue("SpriteSheetY", out var spritesheetY) ? ((int) spritesheetY) : -1;

                return newDef;
            }
            catch {
                return null;
            }
        }

        public override string ToString() => Hash;

        public string Hash;
        public int SpriteSheetX;
        public int SpriteSheetY;
    }
}
