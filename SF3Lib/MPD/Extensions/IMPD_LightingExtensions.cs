using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_LightingExtensions {
        public static string ToJSON_String(this IMPD_Lighting lighting)
            => lighting.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Lighting lighting) {
            return null;
        }
    }
}
