using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;
using SF3.Utils;

namespace SF3.CHR {
    public class FrameGroup : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a FrameGroup.
        /// </summary>
        /// <param name="json">FrameGroup in JSON format as a string.</param>
        /// <returns>A new FrameGroup if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroup FromJSON(string json) {
            var frameGroup = new FrameGroup();
            return frameGroup.AssignFromJSON_String(json) ? frameGroup : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a FrameGroup.
        /// </summary>
        /// <param name="jToken">FrameGroup as a JToken.</param>
        /// <returns>A new FrameGroup if deserializing was successful, or 'null' if not.</returns>
        public static FrameGroup FromJToken(JToken jToken) {
            var frameGroup = new FrameGroup();
            return frameGroup.AssignFromJToken(jToken) ? frameGroup : null;
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
                            ? frames.Select(x => Frame.FromJToken(x)).ToArray()
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

        /// <summary>
        /// Returns 'Frames' if available or a generated set of frames based on the number of directions for the requested sprite.
        /// </summary>
        /// <param name="directions">The set of Frame directions if 'Frames' does not exist.</param>
        /// <returns>A non-null array of Frame's.</returns>
        public Frame[] GetFrames(SpriteDirectionCountType directions) {
            return Frames ?? CHR_Utils.GetCHR_FrameGroupDirections(directions)
                .Select(x => new Frame() { Direction = x })
                .ToArray();
        }

        public string Name;
        public Frame[] Frames;
    }
}
