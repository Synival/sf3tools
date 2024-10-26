using System;

namespace SF3.FileEditors {
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public interface IFileEditor {
        /// <summary>
        /// Loads a file's binary data for editing. Invokes events 'PreLoaded' and 'Loaded'.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        bool LoadFile(string filename);

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
        /// Returns a copy of all data loaded into the editor.
        /// </summary>
        /// <returns>A copy of the editor's data.</returns>
        byte[] GetAllData();

        /// <summary>
        /// Gets the value of 1, 2, 3 or 4 contiguous bytes at an address.
        /// </summary>
        /// <param name="location">The address of the data.</param>
        /// <returns>Sum of all bytes (earlier byte = higher byte).</returns>
        uint GetData(int location, int bytes);

        /// <summary>
        /// Gets the value of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte.</param>
        int GetByte(int location);

        /// <summary>
        /// Gets the value of a 16-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 16-bit integer.</param>
        int GetWord(int location);

        /// <summary>
        /// Gets the value of a 32-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 32-bit integer.</param>
        int GetDouble(int location);

        /// <summary>
        /// Returns the value of string data of a specific size at a location.
        /// </summary>
        /// <param name="location">The address of the string.</param>
        /// <param name="length">The length of the string space.</param>
        string GetString(int location, int length);

        /// <summary>
        /// Returns the value of a single bit of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte containing the bit.</param>
        /// <param name="bit">The position of the bit, in range (1, 8).</param>
        /// <returns>True if the bit is set, false if the bit is unset.</returns>
        bool GetBit(int location, int bit);

        /// <summary>
        /// Sets the value of data with 1, 2, 3, or 4 bytes at an address.
        /// </summary>
        /// <param name="location">The address of the data.</param>
        /// <param name="value">The new value of the data (sized for the maximum number of bytes).</param>
        /// <param name="bytes">The number of bytes to store.</param>
        void SetData(int location, uint value, int bytes);

        /// <summary>
        /// Sets the value of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte.</param>
        /// <param name="value">The new value of the byte.</param>
        void SetByte(int location, byte value);

        /// <summary>
        /// Sets the value of a 16-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 16-bit integer.</param>
        /// <param name="value">The new value of the 16-bit integer.</param>
        void SetWord(int location, int value);

        /// <summary>
        /// Sets the value of 32-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 32-bit integer.</param>
        /// <param name="value">The new value of the 32-bit integer.</param>
        void SetDouble(int location, int value);

        /// <summary>
        /// Sets the value of string data of a specific size at a location.
        /// Data set will not exceed the length provided, and remaining bytes are automatically filled with zeros.
        /// </summary>
        /// <param name="location">The address of the string.</param>
        /// <param name="length">The length of the string space.</param>
        /// <param name="value">The new value of the string.</param>
        void SetString(int location, int length, string value);

        /// <summary>
        /// Sets the value of a single bit of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte containing the bit.</param>
        /// <param name="bit">The position of the bit, in range (1, 8).</param>
        /// <param name="value">The new value of the bit.</param>
        void SetBit(int location, int bit, bool value);

        /// <summary>
        /// 'True' when the file is loaded.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// 'True' when the file is modified.
        /// </summary>
        bool IsModified { get; set; }

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
        /// Event that occurs when the 'Modified' property is changed.
        /// </summary>
        event EventHandler ModifiedChanged;

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
