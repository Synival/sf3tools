using System;

namespace SF3.RawEditors {
    /// <summary>
    /// Editor that automatically decompresses its incoming data in a sub-editor called 'DecompressedEditor'.
    /// Upon Finalize(), it will update its own compressed data using the decompressed snapshot.
    /// </summary>
    public interface ICompressedEditor : IByteEditor {
        /// <summary>
        /// Decompressed data generated from its incoming compressed data. Do not modify!!
        /// </summary>
        byte[] DecompressedData { get; }

        /// <summary>
        /// Editor for 'DecompressedData'.
        /// </summary>
        IByteEditor DecompressedEditor { get; }

        /// <summary>
        /// If 'true', this means the decompressed data has been modified and is out of sync
        /// with compressed data.
        /// </summary>
        bool NeedsRecompression { get; }

        /// <summary>
        /// Invoked when 'NeedsRecompression' value is changed.
        /// </summary>
        event EventHandler NeedsRecompressionChanged;
    }
}
