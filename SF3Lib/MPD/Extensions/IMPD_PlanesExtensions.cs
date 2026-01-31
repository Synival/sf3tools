using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_PlanesExtensions {
        public static string ToJSON_String(this IMPD_Planes plane)
            => plane.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Planes plane) => plane.ToJObject();
        public static JObject ToJObject(this IMPD_Planes plane) {
            return new JObject {
                /* TODO:
                    ITextureData GroundImage { get; }
                    IMPD_TiledPlane GroundTiledImage { get; }
                    ITextureData BackgroundImage { get; }
                    ITextureData SkyImage { get; }
                    IMPD_TiledPlane ForegroundTiledImage { get; }
                */

                { "GroundX",         plane.GroundX },
                { "GroundY",         plane.GroundY },
                { "GroundZ",         plane.GroundZ },
                { "GroundXRotation", plane.GroundXRotation },
                { "BackgroundX",     plane.BackgroundX },
                { "BackgroundY",     plane.BackgroundY },

                { "GroundPalette",   plane.GroundPalette?.ToJArray() },
                { "SkyPalette",      plane.SkyPalette?.ToJArray() },
            };
        }
    }
}
