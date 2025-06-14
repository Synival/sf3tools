using CommonLib.Discovery;
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

        /// <summary>
        /// Limit at which the file should no longer load into RAM.
        /// </summary>
        int RamAddressLimit { get; }

        /// <summary>
        /// Container for discoveries of pointers, functions, tables, etc. in the file.
        /// </summary>
        DiscoveryContext Discoveries { get; }
    }
}
