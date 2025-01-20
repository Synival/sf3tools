using SF3.Models.Structs;

namespace SF3.Models.Tables {
    /// <summary>
    /// Interface for any table of SF3 data that can be modified.
    /// </summary>
    public interface IAddressedTable : IBaseTable {
        /// <summary>
        /// The number of elements this table will have when loaded.
        /// </summary>
        int[] Addresses { get; }
    }

    /// <summary>
    /// Interface for a specific table of SF3 data that can be modified.
    /// </summary>
    public interface IAddressedTable<T> : IAddressedTable, IBaseTable<T> where T : class, IStruct {
    }
}
