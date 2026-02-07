using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_CollisionsExtensions {
        public static string ToJSON_String(this IMPD_Collisions collisions)
            => collisions.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Collisions collisions) => collisions.ToJObject();
        public static JObject ToJObject(this IMPD_Collisions collisions) {
            return new JObject {
                { "Points", JArray.FromObject(collisions.Points?.Select(x => x.ToJObject())?.ToArray()) },
                { "Lines",  JArray.FromObject(collisions.Lines?.Select(x => x.ToJObject())?.ToArray()) },
            };
        }
    }
}
