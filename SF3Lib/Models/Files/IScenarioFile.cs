using SF3.Types;

namespace SF3.Models.Files {
    /// <summary>
    /// File for a specific SF3 Scenario.
    /// </summary>
    public interface IScenarioFile : IGameFile {
        /// <summary>
        /// The scenario/disc/file to edit.
        /// </summary>
        new ScenarioType Scenario { get; }
    }
}
