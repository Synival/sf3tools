using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class FrameGroupDef : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a FrameGroupDef.
        /// </summary>
        /// <param name="json">FrameGroupDef in JSON format as a string.</param>
        /// <returns>A new FrameGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroupDef FromJSON(string json) {
            var frameGroupDef = new FrameGroupDef();
            return frameGroupDef.AssignFromJSON_String(json) ? frameGroupDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a FrameGroupDef.
        /// </summary>
        /// <param name="jToken">FrameGroupDef as a JToken.</param>
        /// <returns>A new FrameGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroupDef FromJToken(JToken jToken) {
            var frameGroupDef = new FrameGroupDef();
            return frameGroupDef.AssignFromJToken(jToken) ? frameGroupDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null)
                return false;

            switch (jToken.Type) {
                case JTokenType.String:
                    Name = (string) jToken;
                    return true;

                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;
                        Name   = jObj.TryGetValue("Name",   out var name)   ? ((string) name) : null;
                        Frames = jObj.TryGetValue("Directions", out var frames)
                            ? frames.Select(x => FrameDef.FromJToken(x)).ToArray()
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

            if (Frames == null)
                return (Name != null) ? new JValue(Name) : null;

            var jObj = new JObject();
            if (Name != null)
                jObj.Add("Name", new JValue(Name));
            if (Frames != null)
                jObj.Add("Directions", JToken.FromObject(Frames.Select(x => x.ToJToken()).ToArray(), jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (Name != null ? Name + ": " : "") + ((Frames != null) ? string.Join(", ", Frames.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string Name;
        public FrameDef[] Frames;
    }
}
