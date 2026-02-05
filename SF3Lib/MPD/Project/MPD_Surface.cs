using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_Surface : MPD_SurfaceBase {
        public MPD_Surface(IMPD_Settings settings) : base(settings) {}
        public MPD_Surface(IMPD_Settings settings, IMPD_Surface original) : base(settings, original) {}

        public static MPD_Surface FromJToken(IMPD_Settings settings, JToken token) => new MPD_Surface(settings, token);
        private MPD_Surface(IMPD_Settings settings, JToken token) : base(settings, token) {}
    }
}
