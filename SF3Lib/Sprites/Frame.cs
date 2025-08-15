using System;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SF3.Types;

namespace SF3.Sprites {
    public class Frame : IJsonResource {
        public Frame() { }

        /// <summary>
        /// Deserializes a JSON object of a Frame.
        /// </summary>
        /// <param name="json">Frame in JSON format as a string.</param>
        /// <returns>A new Frame if deserializing was successful, or 'null' if not.</returns>
        public static Frame FromJSON(string json) {
            var frame = new Frame();
            return frame.AssignFromJSON_String(json) ? frame : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a Frame.
        /// </summary>
        /// <param name="jToken">Frame as a JToken.</param>
        /// <returns>A new Frame if deserializing was successful, or 'null' if not.</returns>
        public static Frame FromJToken(JToken jToken) {
            var frame = new Frame();
            return frame.AssignFromJToken(jToken) ? frame : null;
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
                        SpritesheetX = jObj.TryGetValue("SpritesheetX", out var spritesheetX) ? ((int) spritesheetX) : -1;
                        SpritesheetY = jObj.TryGetValue("SpritesheetY", out var spritesheetY) ? ((int) spritesheetY) : -1;
                        Coding       = (jObj.TryGetValue("Coding", out var coding) && Enum.TryParse<SpriteImageCodingType>(coding.ToString(), out var codingType)) ? codingType : SpriteImageCodingType.On;
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
            var jObj = new JObject() {
                { "SpritesheetX", new JValue(SpritesheetX) },
                { "SpritesheetY", new JValue(SpritesheetY) },
            };
            if (Coding != SpriteImageCodingType.On)
                jObj.Add("Coding", Coding.ToString());

            return jObj;
        }

        public override string ToString() => $"({SpritesheetX}, {SpritesheetY})";

        public int SpritesheetX;
        public int SpritesheetY;

        [JsonConverter(typeof(StringEnumConverter))]
        public SpriteImageCodingType Coding;
    }
}
