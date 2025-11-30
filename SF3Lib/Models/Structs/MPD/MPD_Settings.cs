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

        public ushort ViewDistance {
            get => MPD_File.MPDHeader.ViewDistance;
            set => MPD_File.MPDHeader.ViewDistance = value;
        }
    }
}
