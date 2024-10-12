using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.X1_Editor
{
    public interface IX1_FileEditor : ISF3FileEditor
    {
        // TODO: This really should be read-only, but some of the models set this. How to fix?
        int Map { get; set; }
    }
}
