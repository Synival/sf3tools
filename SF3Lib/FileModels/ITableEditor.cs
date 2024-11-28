using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.TableModels;

namespace SF3.FileModels {
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
