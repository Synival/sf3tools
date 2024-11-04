using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
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
        /// The editor used for this table.
        /// </summary>
        IByteEditor FileEditor { get; }

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
        IModel[] RowObjs { get; }

        /// <summary>
        /// The maximum allowed size the table can be. Optional.
        /// </summary>
        int? MaxSize { get; }
    }

    /// <summary>
    /// Interface for a specific table of SF3 data that can be modified.
    /// </summary>
    public interface ITable<T> : ITable where T : class, IModel {
        /// <summary>
        /// A mutable array of rows of type T.
        /// </summary>
        T[] Rows { get; }
    }
}
