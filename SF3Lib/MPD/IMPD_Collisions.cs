using System.Collections.Generic;

namespace SF3.MPD {
    /// <summary>
    /// Abstract representation of the walking collision map for the MPD.
    /// </summary>
    public interface IMPD_Collisions {
        /// <summary>
        /// The set of collision lines for the map.
        /// </summary>
        IEnumerable<IMPD_CollisionLine> Lines { get; }
    }
}
