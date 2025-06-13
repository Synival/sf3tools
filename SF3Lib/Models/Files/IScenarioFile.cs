using SF3.Types;

namespace SF3.Models.Files {
    /// <summary>
    /// File for a specific SF3 Scenario.
    /// </summary>
    public interface IScenarioFile : IBaseFile {
        /// <summary>
        /// The scenario/disc/file to edit.
        /// </summary>
        ScenarioType Scenario { get; }

        /// <summary>
        /// Address in RAM in to which this file is loaded.
        /// </summary>
        int RamAddress { get; }
    }
}
