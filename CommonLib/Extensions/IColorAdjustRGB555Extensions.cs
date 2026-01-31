using CommonLib.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class IColorAdjustRGB555Extensions {
        public static string ToJSON_String(this IColorAdjustRGB555 color)
            => color.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IColorAdjustRGB555 color) => ToJObject(color);
        public static JObject ToJObject(this IColorAdjustRGB555 color) {
            return new JObject {
                { "R", color.R },
                { "G", color.G },
                { "B", color.B },
            };
        }
    }
}
