using CommonLib.Discovery;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Files {
    /// <summary>
    /// Any kind of file that is loaded in-game.
    /// </summary>
    public interface IGameFile : IBaseFile {
        /// <summary>
        /// Scenario for the file, if relevant.
        /// </summary>
        ScenarioType? Scenario { get; }

        /// <summary>
        /// Binary data loaded in.
        /// </summary>
        IByteData Data { get; }

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
