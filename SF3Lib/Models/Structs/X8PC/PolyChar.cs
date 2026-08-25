using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.Shared.SGL;
using SF3.Models.Tables.X8PC;

namespace SF3.Models.Structs.X8PC {
    public class PolyChar : Struct, ITableContainer {
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
            ModelChunk   = Chunks[2];

            // Initialize structs and tables in chunks.
            TexDefChunkHeader = new PCTexDefChunkHeader(TexDefChunk.DecompressedData, 0, nameof(TexDefChunkHeader), 0);
            TextureTable      = PCTextureTable.Create(TexDefChunk.DecompressedData, TexDataChunk.DecompressedData.Data, "Textures", (int) TexDefChunkHeader.TexDefsOffset, (int) TexDefChunkHeader.NumTextures);
            ModelChunkHeader  = new PCModelChunkHeader(ModelChunk.DecompressedData, 0, nameof(ModelChunkHeader), 0);
            XPDataListTable   = XPDataListTable.Create(ModelChunk.DecompressedData, "XPDATA_Lists", (int) ModelChunkHeader.ModelsOffset);
            XPDataTables      = XPDataListTable.Select(x => XPDataTable.Create(ModelChunk.DecompressedData, $"XPDATAs_{x.ID}", x.XPDataListOffset)).ToArray();

            VertexTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.VertexCount, (int) x.VerticesOffset),
                (count, offset, index) => VertexTable.Create(ModelChunk.DecompressedData, $"{nameof(VertexTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            PolygonTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.FaceCount, (int) x.PolygonsOffset),
                (count, offset, index) => PolygonTable.Create(ModelChunk.DecompressedData, $"{nameof(PolygonTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            AttrTablesByOffset = FetchTablesByOffset(
                XPDataTables,
                x => (x.FaceCount, (int) x.AttributesOffset),
                (count, offset, index) => AttrTable.Create(ModelChunk.DecompressedData, $"{nameof(AttrTable)}_{index:D3} @{offset:X4}", offset, count)
            );

            tables.AddRange(Header.Tables);
            tables.Add(TextureTable);
            tables.Add(XPDataListTable);
            tables.AddRange(XPDataTables);

            Tables = tables.ToArray();
        }

        private static Dictionary<int, T> FetchTablesByOffset<T>(XPDataTable[] tables, Func<XPDataStruct, (int Count, int Offset)> countOffsetFetcher, Func<int, int, int, T> tableMaker) {
            return tables
                .SelectMany(x => x.Select(y => countOffsetFetcher(y)))
                .OrderBy(x => x.Offset)
                .ThenByDescending(x => x.Count)
                .GroupBy(x => x.Offset)
                .Select((x, i) => (Model: x.First(), Index: i))
                .ToDictionary(
                    x => x.Model.Offset,
                    x => tableMaker(x.Model.Count, x.Model.Offset, x.Index)
                );
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

        public PCHeader Header { get; private set; }

        public PCTexDefChunkHeader TexDefChunkHeader { get; private set; }
        public PCTextureTable TextureTable { get; private set; }

        public PCModelChunkHeader ModelChunkHeader { get; private set; }
        public XPDataListTable XPDataListTable { get; private set; }
        public XPDataTable[] XPDataTables { get; private set; }
        public Dictionary<int, VertexTable> VertexTablesByOffset { get; }
        public Dictionary<int, PolygonTable> PolygonTablesByOffset { get; }
        public Dictionary<int, AttrTable> AttrTablesByOffset { get; }

        public ChunkData[] Chunks { get; private set; }
        public ChunkData TexDefChunk { get; private set; }
        public ChunkData TexDataChunk { get; private set; }
        public ChunkData ModelChunk { get; private set; }
    }
}
