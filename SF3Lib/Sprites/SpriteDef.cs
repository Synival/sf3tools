using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.Sprites {
    public class SpriteDef : IJsonResource {
        public SpriteDef() { }

        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="json">SpriteDef in JSON format as a string.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJSON(string json) {
            var spriteDef = new SpriteDef();
            return spriteDef.AssignFromJSON_String(json) ? spriteDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a SpriteDef.
        /// </summary>
        /// <param name="jToken">SpriteDef as a JToken.</param>
        /// <returns>A new SpriteDef if deserializing was successful, or 'null' if not.</returns>
        public static SpriteDef FromJToken(JToken jToken) {
            var spriteDef = new SpriteDef();
            return spriteDef.AssignFromJToken(jToken) ? spriteDef : null;
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

                        Name   = jObj.TryGetValue("Name",   out var name)   ? ((string) name) : null;
                        Width  = jObj.TryGetValue("Width",  out var width)  ? ((int?) width)  : null;
                        Height = jObj.TryGetValue("Height", out var height) ? ((int?) height) : null;

                        if (jObj.TryGetValue("Spritesheets", out var spritesheets) && spritesheets.Type == JTokenType.Object) {
                            Spritesheets = ((IDictionary<string, JToken>) spritesheets)
                                .ToDictionary(x => x.Key, x => {
                                    var size = Spritesheet.KeyToDimensions(x.Key);
                                    return Spritesheet.FromJToken(x.Value, size.Height);
                                });
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

            if (Name != null)
                jObj.Add("Name", new JValue(Name));
            if (Width.HasValue)
                jObj.Add("Width", new JValue(Width.Value));
            if (Height.HasValue)
                jObj.Add("Height", new JValue(Height.Value));
            if (Spritesheets != null) {
                jObj.Add("Spritesheets", JToken.FromObject(Spritesheets.ToDictionary(x => x.Key, x => {
                    var size = Spritesheet.KeyToDimensions(x.Key);
                    return x.Value.ToJToken(size.Height);
                })));
            }

            return jObj;
        }

        public override string ToString() => Name;

        public string Name;
        public int? Width;
        public int? Height;
        public Dictionary<string, Spritesheet> Spritesheets;
    }
}
