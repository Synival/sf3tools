using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Extensions;
using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD.Extensions {
    public static class IMPD_ModelCollectionExtensions {
        public static bool IsHeaderModelCollection(this IMPD_ModelCollection mc)
            => mc.Collection.IsHeaderModelCollection();

        public static string ToJSON_String(this IMPD_ModelCollection mc)
            => mc.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_ModelCollection mc) => mc.ToJObject();
        public static JObject ToJObject(this IMPD_ModelCollection mc) {
            return null;
        }
    }
}
