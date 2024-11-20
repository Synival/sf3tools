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
using System.Runtime.InteropServices;
using SF3.Tables.MPD.TextureGroup;

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
            const int ramOffset = 0x290000;

            // Create and load Header
            var headerAddrPtr = Editor.GetDouble(0x0000) - ramOffset;
            var headerAddr = Editor.GetDouble(headerAddrPtr) - ramOffset;
            Header = new Tables.MPD.HeaderTable(Editor, headerAddr, hasPalette3: Scenario >= ScenarioType.Scenario3);
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

            // Create other tables from header offsets.
            Offset1Table = (header.Offset1 != 0) ? new UnknownUInt16Table(Editor, header.Offset1 - ramOffset, 32) : null;
            Offset2Table = (header.Offset2 != 0) ? new UnknownUInt32Table(Editor, header.Offset2 - ramOffset, 1)  : null;
            Offset3Table = (header.Offset3 != 0) ? new UnknownUInt16Table(Editor, header.Offset3 - ramOffset, 32) : null;
            Offset4Table = (header.Offset4 != 0) ? new Offset4Table(Editor, header.Offset4 - ramOffset) : null;

            if (header.OffsetTextureGroups != 0) {
                TextureGroupHeader = new Tables.MPD.TextureGroup.HeaderTable(Editor, header.OffsetTextureGroups - ramOffset);
                TextureGroupHeader.Load();
                TextureGroupFrames = new FrameTable(Editor, TextureGroupHeader.Address, TextureGroupHeader.Rows);
                TextureGroupFrames.Load();
            }

            // Create chunk data
            ChunkHeader = new ChunkHeaderTable(Editor, 0x2000);
            _ = ChunkHeader.Load();

            Chunks = new Chunk[ChunkHeader.Rows.Length];
            for (var i = 0; i < Chunks.Length; i++) {
                var chunkInfo = ChunkHeader.Rows[i];
                if (chunkInfo.ChunkAddress > 0)
                    Chunks[i] = new Chunk(((ByteEditor) Editor).Data, chunkInfo.ChunkAddress - ramOffset, chunkInfo.ChunkSize);
            }

            // Assign all chunk editors.
            ChunkEditors = new IChunkEditor[Chunks.Length];

            if (Chunks[2]?.Data?.Length > 0) {
                SurfaceChunk = Chunks[2];
                SurfaceChunkEditor = ChunkEditors[2] = new ChunkEditor(SurfaceChunk.Data, false);
            }
            // TODO: this works, but it's kind of a dumb hack!!
            else if (Chunks[20]?.Data?.Length == 52992) {
                SurfaceChunk = Chunks[20];
                SurfaceChunkEditor = ChunkEditors[20] = new ChunkEditor(SurfaceChunk.Data, false);
            }

            if (Chunks[3]?.Data?.Length > 0 && TextureGroupFrames != null) {
                TextureGroupFrameEditors = new CompressedEditor[TextureGroupFrames.Rows.Length];
                for (var i = 0; i < TextureGroupFrames.Rows.Length; i++) {
                    var frame = TextureGroupFrames.Rows[i];
                    if (frame.CompressedTextureOffset != 0xFFFE) {
                        var textureGroup = TextureGroupHeader.Rows[frame.GroupID];
                        // TODO: where's the data????
                    }
                }
            }

            if (Chunks[5]?.Data != null)
                ChunkEditors[5] = new ChunkEditor(Chunks[5].Data, true);

            // Texture editors, in chunks (6...10)
            for (var i = 6; i <= 10; i++) {
                try {
                    ChunkEditors[i] = new ChunkEditor(Chunks[i].Data, true);
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            // We should have all the uncompressed data now. Update read-only info of our chunk table.
            for (var i = 0; i < ChunkHeader.Rows.Length; i++) {
                ChunkHeader.Rows[i].CompressionType =
                    (ChunkEditors[i] == null && (Chunks[i]?.Data?.Length ?? 0) == 0) ? "--" :
                    (ChunkEditors[i] == null) ? "(WIP)" :
                    ChunkEditors[i].IsCompressed ? "Compressed" :
                    "Uncompressed";
            }
            UpdateChunkTableDecompressedSizes();

            // Build a list of all data tables.
            var tables = new List<ITable>() {
                Header,
                ChunkHeader,
                (TileSurfaceHeightmapRows = new TileSurfaceHeightmapRowTable(ChunkEditors[5].DecompressedEditor, 0x0000)),
                (TileHeightTerrainRows    = new TileHeightTerrainRowTable   (ChunkEditors[5].DecompressedEditor, 0x4000)),
                (TileItemRows             = new TileItemRowTable            (ChunkEditors[5].DecompressedEditor, 0x6000)),
            };

            if (TextureGroupHeader != null)
                tables.Add(TextureGroupHeader);

            for (var i = 0; i < Palettes.Length; i++)
                if (Palettes[i] != null)
                    tables.Add(Palettes[i]);

            if (Offset1Table != null)
                tables.Add(Offset1Table);
            if (Offset2Table != null)
                tables.Add(Offset2Table);
            if (Offset3Table != null)
                tables.Add(Offset3Table);
            if (Offset4Table != null)
                tables.Add(Offset4Table);
            if (TextureGroupHeader != null)
                tables.Add(TextureGroupHeader);
            if (TextureGroupFrames != null)
                tables.Add(TextureGroupFrames);

            if (SurfaceChunkEditor?.Data?.Length >= 64 * 64 * 2)
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(SurfaceChunkEditor.DecompressedEditor, 0x0000));

            TextureChunks = new TextureChunkEditor[5];
            for (var i = 0; i < TextureChunks.Length; i++) {
                int chunkIndex = i + 6;
                if (Chunks[chunkIndex].Data?.Length > 0) {
                    TextureChunks[i] = TextureChunkEditor.Create(ChunkEditors[chunkIndex].DecompressedEditor, NameGetterContext, 0x00, "TextureChunk" + (i + 1));
                    tables.Add(TextureChunks[i].HeaderTable);
                    tables.Add(TextureChunks[i].TextureTable);
                }
            }

            // Add some callbacks to all child editors.
            var editors = ChunkEditors
                .Cast<IRawEditor>()
                .Concat(TextureGroupFrameEditors.Cast<IRawEditor>())
                .Where(x => x != null)
                .ToArray();

            foreach (var ce in editors) {
                // If the editor is marked as unmodified (such as after a save), mark child editors as unmodified as well.
                Editor.IsModifiedChanged += (s, e) => ce.IsModified &= Editor.IsModified;

                // If any of the child editors are marked as modified, mark the parent editor as modified as well.
                ce.IsModifiedChanged += (s, e) => Editor.IsModified |= ce.IsModified;
            }

            return tables;
        }

        private void UpdateChunkTableDecompressedSizes() {
            for (var i = 0; i < ChunkHeader.Rows.Length; i++)
                ChunkHeader.Rows[i].DecompressedSize = ChunkEditors[i]?.DecompressedEditor?.Size ?? 0;
        }

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public bool Recompress(bool onlyModified) {
            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !ChunkEditors.Any(x => x != null && (x.IsModified || x.NeedsRecompression)))
                return true;

            const int ramOffset = 0x290000;

            // We'll need to completely rewrite this file. Start by recompressing chunks.
            var currentChunkPos = 0x2100;
            var chunkPositions = new int[Chunks.Length];

            for (int i = 0; i < Chunks.Length; i++) {
                var chunkEditor = ChunkEditors[i];

                // Finalize compressed chunks.
                if (chunkEditor != null && (!onlyModified || chunkEditor.NeedsRecompression || chunkEditor.IsModified)) {
                    if (chunkEditor.IsCompressed && !chunkEditor.Recompress())
                        return false;

                    // TODO: thie invalidates any references!!! maybe just update the thing???
                    Chunks[i] = new Chunk(chunkEditor.Data, 0, chunkEditor.Data.Length);
                }

                // Update the chunk address/size table.
                if (Chunks[i] == null) {
                    ChunkHeader.Rows[i].ChunkAddress = 0;
                    ChunkHeader.Rows[i].ChunkSize    = 0;
                    chunkPositions[i] = 0;
                }
                else {
                    ChunkHeader.Rows[i].ChunkAddress = currentChunkPos + ramOffset;
                    ChunkHeader.Rows[i].ChunkSize    = Chunks[i].Size;
                    chunkPositions[i] = currentChunkPos;
                    currentChunkPos += Chunks[i].Size;

                    // Enforce alignment of 4.
                    if (currentChunkPos % 4 != 0)
                        currentChunkPos += (4 - (currentChunkPos % 4));
                }
            }

            // Start rebuilding new data.
            var newData = new byte[currentChunkPos];
            var inputData = Editor.GetAllData();

            unsafe {
                fixed (byte* output = newData) {
                    // Copy first 0x2100 bytes into our new data.
                    fixed (byte* input = inputData)
                        memcpy((IntPtr) output, (IntPtr) input, 0x2100);

                    // Copy all chunk data into our new data.
                    for (int i = 0; i < Chunks.Length; i++)
                        if (Chunks[i] != null)
                            fixed (byte* input = Chunks[i].Data)
                                _ = memcpy((IntPtr) (output + chunkPositions[i]), (IntPtr) input, Chunks[i].Size);
                }
            }

            if (!((IByteEditor) Editor).SetData(newData))
                return false;

            return true;
        }

        public override bool IsModified {
            get => base.IsModified | ChunkEditors.Any(x => x != null && x.IsModified);
            set {
                base.IsModified = value;
                foreach (var ce in ChunkEditors.Where(x => x != null))
                    ce.IsModified = value;
            }
        }

        public override bool OnFinalize()
            => Recompress(true);

        public override void Dispose() {
            base.Dispose();
            if (ChunkEditors != null)
                foreach (var ci in ChunkEditors.Where(ci => ci != null))
                    ci.Dispose();
            if (TextureGroupFrameEditors != null)
                foreach (var ci in TextureGroupFrameEditors.Where(tgfe => tgfe != null))
                    ci.Dispose();
        }

        public IChunkEditor[] ChunkEditors { get; private set; }

        public IChunkEditor SurfaceChunkEditor { get; private set; }

        [BulkCopyRecurse]
        public Tables.MPD.HeaderTable Header { get; private set; }

        [BulkCopyRecurse]
        public ColorTable[] Palettes { get; private set; }

        [BulkCopyRecurse]
        public ChunkHeaderTable ChunkHeader { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Offset1Table { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt32Table Offset2Table { get; private set; }

        [BulkCopyRecurse]
        public UnknownUInt16Table Offset3Table { get; private set; }

        [BulkCopyRecurse]
        public Offset4Table Offset4Table { get; private set; }

        [BulkCopyRecurse]
        public Tables.MPD.TextureGroup.HeaderTable TextureGroupHeader { get; private set; }

        [BulkCopyRecurse]
        public FrameTable TextureGroupFrames { get; private set; }

        public CompressedEditor[] TextureGroupFrameEditors { get; private set; }

        public Chunk[] Chunks { get; private set; }

        public Chunk SurfaceChunk { get; private set; }

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
