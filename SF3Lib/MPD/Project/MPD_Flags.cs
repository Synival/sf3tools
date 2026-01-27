using SF3.MPD.Interfaces;
using SF3.MPD.Interfaces.Flags;
using SF3.Types;

namespace SF3.MPD.Project {
    public class MPD_Flags : IMPD_EditableFlags {
        public MPD_Flags(IMPD mpd) {
            MPD = mpd;
        }

        public IMPD MPD { get; }

        public bool CanSet_0x0001_Unknown => false;
        public bool CanSet_0x0002_Unknown => false;
        public bool CanSet_0x0002_HasSurfaceTextureRotation => false;
        public bool CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap => false;
        public bool CanSet_0x0008_KeepTexturelessFlatTiles => false;
        public bool CanSet_0x0010_HasTileBasedForegroundImage => false;
        public bool CanSet_0x0020_Unknown => false;
        public bool CanSet_0x0040_HasBackgroundImage => false;
        public bool CanSet_0x0080_HasChunk19ModelWithChunk10Textures => false;
        public bool CanSet_0x0080_SetMSBForGroundPalette => false;
        public bool CanSet_0x0100_HasModels => false;
        public bool CanSet_0x0200_HasSurfaceModel => false;
        public bool CanSet_0x0400_HasGroundImage => false;
        public bool CanSet_0x0800_Unused => false;
        public bool CanSet_0x0800_HasCutsceneSky => false;
        public bool CanSet_0x1000_HasTileBasedGroundImage => false;
        public bool CanSet_0x2000_HasBattleSky => false;
        public bool CanSet_0x2000_NarrowAngleBasedLightmap => false;
        public bool CanSet_0x4000_Unused => false;
        public bool CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures => false;
        public bool CanSet_0x8000_ModelsAreStillLowMemoryWithSurfaceModel => false;

        public bool Bit_0x0001_Unknown { get => true; set {} }
        public bool Bit_0x0002_Unknown { get => true; set {} }
        public bool Bit_0x0002_HasSurfaceTextureRotation { get => MPD.Settings.HasSurfaceTextureRotation; set {} }
        public bool Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap { get => false; set {} }
        public bool Bit_0x0008_KeepTexturelessFlatTiles { get => false; set {} }
        public bool Bit_0x0010_HasTileBasedForegroundImage { get => MPD.Planes.ForegroundTiledImage != null; set {} }
        public bool Bit_0x0020_Unknown { get => false; set {} }
        public bool Bit_0x0040_HasBackgroundImage { get => MPD.Planes.BackgroundImage != null; set {} }
        public bool Bit_0x0080_HasChunk19ModelWithChunk10Textures { get => HasExtraModel; set => HasExtraModel = value; }
        public bool Bit_0x0080_SetMSBForGroundPalette { get => MPD.Settings.SetMSBForGroundPalette; set {} }
        public bool Bit_0x0100_HasModels { get => MPD.ModelCollections.ContainsKey(MPD_CollectionType.Primary); set {} }
        public bool Bit_0x0200_HasSurfaceModel { get => MPD.Settings.HasSurfaceModel; set {} }
        public bool Bit_0x0400_HasGroundImage { get => !MPD.Settings.IgnoreGroundImage && MPD.Planes.GroundImage != null; set {} }
        public bool Bit_0x0800_Unused { get => false; set {} }
        public bool Bit_0x0800_HasCutsceneSky { get => !MPD.Settings.IgnoreSkyImage && MPD.Planes.SkyImage != null; set {} }
        public bool Bit_0x1000_HasTileBasedGroundImage { get => MPD.Planes.GroundTiledImage != null; set {} }
        public bool Bit_0x2000_HasBattleSky { get => !MPD.Settings.IgnoreSkyImage && MPD.Planes.SkyImage != null; set {} }
        public bool Bit_0x2000_NarrowAngleBasedLightmap { get => MPD.Settings.NarrowAngleBasedLightmap; set {} }
        public bool Bit_0x4000_Unused { get => false; set {} }
        public bool Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures { get => HasExtraModel; set => HasExtraModel = value; }
        public bool Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel { get => MPD.Settings.ForceLowMemoryModels; set {} }

        public bool HasAnySky => false;
        public bool HasExtraModel { get => MPD.ModelCollections.ContainsKey(MPD_CollectionType.ExtraModels); set {} }
    }
}
