using CommonLib.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class IRectangleShortExtensions {
        public static string ToJSON_String(this IRectangleShort rect)
            => rect.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IRectangleShort rect) => rect.ToJObject();
        public static JObject ToJObject(this IRectangleShort rect) {
            return new JObject {
                { "X1", new JValue(rect.X1) },
                { "Y1", new JValue(rect.Y1) },
                { "X2", new JValue(rect.X2) },
                { "Y2", new JValue(rect.Y2) },
            };
        }
    }
}
