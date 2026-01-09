using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPD_Settings : IMPD_Settings {
        public MPD_Settings(IMPD_File file) {
            MPD_File = file;
        }

        public IMPD_File MPD_File { get; }

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

        public bool ShortEmptyAnimationTable {
            get => MPD_File.Scenario == ScenarioType.Scenario1 && MPD_File.MPDHeader.OffsetUnknown2 - MPD_File.MPDHeader.OffsetAnimations == 0x02;
            set {}
        }

        public bool LongEmptyIgnoredTextureTable {
            get => MPD_File.Scenario == ScenarioType.Scenario1 && MPD_File.MPDHeader.OffsetGroundPalette - MPD_File.MPDHeader.OffsetIgnoredTextures == 0x04;
            set {}
        }

        public bool HasLightAdjustment => MPD_File.PaletteAdjustment?.HasLightAdjustment ?? false;
        public bool HasGroundAdjustment => MPD_File.PaletteAdjustment?.HasGroundAdjustment ?? false;
        public bool HasShadowTransparency => MPD_File.PaletteAdjustment?.HasShadowTransparency ?? false;

        public short LightRAdjustment {
            get => MPD_File.PaletteAdjustment?.LightRAdjustment ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasLightAdjustment == true)
                    MPD_File.PaletteAdjustment.LightRAdjustment = value;
            }
        }

        public short LightGAdjustment {
            get => MPD_File.PaletteAdjustment?.LightGAdjustment ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasLightAdjustment == true)
                    MPD_File.PaletteAdjustment.LightGAdjustment = value;
            }
        }

        public short LightBAdjustment {
            get => MPD_File.PaletteAdjustment?.LightBAdjustment ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasLightAdjustment == true)
                    MPD_File.PaletteAdjustment.LightBAdjustment = value;
            }
        }

        public short GroundRAdjustment {
            get => MPD_File.PaletteAdjustment?.GroundRAdjustment ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)
                    MPD_File.PaletteAdjustment.GroundRAdjustment = value;
            }
        }

        public short GroundGAdjustment {
            get => MPD_File.PaletteAdjustment?.GroundGAdjustment ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)
                    MPD_File.PaletteAdjustment.GroundGAdjustment = value;
            }
        }

        public short GroundBAdjustment {
            get => MPD_File.PaletteAdjustment?.GroundBAdjustment ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)
                    MPD_File.PaletteAdjustment.GroundBAdjustment = value;
            }
        }

        public ushort ShadowTransparency {
            get => MPD_File.PaletteAdjustment?.ShadowTransparency ?? 0;
            set {
                if (MPD_File.PaletteAdjustment?.HasShadowTransparency == true)
                    MPD_File.PaletteAdjustment.ShadowTransparency = value;
            }
        }
    }
}
