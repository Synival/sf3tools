using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class FrameGroupDef {
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
        public static FrameGroupDef FromJSON(string json)
            => FromJToken(JToken.Parse(json));

        /// <summary>
        /// Deserializes a JSON object of a FrameGroupDef.
        /// </summary>
        /// <param name="jToken">FrameGroupDef as a JToken.</param>
        /// <returns>A new FrameGroupDef if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroupDef FromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return null;

            try {
                var jObj = (JObject) jToken;
                var newDef = new FrameGroupDef();

                if (jObj.TryGetValue("Frames", out var frames) && frames.Type == JTokenType.Object) {
                    newDef.Frames = ((IDictionary<string, JToken>) frames)
                        .ToDictionary(x => x.Key, x => FrameDef.FromJToken(x.Value));
                }

                return newDef;
            }
            catch {
                return null;
            }
        }

        public override string ToString() => string.Join(", ", Frames.Keys);

        public Dictionary<string, FrameDef> Frames;
    }
}
