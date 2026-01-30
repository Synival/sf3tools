using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelSwitchGroupExtensions {
        public static string ToJSON_String(this IMPD_ModelSwitchGroup msg)
            => msg.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_ModelSwitchGroup msg) => msg.ToJObject();
        public static JObject ToJObject(this IMPD_ModelSwitchGroup msg) {
            return new JObject(
                new JProperty("Flag", msg.Flag),
                new JProperty("ModelInstancesVisibleWhenOff", msg.ModelInstancesVisibleWhenOff?.ToArray()),
                new JProperty("ModelInstancesVisibleWhenOn",  msg.ModelInstancesVisibleWhenOn?.ToArray())
            );
        }
    }
}
