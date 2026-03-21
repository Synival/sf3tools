using CommonLib.Imaging;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;

namespace SF3.Models.Structs.MPD {
    public class MPD_SettingsFromFile : IMPD_Settings {
        public MPD_SettingsFromFile(IMPD_File file) {
            MPD_File = file;
            _lightPaletteAdjustment  = new LightAdjustmentColor(file);
            _groundPaletteAdjustment = new GroundAdjustmentColor(file);
        }

        public IMPD_File MPD_File { get; }

        public bool HasSurfaceModel {
            get => MPD_File.SurfaceModelChunk != null;
            set {}
        }

        public bool ForceLowMemoryModels {
            get => MPD_File.Flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel;
            set => MPD_File.Flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel = value;
        }

        public bool NarrowAngleBasedLightmap {
            get => MPD_File.Flags.Bit_0x2000_NarrowAngleBasedLightmap;
            set => MPD_File.Flags.Bit_0x2000_NarrowAngleBasedLightmap = value;
        }

        public bool SetMSBForGroundPalette {
            get => MPD_File.Flags.Bit_0x0080_SetMSBForGroundPalette;
            set => MPD_File.Flags.Bit_0x0080_SetMSBForGroundPalette = value;
        }

        public bool HasSurfaceTextureRotation {
            get => MPD_File.Flags.Bit_0x0002_HasSurfaceTextureRotation;
            set => MPD_File.Flags.Bit_0x0002_HasSurfaceTextureRotation = value;
        }

        public bool AddDotProductBasedNoiseToStandardLightmap {
            get => MPD_File.Flags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap;
            set => MPD_File.Flags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap = value;
        }

        public bool KeepTexturelessFlatTiles {
            get => MPD_File.Flags.Bit_0x0008_KeepTexturelessFlatTiles;
            set => MPD_File.Flags.Bit_0x0008_KeepTexturelessFlatTiles = value;
        }

        public bool UnknownHeaderFlag {
            get => MPD_File.Flags.Bit_0x0020_Unknown;
            set => MPD_File.Flags.Bit_0x0020_Unknown = value;
        }

        public float ModelsYRotation {
            get => MPD_File.MPDHeader.ModelsYRotation;
            set => MPD_File.MPDHeader.ModelsYRotation = value;
        }

        public ushort ModelsViewDistance {
            get => MPD_File.MPDHeader.ModelsViewDistance;
            set => MPD_File.MPDHeader.ModelsViewDistance = value;
        }

        public float ModelsViewAngleMin {
            get => MPD_File.MPDHeader.ModelsViewAngleMin;
            set => MPD_File.MPDHeader.ModelsViewAngleMin = value;
        }

        public float ModelsViewAngleMax {
            get => MPD_File.MPDHeader.ModelsViewAngleMax;
            set => MPD_File.MPDHeader.ModelsViewAngleMax = value;
        }

        public short UnknownHeaderSetting {
            get => MPD_File.MPDHeader.Unknown1;
            set => MPD_File.MPDHeader.Unknown1 = value;
        }

        private class LightAdjustmentColor : IColorAdjustRGB555 {
            public LightAdjustmentColor(IMPD_File file) {
                MPD_File = file;
            }

            public sbyte R {
                get => (sbyte) (MPD_File.PaletteAdjustment?.LightRAdjustment ?? 0);
                set { if (MPD_File.PaletteAdjustment != null) MPD_File.PaletteAdjustment.LightRAdjustment = value; }
            }

            public sbyte G {
                get => (sbyte) (MPD_File.PaletteAdjustment?.LightGAdjustment ?? 0);
                set { if (MPD_File.PaletteAdjustment != null) MPD_File.PaletteAdjustment.LightGAdjustment = value; }
            }

            public sbyte B {
                get => (sbyte) (MPD_File.PaletteAdjustment?.LightBAdjustment ?? 0);
                set { if (MPD_File.PaletteAdjustment != null) MPD_File.PaletteAdjustment.LightBAdjustment = value; }
            }

            public IMPD_File MPD_File { get; }
        }

