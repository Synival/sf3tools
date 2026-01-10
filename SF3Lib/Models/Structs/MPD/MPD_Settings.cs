using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;

namespace SF3.Models.Structs.MPD {
    public class MPD_Settings : IMPD_Settings {
        public MPD_Settings(IMPD_File file) {
            MPD_File = file;
            _lightPaletteAdjustment  = new LightAdjustmentColor(file);
            _groundPaletteAdjustment = new GroundAdjustmentColor(file);
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

        private class LightAdjustmentColor : IMPD_PaletteAdjustColor {
            public LightAdjustmentColor(IMPD_File file) {
                MPD_File = file;
            }

            public sbyte R {
                get => (sbyte) (MPD_File.PaletteAdjustment?.LightRAdjustment ?? 0);
                set {
                    if (MPD_File.PaletteAdjustment != null)
                        MPD_File.PaletteAdjustment.LightRAdjustment = value;
                }
            }

            public sbyte G {
                get => (sbyte) (MPD_File.PaletteAdjustment?.LightGAdjustment ?? 0);
                set {
                    if (MPD_File.PaletteAdjustment != null)
                        MPD_File.PaletteAdjustment.LightGAdjustment = value;
                }
            }

            public sbyte B {
                get => (sbyte) (MPD_File.PaletteAdjustment?.LightBAdjustment ?? 0);
                set {
                    if (MPD_File.PaletteAdjustment != null)
                        MPD_File.PaletteAdjustment.LightBAdjustment = value;
                }
            }

            public IMPD_File MPD_File { get; }
        }

        private class GroundAdjustmentColor : IMPD_PaletteAdjustColor {
            public GroundAdjustmentColor(IMPD_File file) {
                MPD_File = file;
            }

            public sbyte R {
                get => (sbyte) (MPD_File.PaletteAdjustment?.GroundRAdjustment ?? 0);
                set {
                    if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)
                        MPD_File.PaletteAdjustment.GroundRAdjustment = value;
                }
            }

            public sbyte G {
                get => (sbyte) (MPD_File.PaletteAdjustment?.GroundGAdjustment ?? 0);
                set {
                    if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)
                        MPD_File.PaletteAdjustment.GroundGAdjustment = value;
                }
            }

            public sbyte B {
                get => (sbyte) (MPD_File.PaletteAdjustment?.GroundBAdjustment ?? 0);
                set {
                    if (MPD_File.PaletteAdjustment?.HasGroundAdjustment == true)
                        MPD_File.PaletteAdjustment.GroundBAdjustment = value;
                }
            }

            public IMPD_File MPD_File { get; }
        }

        private LightAdjustmentColor _lightPaletteAdjustment;
        public IMPD_PaletteAdjustColor LightPaletteAdjustment {
            get => _lightPaletteAdjustment;
            set {
                if (value != null) {
                    _lightPaletteAdjustment.R = value.R;
                    _lightPaletteAdjustment.G = value.G;
                    _lightPaletteAdjustment.B = value.B;
                }
            }
        }

        private GroundAdjustmentColor _groundPaletteAdjustment;
        public IMPD_PaletteAdjustColor GroundPaletteAdjustment {
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
    }
}
