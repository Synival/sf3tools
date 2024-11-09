using System.Collections.Generic;
using System.IO;
using System.Text;
using MPDLib.Extensions;

namespace MPDLib {
    /// <summary>
    /// 
    /// </summary>
    public class MPDFile {
        public MPDFile(Stream stream) {
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

        public byte[][] DecompressAllChunks(string logFileBaseFilename = null) {
            var data = new byte[Chunks.Length][];
            for (int c = 5; c < Chunks.Length; c++) {
                var chunk = Chunks[c];
                if (chunk.Size > 0 && c != 20)
                    data[c] = chunk.Decompress(logFileBaseFilename + "_" + c + "_log.txt");
            }
            return data;
        }

        public Chunk[] Chunks { get; private set; }
    }
}
