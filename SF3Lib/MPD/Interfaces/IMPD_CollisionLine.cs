namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Abstract representation of a single collision line connected by two IMPD_CollisionPoint's.
    /// </summary>
    public interface IMPD_CollisionLine {
        /// <summary>
        /// Unique identifier for the line.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// The first point of the line. Cannot be 'null'.
        /// </summary>
        IMPD_CollisionPoint Point1 { get; set; }

        /// <summary>
        /// The second point of the line. Cannot be 'null'.
        /// </summary>
        IMPD_CollisionPoint Point2 { get; set; }

        /// <summary>
        /// X component of the first point of the line.
        /// </summary>
        short X1 { get; set; }

        /// <summary>
        /// Y component of the first point of the line.
        /// </summary>
        short Y1 { get; set; }

        /// <summary>
        /// X component of the second point of the line.
        /// </summary>
        short X2 { get; set; }

        /// <summary>
        /// Y component of the second point of the line.
        /// </summary>
        short Y2 { get; set; }

        /// <summary>
        /// Angle of the line.
        /// </summary>
        float Angle { get; }

        /// <summary>
        /// The flag that, when on, disables this collision line.
        /// If set, must be between 0x201 and 0x2FF (inclusive).
        /// </summary>
        int? FlagToDisable { get; set; }

        /// <summary>
        /// Value that seems to follow an order of lines but is sometimes very arbitrary.
        /// </summary>
        byte Tag { get; set; }
    }
}
