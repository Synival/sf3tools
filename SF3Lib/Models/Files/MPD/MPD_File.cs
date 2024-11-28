using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Types;
using System.Linq;
using CommonLib;
using System.Runtime.InteropServices;
using SF3.Models.Files;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.TextureAnimation;
using SF3.RawData;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        protected MPD_File(IRawData editor, INameGetterContext nameContext, ScenarioType scenario) : base(editor, nameContext, scenario) {
        }

        public static MPD_File Create(IRawData editor, INameGetterContext nameContext, ScenarioType scenario) {
            var newEditor = new MPD_File(editor, nameContext, scenario);
            if (!newEditor.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newEditor;
        }

        public override IEnumerable<ITable> MakeTables() {
            const int ramOffset = 0x290000;

            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Create and load Header
            var headerAddrPtr = Data.GetDouble(0x0000) - ramOffset;
            var headerAddr = Data.GetDouble(headerAddrPtr) - ramOffset;
            MPDHeader = new MPDHeaderTable(Data, headerAddr, hasPalette3: Scenario >= ScenarioType.Scenario3);
            _ = MPDHeader.Load();
            var header = MPDHeader.Rows[0];

            // Load palettes
            Palettes = new ColorTable[3];
            if (header.OffsetPal1 > 0)
                Palettes[0] = new ColorTable(Data, header.OffsetPal1 - ramOffset, 256);
            if (header.OffsetPal2 > 0)
                Palettes[1] = new ColorTable(Data, header.OffsetPal2 - ramOffset, 256);
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 > 0)
                Palettes[2] = new ColorTable(Data, MPDHeader.Rows[0].OffsetPal3 - ramOffset, 256);

            // Create other tables from header offsets.
            Offset1Table = header.Offset1 != 0 ? new UnknownUInt16Table(Data, header.Offset1 - ramOffset, 32) : null;
            Offset2Table = header.Offset2 != 0 ? new UnknownUInt32Table(Data, header.Offset2 - ramOffset, 1) : null;
            Offset3Table = header.Offset3 != 0 ? new UnknownUInt16Table(Data, header.Offset3 - ramOffset, 32) : null;
            Offset4Table = header.Offset4 != 0 ? new Offset4Table(Data, header.Offset4 - ramOffset) : null;

            if (header.OffsetTextureAnimations != 0) {
                TextureAnimations = new TextureAnimationTable(Data, header.OffsetTextureAnimations - ramOffset, areAnimatedTextures32Bit);
                TextureAnimations.Load();
                TextureAnimFrames = new FrameTable(Data, TextureAnimations.Address, areAnimatedTextures32Bit, TextureAnimations.Rows);
                TextureAnimFrames.Load();
            }

            // Create chunk data
            ChunkHeader = new ChunkHeaderTable(Data, 0x2000);
            _ = ChunkHeader.Load();

            Chunks = new Chunk[ChunkHeader.Rows.Length];
            for (var i = 0; i < Chunks.Length; i++) {
                var chunkInfo = ChunkHeader.Rows[i];
                if (chunkInfo.ChunkAddress > 0)
                    Chunks[i] = new Chunk(((ByteData) Data).Data, chunkInfo.ChunkAddress - ramOffset, chunkInfo.ChunkSize);
            }

            // Assign all chunk editors.
            ChunkEditors = new IChunkData[Chunks.Length];

            if (Chunks[2]?.Data?.Length > 0) {
                SurfaceChunk = Chunks[2];
                SurfaceChunkEditor = ChunkEditors[2] = new ChunkData(SurfaceChunk.Data, false);
            }
            // TODO: this works, but it's kind of a dumb hack!!
            else if (Chunks[20]?.Data?.Length == 52992) {
                SurfaceChunk = Chunks[20];
                SurfaceChunkEditor = ChunkEditors[20] = new ChunkData(SurfaceChunk.Data, false);
            }

            if (Chunks[3]?.Data?.Length > 0 && TextureAnimFrames != null) {
                TextureAnimFrameEditors = new CompressedData[TextureAnimFrames.Rows.Length];
                var frameOffsetEndId = areAnimatedTextures32Bit ? 0xFFFF_FFFEu : 0xFFFFu;
                for (var i = 0; i < TextureAnimFrames.Rows.Length; i++) {
                    var frame = TextureAnimFrames.Rows[i];
                    if (frame.FrameNum > 0 && frame.CompressedTextureOffset != frameOffsetEndId) {
                        var totalBytes = frame.Width * frame.Height * 2;
                        // TODO: this is super inefficient!!!
                        var bytes = Chunks[3].Data.Skip((int) frame.CompressedTextureOffset).Take(totalBytes).ToArray();
                        TextureAnimFrameEditors[i] = new CompressedData(bytes, totalBytes);
                    }
                }
            }

            if (Chunks[5]?.Data != null)
                ChunkEditors[5] = new ChunkData(Chunks[5].Data, true);

            // Texture editors, in chunks (6...10)
            for (var i = 6; i <= 10; i++) {
                try {
                    ChunkEditors[i] = new ChunkData(Chunks[i].Data, true);
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            // We should have all the uncompressed data now. Update read-only info of our chunk table.
            for (var i = 0; i < ChunkHeader.Rows.Length; i++) {
                ChunkHeader.Rows[i].CompressionType =
                    ChunkEditors[i] == null && (Chunks[i]?.Data?.Length ?? 0) == 0 ? "--" :
                    ChunkEditors[i] == null ? "(WIP)" :
                    ChunkEditors[i].IsCompressed ? "Compressed" :
                    "Uncompressed";
            }
            UpdateChunkTableDecompressedSizes();

            // Build a list of all data tables.
            var tables = new List<ITable>() {
                MPDHeader,
                ChunkHeader,
                (TileSurfaceHeightmapRows = new TileSurfaceHeightmapRowTable(ChunkEditors[5].DecompressedData, 0x0000)),
                (TileHeightTerrainRows    = new TileHeightTerrainRowTable   (ChunkEditors[5].DecompressedData, 0x4000)),
                (TileItemRows             = new TileItemRowTable            (ChunkEditors[5].DecompressedData, 0x6000)),
            };

            if (TextureAnimations != null)
                tables.Add(TextureAnimations);

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
            if (TextureAnimations != null)
                tables.Add(TextureAnimations);
            if (TextureAnimFrames != null)
                tables.Add(TextureAnimFrames);

            if (SurfaceChunkEditor?.Data?.Length >= 64 * 64 * 2)
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(SurfaceChunkEditor.DecompressedData, 0x0000));

            TextureChunks = new MPD_FileTextureChunk[5];
            for (var i = 0; i < TextureChunks.Length; i++) {
                var chunkIndex = i + 6;
                if (Chunks[chunkIndex].Data?.Length > 0) {
                    TextureChunks[i] = MPD_FileTextureChunk.Create(ChunkEditors[chunkIndex].DecompressedData, NameGetterContext, 0x00, "TextureChunk" + (i + 1));
                    tables.Add(TextureChunks[i].TextureHeaderTable);
                    tables.Add(TextureChunks[i].TextureTable);
                }
            }

            // Add some callbacks to all child editors.
            var editors = ChunkEditors
                .Cast<IRawData>()
                .Concat(TextureAnimFrameEditors?.Cast<IRawData>() ?? new IRawData[0])
                .Where(x => x != null)
                .ToArray();

            foreach (var ce in editors) {
                // If the editor is marked as unmodified (such as after a save), mark child editors as unmodified as well.
                Data.IsModifiedChanged += (s, e) => ce.IsModified &= Data.IsModified;

                // If any of the child editors are marked as modified, mark the parent editor as modified as well.
                ce.IsModifiedChanged += (s, e) => Data.IsModified |= ce.IsModified;
            }

            return tables;
        }

        private void UpdateChunkTableDecompressedSizes() {
            for (var i = 0; i < ChunkHeader.Rows.Length; i++)
                ChunkHeader.Rows[i].DecompressedSize = ChunkEditors[i]?.DecompressedData?.Size ?? 0;
        }

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public bool Recompress(bool onlyModified) {
            // TODO: update group texture frame textures as well!!

            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !ChunkEditors.Any(x => x != null && (x.IsModified || x.NeedsRecompression)))
                return true;

            const int ramOffset = 0x290000;

            // We'll need to completely rewrite this file. Start by recompressing chunks.
            var currentChunkPos = 0x2100;
            var chunkPositions = new int[Chunks.Length];

            for (var i = 0; i < Chunks.Length; i++) {
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
                        currentChunkPos += 4 - currentChunkPos % 4;
                }
            }

            // Start rebuilding new data.
            var newData = new byte[currentChunkPos];
            var inputData = Data.GetAllData();

            unsafe {
                fixed (byte* output = newData) {
                    // Copy first 0x2100 bytes into our new data.
                    fixed (byte* input = inputData)
                        memcpy((IntPtr) output, (IntPtr) input, 0x2100);

                    // Copy all chunk data into our new data.
                    for (var i = 0; i < Chunks.Length; i++)
                        if (Chunks[i] != null)
                            fixed (byte* input = Chunks[i].Data)
                                _ = memcpy((IntPtr) (output + chunkPositions[i]), (IntPtr) input, Chunks[i].Size);
                }
            }

            if (!((IByteData) Data).SetData(newData))
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
            if (TextureAnimFrameEditors != null)
                foreach (var ci in TextureAnimFrameEditors.Where(tgfe => tgfe != null))
                    ci.Dispose();
        }

        public IChunkData[] ChunkEditors { get; private set; }

        public IChunkData SurfaceChunkEditor { get; private set; }

        [BulkCopyRecurse]
        public MPDHeaderTable MPDHeader { get; private set; }

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
        public TextureAnimationTable TextureAnimations { get; private set; }

        [BulkCopyRecurse]
        public FrameTable TextureAnimFrames { get; private set; }

        public CompressedData[] TextureAnimFrameEditors { get; private set; }

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
        public MPD_FileTextureChunk[] TextureChunks { get; private set; }
    }
}
