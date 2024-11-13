using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.Tables;

namespace SF3.Editors {
    /// <summary>
    /// Editor for any kind of file that has tables.
    /// </summary>
    public interface ITableEditor : IBaseEditor {
        /// <summary>
        /// Collection of Tables initialized upon loading.
        /// </summary>
        IEnumerable<ITable> Tables { get; }
    }
}
