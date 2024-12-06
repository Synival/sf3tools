using CommonLib;

namespace SF3.RawData {
    /// <summary>
    /// Wrapper class for editing a byte[].
    /// </summary>
    public interface IByteData : IRawData {
        /// <summary>
        /// Reference to the byte array.
        /// </summary>
        ByteArray Data { get; }

        /// <summary>
        /// Sets the data to edit. Should also set 'IsModified' to 'false'.
        /// </summary>
        /// <param name="data">The data to modify.</param>
        /// <returns>'true' if the data was set, otherwise 'false'.</returns>
        bool SetData(byte[] data);
    }
}
