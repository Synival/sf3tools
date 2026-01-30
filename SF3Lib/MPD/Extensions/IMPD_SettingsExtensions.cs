using CommonLib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_SettingsExtensions {
        public static string ToJSON_String(this IMPD_Settings settings)
            => settings.ToJToken().ToString(Formatting.Indented);

        public static JToken ToJToken(this IMPD_Settings settings) => settings.ToJObject();
        public static JObject ToJObject(this IMPD_Settings settings) {
            return new JObject {
                { "HasSurfaceModel",                           settings.HasSurfaceModel },
                { "ForceLowMemoryModels",                      settings.ForceLowMemoryModels },
                { "NarrowAngleBasedLightmap",                  settings.NarrowAngleBasedLightmap },
                { "SetMSBForGroundPalette",                    settings.SetMSBForGroundPalette },
                { "HasSurfaceTextureRotation",                 settings.HasSurfaceTextureRotation },
                { "AddDotProductBasedNoiseToStandardLightmap", settings.AddDotProductBasedNoiseToStandardLightmap },
                { "KeepTexturelessFlatTiles",                  settings.KeepTexturelessFlatTiles },
                { "UnknownHeaderFlag",                         settings.UnknownHeaderFlag },
                { "ModelsYRotation",                           settings.ModelsYRotation },
                { "ModelsViewDistance",                        settings.ModelsViewDistance },
                { "ModelsViewAngleMin",                        settings.ModelsViewAngleMin },
                { "ModelsViewAngleMax",                        settings.ModelsViewAngleMax },
                { "UnknownHeaderSetting",                      settings.UnknownHeaderSetting },
                { "LightPaletteAdjustment",                    settings.LightPaletteAdjustment?.ToJToken() },
                { "GroundPaletteAdjustment",                   settings.GroundPaletteAdjustment?.ToJToken() },
                { "ShadowTransparency",                        settings.ShadowTransparency },
                { "AreGroundAnimationsDummiedOut",             settings.AreGroundAnimationsDummiedOut },
                { "IsGradientDummiedOut",                      settings.IsGradientDummiedOut },
                { "IsUnknown2TableDummiedOut",                 settings.IsUnknown2TableDummiedOut },
                { "AreIgnoredTexturesDummiedOut",              settings.AreIgnoredTexturesDummiedOut },
                { "IgnoreGroundImage",                         settings.IgnoreGroundImage },
                { "IgnoreGroundTiledImage",                    settings.IgnoreGroundTiledImage },
                { "IgnoreSkyImage",                            settings.IgnoreSkyImage },
                { "IgnoreBackgroundImage",                     settings.IgnoreBackgroundImage },
                { "IgnoreForegroundTiledImage",                settings.IgnoreForegroundTiledImage },
                { "HasBattleBackground",                       settings.HasBattleBackground },
                { "IgnoreSurfaceModel",                        settings.IgnoreSurfaceModel },
                { "IsUnknown2InLaterFileAfterGradient",        settings.IsUnknown2InLaterFileAfterGradient },
            };
        }
    }
}
