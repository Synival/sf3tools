using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelSwitchGroupExtensions {
        public static string ToJSON_String(this IMPD_ModelSwitchGroup msg)
            => msg.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_ModelSwitchGroup msg) {
            return null;
        }
    }
}
