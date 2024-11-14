using System;
using System.IO;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Editors;
using static SF3.Loaders.FileLoaderDelegates;

namespace SF3.Loaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public interface IFileLoader : IEditorLoader {
        /// <summary>
        /// Loads a file's binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <param name="createEditor">Callback to create an editor once after creating the data editor.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename, FileLoaderCreateEditorDelegate createEditor);

        /// <summary>
        /// Loads a stream of binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The filename of 'stream' to be stored.</param>
        /// <param name="stream">The data stream to load.</param>
        /// <param name="createEditor">Callback to create an editor once after creating the data editor.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename, Stream stream, FileLoaderCreateEditorDelegate createEditor);

        /// <summary>
        /// Saves a file's binary data for editing. Invokes events 'PreSaved' and 'Saved'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool SaveFile(string filename);

        /// <summary>
        /// Filename of the file loaded, with full path.
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// Filename without the path.
        /// </summary>
        string ShortFilename { get; }
    }
}
