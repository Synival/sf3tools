using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_PlanesExtensions {
        public static string ToJSON_String(this IMPD_Planes plane)
            => plane.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Planes plane) => plane.ToJObject();
        public static JObject ToJObject(this IMPD_Planes plane) {
            return new JObject(
                /* TODO:
                    ITextureData GroundImage { get; }
                    IMPD_TiledPlane GroundTiledImage { get; }
                    ITextureData BackgroundImage { get; }
                    ITextureData SkyImage { get; }
                    IMPD_TiledPlane ForegroundTiledImage { get; }
                */

                new JProperty("GroundX",         plane.GroundX),
                new JProperty("GroundY",         plane.GroundY),
                new JProperty("GroundZ",         plane.GroundZ),
                new JProperty("GroundXRotation", plane.GroundXRotation),
                new JProperty("BackgroundX",     plane.BackgroundX),
                new JProperty("BackgroundY",     plane.BackgroundY),

                new JProperty("GroundPalette",   plane.GroundPalette?.ToJToken()),
                new JProperty("SkyPalette",      plane.SkyPalette?.ToJToken())
            );
        }
    }
}
