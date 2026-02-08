using CommonLib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_GradientExtensions {
        public static string ToJSON_String(this IMPD_Gradient gradient)
            => gradient.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Gradient gradient) => gradient.ToJObject();
        public static JObject ToJObject(this IMPD_Gradient gradient) {
            return new JObject {
                { "TopPosition",               gradient.TopPosition },
                { "BottomPosition",            gradient.BottomPosition },
                { "TopColor",                  gradient.TopColor.ToJObject() },
                { "BottomColor",               gradient.BottomColor.ToJObject() },
                { "AffectsModelsAndSurface",   gradient.AffectsModelsAndSurface },
                { "ModelsAndSurfaceIntensity", gradient.ModelsAndSurfaceIntensity },
                { "AffectsGround",             gradient.AffectsGround },
                { "GroundIntensity",           gradient.GroundIntensity },
                { "AffectsSky",                gradient.AffectsSky },
                { "SkyIntensity",              gradient.SkyIntensity },
            };
        }
    }
}
