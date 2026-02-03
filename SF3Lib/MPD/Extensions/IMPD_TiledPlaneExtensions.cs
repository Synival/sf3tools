using CommonLib.Extensions;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_TiledPlaneExtensions {
        public static JObject ToJObject(this IMPD_TiledPlane tiledPlane) {
            return new JObject {
                { "Tileset", tiledPlane.Tileset?.ToJValue() },
                { "TileAssignment", tiledPlane.TileAssignment?.ToJArray() }
            };
        }
    }
}
