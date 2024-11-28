using System;

namespace SF3.RawData {
    /// <summary>
    /// Data that automatically decompresses its incoming bytes into a "sub-data" object called 'DecompressedData'.
    /// Upon Finalize(), it will update its own compressed data using the decompressed snapshot.
    /// </summary>
    public interface ICompressedData : IByteData {
        /// <summary>
        /// Performs compression on the data in DecompressedData and assigns it to the ICompressedData's own data
        /// in the form of a completely new byte[] array. The DecompressedData's IsModified flag is set to 'false'
        /// and the ICompressedData's 'NeedsRecompressedChanged' flag is set to 'false'.
        /// </summary>
        /// <returns></returns>
        bool Recompress();

        /// <summary>
        /// Decompressed data initialized with the data from the ICompressedData. May become out of sync withe the
        /// ICompressedData's data, in which case it should be Finalized (committed).
        /// </summary>
        IByteData DecompressedData { get; }

        /// <summary>
        /// If 'true', this means the decompressed data has been modified and is out of sync with compressed data.
        /// This should be set to 'true' whenever DecompressedData.IsModified is 'true'.
        /// </summary>
        bool NeedsRecompression { get; set; }

        /// <summary>
        /// Invoked when 'NeedsRecompression' value is changed.
        /// </summary>
        event EventHandler NeedsRecompressionChanged;
    }
}
