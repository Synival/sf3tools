using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelExtensions {
        public static JToken ToJToken(this IMPD_Model model) => model.ToJObject();
        public static JObject ToJObject(this IMPD_Model model) {
            return new JObject {
                // TODO: content!
            };
        }
    }
}
