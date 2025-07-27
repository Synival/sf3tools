using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class SpriteFramesDef : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a SpriteFramesDef.
        /// </summary>
        /// <param name="json">SpriteFramesDef in JSON format as a string.</param>
        /// <returns>A new SpriteFramesDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteFramesDef FromJSON(string json) {
            var framesDef = new SpriteFramesDef();
            return framesDef.AssignFromJSON_String(json) ? framesDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpriteFramesDef.
        /// </summary>
        /// <param name="jToken">SpriteFramesDef as a JToken.</param>
        /// <returns>A new SpriteFramesDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteFramesDef FromJToken(JToken jToken) {
            var framesDef = new SpriteFramesDef();
            return framesDef.AssignFromJToken(jToken) ? framesDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.Array:
                    var jArray = (JArray) jToken;
                    FrameGroups = jArray.Select(x => FrameGroupDef.FromJToken(x)).ToArray();
                    return true;

                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;
                        SpriteName = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;
                        FrameGroups = jObj.TryGetValue("Frames", out var frameGroups)
                            ? frameGroups.Select(x => FrameGroupDef.FromJToken(x)).ToArray()
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

            if (SpriteName == null)
                return FrameGroups != null ? JToken.FromObject(FrameGroups.Select(x => x.ToJToken()).ToArray(), jsonSettings) : null;

            var jObj = new JObject();
            jObj.Add("SpriteName", new JValue(SpriteName));
            if (FrameGroups != null)
                jObj.Add("Frames", JToken.FromObject(FrameGroups.Select(x => x.ToJToken()), jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((FrameGroups != null) ? string.Join(", ", FrameGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public FrameGroupDef[] FrameGroups;
    }
}
