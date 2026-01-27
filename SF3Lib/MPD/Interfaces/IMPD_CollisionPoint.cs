namespace SF3.MPD.Interfaces {
    public interface IMPD_CollisionPoint {
        /// <summary>
        /// Identifier for the collision point.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// X component of the point.
        /// </summary>
        short X { get; set; }

        /// <summary>
        /// Y component of the point.
        /// </summary>
        short Y { get; set; }
    }
}
