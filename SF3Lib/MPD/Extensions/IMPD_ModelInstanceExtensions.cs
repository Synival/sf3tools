using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelInstanceExtensions {
        public static JToken ToJToken(this IMPD_ModelInstance instance) => instance.ToJObject();
        public static JObject ToJObject(this IMPD_ModelInstance instance) {
            return new JObject {
                // TODO: content!
            };
        }
    }
}
