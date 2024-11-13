using System.Collections.Generic;
using SF3.Tables;
using SF3.Types;

namespace SF3.Editors {
    /// <summary>
    /// IFileEditor specifically for files in Shining Force 3
    /// </summary>
    public interface IScenarioTableEditor : IFileEditor {
        /// <summary>
        /// The scenario/disc/file to edit.
        /// </summary>
        ScenarioType Scenario { get; }

        /// <summary>
        /// Collection of Tables initialized upon loading.
        /// </summary>
        IEnumerable<ITable> Tables { get; }
    }
}