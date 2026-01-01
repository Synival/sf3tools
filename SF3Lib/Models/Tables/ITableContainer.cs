using System.Collections.Generic;

namespace SF3.Models.Tables {
    /// <summary>
    /// Any kind of structure that has tables.
    /// </summary>
    public interface ITableContainer {
        /// <summary>
        /// Collection of Tables.
        /// </summary>
        IEnumerable<ITable> Tables { get; }
    }
}
