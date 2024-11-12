using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using MPDLib;
using SF3.Models.MPD.TextureChunk;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.MPD;
using SF3.Types;

namespace SF3.FileEditors {
    public class MPD_FileEditor : SF3FileEditor, IMPD_FileEditor {
        public MPD_FileEditor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override bool SaveFile(string filename)
            => throw new NotImplementedException();

        public override IEnumerable<ITable> MakeTables() {
            // Get address for the header
            var headerAddrPtr = GetDouble(0x0000) - 0x290000;
            var headerAddr = GetDouble(headerAddrPtr) - 0x290000;

            // Create chunk data
            ChunkHeader = new ChunkHeaderTable(this, 0x2000);
            _ = ChunkHeader.Load();

            Chunks = new Chunk[ChunkHeader.Rows.Length];
            for (int i = 0; i < Chunks.Length; i++) {
                var chunkInfo = ChunkHeader.Rows[i];
                if (chunkInfo.ChunkAddress > 0)
                    Chunks[i] = new Chunk(Data, chunkInfo.ChunkAddress - 0x00290000, chunkInfo.ChunkSize);
            }

            ChunkEditors = new IByteEditor[Chunks.Length];
            if (Chunks[2]?.Data != null)
                ChunkEditors[2] = new ByteEditor(Chunks[2].Data);
            if (Chunks[5]?.Data != null)
                ChunkEditors[5] = new ByteEditor(Chunks[5].Decompress());

            // Texture editors.
            for (int i = 0; i < 4; i++) {
                try {
                    ChunkEditors[i + 6] = new ByteEditor(Chunks[i + 6].Decompress());
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            var tables = new List<ITable>() {
                (Header            = new HeaderTable          (this, headerAddr)),
                ChunkHeader,
                (TileHeightmapRows = new TileHeightmapRowTable(ChunkEditors[5], 0x0000)),
                (TileHeightRows    = new TileHeightRowTable   (ChunkEditors[5], 0x4000)),
                (TileTerrainRows   = new TileTerrainRowTable  (ChunkEditors[5], 0x4001)),
                (TileItemRows      = new TileItemRowTable     (ChunkEditors[5], 0x6000)),
            };

            if (Chunks[2].Data?.Length >= (64 * 64 * 2))
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(ChunkEditors[2], 0x0000));

            TextureChunks = new TextureChunk[4];
            for (int i = 0; i < 4; i++) {
                if (Chunks[i + 6].Data?.Length > 0)
                    TextureChunks[i] = new TextureChunk(ChunkEditors[i + 6], 0x00, "TextureChunk" + (i + 1));
            }

            return tables;
        }

        public override void DestroyTables() {
            Header                   = null;
            ChunkHeader              = null;
            TileSurfaceCharacterRows = null;
            TileHeightmapRows        = null;
            TileHeightRows           = null;
            TileTerrainRows          = null;
            TileItemRows             = null;
            TextureChunks            = null;
            ChunkEditors             = null;
            Chunks                   = null;
        }

        public IByteEditor[] ChunkEditors { get; private set; }

        [BulkCopyRecurse]
        public HeaderTable Header { get; private set; }

        [BulkCopyRecurse]
        public ChunkHeaderTable ChunkHeader { get; private set; }

        public Chunk[] Chunks { get; private set; }

        [BulkCopyRecurse]
        public TileSurfaceCharacterRowTable TileSurfaceCharacterRows { get; private set; }

        [BulkCopyRecurse]
        public TileHeightmapRowTable TileHeightmapRows { get; private set; }

        [BulkCopyRecurse]
        public TileHeightRowTable TileHeightRows { get; private set; }

        [BulkCopyRecurse]
        public TileTerrainRowTable TileTerrainRows { get; private set; }

        [BulkCopyRecurse]
        public TileItemRowTable TileItemRows { get; private set; }

        [BulkCopyRecurse]
        public TextureChunk[] TextureChunks { get; private set; }
    }
}
