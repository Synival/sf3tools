using System;
using SF3.RawEditors;
using SF3.Editors;

namespace SF3.Loaders {
    /// <summary>
    /// Interface for loading editors.
    /// </summary>
    public interface IEditorLoader : IModifiable, IDisposable {
        /// <summary>
        /// Closes an editor if opened. Invokes events 'PreClosed' and 'Closed' if an editor was closed.
        /// </summary>
        /// <returns>'true' if 'IsLoaded' is 'false' after closing is completed or aborted.  Otherwise 'false'.</returns>
        bool Close();

        /// <summary>
        /// The title used for the editor, with the current editor and '*' indicating modification.
        /// </summary>
        string EditorTitle(string formTitle);

        /// <summary>
        /// The "real" editor used, backed by an underlying IDataEditor.
        /// </summary>
        IBaseEditor Editor { get; }

        /// <summary>
        /// The editor that exists when IsLoaded is 'true'.
        /// </summary>
        IRawEditor RawEditor { get; }

        /// <summary>
        /// 'True' when the editor is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// The title of the loaded data being modified for display.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Event that occurs when the 'IsLoaded' property is changed.
        /// </summary>
        event EventHandler IsLoadedChanged;

        /// <summary>
        /// Event that occurs before the stream is opened.
        /// </summary>
        event EventHandler PreLoaded;

        /// <summary>
        /// Event that occurs when the stream is opened.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Event that occurs before the stream is saved.
        /// </summary>
        event EventHandler PreSaved;

        /// <summary>
        /// Event that occurs when the stream is saved.
        /// </summary>
        event EventHandler Saved;

        /// <summary>
        /// Event that occurs before the stream is closed.
        /// </summary>
        event EventHandler PreClosed;

        /// <summary>
        /// Event that occurs when the stream is closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Event that occurs whenever the 'Title' property is changed.
        /// </summary>
        event EventHandler TitleChanged;
    }
}
