using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class AnimationDef {
        public AnimationDef() { }

        public AnimationDef(UniqueAnimationDef aniInfo) : this() {
            AnimationFrames = aniInfo.AnimationFrames;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationDef.
        /// </summary>
        /// <param name="json">AnimationDef in JSON format as a string.</param>
        /// <returns>A new AnimationDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a AnimationDef.
        /// </summary>
        /// <param name="jToken">AnimationDef as a JToken.</param>
        /// <returns>A new AnimationDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new AnimationDef();

                newDef.AnimationFrames = jObj.TryGetValue("AnimationFrames", out var animationFrames)
                    ? animationFrames.Select(x => AnimationFrameDef.FromJToken(x)).ToArray()
                    : null;

                return newDef;
            }
            catch {
                return null;
            }
        }

        public override string ToString() => string.Join(", ", AnimationFrames.Select(x => x.ToString()));

        public AnimationFrameDef[] AnimationFrames;
    };
}
