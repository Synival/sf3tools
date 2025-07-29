using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class FrameGroupsForSpritesheet : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a FrameGroupsForSpritesheet.
        /// </summary>
        /// <param name="json">FrameGroupsForSpritesheet in JSON format as a string.</param>
        /// <returns>A new FrameGroupsForSpritesheet if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroupsForSpritesheet FromJSON(string json) {
            var frameGroups = new FrameGroupsForSpritesheet();
            return frameGroups.AssignFromJSON_String(json) ? frameGroups : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a FrameGroupsForSpritesheet.
        /// </summary>
        /// <param name="jToken">FrameGroupsForSpritesheet as a JToken.</param>
        /// <returns>A new FrameGroupsForSpritesheet if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroupsForSpritesheet FromJToken(JToken jToken) {
            var frameGroups = new FrameGroupsForSpritesheet();
            return frameGroups.AssignFromJToken(jToken) ? frameGroups : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Array:
                    var jArray = (JArray) jToken;
                    FrameGroups = jArray.Select(x => FrameGroup.FromJToken(x)).ToArray();
                    return true;

                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;

                        SpriteName = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;
                        Width      = jObj.TryGetValue("Width",      out var width)      ? ((int?) width)        : null;
                        Height     = jObj.TryGetValue("Height",     out var height)     ? ((int?) height)       : null;

                        FrameGroups = jObj.TryGetValue("Frames", out var frameGroups)
                            ? frameGroups.Select(x => FrameGroup.FromJToken(x)).ToArray()
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

            if (SpriteName == null && !Width.HasValue && !Height.HasValue)
                return FrameGroups != null ? JToken.FromObject(FrameGroups.Select(x => x.ToJToken()).ToArray(), jsonSettings) : null;

            var jObj = new JObject();
            if (SpriteName != null)
                jObj.Add("SpriteName", new JValue(SpriteName));
            if (Width.HasValue)
                jObj.Add("Width", new JValue(Width.Value));
            if (Height.HasValue)
                jObj.Add("Height", new JValue(Height.Value));
            if (FrameGroups != null)
                jObj.Add("Frames", JToken.FromObject(FrameGroups.Select(x => x.ToJToken()), jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((FrameGroups != null) ? string.Join(", ", FrameGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public int? Width;
        public int? Height;

        public FrameGroup[] FrameGroups;
    }
}
