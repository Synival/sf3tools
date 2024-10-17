using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Types
{
    /// <summary>
    /// Used to designate specific files that need special offsets of logic.
    /// This may or may not be different depending on the scenario.
    /// </summary>
    public enum FileType
    {
        // IconPointerEditor files
        X011_X021,
        X026,

        // X002 Editor Files
        X002,

        // X013 Editor Files
        X013,

        // X019 Editor Files
        X019,
        X044,

        // X033/X031 Editor Files
        X031_X033,

        // X1 Editor Files
        X1Any,
        X1BTL99,
    }
}
