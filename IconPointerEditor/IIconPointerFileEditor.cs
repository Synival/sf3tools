using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3.IconPointerEditor
{
    public interface IIconPointerFileEditor : ISF3FileEditor
    {
        bool X026 { get; }
    }
}
