using SF3.Models.Files.MPD;
using SF3.MPD;

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

        public bool SetMSBForPalette1 {
            get => MPD_File.Flags.Bit_0x0080_SetMSBForPalette1;
            set => MPD_File.Flags.Bit_0x0080_SetMSBForPalette1 = value;
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

        public short GroundX {
            get => MPD_File.MPDHeader.GroundX;
            set => MPD_File.MPDHeader.GroundX = value;
        }

        public short GroundY {
            get => MPD_File.MPDHeader.GroundY;
            set => MPD_File.MPDHeader.GroundY = value;
        }

        public short GroundZ {
            get => MPD_File.MPDHeader.GroundZ;
            set => MPD_File.MPDHeader.GroundZ = value;
        }

        public float GroundXRotation {
            get => MPD_File.MPDHeader.GroundXRotation;
            set => MPD_File.MPDHeader.GroundXRotation = value;
        }

        public short BackgroundX {
            get => MPD_File.MPDHeader.BackgroundX;
            set => MPD_File.MPDHeader.BackgroundX = value;
        }

        public short BackgroundY {
            get => MPD_File.MPDHeader.BackgroundY;
            set => MPD_File.MPDHeader.BackgroundY = value;
        }
    }
}
