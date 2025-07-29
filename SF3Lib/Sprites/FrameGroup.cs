using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class FrameGroup : IJsonResource {
        public FrameGroup() { }

        public FrameGroup(UniqueFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction, x => new Frame(x));
        }

        public FrameGroup(StandaloneFrameDef[] frames) {
            Frames = frames.ToDictionary(x => x.Direction, x => new Frame(x));
        }

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
                case JTokenType.Object:
                    try {
                        Frames = ((IDictionary<string, JToken>) jToken)
                            .ToDictionary(x => (SpriteFrameDirection) Enum.Parse(typeof(SpriteFrameDirection), x.Key), x => Frame.FromJToken(x.Value));
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

        public JToken ToJToken()
            => Frames != null ? JToken.FromObject(Frames.ToDictionary(x => x.Key.ToString(), x => x.Value.ToJToken())) : null;

        public override string ToString() => string.Join(", ", Frames.Keys);

        public Dictionary<SpriteFrameDirection, Frame> Frames;
    }
}
