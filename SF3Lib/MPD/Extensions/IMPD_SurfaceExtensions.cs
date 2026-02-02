using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_SurfaceExtensions {
        public static string ToJSON_String(this IMPD_Surface surface)
            => surface.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Surface surface) => surface.ToJObject();
        public static JObject ToJObject(this IMPD_Surface surface) {
            JArray MakeByteTable(Func<int, int, byte> fetcher, int blankValue) {
                var width  = surface.Width;
                var height = surface.Height;

                var jArray = new JArray();
                for (int y = 0; y < height; y++) {
                    var str = new StringBuilder();
                    for (int x = 0; x < width; x++) {
                        var value = fetcher(x, y);
                        str.Append(value == blankValue ? "  " : value.ToString("X2"));
                    }
                    jArray.Add(str.ToString());
                }

                return jArray;
            };

            return new JObject {
                { "TextureIDs", MakeByteTable((x, y) => surface.GetTile(x, y).TextureID, 0xFF) },
                { "EventIDs",   MakeByteTable((x, y) => surface.GetTile(x, y).EventID, 0) },
            };
        }
    }
}
