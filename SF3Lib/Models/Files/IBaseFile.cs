using System;
using CommonLib.NamedValues;

namespace SF3.Models.Files {
    /// <summary>
    /// Any kind of file.
    /// </summary>
    public interface IBaseFile : IModifiable, IDisposable {
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
        /// The name of this file for display.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Triggered when Finalize() completes successfully.
        /// </summary>
        event EventHandler Finalized;
    }
}
