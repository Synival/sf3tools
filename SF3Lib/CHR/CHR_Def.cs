using System.IO;
using System.Linq;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.Models.Files.CHR;
using SF3.Types;

namespace SF3.CHR {
    public class CHR_Def : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a CHR_Def.
        /// </summary>
        /// <param name="json">CHR_Def in JSON format as a string.</param>
        /// <returns>A new CHR_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHR_Def FromJSON(string json) {
            var chrDef = new CHR_Def();
            return chrDef.AssignFromJSON_String(json) ? chrDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a CHR_Def.
        /// </summary>
        /// <param name="jToken">CHR_Def as a JObject.</param>
        /// <returns>A new CHR_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHR_Def FromJToken(JToken jToken) {
            var chpDef = new CHR_Def();
            return chpDef.AssignFromJToken(jToken) ? chpDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return true;

            try {
                var jObj = (JObject) jToken;

                WriteFrameImagesBeforeTables = jObj.TryGetValue("WriteFrameImagesBeforeTables", out var wfi)     ? ((bool?) wfi)    : null;
                MaxSize                      = jObj.TryGetValue("MaxSize",                      out var maxSize) ? ((int?) maxSize) : null;
                JunkAfterFrameTables         = jObj.TryGetValue("JunkAfterFrameTables",         out var jaft)    ? ((byte[]) jaft)  : null;

                Sprites = jObj.TryGetValue("Sprites", out var sprites)
                    ? sprites.Select(x => SpriteDef.FromJToken(x)).ToArray()
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

            if (WriteFrameImagesBeforeTables.HasValue)
                jObj.Add("WriteFrameImagesBeforeTables", new JValue(WriteFrameImagesBeforeTables.Value));
            if (MaxSize.HasValue)
                jObj.Add("MaxSize", new JValue(MaxSize.Value));
            if (JunkAfterFrameTables != null)
                jObj.Add("JunkAfterFrameTables", JToken.FromObject(JunkAfterFrameTables, jsonSettings));
            if (Sprites != null)
                jObj.Add("Sprites", JArray.FromObject(Sprites.Select(x => x.ToJToken()).ToArray(), jsonSettings));

            return jObj;
        }

        public bool? WriteFrameImagesBeforeTables;
        public int? MaxSize;
        public byte[] JunkAfterFrameTables;
        public SpriteDef[] Sprites;
    }
}
