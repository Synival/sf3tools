using System;
using System.Collections.Generic;
using CommonLib.NamedValues;

namespace SF3.Models.Files {
    /// <summary>
    /// Any kind of file.
    /// </summary>
    public interface IBaseFile : IModifiable, IDisposable {
        /// <summary>
        /// Gets any errors or mistakes that may be present in the file.
        /// </summary>
        /// <returns>An array of errors or mistakes in the file.</returns>
        string[] GetErrors();

        /// <summary>
        /// Perform any extra tasks that need to be performed before saving.
        /// </summary>
        /// <returns>'true' if finalization was successful, otherwise 'false'.</returns>
        bool Finish();

        /// <summary>
        /// The context for fetching named values.
        /// </summary>
        INameGetterContext NameGetterContext { get; }

        /// <summary>
        /// The name of this file for display.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Triggered when Finish() completes successfully.
        /// </summary>
        event EventHandler Finished;
    }
}
