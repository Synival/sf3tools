using System;
using CommonLib.Imaging;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Gradient : IMPD_Gradient {
        public MPD_Gradient(IMPD_Gradient original) {
            TopPosition               = original.TopPosition;
            BottomPosition            = original.BottomPosition;
            AffectsModelsAndSurface   = original.AffectsModelsAndSurface;
            ModelsAndSurfaceIntensity = original.ModelsAndSurfaceIntensity;
            AffectsGround             = original.AffectsGround;
            GroundIntensity           = original.GroundIntensity;
            AffectsSky                = original.AffectsSky;
            SkyIntensity              = original.SkyIntensity;

            if (original.TopColor != null)
                TopColor = new ColorRGB555(original.TopColor);
            if (original.BottomColor != null)
                BottomColor = new ColorRGB555(original.BottomColor);

        }

        public static IMPD_Gradient FromJToken(JToken token) => new MPD_Gradient(token);
        private MPD_Gradient(JToken token) {
            var jObject = (JObject) token;

            TopPosition               = (float) token["TopPosition"];
            BottomPosition            = (float) token["BottomPosition"];
            TopColor                  = ColorRGB555.FromJToken(token["TopColor"]);
            BottomColor               = ColorRGB555.FromJToken(token["BottomColor"]);
            AffectsModelsAndSurface   =  (bool) token["AffectsModelsAndSurface"];
            ModelsAndSurfaceIntensity = (float) token["ModelsAndSurfaceIntensity"];
            AffectsGround             =  (bool) token["AffectsGround"];
            GroundIntensity           = (float) token["GroundIntensity"];
            AffectsSky                =  (bool) token["AffectsSky"];
            SkyIntensity              = (float) token["SkyIntensity"];
        }

        public float TopPosition { get; set; }
        public float BottomPosition { get; set; }
        public IColorRGB555 TopColor { get; set; }
        public IColorRGB555 BottomColor { get; set; }
        public bool AffectsModelsAndSurface { get; set; }
        public float ModelsAndSurfaceIntensity { get; set; }
        public bool AffectsGround { get; set; }
        public float GroundIntensity { get; set; }
        public bool AffectsSky { get; set; }
        public float SkyIntensity { get; set; }
    }
}
