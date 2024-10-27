using System;
using System.IO;

namespace SF3.FileEditors {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public interface IFileEditor : IByteEditor {
        /// <summary>
        /// Loads a file's binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename);

        /// <summary>
        /// Loads a stream of binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The filename of 'stream' to be stored.</param>
        /// <param name="stream">The data stream to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename, Stream stream);

        /// <summary>
        /// Saves a file's binary data for editing. Invokes events 'PreSaved' and 'Saved'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool SaveFile(string filename);

        /// <summary>
        /// Closes a file if opened. Invokes events 'PreClosed' and 'Closed' if a file is open.
        /// </summary>
        /// <returns>'true' if a file was closed, otherwise 'false'.</returns>
        bool CloseFile();

        /// <summary>
        /// 'True' when the file is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Filename of the file loaded.
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// The title of the file being modified for display
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The title used for the editor, with the current file and '*' indicating modification.
        /// </summary>
        string EditorTitle(string formTitle);

        /// <summary>
        /// Event that occurs before a file is opened.
        /// </summary>
        event EventHandler PreLoaded;

        /// <summary>
        /// Event that occurs when a file is opened.
        /// </summary>
        event EventHandler Loaded;

        /// <summary>
        /// Event that occurs before a file is saved.
        /// </summary>
        event EventHandler PreSaved;

        /// <summary>
        /// Event that occurs when a file is saved.
        /// </summary>
        event EventHandler Saved;

        /// <summary>
        /// Event that occurs before a file is closed.
        /// </summary>
        event EventHandler PreClosed;

        /// <summary>
        /// Event that occurs when a file is closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Event that occurs whenever the 'Title' property is changed.
        /// </summary>
        event EventHandler TitleChanged;

        /// <summary>
        /// Event that occurs whenever the 'IsLoaded' property is changed.
        /// </summary>
        event EventHandler IsLoadedChanged;
    }
}
