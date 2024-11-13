using System;

namespace SF3.Editors {
    /// <summary>
    /// Any kind of editor.
    /// </summary>
    public interface IBaseEditor : IDisposable {

        /// <summary>
        /// The name of this editor for display.
        /// </summary>
        string Title { get; }
    }
}
