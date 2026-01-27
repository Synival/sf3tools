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

        public int ID { get; }

        public short X { get; set; }
        public short Y { get; set; }
    }
}
