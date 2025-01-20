using System.Collections;
using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Interface for any table of SF3 data that can be modified.
    /// </summary>
    public interface ITable : IEnumerable {
        /// <summary>
        /// Loads rows from its respective XML file(s).
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' if the .XML file(s) do not exist or are in use.</returns>
        bool Load();

        /// <summary>
        /// Unloads all loaded data.
        /// </summary>
        /// <returns>'true' when a reset has occurred or nothing was loaded.</returns>
        bool Unload();

        /// <summary>
        /// The byte data used for this table.
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
    public interface ITable<T> : ITable, IEnumerable<T> where T : class, IStruct {
        /// <summary>
        /// A mutable array of rows of type T.
        /// </summary>
        T[] Rows { get; }

        /// <summary>
        /// Retrieves a row.
        /// </summary>
        /// <param name="index">Index of the row to retrieve.</param>
        /// <returns>A row of a concrete type.</returns>
        T this[int index] { get; }

        /// <summary>
        /// Number of elements in the table.
        /// </summary>
        int Length { get; }
    }
}
