using System;
using System.Collections.Generic;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PolyChar : Struct, ITableContainer {
        public PCHeader Header { get; private set; }
        public PCTexDefChunkHeader TexDefChunkHeader { get; private set; }
        public PCTextureTable TextureTable { get; private set; }

        public ChunkData[] Chunks { get; private set; }
        public ChunkData TexDefChunk { get; private set; }
        public ChunkData TexDataChunk { get; private set; }

        public PolyChar(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0 /* not applicable */) {
            var tables = new List<ITable>();

            // Build the first header, which is the chunk table.
            Header = new PCHeader(Data, 0, nameof(PCHeader), Address);

            // Build all chunks.
            Chunks = new ChunkData[Header.ChunkDefTable.Count];
            for (int i = 0; i < Chunks.Length; i++) {
                var isCompressed = (i == 1);
                var def = Header.ChunkDefTable[i];
                Chunks[i] = new ChunkData(new ByteArray(Data.Data.GetDataCopyAt(Address + (int) def.Offset, (int) def.DataSize)), isCompressed, i);
                Chunks[i].DecompressedData.IsModifiedChanged += (s, e) => Data.IsModified |= ((IByteData) s).IsModified;
            }

            // Store references to chunks by name as well as index.
            TexDefChunk  = Chunks[0];
            TexDataChunk = Chunks[1];

            // Initialize structs and tables in chunks.
            TexDefChunkHeader = new PCTexDefChunkHeader(TexDefChunk.DecompressedData, 0, nameof(TexDefChunkHeader), 0);
            TextureTable      = PCTextureTable.Create(TexDefChunk.DecompressedData, TexDataChunk.DecompressedData.Data, "Textures", (int) TexDefChunkHeader.TexDefsOffset, (int) TexDefChunkHeader.NumTextures);

            tables.AddRange(Header.Tables);
            tables.Add(TextureTable);

            Tables = tables.ToArray();
        }

        public bool UpdateAndCommitChunks() {
            // Rebuild the entire chunk table.
            uint nextOffset = 0x800;
            uint polyCharOffset = (uint) Address;
            foreach (var chunk in Chunks) {
                var def = Header.ChunkDefTable[chunk.Index];

                if (!chunk.DecompressedData.IsModified && !chunk.IsModified) {
                    nextOffset += def.ChunkSize;
                    continue;
                }

                // Recompress if necessary.
                if (TexDataChunk.NeedsRecompression)
                    if (!TexDataChunk.Recompress())
                        return false;

                // Update the chunk table entry.
                def.Offset = nextOffset;
                def.DataSize = (uint) chunk.Data.Length;
                def.ChunkSize = (uint) ((chunk.Data.Length + 0x7FF) / 0x800) * 0x800;

                // Copy to the actual data.
                Data.Data.SetDataAtTo((int) (polyCharOffset + def.Offset), (int) def.DataSize, chunk.Data.GetDataCopyOrReference());

                // Pad the rest of the chunk with 0xFF.
                var paddingBytes = new byte[(int) def.ChunkSize - def.DataSize];
                paddingBytes.AsSpan().Fill(0xFF);
                Data.Data.SetDataAtTo((int) (polyCharOffset + def.Offset + def.DataSize), paddingBytes.Length, paddingBytes);

                nextOffset += def.ChunkSize;
            }

            return true;
        }

        public IEnumerable<ITable> Tables { get; private set; }
    }
}
