using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class SpriteFramesDef {
        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((FrameGroups != null) ? string.Join(", ", FrameGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public FrameGroupDef[] FrameGroups;

        /// <summary>
        /// Deserializes a JSON object of a SpriteFramesDef.
        /// </summary>
        /// <param name="json">SpriteFramesDef in JSON format as a string.</param>
        /// <returns>A new SpriteFramesDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteFramesDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a SpriteFramesDef.
        /// </summary>
        /// <param name="jToken">SpriteFramesDef as a JToken.</param>
        /// <returns>A new SpriteFramesDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteFramesDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new SpriteFramesDef();

                newDef.SpriteName = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;

                newDef.FrameGroups = jObj.TryGetValue("FrameGroups", out var frameGroups)
                    ? frameGroups.Select(x => FrameGroupDef.FromJToken(x)).ToArray()
                    : null;

                return newDef;
            }
            catch {
                return null;
            }
        }
    }
}
