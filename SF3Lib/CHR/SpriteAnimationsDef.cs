using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class SpriteAnimationsDef {
        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((AnimationGroups != null) ? string.Join(", ", AnimationGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public AnimationGroupDef[] AnimationGroups;

        /// <summary>
        /// Deserializes a JSON object of a SpriteAnimationsDef.
        /// </summary>
        /// <param name="json">SpriteAnimationsDef in JSON format as a string.</param>
        /// <returns>A new SpriteAnimationsDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteAnimationsDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a SpriteAnimationsDef.
        /// </summary>
        /// <param name="jToken">SpriteAnimationsDef as a JToken.</param>
        /// <returns>A new SpriteAnimationsDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteAnimationsDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new SpriteAnimationsDef();

                newDef.SpriteName = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;

                newDef.AnimationGroups = jObj.TryGetValue("AnimationGroups", out var animationGroups)
                    ? animationGroups.Select(x => AnimationGroupDef.FromJToken(x)).ToArray()
                    : null;

                return newDef;
            }
            catch {
                return null;
            }
        }
    }
}
