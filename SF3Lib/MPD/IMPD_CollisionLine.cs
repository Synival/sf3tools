using System;
using System.Collections.Generic;
using System.Text;

namespace SF3.MPD {
    public interface IMPD_CollisionLine {
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
    }
}
