using System;
using System.Collections.Generic;
using System.Text;

namespace SF3.MPD {
    public interface IMPD_AllScenarioFlags {
        /// <summary>
        /// Always on for all MPDs across all four scenarios. Not known if this is read anywhere and must be set.
        /// </summary>
        bool Bit_0x0001_Unknown { get; set; }

        /// <summary>
        /// When set, the first bit of the lightmap index to use is toggled by pseudo-random values taken from certain
        /// bits in the dot product produced during lighting calculations.
        /// </summary>
        bool Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap { get; set; }

        /// <summary>
        /// When set, flat surface tiles without a texture (set to 0xFF) aren't discarded but added to the set of 150
        /// flat tiles. This is so they can be rendered in battle with the grid or the "move" or "attack" masks.
        /// (Potentially exists in Scenario 1 but unused)
        /// </summary>
        bool Bit_0x0008_KeepTexturelessFlatTiles { get; set; }

        /// <summary>
        /// Is set sometimes in Scenario 2 and 3 but appears to do nothing (no breakpoints hit).
        /// (Potentially exists in Scenario 1 but unused)
        /// </summary>
        bool Bit_0x0020_Unknown { get; set; }
    }
}
