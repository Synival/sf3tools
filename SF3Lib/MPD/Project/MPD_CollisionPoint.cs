using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    /// <summary>
    /// A single point from which MPD collision lines can be connected.
    /// </summary>
    public class MPD_CollisionPoint : IMPD_CollisionPoint {
        public MPD_CollisionPoint(int id, short x, short y) {
            ID = id;
            X = x;
            Y = y;
        }

        public MPD_CollisionPoint(IMPD_CollisionPoint original) {
            ID = original.ID;
            X  = original.X;
            Y  = original.Y;
        }

        public static MPD_CollisionPoint FromJToken(JToken token) => new MPD_CollisionPoint(token);
        private MPD_CollisionPoint(JToken token) {
            var jObject = (JObject) token;
            ID =   (int) jObject["ID"];
            X  = (short) jObject["X"];
            Y  = (short) jObject["Y"];
        }

        public int ID { get; }

        public short X { get; set; }
        public short Y { get; set; }
    }
}
