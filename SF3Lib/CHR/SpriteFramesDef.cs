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
            if (jToken == null || jToken.Type != JTokenType.Object)
                return false;

            try {
                var jObj = (JObject) jToken;

                SpriteName = jObj.TryGetValue("SpriteName", out var spriteName) ? ((string) spriteName) : null;

                FrameGroups = jObj.TryGetValue("FrameGroups", out var frameGroups)
                    ? frameGroups.Select(x => FrameGroupDef.FromJToken(x)).ToArray()
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

            jObj.Add("SpriteName", new JValue(SpriteName));
            if (FrameGroups != null)
                jObj.Add("FrameGroups", JToken.FromObject(FrameGroups, jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (SpriteName != null ? SpriteName + ": " : "") + ((FrameGroups != null) ? string.Join(", ", FrameGroups.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string SpriteName;
        public FrameGroupDef[] FrameGroups;
    }
}
