using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class AnimationGroupDef {
        public AnimationGroupDef() { }

        public AnimationGroupDef(UniqueAnimationDef[] animations) {
            Animations = animations
                .OrderBy(x => x.AnimationName)
                .ToDictionary(x => x.AnimationName, x => new AnimationDef(x));
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationGroupDef.
        /// </summary>
        /// <param name="json">AnimationGroupDef in JSON format as a string.</param>
        /// <returns>A new AnimationGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationGroupDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a AnimationGroupDef.
        /// </summary>
        /// <param name="jToken">AnimationGroupDef as a JToken.</param>
        /// <returns>A new AnimationGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationGroupDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new AnimationGroupDef();

                if (jObj.TryGetValue("Animations", out var animations) && animations.Type == JTokenType.Object) {
                    newDef.Animations = ((IDictionary<string, JToken>) animations)
                        .ToDictionary(x => x.Key, x => AnimationDef.FromJToken(x.Value));
                }

                return newDef;
            }
            catch {
                return null;
            }
        }

        public override string ToString() => string.Join(", ", Animations.Keys.Select(x => x.ToString()));

        public Dictionary<string, AnimationDef> Animations;
    }
}
