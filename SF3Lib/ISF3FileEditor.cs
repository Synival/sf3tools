using SF3.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF3
{
    /// <summary>
    /// IFileEditor specifically for files in Shining Force 3
    /// </summary>
    public interface ISF3FileEditor : IFileEditor
    {
        /// <summary>
        /// The scenario/disc/file to edit.
        /// </summary>
        ScenarioType Scenario { get; }
    }
}
