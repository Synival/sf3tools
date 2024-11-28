using System;

namespace SF3.RawData {
    /// <summary>
    /// Editor that automatically decompresses its incoming data in a sub-editor called 'DecompressedEditor'.
    /// Upon Finalize(), it will update its own compressed data using the decompressed snapshot.
    /// </summary>
    public interface ICompressedData : IByteData {
        /// <summary>
        /// Performs compression on the data in DecompressedEditor and assigns it to the ICompressedEditor's own data
        /// in the form of a completely new byte[] array. The DecompressedEditor's IsModified flag is set to 'false'
        /// and the ICompressedEditor's 'NeedsRecompressedChanged' flag is set to 'false'.
        /// </summary>
        /// <returns></returns>
        bool Recompress();

        /// <summary>
        /// Decompressed data generated from its incoming compressed data. Do not modify!!
        /// </summary>
        byte[] DecompressedData { get; }

        /// <summary>
        /// Editor for 'DecompressedData'.
        /// </summary>
        IByteData DecompressedEditor { get; }

        /// <summary>
        /// If 'true', this means the decompressed data has been modified and is out of sync with compressed data.
        /// This should be set to 'true' whenever DecompressedEditor.IsModified is 'true'.
        /// </summary>
        bool NeedsRecompression { get; set; }

        /// <summary>
        /// Invoked when 'NeedsRecompression' value is changed.
        /// </summary>
        event EventHandler NeedsRecompressionChanged;
    }
}
