using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Tables;
using SF3.Tables.MPD;
using SF3.Types;
using System.Linq;
using CommonLib;

namespace SF3.Editors.MPD {
    public class MPD_Editor : ScenarioTableEditor, IMPD_Editor {
        protected MPD_Editor(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext, scenario) {
        }

        public static MPD_Editor Create(IRawEditor editor, INameGetterContext nameContext, ScenarioType scenario) {
            var newEditor = new MPD_Editor(editor, nameContext, scenario);
            if (!newEditor.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newEditor;
        }

        public override IEnumerable<ITable> MakeTables() {
            var ramOffset = 0x290000;

            // Create and load Header
            var headerAddrPtr = Editor.GetDouble(0x0000) - ramOffset;
            var headerAddr = Editor.GetDouble(headerAddrPtr) - ramOffset;
            Header = new HeaderTable(Editor, headerAddr, hasPalette3: Scenario >= ScenarioType.Scenario3);
            _ = Header.Load();
            var header = Header.Rows[0];

            // Load palettes
            Palettes = new ColorTable[3];
            if (header.OffsetPal1 > 0)
                Palettes[0] = new ColorTable(Editor, header.OffsetPal1 - ramOffset, 256);
            if (header.OffsetPal2 > 0)
                Palettes[1] = new ColorTable(Editor, header.OffsetPal2 - ramOffset, 256);
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 > 0)
                Palettes[2] = new ColorTable(Editor, Header.Rows[0].OffsetPal3 - ramOffset, 256);

            // Create chunk data
            ChunkHeader = new ChunkHeaderTable(Editor, 0x2000);
            _ = ChunkHeader.Load();

            Chunks = new Chunk[ChunkHeader.Rows.Length];
            for (var i = 0; i < Chunks.Length; i++) {
                var chunkInfo = ChunkHeader.Rows[i];
                if (chunkInfo.ChunkAddress > 0)
                    Chunks[i] = new Chunk(((ByteEditor) Editor).Data, chunkInfo.ChunkAddress - ramOffset, chunkInfo.ChunkSize);
            }

            CompressedEditors = new ICompressedEditor[Chunks.Length];
            ChunkEditors = new IByteEditor[Chunks.Length];

            if (Chunks[2]?.Data?.Length > 0)
                ChunkEditors[2] = new ByteEditor(Chunks[2].Data);
            // TODO: this works, but it's kind of a dumb hack!!
            else if (Chunks[20]?.Data?.Length == 52992)
                ChunkEditors[2] = new ByteEditor(Chunks[20].Data);
            if (Chunks[5]?.Data != null) {
                CompressedEditors[5] = new CompressedEditor(Chunks[5].Data);
                ChunkEditors[5] = CompressedEditors[5].DecompressedEditor;
            }

            // Texture editors, in chunks (6...9)
            for (var i = 6; i <= 9; i++) {
                try {
                    CompressedEditors[i] = new CompressedEditor(Chunks[i].Data);
                    ChunkEditors[i] = CompressedEditors[i].DecompressedEditor;
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            var tables = new List<ITable>() {
                Header,
                ChunkHeader,
                (TileSurfaceHeightmapRows = new TileSurfaceHeightmapRowTable(ChunkEditors[5], 0x0000)),
                (TileHeightTerrainRows    = new TileHeightTerrainRowTable   (ChunkEditors[5], 0x4000)),
                (TileItemRows             = new TileItemRowTable            (ChunkEditors[5], 0x6000)),
            };

            if (ChunkEditors[2]?.Data?.Length >= 64 * 64 * 2)
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(ChunkEditors[2], 0x0000));

            for (var i = 0; i < Palettes.Length; i++)
                if (Palettes[i] != null)
                    tables.Add(Palettes[i]);

            TextureChunks = new TextureChunkEditor[4];
            for (var i = 0; i < 4; i++) {
                int chunkIndex = i + 6;
                if (Chunks[chunkIndex].Data?.Length > 0) {
                    TextureChunks[i] = TextureChunkEditor.Create(ChunkEditors[chunkIndex], NameGetterContext, 0x00, "TextureChunk" + (i + 1));
                    tables.Add(TextureChunks[i].HeaderTable);
                    tables.Add(TextureChunks[i].TextureTable);
                }
            }

            // Add some callbacks to all child editors.
            foreach (var ci in CompressedEditors
                .Concat(ChunkEditors)
                .Where(ci => ci != null)
            ) {
                // If the editor is marked as unmodified (such as after a save), mark child editors as unmodified as well.
                Editor.IsModifiedChanged += (s, e) => ci.IsModified &= Editor.IsModified;

                // If any of the child editors are marked as modified, mark the parent editor as modified as well.
                ci.IsModifiedChanged += (s, e) => Editor.IsModified |= ci.IsModified;
            }

            return tables;
        }

        public override bool OnFinalize() {
            bool success = true;
            foreach (var ci in CompressedEditors
                .Concat(ChunkEditors)
                .Where(ci => ci != null)
            ) {
                success &= ci.Finalize();
            }
            return success;
        }

        public override void Dispose() {
            base.Dispose();

            if (CompressedEditors != null) {
                foreach (var ci in CompressedEditors.Where(ci => ci != null))
                    ci.Dispose();
                CompressedEditors = null;
            }
            if (ChunkEditors != null) {
                foreach (var ci in ChunkEditors.Where(ci => ci != null))
                    ci.Dispose();
                ChunkEditors = null;
            }
        }

        public ICompressedEditor[] CompressedEditors { get; private set; }
        public IByteEditor[] ChunkEditors { get; private set; }

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
        public TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; private set; }

        [BulkCopyRecurse]
        public TileHeightTerrainRowTable TileHeightTerrainRows { get; private set; }

        [BulkCopyRecurse]
        public TileItemRowTable TileItemRows { get; private set; }

        [BulkCopyRecurse]
        public TextureChunkEditor[] TextureChunks { get; private set; }
    }
}
