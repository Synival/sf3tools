using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class AnimationGroupDef : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a AnimationGroupDef.
        /// </summary>
        /// <param name="json">AnimationGroupDef in JSON format as a string.</param>
        /// <returns>A new AnimationGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationGroupDef FromJSON(string json) {
            var aniGroup = new AnimationGroupDef();
            return aniGroup.AssignFromJSON_String(json) ? aniGroup : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a AnimationGroupDef.
        /// </summary>
        /// <param name="jToken">AnimationGroupDef as a JToken.</param>
        /// <returns>A new AnimationGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationGroupDef FromJToken(JToken jToken) {
            var aniGroup = new AnimationGroupDef();
            return aniGroup.AssignFromJToken(jToken) ? aniGroup : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Array:
                    Animations = jToken.Select(x => (string) x).ToArray();
                    return true;

                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;

                        Directions = jObj.TryGetValue("Directions", out var directions) ? ((int?) directions) : null;
                        Width      = jObj.TryGetValue("Width",      out var width)      ? ((int?) width)      : null;
                        Height     = jObj.TryGetValue("Height",     out var height)     ? ((int?) height)     : null;

                        Animations = jObj.TryGetValue("Animations", out var animations)
                            ? animations.Select(x => (string) x).ToArray()
                            : null;
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
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            if (!Directions.HasValue && !Width.HasValue && !Height.HasValue)
                return (Animations != null) ? JToken.FromObject(Animations, jsonSettings) : null;

            var jObj = new JObject();
            if (Directions.HasValue)
                jObj.Add("Directions", new JValue(Directions));
            if (Width.HasValue)
                jObj.Add("Width", new JValue(Width.Value));
            if (Height.HasValue)
                jObj.Add("Height", new JValue(Height.Value));
            if (Animations != null)
                jObj.Add("Animations", JToken.FromObject(Animations, jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (Directions.HasValue ? Directions.ToString() + ": " : "") + ((Animations != null) ? string.Join(", ", Animations) : "[]");

        public int? Directions;
        public int? Width;
        public int? Height;
        public string[] Animations;
    }
}
