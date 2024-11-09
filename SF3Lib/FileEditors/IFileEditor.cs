using System;
using System.IO;
using CommonLib.NamedValues;

namespace SF3.FileEditors {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public interface IFileEditor : IByteEditor {
        /// <summary>
        /// Loads a file's binary data for editing. Invokes events 'PreFileLoaded' and 'FileLoaded'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename);

        /// <summary>
        /// Loads a stream of binary data for editing. Invokes events 'PreFileLoaded' and 'FileLoaded'.
        /// </summary>
        /// <param name="filename">The filename of 'stream' to be stored.</param>
        /// <param name="stream">The data stream to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename, Stream stream);

        /// <summary>
        /// Saves a file's binary data for editing. Invokes events 'PreFileSaved' and 'FileSaved'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool SaveFile(string filename);

        /// <summary>
        /// Closes a file if opened. Invokes events 'PreFileClosed' and 'FileClosed' if a file is open.
        /// </summary>
        /// <returns>'true' if a file was closed, otherwise 'false'.</returns>
        bool CloseFile();

        /// <summary>
        /// Filename of the file loaded, with full path.
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// Filename without the path.
        /// </summary>
        string ShortFilename { get; }

        /// <summary>
        /// The title of the file being modified for display
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The context for fetching named values.
        /// </summary>
        INameGetterContext NameContext { get; }

        /// <summary>
        /// The title used for the editor, with the current file and '*' indicating modification.
        /// </summary>
        string EditorTitle(string formTitle);

        /// <summary>
        /// Event that occurs before a file is opened.
        /// </summary>
        event EventHandler PreFileLoaded;

        /// <summary>
        /// Event that occurs when a file is opened.
        /// </summary>
        event EventHandler FileLoaded;

        /// <summary>
        /// Event that occurs before a file is saved.
        /// </summary>
        event EventHandler PreFileSaved;

        /// <summary>
        /// Event that occurs when a file is saved.
        /// </summary>
        event EventHandler FileSaved;

        /// <summary>
        /// Event that occurs before a file is closed.
        /// </summary>
        event EventHandler PreFileClosed;

        /// <summary>
        /// Event that occurs when a file is closed.
        /// </summary>
        event EventHandler FileClosed;

        /// <summary>
        /// Event that occurs whenever the 'Title' property is changed.
        /// </summary>
        event EventHandler TitleChanged;
    }
}
