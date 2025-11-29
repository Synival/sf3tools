using SF3.Models.Files.MPD;
using SF3.MPD;

namespace SF3.Models.Structs.MPD {
    public class MPDSettings : IMPD_Settings {
        public MPDSettings(IMPD_File file) {
            MPD_File = file;
        }

        public IMPD_File MPD_File { get; }
    }
}
