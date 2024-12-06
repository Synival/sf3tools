using System;
using System.IO;

namespace CommonLib {
    /// <summary>
    /// Definition for a single chunk that can possibly be compressed or decompressed.
    /// </summary>
    public class Chunk {
        public Chunk(Stream stream, int size) {
            // Snarf all the data.
            Data = new ByteArray(size);
            if (size > 0) {
                var bytes = new byte[size];
                _ = stream.Read(bytes, 0, size);
                Data.SetDataTo(bytes);
            }
        }

        public Chunk(byte[] data) {
            Data = new ByteArray(data);
        }

        public Chunk(ByteArray data) {
            Data = data;
        }

        public Chunk(byte[] data, int offset, int size) {
            // Snarf all the data.
            Data = new ByteArray(size);
            if (size > 0) {
                using (Stream stream = new MemoryStream(data, offset, size)) {
                    var bytes = new byte[size];
                    _ = stream.Read(bytes, 0, size);
                    Data.SetDataTo(bytes);
                }
            }
        }

        /// <summary>
        /// Processed compressed chunk data and returns an uncompressed byte[].
        /// All credit to AggroCrag for this decompression code!
        /// </summary>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public byte[] Decompress()
            => Utils.Compression.Decompress(Data.GetDataCopy());

        /// <summary>
        /// Size of the chunk in the MPD file
        /// </summary>
        public int Size => Data.Length;

        /// <summary>
        /// Data read from the MPD file
        /// </summary>
        public ByteArray Data { get; }
    }
}
