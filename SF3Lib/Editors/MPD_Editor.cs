using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using MPDLib;
using SF3.RawEditors;
using SF3.Models.MPD.TextureChunk;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.MPD;
using SF3.Types;

namespace SF3.Editors {
    public class MPD_Editor : ScenarioTableEditor, IMPD_Editor {
        public MPD_Editor(ScenarioType scenario) : base(scenario, new NameGetterContext(scenario)) {
        }

        public override bool SaveFile(string filename)
            => throw new NotImplementedException();

        public override IEnumerable<ITable> MakeTables() {
            var ramOffset = 0x290000;

            // Create and load Header
            var headerAddrPtr = GetDouble(0x0000) - ramOffset;
            var headerAddr = GetDouble(headerAddrPtr) - ramOffset;
            Header = new HeaderTable(this, headerAddr, hasPalette3: Scenario >= ScenarioType.Scenario3);
            _ = Header.Load();
            var header = Header.Rows[0];

            // Load palettes
            Palettes = new ColorTable[3];
            if (header.OffsetPal1 > 0)
                Palettes[0] = new ColorTable(this, header.OffsetPal1 - ramOffset, 256);
            if (header.OffsetPal2 > 0)
                Palettes[1] = new ColorTable(this, header.OffsetPal2 - ramOffset, 256);
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 > 0)
                Palettes[2] = new ColorTable(this, Header.Rows[0].OffsetPal3 - ramOffset, 256);

            // Create chunk data
            ChunkHeader = new ChunkHeaderTable(this, 0x2000);
            _ = ChunkHeader.Load();

            Chunks = new Chunk[ChunkHeader.Rows.Length];
            for (int i = 0; i < Chunks.Length; i++) {
                var chunkInfo = ChunkHeader.Rows[i];
                if (chunkInfo.ChunkAddress > 0)
                    Chunks[i] = new Chunk(Data, chunkInfo.ChunkAddress - ramOffset, chunkInfo.ChunkSize);
            }

            ChunkEditors = new IRawEditor[Chunks.Length];
            if (Chunks[2]?.Data?.Length > 0)
                ChunkEditors[2] = new ByteEditor(Chunks[2].Data);
            // TODO: this works, but it's kind of a dumb hack!!
            else if (Chunks[20]?.Data?.Length == 52992)
                ChunkEditors[2] = new ByteEditor(Chunks[20].Data);
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
                Header,
                ChunkHeader,
                (TileHeightmapRows = new TileHeightmapRowTable(ChunkEditors[5], 0x0000)),
                (TileHeightRows    = new TileHeightRowTable   (ChunkEditors[5], 0x4000)),
                (TileTerrainRows   = new TileTerrainRowTable  (ChunkEditors[5], 0x4001)),
                (TileItemRows      = new TileItemRowTable     (ChunkEditors[5], 0x6000)),
            };

            if (ChunkEditors[2]?.Data?.Length >= (64 * 64 * 2))
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(ChunkEditors[2], 0x0000));

            for (var i = 0; i < Palettes.Length; i++)
                if (Palettes[i] != null)
                    tables.Add(Palettes[i]);

            TextureChunks = new TextureChunk[4];
            for (int i = 0; i < 4; i++) {
                if (Chunks[i + 6].Data?.Length > 0) {
                    TextureChunks[i] = new TextureChunk(ChunkEditors[i + 6], 0x00, "TextureChunk" + (i + 1));
                    tables.Add(TextureChunks[i].HeaderTable);
                    tables.Add(TextureChunks[i].TextureTable);
                }
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

        public IRawEditor[] ChunkEditors { get; private set; }

        [BulkCopyRecurse]
        public HeaderTable Header { get; private set; }

        [BulkCopyRecurse]
        public ColorTable[] Palettes { get; private set; }

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
