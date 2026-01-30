using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_LightingExtensions {
        public static string ToJSON_String(this IMPD_Lighting lighting)
            => lighting.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Lighting lighting) => lighting.ToJObject();
        public static JObject ToJObject(this IMPD_Lighting lighting) {
            return new JObject(
                new JProperty("Palette", lighting.Palette.ToJArray()),
                new JProperty("Pitch",   lighting.Pitch),
                new JProperty("Yaw",     lighting.Yaw)
            );
        }
    }
}
