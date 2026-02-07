using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_CollisionPointExtensions {
        public static JObject ToJObject(this IMPD_CollisionPoint point) {
            return new JObject {
                { "ID", point.ID },
                { "X",  point.X },
                { "Y",  point.Y },
            };
        }
    }
}
