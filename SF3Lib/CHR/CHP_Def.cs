using System.Collections.Generic;
using System.Linq;
using CommonLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SF3.CHR {
    public class CHP_Def : IJsonResource {
        /// <summary>
        /// Deserializes a JSON object of a CHP_Def.
        /// </summary>
        /// <param name="json">CHP_Def in JSON format as a string.</param>
        /// <returns>A new CHP_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHP_Def FromJSON(string json) {
            var chpDef = new CHP_Def();
            return chpDef.AssignFromJSON_String(json) ? chpDef : null;
        }

        /// <summary>
        /// Deserializes a JSON object of a CHP_Def.
        /// </summary>
        /// <param name="jToken">CHP_Def as a JToken.</param>
        /// <returns>A new CHP_Def if deserializing was successful, or 'null' if not.</returns>
        public static CHP_Def FromJToken(JToken jToken) {
            var chpDef = new CHP_Def();
            return chpDef.AssignFromJToken(jToken) ? chpDef : null;
        }

        public bool AssignFromJSON_String(string json)
            => AssignFromJToken(JToken.Parse(json));

        public bool AssignFromJToken(JToken jToken) {
            if (jToken == null || jToken.Type != JTokenType.Object)
                return false;

            try {
                var jObj = (JObject) jToken;
                TotalSectors = jObj.TryGetValue("TotalSectors", out var totalSectors) ? ((int?) totalSectors ?? 0) : 0;

                if (jObj.TryGetValue("CHRs", out var chrsOut) && chrsOut.Type == JTokenType.Array) {
                    CHRs = ((JArray) chrsOut)
                        .Select(x => CHR_Def.FromJToken(x))
                        .ToArray();
                }
                else if (jObj.TryGetValue("CHRsBySector", out var chrsBySectorOut) && chrsBySectorOut.Type == JTokenType.Object) {
                    CHRs = ((IDictionary<string, JToken>) ((JObject) chrsBySectorOut))
                        .Where(x => int.TryParse(x.Key, out _))
                        .Select(x => {
                            var chrDef = CHR_Def.FromJToken(x.Value);
                            chrDef.Sector = int.Parse(x.Key);
                            return chrDef;
                        })
                        .ToArray();
                }

                return true;
            }
            catch {
                return false;
            }
        }

        public string ToJSON_String()
            => ToJToken().ToString(Formatting.Indented);

        public JToken ToJToken() {
            var jsonSettings = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };

            return new JObject {
                { "TotalSectors", new JValue(TotalSectors) },
                { "CHRs", JArray.FromObject(CHRs.Select(x => x.ToJToken()).ToArray(), jsonSettings) },
            };
        }

        public int TotalSectors;
        public CHR_Def[] CHRs;
    }
}
