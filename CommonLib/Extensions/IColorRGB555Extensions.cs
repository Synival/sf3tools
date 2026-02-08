using CommonLib.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class IColorRGB555Extensions {
        public static string ToJSON_String(this IColorRGB555 color)
            => color.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IColorRGB555 color) => ToJObject(color);
        public static JObject ToJObject(this IColorRGB555 color) {
            return new JObject {
                { "R", color.R },
                { "G", color.G },
                { "B", color.B },
            };
        }
    }
}
