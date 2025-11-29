using SF3.Models.Files.MPD;
using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public class MPD_Settings : IMPD_Settings {
        public MPD_Settings(IMPD_File file) {
            MPD_File = file;
        }

        public IMPD_File MPD_File { get; }
    }
}
