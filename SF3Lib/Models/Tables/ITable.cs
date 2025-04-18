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
        /// Loads all rows of the table.
        /// </summary>
        /// <returns>Return 'true' on success, or 'false' on any kind of failure.</returns>
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
        /// Name of the table.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Is 'true' when a successful Load() has occurred.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// An mutable array of rows.
        /// </summary>
        IStruct[] RowObjs { get; }

        /// <summary>
        /// Number of elements in the table.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns the number of bytes occupied by all rows together.
        /// </summary>
        int SizeInBytes { get; }

        /// <summary>
        /// Extra bytes at the end of the table if it is a terminated table.
        /// </summary>
        int TerminatorSize { get; }

        /// <summary>
        /// SizeInBytes + TerminatorSize
        /// </summary>
        int SizeInBytesPlusTerminator { get; }

        /// <summary>
        /// The rows represent a contiguous set of data from Address to Address + SizeInBytesPlusTerminator.
        /// </summary>
        bool IsContiguous { get; }
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
    }
}
