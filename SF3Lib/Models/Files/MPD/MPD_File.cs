using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Types;
using System.Linq;
using CommonLib;
using System.Runtime.InteropServices;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.Models.Tables.MPD.TextureAnimation;
using SF3.RawData;
using System.IO;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        protected MPD_File(IRawData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static MPD_File Create(IRawData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new MPD_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
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

            // Assign all chunk data.
            ChunkData = new IChunkData[Chunks.Length];

            if (Chunks[2]?.Data?.Length > 0) {
                SurfaceChunk = Chunks[2];
                SurfaceChunkData = ChunkData[2] = new ChunkData(SurfaceChunk.Data, false);
            }
            // TODO: this works, but it's kind of a dumb hack!!
            else if (Chunks[20]?.Data?.Length == 52992) {
                SurfaceChunk = Chunks[20];
                SurfaceChunkData = ChunkData[20] = new ChunkData(SurfaceChunk.Data, false);
            }

            if (Chunks[3]?.Data?.Length > 0 && TextureAnimFrames != null) {
                ChunkData[3] = new ChunkData(Chunks[3].Data, false);
                BuildTextureAnimFrameData();
            }

            if (Chunks[5]?.Data != null)
                ChunkData[5] = new ChunkData(Chunks[5].Data, true);

            // Texture data, in chunks (6...10)
            for (var i = 6; i <= 10; i++) {
                try {
                    ChunkData[i] = new ChunkData(Chunks[i].Data, true);
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            // We should have all the uncompressed data now. Update read-only info of our chunk table.
            for (var i = 0; i < ChunkHeader.Rows.Length; i++) {
                ChunkHeader.Rows[i].CompressionType =
                    ChunkData[i] == null && (Chunks[i]?.Data?.Length ?? 0) == 0 ? "--" :
                    ChunkData[i] == null ? "(WIP)" :
                    (i == 3) ? "Individually Compressed" :
                    ChunkData[i].IsCompressed ? "Compressed" :
                    "Uncompressed";
            }
            UpdateChunkTableDecompressedSizes();

            // Build a list of all data tables.
            var tables = new List<ITable>() {
                MPDHeader,
                ChunkHeader,
                (TileSurfaceHeightmapRows = new TileSurfaceHeightmapRowTable(ChunkData[5].DecompressedData, 0x0000)),
                (TileHeightTerrainRows    = new TileHeightTerrainRowTable   (ChunkData[5].DecompressedData, 0x4000)),
                (TileItemRows             = new TileItemRowTable            (ChunkData[5].DecompressedData, 0x6000)),
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

            if (SurfaceChunkData?.Data?.Length >= 64 * 64 * 2)
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(SurfaceChunkData.DecompressedData, 0x0000));

            TextureChunks = new MPD_FileTextureChunk[5];
            for (var i = 0; i < TextureChunks.Length; i++) {
                var chunkIndex = i + 6;
                if (Chunks[chunkIndex].Data?.Length > 0) {
                    TextureChunks[i] = MPD_FileTextureChunk.Create(ChunkData[chunkIndex].DecompressedData, NameGetterContext, 0x00, "TextureChunk" + (i + 1));
                    tables.Add(TextureChunks[i].TextureHeaderTable);
                    tables.Add(TextureChunks[i].TextureTable);
                }
            }

            // Add some callbacks to all child data.
            var allData = ChunkData
                .Cast<IRawData>()
                .Concat(TextureAnimFrameData?.Cast<IRawData>() ?? new IRawData[0])
                .Where(x => x != null)
                .ToArray();

            foreach (var d in allData) {
                // If the data is marked as unmodified (such as after a save), mark child data as unmodified as well.
                Data.IsModifiedChanged += (s, e) => d.IsModified &= Data.IsModified;

                // If any of the child data is marked as modified, mark the parent data as modified as well.
                d.IsModifiedChanged += (s, e) => Data.IsModified |= d.IsModified;
            }

            return tables;
        }

        private void BuildTextureAnimFrameData() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            TextureAnimFrameData = new CompressedData[TextureAnimFrames.Rows.Length];
            var frameOffsetEndId = areAnimatedTextures32Bit ? 0xFFFF_FFFEu : 0xFFFFu;
            for (var i = 0; i < TextureAnimFrames.Rows.Length; i++) {
                var frame = TextureAnimFrames.Rows[i];
                if (frame.FrameNum > 0 && frame.CompressedTextureOffset != frameOffsetEndId) {
                    var totalBytes = frame.Width * frame.Height * 2;

                    var bytes = new byte[totalBytes];
                    unsafe {
                        fixed (byte* dest = bytes, src = ChunkData[3].Data)
                            _ = memcpy((IntPtr) dest, (IntPtr) (src + frame.CompressedTextureOffset), totalBytes);
                    }

                    TextureAnimFrameData[i] = new CompressedData(bytes, totalBytes);
                    _ = TextureAnimFrameData[i].Recompress(); // Sets the correct compressed size, which wasn't available before
                    _ = frame.FetchAndAssignImageData(TextureAnimFrameData[i].DecompressedData);
                }
            }
        }

        private void UpdateChunkTableDecompressedSizes() {
            for (var i = 0; i < ChunkHeader.Rows.Length; i++)
                ChunkHeader.Rows[i].DecompressedSize = ChunkData[i]?.DecompressedData?.Size ?? 0;
        }

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public bool Recompress(bool onlyModified) {
            var framesModified = TextureAnimFrameData?.Any(x => x != null && (x.IsModified || x.NeedsRecompression)) ?? false;
            var chunksModified = framesModified || ChunkData.Any(x => x != null && (x.IsModified || x.NeedsRecompression));

            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !framesModified && !chunksModified)
                return true;

            RebuildChunk3(onlyModified);
            RebuildChunkTable(onlyModified, out var chunkPositions, out var chunkTableEnd);

            // Start rebuilding new data.
            var newData = new byte[chunkTableEnd];
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

        private void RebuildChunk3(bool onlyModified) {
            if (TextureAnimFrameData == null)
                return;

            var framesGroupedByOffset = TextureAnimFrames.Rows
                .Select((x, i) => new { Index = i, Texture = x })
                .Where(x => x != null && x.Texture.ImageIsLoaded)
                .GroupBy(x => x.Texture.CompressedTextureOffset)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.ToList());

            var chunk3 = ChunkData[3].DecompressedData;
            var newChunk3Textures = new List<byte[]>();

            int newLength = 0;
            foreach (var frameGroup in framesGroupedByOffset) {
                var firstFrame = frameGroup.Value.First();
                var frameData = TextureAnimFrameData[firstFrame.Index];

                if (!onlyModified || frameData.NeedsRecompression || frameData.IsModified)
                    _ = frameData.Recompress();

                newLength += frameData.Data.Length;
            }

            var newChunk3Data = new byte[newLength];
            int off = 0;
            foreach (var frameGroup in framesGroupedByOffset) {
                foreach (var frame in frameGroup.Value)
                    TextureAnimFrames.Rows[frame.Index].CompressedTextureOffset = (uint) off;

                var firstFrame = frameGroup.Value.First();
                var frameData = TextureAnimFrameData[firstFrame.Index];

                unsafe {
                    fixed (byte* dest = newChunk3Data, src = frameData.Data)
                        _ = memcpy((IntPtr) (dest + off), (IntPtr) src, frameData.Data.Length);
                    off += frameData.Data.Length;
                }
            }

            Chunks[3] = new Chunk(new MemoryStream(newChunk3Data), newChunk3Data.Length);
            ChunkData[3] = new ChunkData(Chunks[3].Data, false);
            BuildTextureAnimFrameData();
        }

        private void RebuildChunkTable(bool onlyModified, out int[] chunkPositions, out int chunkTableEnd) {
            // We'll need to completely rewrite this file. Start by recompressing chunks.
            var currentChunkPos = 0x2100;
            chunkPositions = new int[Chunks.Length];

            const int ramOffset = 0x290000;

            for (var i = 0; i < Chunks.Length; i++) {
                var chunkData = ChunkData[i];

                // Finish compressed chunks.
                if (chunkData != null && (!onlyModified || chunkData.NeedsRecompression || chunkData.IsModified)) {
                    if (chunkData.IsCompressed && !chunkData.Recompress())
                        continue;

                    // TODO: thie invalidates any references!!! maybe just update the thing???
                    Chunks[i] = new Chunk(chunkData.Data, 0, chunkData.Data.Length);
                    ChunkData[i] = new ChunkData(Chunks[i].Data, ChunkData[i].IsCompressed);
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

            chunkTableEnd = currentChunkPos;
        }

        public override bool IsModified {
            get => base.IsModified | ChunkData.Any(x => x != null && x.IsModified);
            set {
                base.IsModified = value;
                foreach (var ce in ChunkData.Where(x => x != null))
                    ce.IsModified = value;
            }
        }

        public override bool OnFinish()
            => Recompress(true);

        public override void Dispose() {
            base.Dispose();
            if (ChunkData != null)
                foreach (var ci in ChunkData.Where(ci => ci != null))
                    ci.Dispose();
            if (TextureAnimFrameData != null)
                foreach (var ci in TextureAnimFrameData.Where(tgfe => tgfe != null))
                    ci.Dispose();
        }

        public IChunkData[] ChunkData { get; private set; }

        public IChunkData SurfaceChunkData { get; private set; }

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

        public CompressedData[] TextureAnimFrameData { get; private set; }

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
