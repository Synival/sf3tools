using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class SpriteAnimationsDef : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a SpriteAnimationsDef.
        /// </summary>
        /// <param name="json">SpriteAnimationsDef in JSON format as a string.</param>
        /// <returns>A new SpriteAnimationsDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteAnimationsDef FromJSON(string json) {
            var spriteAnimations = new SpriteAnimationsDef();
            return spriteAnimations.AssignFromJSON_String(json) ? spriteAnimations : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpriteAnimationsDef.
        /// </summary>
        /// <param name="jToken">SpriteAnimationsDef as a JToken.</param>
        /// <returns>A new SpriteAnimationsDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteAnimationsDef FromJToken(JToken jToken) {
            var spriteAnimations = new SpriteAnimationsDef();
            return spriteAnimations.AssignFromJToken(jToken) ? spriteAnimations : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return false;

            try {
                var jObj = (JObject) jToken;
                SpriteName      = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;
                AnimationGroups = jObj.TryGetValue("Animations", out var animationGroups)
                    ? animationGroups.Select(x => AnimationGroupDef.FromJToken(x)).ToArray()
                    : null;

                return true;
            }
            catch {
                return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() {
            var jObj = new JObject();
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            if (SpriteName != null)
                jObj.Add("SpriteName", new JValue(SpriteName));
            if (AnimationGroups != null)
                jObj.Add("Animations", JToken.FromObject(AnimationGroups.Select(x => x.ToJToken()).ToArray(), jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((AnimationGroups != null) ? string.Join(", ", AnimationGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public AnimationGroupDef[] AnimationGroups;
    }
}
