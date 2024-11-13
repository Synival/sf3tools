namespace SF3.RawEditors {
    /// <summary>
    /// Wrapper class for editing a byte[].
    /// </summary>
    public interface IByteEditor : IRawEditor {
        /// <summary>
        /// Reference to the byte array. Please don't modify it!
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        /// Sets the data to edit. Should also set 'IsModified' to 'false'.
        /// </summary>
        /// <param name="data">The data to modify.</param>
        /// <returns>'true' if the data was set, otherwise 'false'.</returns>
        bool SetData(byte[] data);
    }
}
