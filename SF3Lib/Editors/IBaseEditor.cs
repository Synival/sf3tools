using System;
using CommonLib.NamedValues;

namespace SF3.Editors {
    /// <summary>
    /// Any kind of editor.
    /// </summary>
    public interface IBaseEditor : IDisposable {
        /// <summary>
        /// The context for fetching named values.
        /// </summary>
        INameGetterContext NameGetterContext { get; }

        /// <summary>
        /// The name of this editor for display.
        /// </summary>
        string Title { get; }
    }
}
