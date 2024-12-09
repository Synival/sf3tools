using CommonLib;

namespace SF3.RawData {
    /// <summary>
    /// Wrapper class for editing a byte[].
    /// </summary>
    public interface IByteData : IRawData {
        /// <summary>
        /// Sets the data to edit.
        /// </summary>
        /// <param name="data">The data to modify.</param>
        /// <returns>'true' if the data was set, otherwise 'false'.</returns>
        bool SetDataTo(byte[] data);
    }
}
