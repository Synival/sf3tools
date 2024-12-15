using SF3.Models.Structs;
using SF3.RawData;

namespace SF3.Models.Tables {
    /// <summary>
    /// Interface for any table of SF3 data that can be modified.
    /// </summary>
    public interface ITable {
        /// <summary>
        /// Loads rows from its respective XML file(s).
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' if the .XML file(s) do not exist or are in use.</returns>
        bool Load();

        /// <summary>
        /// Resets all loaded data.
        /// </summary>
        /// <returns>'true' when a reset has occurred or nothing was loaded.</returns>
        bool Reset();

        /// <summary>
        /// The raw data used for this table.
        /// </summary>
        IByteData Data { get; }

        /// <summary>
        /// The address of the first row of the table.
        /// </summary>
        int Address { get; }

        /// <summary>
        /// Is 'true' when a successful Load() has occurred.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// The XML file to load for this resource.
        /// </summary>
        string ResourceFile { get; }

        /// <summary>
        /// An mutable array of rows.
        /// </summary>
        IStruct[] RowObjs { get; }

        /// <summary>
        /// The maximum allowed size the table can be. Optional.
        /// </summary>
        int? MaxSize { get; }
    }

    /// <summary>
    /// Interface for a specific table of SF3 data that can be modified.
    /// </summary>
    public interface ITable<T> : ITable where T : class, IStruct {
        /// <summary>
        /// A mutable array of rows of type T.
        /// </summary>
        T[] Rows { get; }
    }
}
