﻿using System;
using CommonLib.NamedValues;

namespace SF3.Editors {
    /// <summary>
    /// Any kind of editor.
    /// </summary>
    public interface IBaseEditor : IDisposable {
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