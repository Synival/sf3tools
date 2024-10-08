using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.Editor
{
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public interface IFileEditor
    {
        bool LoadFile(string filename);
    }
}
