using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Interface for any table of SF3 data that can be modified.
    /// </summary>
    public interface IResourceTable : ITable {
        /// <summary>
        /// The XML file to load for this resource.
        /// </summary>
        string ResourceFile { get; }

        /// <summary>
        /// The maximum allowed size the table can be. Optional.
        /// </summary>
        int? MaxSize { get; }
    }

    /// <summary>
    /// Interface for a specific table of SF3 data that can be modified.
    /// </summary>
    public interface IResourceTable<T> : IResourceTable, ITable<T> where T : class, IStruct {
    }
}
