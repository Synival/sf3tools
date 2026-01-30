using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_BinaryReproductionFlagsExtensions {
        public static string ToJSON_String(this IMPD_BinaryReproductionFlags flags)
            => flags.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_BinaryReproductionFlags flags) => flags.ToJObject();
        public static JObject ToJObject(this IMPD_BinaryReproductionFlags flags) {
            return null;
        }
    }
}
