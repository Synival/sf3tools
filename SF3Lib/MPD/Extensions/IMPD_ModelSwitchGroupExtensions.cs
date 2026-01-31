using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommonLib.Extensions;
using SF3.MPD.Interfaces;
using System.Linq;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelSwitchGroupExtensions {
        public static string ToJSON_String(this IMPD_ModelSwitchGroup msg)
            => msg.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_ModelSwitchGroup msg) => msg.ToJObject();
        public static JObject ToJObject(this IMPD_ModelSwitchGroup msg) {
            return new JObject {
                { "Flag", msg.Flag },
                { "ModelInstancesVisibleWhenOff", msg.ModelInstancesVisibleWhenOff?.ToArray()?.ToJArray() },
                { "ModelInstancesVisibleWhenOn",  msg.ModelInstancesVisibleWhenOn?.ToArray()?.ToJArray() }
            };
        }
    }
}
