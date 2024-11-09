using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DFRLib {
    /// <summary>
    /// A collection of differences between two sets of data.
    /// Can be used to generate DFR files.
    /// </summary>
    public class ByteDiff {
        /// <summary>
        /// Creates a list of differences between two files.
        /// </summary>
        /// <param name="fileFromPath">The original file to compare to.</param>
        /// <param name="fileToPath">The altered file to compare to.</param>
        /// <returns>A list containing all differences between the two files.</returns>
        public static List<ByteDiffChunk> MakeByteDiffChunks(string fileFromPath, string fileToPath, ByteDiffChunkBuilderOptions? options = null) {
            using (FileStream fileStreamFrom = new FileStream(fileFromPath, FileMode.Open, FileAccess.Read),
                              fileStreamTo = new FileStream(fileToPath, FileMode.Open, FileAccess.Read)) {
                var fileStreamFromLen = fileStreamFrom.Length;
                var fileStreamToLen = fileStreamTo.Length;
                return MakeByteDiffChunks(fileStreamFrom, fileStreamTo, options);
            }
        }

        /// <summary>
        /// Creates a list of differences between two sets of bytes.
        /// </summary>
        /// <param name="fileFromPath">The original bytes to compare to.</param>
        /// <param name="fileToPath">The altered bytes to compare to.</param>
        /// <returns>A list containing all differences between the two sets of bytes.</returns>
        public static List<ByteDiffChunk> MakeByteDiffChunks(byte[] bytesFrom, byte[] bytesTo, ByteDiffChunkBuilderOptions? options = null) {
            using (MemoryStream bytesFromStream = new MemoryStream(bytesFrom),
                                bytesToStream = new MemoryStream(bytesTo))
                return MakeByteDiffChunks(bytesFromStream, bytesToStream, options);
        }

        /// <summary>
        /// Creates a list of differences between two streams.
        /// </summary>
        /// <param name="streamFrom">The original stream to compare to.</param>
        /// <param name="streamTo">The altered stream to compare to.</param>
        /// <returns>A list containing all differences between the two files.</returns>
        public static List<ByteDiffChunk> MakeByteDiffChunks(Stream streamFrom, Stream streamTo, ByteDiffChunkBuilderOptions? options = null) {
            var streamFromRead = 0;
            var streamFromBuf = new byte[1024];
            var streamToBuf = new byte[1024];

            var chunkBuilder = new ByteDiffChunkBuilder(options);

            // Read from streams until no more data is available.
            var streamFromEOF = false;
            while (true) {
                if (!streamFromEOF) {
                    streamFromRead = streamFrom.Read(streamFromBuf, 0, streamFromBuf.Length);
                    if (streamFromRead == 0)
                        streamFromEOF = true;
                }

                var streamToRead = streamTo.Read(streamToBuf, 0, streamToBuf.Length);
                if (streamToRead == 0)
                    break;

                // Process bytes read from both streams.
                for (var i = 0; i < streamFromRead && i < streamToRead; i++)
                    chunkBuilder.FeedDiff(streamFromBuf[i], streamToBuf[i]);

                // Process bytes read from only the 'to' stream.
                for (var i = streamFromRead; i < streamToRead; i++)
                    chunkBuilder.FeedAppend(streamToBuf[i]);
            }

            return chunkBuilder.FetchChunks();
        }

        /// <summary>
        /// Creates a list of differences read from a DFR file.
        /// </summary>
        /// <param name="dfrFilename">Name of the DFR file.</param>
        public static List<ByteDiffChunk> MakeByteDiffChunks(string dfrFilename) {
            using (var stream = new FileStream(dfrFilename, FileMode.Open, FileAccess.Read))
                return MakeByteDiffChunks(stream);
        }

        /// <summary>
        /// Creates a list of differences using a stream of text in DFR format.
        /// </summary>
        /// <param name="dfrStream">Stream containing DFR text.</param>
        public static List<ByteDiffChunk> MakeByteDiffChunks(Stream dfrStream) {
            using (var reader = new StreamReader(dfrStream, Encoding.UTF8, false, 256, true)) {
                var chunkList = new List<ByteDiffChunk>();
                string line;
                while ((line = reader.ReadLine()?.Trim()) != null) {
                    int index;
                    if ((index = line.IndexOf(';')) >= 0)
                        line = line.Substring(0, index).Trim();
                    if (line.Length > 0)
                        chunkList.Add(new ByteDiffChunk(line));
                }
                return chunkList;
            }
        }

        /// <summary>
        /// Creates a diff between two files.
        /// </summary>
        /// <param name="fileFromPath">The original file to compare to.</param>
        /// <param name="fileToPath">The altered file to compare to.</param>
        public ByteDiff(string fileFromPath, string fileToPath, ByteDiffChunkBuilderOptions? options = null) {
            Chunks = MakeByteDiffChunks(fileFromPath, fileToPath, options);
        }

        /// <summary>
        /// Creates a diff between two streams.
        /// </summary>
        /// <param name="streamFrom">The original stream to compare to.</param>
        /// <param name="streamTo">The altered stream to compare to.</param>
        public ByteDiff(Stream streamFrom, Stream streamTo, ByteDiffChunkBuilderOptions? options = null) {
            Chunks = MakeByteDiffChunks(streamFrom, streamTo, options);
        }

        /// <summary>
        /// Creates a diff between two sequences of bytes.
        /// </summary>
        /// <param name="bytesFrom">The original bytes to compare to.</param>
        /// <param name="bytesTo">The altered bytes to compare to.</param>
        public ByteDiff(byte[] bytesFrom, byte[] bytesTo, ByteDiffChunkBuilderOptions? options = null) {
            Chunks = MakeByteDiffChunks(bytesFrom, bytesTo, options);
        }

        /// <summary>
        /// Creates a diff based on a file in DFR format.
        /// </summary>
        /// <param name="dfrFilename">Filename of the DFR file.</param>
        public ByteDiff(string dfrFilename) {
            Chunks = MakeByteDiffChunks(dfrFilename);
        }

        /// <summary>
        /// Creates a diff based on a stream of text in DFR format.
        /// </summary>
        /// <param name="dfrStream">Stream containing DFR text.</param>
        public ByteDiff(Stream dfrStream) {
            Chunks = MakeByteDiffChunks(dfrStream);
        }

        /// <summary>
        /// Converts all changes into a DFR file.
        /// </summary>
        /// <returns>A single newline-separated string with a trailing newline.</returns>
        public string ToDFR() {
            var sb = new StringBuilder();
            foreach (var chunk in Chunks)
                _ = sb.Append(chunk.ToDFR() + "\n");
            return sb.ToString();
        }

        /// <summary>
        /// Applies this set of ByteChunk's to a copy of an input buffer and returns that copy.
        /// </summary>
        /// <param name="bytes">Input data.</param>
        /// <returns>A copy of 'bytes', sizes to fit the data requested</returns>
        public byte[] ApplyTo(byte[] bytes) {
            // TODO: this doesn't handle a lot of cases with overlap or things like that!!
            var length = Math.Max(bytes.Length, Chunks.Max(x => (int) x.Address + x.BytesTo.Length));
            var newBytes = new byte[length];
            bytes.CopyTo(newBytes, 0);

            foreach (var chunk in Chunks)
                chunk.ApplyTo(newBytes);

            return newBytes;
        }

        /// <summary>
        /// Array of all chunks.
        /// </summary>
        private IEnumerable<ByteDiffChunk> Chunks { get; }
    }
}
