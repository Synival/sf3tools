using DFRLib.Exceptions;
using DFRLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DFRLib
{
    /// <summary>
    /// A collection of differences between two sets of data.
    /// Can be used to generate DFR files.
    /// </summary>
    public class ByteDiff
    {
        /// <summary>
        /// Creates a list of differences between two files of the same size.
        /// </summary>
        /// <param name="fileFromPath">The original file to compare to.</param>
        /// <param name="fileToPath">The altered file to compare to.</param>
        /// <returns>A list containing all differences between the two files.</returns>
        /// <exception cref="ByteDiffException">Thrown when the file streams are not of the same length.</exception>
        static public List<ByteDiffChunk> MakeByteDiffChunks(string fileFromPath, string fileToPath)
        {
            FileStream? fileStreamFrom = null;
            FileStream? fileStreamTo = null;

            try
            {
                fileStreamFrom = new FileStream(fileFromPath, FileMode.Open);
                fileStreamTo = new FileStream(fileToPath, FileMode.Open);

                var fileStreamFromLen = fileStreamFrom.Length;
                var fileStreamToLen = fileStreamTo.Length;

                if (fileStreamFromLen != fileStreamToLen)
                    throw new ByteDiffException("Input streams are not the same length");

                return MakeByteDiffChunks(fileStreamFrom, fileStreamTo);
            }
            finally
            {
                if (fileStreamFrom != null)
                    fileStreamFrom.Close();
                if (fileStreamTo != null)
                    fileStreamTo.Close();
            }
        }

        /// <summary>
        /// Creates a list of differences between two sets of bytes of the same size.
        /// </summary>
        /// <param name="fileFromPath">The original bytes to compare to.</param>
        /// <param name="fileToPath">The altered bytes to compare to.</param>
        /// <returns>A list containing all differences between the two sets of bytes.</returns>
        /// <exception cref="ByteDiffException">Thrown when the sets of bytes are not of the same length.</exception>
        static public List<ByteDiffChunk> MakeByteDiffChunks(byte[] bytesFrom, byte[] bytesTo)
        {
            if (bytesFrom.Length != bytesTo.Length)
                throw new ByteDiffException("Input data sets are not the same length");

            return MakeByteDiffChunks(new MemoryStream(bytesFrom), new MemoryStream(bytesTo));
        }

        /// <summary>
        /// Creates a list of differences between two streams of the same size.
        /// </summary>
        /// <param name="streamFrom">The original stream to compare to.</param>
        /// <param name="streamTo">The altered stream to compare to.</param>
        /// <returns>A list containing all differences between the two files.</returns>
        static public List<ByteDiffChunk> MakeByteDiffChunks(Stream streamFrom, Stream streamTo)
        {
            int streamFromRead;
            int streamToRead;
            byte[] streamFromBuf = new byte[1024];
            byte[] streamToBuf = new byte[1024];

            var chunkBuilder = new ByteDiffChunkBuilder();

            // Read from streams until no more data is available.
            while (true)
            {
                streamFromRead = streamFrom.Read(streamFromBuf, 0, streamFromBuf.Length);
                streamToRead = streamTo.Read(streamToBuf, 0, streamToBuf.Length);

                if (streamFromRead != streamToRead)
                {
                    var shorterStream = (streamFromRead < streamToRead) ? "'From' stream" : "'To' stream";
                    throw new ByteDiffException(shorterStream + " reached EOF before other stream was finished");
                }

                if (streamFromRead == 0 || streamToRead == 0)
                {
                    break;
                }

                // Process bytes read.
                for (int i = 0; i < streamFromRead; i++)
                    chunkBuilder.Feed(streamFromBuf[i], streamToBuf[i]);
            }

            return chunkBuilder.FetchChunks();
        }

        /// <summary>
        /// Creates a diff between two files of the same size.
        /// </summary>
        /// <param name="fileFromPath">The original file to compare to.</param>
        /// <param name="fileToPath">The altered file to compare to.</param>
        public ByteDiff(string fileFromPath, string fileToPath)
        {
            Chunks = MakeByteDiffChunks(fileFromPath, fileToPath);
        }

        /// <summary>
        /// Creates a diff between two streams of the same size.
        /// </summary>
        /// <param name="streamFrom">The original stream to compare to.</param>
        /// <param name="streamTo">The altered stream to compare to.</param>
        public ByteDiff(Stream streamFrom, Stream streamTo)
        {
            Chunks = MakeByteDiffChunks(streamFrom, streamTo);
        }

        /// <summary>
        /// Creates a diff between two sequences of bytes.
        /// </summary>
        /// <param name="bytesFrom">The original bytes to compare to.</param>
        /// <param name="bytesTo">The altered bytes to compare to.</param>
        public ByteDiff(byte[] bytesFrom, byte[] bytesTo)
        {
            Chunks = MakeByteDiffChunks(bytesFrom, bytesTo);
        }

        /// <summary>
        /// Converts all changes into a DFR file.
        /// </summary>
        /// <returns>A single newline-separated string with a trailing newline.</returns>
        public string ToDFR()
        {
            var sb = new StringBuilder();
            foreach (var chunk in Chunks)
                sb.Append(chunk.ToDFR() + "\n");
            return sb.ToString();
        }

        /// <summary>
        /// Array of all chunks.
        /// </summary>
        IEnumerable<ByteDiffChunk> Chunks { get; }
    }
}
