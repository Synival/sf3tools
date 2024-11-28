using System;

namespace SF3.RawData {
    /// <summary>
    /// Helper class for modifying any kind of data blob and tracking whether or not it's been edited.
    /// </summary>
    public interface IRawData : IModifiable, IDisposable {
        /// <summary>
        /// The amount of data being edited.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Returns a copy of all raw data loaded.
        /// </summary>
        /// <returns>A copy of the raw data loaded as a byte[].</returns>
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
        /// Makes any pending changes necessary before data should be saved.
        /// Should be invoked before any save operations.
        /// </summary>
        /// <returns>'true' on success, otherwise 'false'.</returns>
        bool Finish();

        /// <summary>
        /// Event that occurs when the Finish() is completed successfully.
        /// </summary>
        event EventHandler Finished;
    }
}
