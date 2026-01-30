using System;
using Newtonsoft.Json.Linq;

namespace CommonLib.Imaging {
    public class ColorAdjustRGB555 : IColorAdjustRGB555 {
        public ColorAdjustRGB555() { }

        public ColorAdjustRGB555(IColorAdjustRGB555 original) {
            R = original.R;
            G = original.G;
            B = original.B;
        }

        public ColorAdjustRGB555(sbyte r, sbyte g, sbyte b) {
            R = r;
            G = g;
            B = b;
        }

        public static ColorAdjustRGB555 FromJToken(JToken token) => new ColorAdjustRGB555(token);
        public ColorAdjustRGB555(JToken token) {
            var jObject = (JObject) token;

            R = (sbyte) jObject["R"];
            G = (sbyte) jObject["G"];
            B = (sbyte) jObject["B"];
        }

        public sbyte R { get; set; }
        public sbyte G { get; set; }
        public sbyte B { get; set; }
    }
}
