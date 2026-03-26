using CommonLib.Imaging;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Lighting : IMPD_Lighting {
        public MPD_Lighting() {
            Palette = new Palette(0x20);
        }

        public MPD_Lighting(IMPD_Lighting original) {
            if (original.Palette != null)
                Palette = new Palette(original.Palette);
            Pitch = original.Pitch;
            Yaw   = original.Yaw;
        }

        public static MPD_Lighting FromJToken(JToken token) => new MPD_Lighting(token);
        private MPD_Lighting(JToken token) {
            var jObject = (JObject) token;
            Palette = CommonLib.Imaging.Palette.FromJToken(jObject["Palette"]);
            Pitch   = (float) jObject["Pitch"];
            Yaw     = (float) jObject["Yaw"];
        }

        public IPalette Palette { get; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
    }
}
