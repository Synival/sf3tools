namespace SF3.ByteData {
    public interface IChunkData : ICompressedData {
        /// <summary>
        /// When 'true', the Recompress() function will work and 'NeedsRecompression' can be modified.
        /// When 'false', Recompress() will throw and setting 'NeedsRecompression' to 'true' will throw as well.
        /// </summary>
        bool IsCompressed { get; }

        /// <summary>
        /// Index of the Chunk in the file.
        /// </summary>
        int Index { get; }
    }
}
