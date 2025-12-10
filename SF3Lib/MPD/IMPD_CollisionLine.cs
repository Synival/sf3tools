namespace SF3.MPD {
    public interface IMPD_CollisionLine {
        /// <summary>
        /// Index of the point used for (X1, Y1).
        /// </summary>
        ushort Point1Index { get; set; }

        /// <summary>
        /// Index of the point used for (X2, Y2).
        /// </summary>
        ushort Point2Index { get; set; }

        /// <summary>
        /// X component of the first point of the line.
        /// </summary>
        short X1 { get; set; }

        /// <summary>
        /// X component of the first point of the line.
        /// </summary>
        short Y1 { get; set; }

        /// <summary>
        /// X component of the second point of the line.
        /// </summary>
        short X2 { get; set; }

        /// <summary>
        /// X component of the second point of the line.
        /// </summary>
        short Y2 { get; set; }

        /// <summary>
        /// Angle of the line.
        /// </summary>
        float Angle { get; set; }

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
