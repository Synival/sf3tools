using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.CHR {
    public class AnimationsForSpritesheetAndDirection : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a SpriteAnimationsDef.
        /// </summary>
        /// <param name="json">SpriteAnimationsDef in JSON format as a string.</param>
        /// <returns>A new SpriteAnimationsDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationsForSpritesheetAndDirection FromJSON(string json) {
            var spriteAnimations = new AnimationsForSpritesheetAndDirection();
            return spriteAnimations.AssignFromJSON_String(json) ? spriteAnimations : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpriteAnimationsDef.
        /// </summary>
        /// <param name="jToken">SpriteAnimationsDef as a JToken.</param>
        /// <returns>A new SpriteAnimationsDef if deserializing was successful, or 'null' if not.</returns>
        public static AnimationsForSpritesheetAndDirection FromJToken(JToken jToken) {
            var spriteAnimations = new AnimationsForSpritesheetAndDirection();
            return spriteAnimations.AssignFromJToken(jToken) ? spriteAnimations : null;
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
                        SpriteName = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;
                        Width      = jObj.TryGetValue("Width",      out var width)      ? ((int?) width)        : null;
                        Height     = jObj.TryGetValue("Height",     out var height)     ? ((int?) height)       : null;
                        Directions = jObj.TryGetValue("Directions", out var directions) ? SpriteDirectionCountTypeExtensions.FromJToken(directions) : (SpriteDirectionCountType?) null;

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

            if (SpriteName == null && !Directions.HasValue && !Width.HasValue && !Height.HasValue)
                return (Animations != null) ? JToken.FromObject(Animations, jsonSettings) : null;

            var jObj = new JObject();

            if (SpriteName != null)
                jObj.Add("SpriteName", new JValue(SpriteName));
            if (Width.HasValue)
                jObj.Add("Width", new JValue(Width.Value));
            if (Height.HasValue)
                jObj.Add("Height", new JValue(Height.Value));
            if (Directions.HasValue)
                jObj.Add("Directions", Directions.Value.ToJToken());
            if (Animations != null)
                jObj.Add("Animations", JToken.FromObject(Animations, jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (SpriteName != null ? SpriteName + $" ({Width}x{Height}x{Directions}): " : "") + ((Animations != null) ? string.Join(", ", Animations) : "[]");

        public string SpriteName;
        public int? Width;
        public int? Height;
        public SpriteDirectionCountType? Directions;
        public string[] Animations;
    }
}
