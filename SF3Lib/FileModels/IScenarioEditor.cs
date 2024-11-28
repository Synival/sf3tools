using System.Collections.Generic;
using SF3.Tables;
using SF3.Types;

namespace SF3.FileModels {
    /// <summary>
    /// Editor for a specific SF3 Scenario.
    /// </summary>
    public interface IScenarioEditor : IBaseEditor {
        /// <summary>
        /// The scenario/disc/file to edit.
        /// </summary>
        ScenarioType Scenario { get; }
    }
}
