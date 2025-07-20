using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SF3.Models.Files.CHP;

namespace SF3.CHR {
    public class CHP_Def {
        public Dictionary<int, CHR_Def> CHRsByOffset;

        public ICHP_File ToCHP_File() {
            // TODO: deserialize!
            return null;
        }

        /// <summary>
        /// Converts the CHP_Def to a JSON object string.
        /// </summary>
        /// <returns>A string in JSON format.</returns>
        public string ToJSON_String() {
            var settings = new JsonSerializerSettings() {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
