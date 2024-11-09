using System.Collections.Generic;
using System.IO;
using System.Text;
using MPDLib.Extensions;

namespace MPDLib {
    /// <summary>
    /// 
    /// </summary>
    public class MPDFile {
        public void FetchChunkDefinitions(Stream stream) {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true)) {
                stream.Seek(0x2000, SeekOrigin.Begin);
                Chunks = new ChunkDefinition[32];
                for (int i = 0; i < Chunks.Length; i++)
                    Chunks[i] = new ChunkDefinition(reader.ReadLittleEndianInt32() - 0x290000, reader.ReadLittleEndianInt32());
            }
        }

        public Dictionary<int, byte[]> DecompressAllChunks(Stream stream, string logFileBaseFilename = null) {
            var data = new Dictionary<int, byte[]>();
            for (int c = 5; c < Chunks.Length; c++) {
                var chunk = Chunks[c];
                if (chunk.Length > 0 && c != 20)
                    data[c] = chunk.Decompress(stream, logFileBaseFilename + "_" + c + "_log.txt");
            }
            return data;
        }

        public ChunkDefinition[] Chunks { get; private set; }
    }
}
