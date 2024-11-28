using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.Models.Tables;

namespace SF3.Models.Files {
    /// <summary>
    /// Any kind of file that has tables.
    /// </summary>
    public interface ITableFile : IBaseFile {
        /// <summary>
        /// Collection of Tables initialized upon loading.
        /// </summary>
        IEnumerable<ITable> Tables { get; }
    }
}
