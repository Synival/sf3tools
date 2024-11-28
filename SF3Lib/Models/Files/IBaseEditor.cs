using System;
using CommonLib.NamedValues;
using SF3.RawEditors;

namespace SF3.Models.Files {
    /// <summary>
    /// Any kind of editor.
    /// </summary>
    public interface IBaseEditor : IModifiable, IDisposable {
        /// <summary>
        /// Perform any extra tasks that need to be performed before saving.
        /// </summary>
        /// <returns>'true' if finalization was successful, otherwise 'false'.</returns>
        bool Finalize();

        /// <summary>
        /// The context for fetching named values.
        /// </summary>
        INameGetterContext NameGetterContext { get; }

        /// <summary>
        /// The name of this editor for display.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Triggered when Finalize() completes successfully.
        /// </summary>
        event EventHandler Finalized;
    }
}
