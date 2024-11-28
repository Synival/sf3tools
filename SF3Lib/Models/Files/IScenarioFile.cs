using SF3.Types;

namespace SF3.Models.Files {
    /// <summary>
    /// Editor for a specific SF3 Scenario.
    /// </summary>
    public interface IScenarioFile : IBaseFile {
        /// <summary>
        /// The scenario/disc/file to edit.
        /// </summary>
        ScenarioType Scenario { get; }
    }
}
