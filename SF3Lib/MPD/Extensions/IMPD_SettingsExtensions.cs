using CommonLib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_SettingsExtensions {
        public static string ToJSON_String(this IMPD_Settings settings)
            => settings.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Settings settings) {
            return new JObject {
                { "HasSurfaceModel",                           new JValue(settings.HasSurfaceModel) },
                { "ForceLowMemoryModels",                      new JValue(settings.ForceLowMemoryModels) },
                { "NarrowAngleBasedLightmap",                  new JValue(settings.NarrowAngleBasedLightmap) },
                { "SetMSBForGroundPalette",                    new JValue(settings.SetMSBForGroundPalette) },
                { "HasSurfaceTextureRotation",                 new JValue(settings.HasSurfaceTextureRotation) },
                { "AddDotProductBasedNoiseToStandardLightmap", new JValue(settings.AddDotProductBasedNoiseToStandardLightmap) },
                { "KeepTexturelessFlatTiles",                  new JValue(settings.KeepTexturelessFlatTiles) },
                { "UnknownHeaderFlag",                         new JValue(settings.UnknownHeaderFlag) },
                { "ModelsYRotation",                           new JValue(settings.ModelsYRotation) },
                { "ModelsViewDistance",                        new JValue(settings.ModelsViewDistance) },
                { "ModelsViewAngleMin",                        new JValue(settings.ModelsViewAngleMin) },
                { "ModelsViewAngleMax",                        new JValue(settings.ModelsViewAngleMax) },
                { "UnknownHeaderSetting",                      new JValue(settings.UnknownHeaderSetting) },
                { "LightPaletteAdjustment",                    settings.LightPaletteAdjustment?.ToJToken() },
                { "GroundPaletteAdjustment",                   settings.GroundPaletteAdjustment?.ToJToken() },
                { "ShadowTransparency",                        new JValue(settings.ShadowTransparency) },
                { "AreGroundAnimationsDummiedOut",             new JValue(settings.AreGroundAnimationsDummiedOut) },
                { "IsGradientDummiedOut",                      new JValue(settings.IsGradientDummiedOut) },
                { "IsUnknown2TableDummiedOut",                 new JValue(settings.IsUnknown2TableDummiedOut) },
                { "AreIgnoredTexturesDummiedOut",              new JValue(settings.AreIgnoredTexturesDummiedOut) },
                { "IgnoreGroundImage",                         new JValue(settings.IgnoreGroundImage) },
                { "IgnoreGroundTiledImage",                    new JValue(settings.IgnoreGroundTiledImage) },
                { "IgnoreSkyImage",                            new JValue(settings.IgnoreSkyImage) },
                { "IgnoreBackgroundImage",                     new JValue(settings.IgnoreBackgroundImage) },
                { "IgnoreForegroundTiledImage",                new JValue(settings.IgnoreForegroundTiledImage) },
                { "HasBattleBackground",                       new JValue(settings.HasBattleBackground) },
                { "IgnoreSurfaceModel",                        new JValue(settings.IgnoreSurfaceModel) },
                { "IsUnknown2InLaterFileAfterGradient",        new JValue(settings.IsUnknown2InLaterFileAfterGradient) },
            };
        }
    }
}
