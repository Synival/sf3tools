using System.Linq;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class FrameGroupDef {
        public override string ToString()
            => (Name != null ? Name + ": " : "") + ((Frames != null) ? string.Join(", ", Frames.Select(x => "{" + x.ToString() + "}")) : "[]");

        public string Name;
        public int? Width;
        public int? Height;
        public FrameDef[] Frames;

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

                newDef.Name   = jObj.TryGetValue("Name",   out var name)   ? ((string) name) : null;
                newDef.Width  = jObj.TryGetValue("Width",  out var width)  ? ((int?) width)  : null;
                newDef.Height = jObj.TryGetValue("Height", out var height) ? ((int?) height) : null;

                newDef.Frames = jObj.TryGetValue("Frames", out var frames)
                    ? frames.Select(x => FrameDef.FromJToken(x)).ToArray()
                    : null;

                return newDef;
            }
            catch {
                return null;
            }
        }
    }
}
