using System;
using System.Collections.Generic;
using System.Text;

namespace SF3.MPD {
    public interface IMPD_AllScenarioFlags {
        /// <summary>
        /// Always on for all MPDs across all four scenarios. Not known if this is read anywhere and must be set.
        /// </summary>
        bool Bit_0x0001_Unknown { get; set; }
    }
}
