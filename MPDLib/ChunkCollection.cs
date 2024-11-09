using System.Collections.Generic;
using System.IO;
using System.Text;
using MPDLib.Extensions;

namespace MPDLib {
    /// <summary>
    /// A collection of chunks read from an MPD file.
    /// </summary>
    /// TODO: make this an actual collection!
    public class ChunkCollection {
        public ChunkCollection(Stream stream) {
            FetchChunks(stream);
        }

        private void FetchChunks(Stream stream) {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true)) {
                _ = stream.Seek(0x2000, SeekOrigin.Begin);
                Chunks = new Chunk[20];

                for (int i = 0; i < Chunks.Length; i++) {
                    var offset = reader.ReadLittleEndianInt32() - 0x290000;
                    var size = reader.ReadLittleEndianInt32();
                    var pos = stream.Seek(0, SeekOrigin.Current);
                    stream.Seek(offset, SeekOrigin.Begin);
                    Chunks[i] = new Chunk(stream, size);
                    _ = stream.Seek(pos, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Processes all chunks and returns them decompressed
        /// </summary>
        /// <param name="logFileBaseFilename">Optional filename for logging</param>
        /// <returns></returns>
        public byte[][] DecompressAllChunks(string logFileBaseFilename = null) {
            var data = new byte[Chunks.Length][];
            for (int c = 5; c < Chunks.Length; c++) {
                var chunk = Chunks[c];
                if (chunk.Size > 0 && c != 20)
                    data[c] = chunk.Decompress(logFileBaseFilename + "_" + c + "_log.txt");
            }
            return data;
        }

        /// <summary>
        /// Gets a specific chunk at 'index'
        /// </summary>
        /// <param name="index">Index of chunk to get</param>
        /// <returns>A Chunk at 'index'. Throws an exception if out of range.</returns>
        public Chunk this[int index] => Chunks[index];

        /// <summary>
        /// All chunks loaded.
        /// TODO: make this private, and let us iterate over it from the outside?
        /// </summary>
        public Chunk[] Chunks { get; private set; }
    }
}
