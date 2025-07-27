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
            if (jToken == null || jToken.Type != JTokenType.Object)
                return false;

            try {
                var jObj = (JObject) jToken;

                Name   = jObj.TryGetValue("Name",   out var name)   ? ((string) name) : null;
                Width  = jObj.TryGetValue("Width",  out var width)  ? ((int?) width)  : null;
                Height = jObj.TryGetValue("Height", out var height) ? ((int?) height) : null;

                Frames = jObj.TryGetValue("Frames", out var frames)
                    ? frames.Select(x => FrameDef.FromJToken(x)).ToArray()
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

            if (Name != null)
                jObj.Add("Name", new JValue(Name));
            if (Width.HasValue)
                jObj.Add("Width", new JValue(Width.Value));
            if (Height.HasValue)
                jObj.Add("Height", new JValue(Height.Value));
            if (Frames != null)
                jObj.Add("Frames", JToken.FromObject(Frames.Select(x => x.ToJToken()).ToArray(), jsonSettings));

            return jObj;
        }

        public override string ToString()
            => (Name != null ? Name + ": " : "") + ((Frames != null) ? string.Join(", ", Frames.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string Name;
        public int? Width;
        public int? Height;
        public FrameDef[] Frames;
    }
}
