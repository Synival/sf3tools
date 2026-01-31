using CommonLib.SGL;
using Newtonsoft.Json.Linq;

namespace CommonLib.Extensions {
    public static class IATTRExtensions {
        public static JObject ToJObject(this IATTR attributes) {
            return new JObject {
                { "Plane",          attributes.Plane },
                { "SortAndOptions", attributes.SortAndOptions },
                { "TextureNo",      attributes.TextureNo },
                { "Mode",           attributes.Mode },
                { "ColorNo",        attributes.ColorNo },
                { "GouraudShadingTable", attributes.GouraudShadingTable },
                { "Dir",            attributes.Dir },
            };
        }
    }
}
