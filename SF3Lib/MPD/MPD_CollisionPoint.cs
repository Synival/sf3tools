namespace SF3.MPD {
    /// <summary>
    /// A single point from which MPD collision lines can be connected.
    /// </summary>
    public class MPD_CollisionPoint : IMPD_CollisionPoint {
        public MPD_CollisionPoint(short x, short y) {
            X = x;
            Y = y;
        }

        public short X { get; set; }
        public short Y { get; set; }
    }
}
