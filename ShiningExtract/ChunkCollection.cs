using System.Text;
using CommonLib;
using CommonLib.Extensions;

namespace ShiningExtract {
    /// <summary>
    /// A collection of chunks read from an MPD file.
    /// </summary>
    /// TODO: make this an actual collection!
    public class ChunkCollection {
        public ChunkCollection(Stream stream) {
            Chunks = FetchChunks(stream);
        }

        private static Chunk[] FetchChunks(Stream stream) {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true)) {
                _ = stream.Seek(0x2000, SeekOrigin.Begin);
                var chunkList = new List<Chunk>();

                for (int i = 0; i < 32; i++) {
                    var offset = reader.ReadLittleEndianInt32();
                    var size = reader.ReadLittleEndianInt32();
                    if (offset == 0)
                        break;
                    offset -= 0x00290000;

                    var pos = stream.Seek(0, SeekOrigin.Current);
                    stream.Seek(offset, SeekOrigin.Begin);
                    chunkList.Add(new Chunk(stream, size));
                    _ = stream.Seek(pos, SeekOrigin.Begin);
                }

                return chunkList.ToArray();
            }
        }

        /// <summary>
        /// Processes all chunks and returns them decompressed
        /// </summary>
        /// <param name="logFileBaseFilename">Optional filename for logging</param>
        /// <returns></returns>
        public byte[][] DecompressAllChunks(string? logFileBaseFilename = null) {
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
