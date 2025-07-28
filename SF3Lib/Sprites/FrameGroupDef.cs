using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class FrameGroupDef : IJsonResource {
        public FrameGroupDef() { }

        public FrameGroupDef(UniqueFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction.ToString(), x => new FrameDef(x));
        }

        public FrameGroupDef(StandaloneFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction.ToString(), x => new FrameDef(x));
        }

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
                case JTokenType.Object:
                    try {
                        var jObj = (JObject) jToken;

                        if (jObj.TryGetValue("Frames", out var frames) && frames.Type == JTokenType.Object) {
                            Frames = ((IDictionary<string, JToken>) frames)
                                .ToDictionary(x => x.Key, x => FrameDef.FromJToken(x.Value));
                        }
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
            var jObj = new JObject();
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            if (Frames != null)
                jObj.Add("Frames", JToken.FromObject(Frames, jsonSettings));

            return jObj;
        }

        public override string ToString() => string.Join(", ", Frames.Keys);

        public Dictionary<string, FrameDef> Frames;
    }
}
