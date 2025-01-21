using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Interface for any table of SF3 data that can be modified.
    /// </summary>
    public interface IFixedSizeTable : ITable {
        /// <summary>
        /// The number of elements this table will have when loaded.
        /// </summary>
        int Size { get; }
    }

    /// <summary>
    /// Interface for a specific table of SF3 data that can be modified.
    /// </summary>
    public interface IFixedSizeTable<T> : IFixedSizeTable, ITable<T> where T : class, IStruct {
    }
}
