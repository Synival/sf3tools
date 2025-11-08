using System;
using System.IO;
using SF3.ByteData;
using SF3.Models.Files;

namespace SF3.ModelLoaders {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public interface IModelFileLoader : IModelLoader {
        /// <summary>
        /// Loads a file's binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <param name="fileDialogFilter">The filter used for saving the dialog or looking for similar files.</param>
        /// <param name="createModel">Callback to create a model once after creating the byte data.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename, string fileDialogFilter, Func<IByteData, IBaseFile> createModel);

        /// <summary>
        /// Loads a stream of binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The filename of 'stream' to be stored.</param>
        /// <param name="fileDialogFilter">The filter used for saving the dialog or looking for similar files.</param>
        /// <param name="stream">The data stream to load.</param>
        /// <param name="createModel">Callback to create a model once after creating the byte data.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename, string fileDialogFilter, Stream stream, Func<IByteData, IBaseFile> createModel);

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
