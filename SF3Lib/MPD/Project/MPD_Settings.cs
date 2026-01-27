using CommonLib.Imaging;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Settings : IMPD_Settings {
        public MPD_Settings() { }

        public MPD_Settings(IMPD_Settings original) {
            HasSurfaceModel               = original.HasSurfaceModel;
            ForceLowMemoryModels          = original.ForceLowMemoryModels;
            NarrowAngleBasedLightmap      = original.NarrowAngleBasedLightmap;
            SetMSBForGroundPalette        = original.SetMSBForGroundPalette;
            HasSurfaceTextureRotation     = original.HasSurfaceTextureRotation;
            AddDotProductBasedNoiseToStandardLightmap = original.AddDotProductBasedNoiseToStandardLightmap;
            KeepTexturelessFlatTiles      = original.KeepTexturelessFlatTiles;
            UnknownHeaderFlag             = original.UnknownHeaderFlag;

            ModelsYRotation               = original.ModelsYRotation;
            ModelsViewDistance            = original.ModelsViewDistance;
            ModelsViewAngleMin            = original.ModelsViewAngleMin;
            ModelsViewAngleMax            = original.ModelsViewAngleMax;
            UnknownHeaderSetting          = original.UnknownHeaderSetting;

            if (original.LightPaletteAdjustment != null)
                LightPaletteAdjustment = new ColorAdjustRGB555(original.LightPaletteAdjustment);
            if (original.GroundPaletteAdjustment != null)
                GroundPaletteAdjustment = new ColorAdjustRGB555(original.GroundPaletteAdjustment);

            ShadowTransparency            = original.ShadowTransparency;
            AreGroundAnimationsDummiedOut = original.AreGroundAnimationsDummiedOut;
            IsGradientDummiedOut          = original.IsGradientDummiedOut;
            IsUnknown2TableDummiedOut     = original.IsUnknown2TableDummiedOut;
            AreIgnoredTexturesDummiedOut  = original.AreIgnoredTexturesDummiedOut;
            IgnoreGroundImage             = original.IgnoreGroundImage;
            IgnoreGroundTiledImage        = original.IgnoreGroundTiledImage;
            IgnoreSkyImage                = original.IgnoreSkyImage;
            IgnoreBackgroundImage         = original.IgnoreBackgroundImage;
            IgnoreForegroundTiledImage    = original.IgnoreForegroundTiledImage;
            HasBattleBackground           = original.HasBattleBackground;
        }

        public bool HasSurfaceModel { get; set; }
        public bool ForceLowMemoryModels { get; set; }
        public bool NarrowAngleBasedLightmap { get; set; }
        public bool SetMSBForGroundPalette { get; set; }
        public bool HasSurfaceTextureRotation { get; set; }
        public bool AddDotProductBasedNoiseToStandardLightmap { get; set; }
        public bool KeepTexturelessFlatTiles { get; set; }
        public bool UnknownHeaderFlag { get; set; }
        public float ModelsYRotation { get; set; }
        public ushort ModelsViewDistance { get; set; }
        public float ModelsViewAngleMin { get; set; }
        public float ModelsViewAngleMax { get; set; }
        public short UnknownHeaderSetting { get; set; }
        public IColorAdjustRGB555 LightPaletteAdjustment { get; set; }
        public IColorAdjustRGB555 GroundPaletteAdjustment { get; set; }
        public byte ShadowTransparency { get; set; }
        public bool AreGroundAnimationsDummiedOut { get; set; }
        public bool IsGradientDummiedOut { get; set; }
        public bool IsUnknown2TableDummiedOut { get; set; }
        public bool AreIgnoredTexturesDummiedOut { get; set; }
        public bool IgnoreGroundImage { get; set; }
        public bool IgnoreGroundTiledImage { get; set; }
        public bool IgnoreSkyImage { get; set; }
        public bool IgnoreBackgroundImage { get; set; }
        public bool IgnoreForegroundTiledImage { get; set; }
        public bool HasBattleBackground { get; set; }
    }
}
