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

        /// <summary>
        /// Angle of the line.
        /// </summary>
        float Angle { get; set; }

        /// <summary>
        /// Lower two digits of the 2XX flag that, when on, disables this collision line.
        /// </summary>
        byte Flag2XXToDisable { get; set; }

        /// <summary>
        /// Value that seems to follow an order of lines but is sometimes very arbitrary.
        /// </summary>
        byte Tag { get; set; }
    }
}
