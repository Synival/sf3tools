using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Types
{
    /// <summary>
    /// The leader of a map in X1*.BIN files.
    /// </summary>
    public enum MapLeaderType
    {
        Synbios = 0x00,
        Medion = 0x04,
        Julian = 0x08,
        Extra = 0x0C
    }
}
