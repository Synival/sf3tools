using System;
using System.Collections.Generic;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;
using SF3.Types;
using SF3.Models.Tables;
using CommonLib.Arrays;

namespace SF3.Models.Files.X8PC {
    public class X8PC_File : ScenarioTableFile, IX8PC_File {
        public override int RamAddress      => 0x060A0000;
        public override int RamAddressLimit => 0x060A8000; // TODO: confirm this!

        protected X8PC_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario)
        : base(data, nameContext, scenario) {

            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X8PC_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X8PC_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException($"Couldn't initialize {nameof(X8PC_File)}");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>();

            // Build the first header, which is the chunk table.
            Header = new PCHeader(Data, 0, nameof(PCHeader), 0x00);

            // Build all chunks.
            Chunks = new ChunkData[Header.BattleModelChunkDefTable.Count];
            for (int i = 0; i < Chunks.Length; i++) {
                var isCompressed = (i == 1);
                var def = Header.BattleModelChunkDefTable[i];
                Chunks[i] = new ChunkData(new ByteArray(Data.Data.GetDataCopyAt((int) def.Offset, (int) def.DataSize)), isCompressed, i);
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

            return tables.ToArray();
        }

        public override bool OnFinish() {
            base.OnFinish();

            // Rebuild the entire chunk table.
            uint nextOffset = 0x800;
            foreach (var chunk in Chunks) {
                if (!chunk.DecompressedData.IsModified && !chunk.IsModified)
                    continue;

                // Recompress if necessary.
                if (TexDataChunk.NeedsRecompression)
                    if (!TexDataChunk.Recompress())
                        return false;

                // Update the chunk table entry.
                var def = Header.BattleModelChunkDefTable[chunk.Index];
                def.Offset = nextOffset;
                def.DataSize = (uint) chunk.Data.Length;
                def.ChunkSize = (uint) ((chunk.Data.Length + 0x7FF) / 0x800) * 0x800;

                // Copy to the actual data.
                Data.Data.SetDataAtTo((int) def.Offset, (int) def.DataSize, chunk.Data.GetDataCopyOrReference());

                // Pad the rest of the chunk with 0xFF.
                var paddingBytes = new byte[(int) def.ChunkSize - def.DataSize];
                paddingBytes.AsSpan().Fill(0xFF);
                Data.Data.SetDataAtTo((int) (def.Offset + def.DataSize), paddingBytes.Length, paddingBytes);

                nextOffset += def.ChunkSize;
            }

            return true;
        }

        public PCHeader Header { get; private set; }
        public PCTexDefChunkHeader TexDefChunkHeader { get; private set; }
        public PCTextureTable TextureTable { get; private set; }

        public ChunkData[] Chunks { get; private set; }
        public ChunkData TexDefChunk { get; private set; }
        public ChunkData TexDataChunk { get; private set; }
    }
}
