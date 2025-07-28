using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class AnimationGroupDef : IJsonResource {
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
        public static AnimationGroupDef FromJSON(string json) {
            var animationGroupDef = new AnimationGroupDef();
            return animationGroupDef.AssignFromJSON_String(json) ? animationGroupDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationGroupDef.
        /// </summary>
        /// <param name="jToken">AnimationGroupDef as a JToken.</param>
        /// <returns>A new AnimationGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationGroupDef FromJToken(JToken jToken) {
            var animationGroupDef = new AnimationGroupDef();
            return animationGroupDef.AssignFromJToken(jToken) ? animationGroupDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Object:
                    try {
                        Animations = ((IDictionary<string, JToken>) jToken)
                            .ToDictionary(x => x.Key, x => AnimationDef.FromJToken(x.Value));
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

        public JToken ToJToken()
            => Animations != null ? JToken.FromObject(Animations.ToDictionary(x => x.Key, x => x.Value.ToJToken())) : null;

        public override string ToString() => string.Join(", ", Animations.Keys.Select(x => x.ToString()));

        public Dictionary<string, AnimationDef> Animations;
    }
}
