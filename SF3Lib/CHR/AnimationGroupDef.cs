using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class AnimationGroupDef {
        public override string ToString()
            => (Directions.HasValue ? Directions.ToString() + ": " : "") + ((Animations != null) ? string.Join(", ", Animations) : "[]");

        public int? Directions;
        public int? Width;
        public int? Height;
        public string[] Animations;

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

                newDef.Directions = jObj.TryGetValue("Directions", out var directions) ? ((int?) directions) : null;
                newDef.Width      = jObj.TryGetValue("Width",      out var width)      ? ((int?) width)      : null;
                newDef.Height     = jObj.TryGetValue("Height",     out var height)     ? ((int?) height)     : null;

                newDef.Animations = jObj.TryGetValue("Animations", out var animations)
                    ? animations.Select(x => (string) x).ToArray()
                    : null;

                return newDef;
            }
            catch {
                return null;
            }
        }
    }
}