        private LightAdjustmentColor _lightPaletteAdjustment;
        public IColorAdjustRGB555 LightPaletteAdjustment {
            get => _lightPaletteAdjustment;
            set {
                if (value != null) {
                    _lightPaletteAdjustment.R = value.R;
                    _lightPaletteAdjustment.G = value.G;
                    _lightPaletteAdjustment.B = value.B;
                }
            }
        }

        private class GroundAdjustmentColor : IColorAdjustRGB555 {
            public GroundAdjustmentColor(IMPD_File file) {
                MPD_File = file;
            }

            public sbyte R {
                get => (sbyte) (MPD_File.PaletteAdjustment?.GroundRAdjustment ?? 0);
                set { if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)  MPD_File.PaletteAdjustment.GroundRAdjustment = value; }
            }

            public sbyte G {
                get => (sbyte) (MPD_File.PaletteAdjustment?.GroundGAdjustment ?? 0);
                set { if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true) MPD_File.PaletteAdjustment.GroundGAdjustment = value; }
            }

            public sbyte B {
                get => (sbyte) (MPD_File.PaletteAdjustment?.GroundBAdjustment ?? 0);
                set { if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true) MPD_File.PaletteAdjustment.GroundBAdjustment = value; }
            }

            public IMPD_File MPD_File { get; }
        }

        private GroundAdjustmentColor _groundPaletteAdjustment;
        public IColorAdjustRGB555 GroundPaletteAdjustment {
            get => _groundPaletteAdjustment;
            set {
                if (value != null) {
                    _groundPaletteAdjustment.R = value.R;
                    _groundPaletteAdjustment.G = value.G;
                    _groundPaletteAdjustment.B = value.B;
                }
            }
        }

        public byte ShadowTransparency {
            get => (byte) (MPD_File.PaletteAdjustment?.ShadowTransparency ?? 0x0F);
            set {
                if (MPD_File.PaletteAdjustment?.HasShadowTransparency == true)
                    MPD_File.PaletteAdjustment.ShadowTransparency = value;
            }
        }

        public bool AreGroundAnimationsDummiedOut {
            get => MPD_File.GroundAnimationTable?.IsDummiedOut == true;
            set {}
        }

        public bool IsGradientDummiedOut {
            get => (MPD_File.GradientTable?.Count == 1) && MPD_File.GradientTable[0].IsDummiedOut;
            set {}
        }

        public bool IsUnknown2TableDummiedOut {
            get => MPD_File.Unknown2Table?.IsDummiedOut == true;
            set {}
        }

        public bool AreIgnoredTexturesDummiedOut {
            get => MPD_File.IgnoredTextureTable?.IsDummiedOut == true;
            set {}
        }

        public bool IgnoreGroundImage {
            get => !MPD_File.Flags.Bit_0x0400_HasGroundImage && MPD_File.Planes?.GroundImage != null;
            set {}
        }

        public bool IgnoreGroundTiledImage {
            get => !MPD_File.Flags.Bit_0x1000_HasTileBasedGroundImage && MPD_File.Planes?.GroundTiledImage != null;
            set {}
        }

        public bool IgnoreSkyImage {
            get => !MPD_File.Flags.HasAnySky && MPD_File.Planes?.SkyImage != null;
            set {}
        }

        public bool IgnoreBackgroundImage {
            get => !MPD_File.Flags.Bit_0x0040_HasBackgroundImage && MPD_File.Planes?.BackgroundImage != null;
            set {}
        }

        public bool IgnoreForegroundTiledImage {
            get => !MPD_File.Flags.Bit_0x0010_HasTileBasedForegroundImage && MPD_File.Planes?.ForegroundTiledImage != null;
            set {}
        }

        public bool HasBattleBackground {
            get => MPD_File.Flags.HasAnySky;
            set {}
        }

        public bool IgnoreSurfaceModel {
            get => !MPD_File.Flags.Bit_0x0200_HasSurfaceModel && MPD_File.SurfaceModelChunk != null;
            set {}
        }

        public bool IsUnknown2InLaterFileAfterGradient {
            get => MPD_File.Scenario >= Types.ScenarioType.Scenario2 && MPD_File.Unknown2Table?.IsDummiedOut == true;
            set {}
        }
    }
}
