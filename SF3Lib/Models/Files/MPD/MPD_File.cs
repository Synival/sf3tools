using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Types;
using System.Linq;
using System.Runtime.InteropServices;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.RawData;
using CommonLib.Arrays;
using SF3.Models.Structs.MPD.TextureChunk;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        private const int c_RamOffset = 0x290000;
        private const int c_SurfaceChunkSize = 0xCF00;

        protected MPD_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static MPD_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new MPD_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        byte[] FetchChunkDataCopy(int chunkIndex)
            => Data.GetDataCopyAt(ChunkHeader.Rows[chunkIndex].ChunkAddress - c_RamOffset, ChunkHeader.Rows[chunkIndex].ChunkSize);

        ByteArraySegment FetchChunkByteArray(int chunkIndex)
            => new ByteArraySegment(Data.Data, ChunkHeader.Rows[chunkIndex].ChunkAddress - c_RamOffset, ChunkHeader.Rows[chunkIndex].ChunkSize);

        IChunkData FetchChunkData(int chunkIndex, bool chunkIsCompressed)
            => new ChunkData(FetchChunkByteArray(chunkIndex), chunkIsCompressed);

        public override IEnumerable<ITable> MakeTables() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Create and load Header
            var headerAddrPtr = Data.GetDouble(0x0000) - c_RamOffset;
            var headerAddr = Data.GetDouble(headerAddrPtr) - c_RamOffset;
            MPDHeader = new MPDHeaderTable(Data, headerAddr, hasPalette3: Scenario >= ScenarioType.Scenario3);
            _ = MPDHeader.Load();
            var header = MPDHeader.Rows[0];

            // Load palettes
            Palettes = new ColorTable[3];
            if (header.OffsetPal1 > 0)
                Palettes[0] = new ColorTable(Data, header.OffsetPal1 - c_RamOffset, 256);
            if (header.OffsetPal2 > 0)
                Palettes[1] = new ColorTable(Data, header.OffsetPal2 - c_RamOffset, 256);
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 > 0)
                Palettes[2] = new ColorTable(Data, MPDHeader.Rows[0].OffsetPal3 - c_RamOffset, 256);

            // Create other tables from header offsets.
            Offset1Table = header.Offset1 != 0 ? new UnknownUInt16Table(Data, header.Offset1 - c_RamOffset, 32) : null;
            Offset2Table = header.Offset2 != 0 ? new UnknownUInt32Table(Data, header.Offset2 - c_RamOffset, 1) : null;
            Offset3Table = header.Offset3 != 0 ? new UnknownUInt16Table(Data, header.Offset3 - c_RamOffset, 32) : null;
            Offset4Table = header.Offset4 != 0 ? new Offset4Table(Data, header.Offset4 - c_RamOffset) : null;

            if (header.OffsetTextureAnimations != 0) {
                TextureAnimations = new TextureAnimationTable(Data, header.OffsetTextureAnimations - c_RamOffset, areAnimatedTextures32Bit);
                _ = TextureAnimations.Load();
            }

            // Create chunk data
            ChunkHeader = new ChunkHeaderTable(Data, 0x2000);
            _ = ChunkHeader.Load();

            var chunks = ChunkHeader.Rows;

            // Assign all chunk data.
            ChunkData = new IChunkData[chunks.Length];

            if (chunks[2].Exists && chunks[2].ChunkSize == c_SurfaceChunkSize)
                _surfaceChunkIndex = 2;
            else if (chunks[20].Exists && chunks[20].ChunkSize == c_SurfaceChunkSize)
                _surfaceChunkIndex = 20;
            else
                _surfaceChunkIndex = -1;

            if (_surfaceChunkIndex != -1)
                ChunkData[_surfaceChunkIndex] = FetchChunkData(_surfaceChunkIndex, false);

            if (chunks[3].Exists && chunks[3].Size > 0 && TextureAnimations != null)
                ChunkData[3] = FetchChunkData(3, false);

            if (chunks[5].Exists && chunks[5].Size > 0)
                ChunkData[5] = FetchChunkData(5, true);

            // Texture data, in chunks (6...10)
            for (var i = 6; i <= 10; i++) {
                try {
                    ChunkData[i] = FetchChunkData(i, true);
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            // We should have all the uncompressed data now. Update read-only info of our chunk table.
            for (var i = 0; i < chunks.Length; i++) {
                ChunkHeader.Rows[i].CompressionType =
                    (ChunkData[i] == null && chunks[i].Exists && chunks[i].Size > 0) ? "(WIP)" :
                    ChunkData[i] == null ? "--" :
                    (i == 3) ? "Individually Compressed" :
                    ChunkData[i].IsCompressed ? "Compressed" :
                    "Uncompressed";
            }

            // Add triggers to update the chunk table when chunks are resized or moved.
            for (var i = 0; i < chunks.Length; i++) {
                if (ChunkData[i] == null)
                    continue;

                var chunkData = ChunkData[i];
                var chunkHeader = ChunkHeader.Rows[i];

                chunkHeader.DecompressedSize = chunkData.DecompressedData.Length;
                chunkData.DecompressedData.Data.RangeModified += (s, a) => {
                    if (a.Resized)
                        chunkHeader.DecompressedSize = chunkData.DecompressedData.Length;
                };

                chunkData.Data.RangeModified += (s, a) => {
                    if (a.Moved)
                        chunkHeader.ChunkAddress = ((ByteArraySegment) chunkData.Data).Offset + c_RamOffset;
                    if (a.Resized)
                        chunkHeader.ChunkSize = chunkData.Data.Length;
                };
            }

            // Add triggers to update texture animation frames when their offsets or content are modified.
            if (Chunk3Frames != null) {
                foreach (var c3frame in Chunk3Frames) {
                    c3frame.Data.Data.RangeModified += (s, a) => {
                        if (a.Moved) {
                            var newOffset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                            var oldOffset = newOffset - a.OffsetChange;
                            var affectedFrames = TextureAnimations.Rows.SelectMany(x => x.Frames).Where(x => x.CompressedTextureOffset == oldOffset).ToArray();
                            foreach (var frame in affectedFrames)
                                frame.CompressedTextureOffset = (uint) newOffset;
                        }
                    };
                    c3frame.Data.DecompressedData.Data.RangeModified += (s, a) => {
                        if (a.Resized || a.Modified) {
                            var offset = ((ByteArraySegment) c3frame.Data.Data).Offset;
                            var affectedFrames = TextureAnimations.Rows.SelectMany(x => x.Frames).Where(x => x.CompressedTextureOffset == offset).ToArray();
                            foreach (var frame in affectedFrames) {
                                var referenceTex = GetTextureModelByID(frame.TextureID)?.Texture;
                                frame.FetchAndCacheTexture(c3frame.Data.DecompressedData, frame.AssumedPixelFormat, referenceTex);
                            }
                        }
                    };
                }
            }

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

            if (SurfaceChunkData?.Length >= 64 * 64 * 2)
                tables.Add(TileSurfaceCharacterRows = new TileSurfaceCharacterRowTable(SurfaceChunkData.DecompressedData, 0x0000));

            TextureChunks = new MPD_FileTextureChunk[5];
            for (var i = 0; i < TextureChunks.Length; i++) {
                var chunkIndex = i + 6;
                if (ChunkData[chunkIndex]?.Length > 0) {
                    TextureChunks[i] = MPD_FileTextureChunk.Create(ChunkData[chunkIndex].DecompressedData, NameGetterContext, 0x00, "TextureChunk" + (i + 1));
                    tables.Add(TextureChunks[i].TextureHeaderTable);
                    tables.Add(TextureChunks[i].TextureTable);
                }
            }

            // Now that textures are loaded, build the texture animation frame data.
            BuildTextureAnimFrameData();

            // Add some callbacks to all child data.
            var allData = ChunkData
                .Where(x => x != null)
                .Cast<IByteData>()
                .Concat(Chunk3Frames.Select(x => x.Data))
                .ToArray();

            foreach (var d in allData) {
                // If the data is marked as unmodified (such as after a save), mark child data as unmodified as well.
                Data.IsModifiedChanged += (s, e) => d.IsModified &= Data.IsModified;

                // If any of the child data is marked as modified, mark the parent data as modified as well.
                d.IsModifiedChanged += (s, e) => Data.IsModified |= d.IsModified;
            }

            return tables;
        }

        private TextureModel GetTextureModelByID(int textureId) {
            if (TextureChunks == null)
                return null;
            return TextureChunks.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x.Rows).FirstOrDefault(x => x.ID == textureId);
        }

        private void BuildTextureAnimFrameData() {
            if (ChunkData[3] == null)
                return;

            if (Chunk3Frames == null)
                Chunk3Frames = new List<Chunk3Frame>();
            else
                Chunk3Frames.Clear();

            var chunk3Textures = new Dictionary<uint, ITexture>();

            foreach (var anim in TextureAnimations.Rows) {
                foreach (var frame in anim.Frames) {
                    var offset = frame.CompressedTextureOffset;
                    var existingFrame = Chunk3Frames.FirstOrDefault(x => x.Offset == offset);
                    var referenceTex = GetTextureModelByID(frame.TextureID)?.Texture;

                    if (existingFrame != null) {
                        frame.FetchAndCacheTexture(existingFrame.Data.DecompressedData, chunk3Textures[offset].PixelFormat, referenceTex);
                        continue;
                    }

                    var uncompressedBytes8 = frame.Width * frame.Height;
                    var uncompressedBytes16 = frame.Width * frame.Height * 2;

                    CompressedData newData = null;
                    ByteArraySegment byteArray = null;

                    try {
                        var compressedBytes = Math.Min(uncompressedBytes16 + 8, ChunkData[3].Length - (int) offset);
                        byteArray = new ByteArraySegment(ChunkData[3].Data, (int) offset, compressedBytes);
                        newData = new CompressedData(byteArray, uncompressedBytes16);
                        frame.FetchAndCacheTexture(newData.DecompressedData, TexturePixelFormat.ABGR1555, referenceTex);
                    }
                    catch {
                        var compressedBytes = Math.Min(uncompressedBytes8 + 8, ChunkData[3].Length - (int) offset);
                        byteArray = new ByteArraySegment(ChunkData[3].Data, (int) offset, compressedBytes);
                        newData = new CompressedData(byteArray, uncompressedBytes8);
                        frame.FetchAndCacheTexture(newData.DecompressedData, TexturePixelFormat.UnknownPalette, referenceTex);
                    }

                    if (newData != null && frame.Texture != null) {
                        byteArray.Redefine(byteArray.Offset, newData.LastDataReadForDecompress.Value); // Sets the correct compressed size, which wasn't available before
                        newData.IsModified = false;
                        chunk3Textures.Add(offset, frame.Texture);
                        Chunk3Frames.Add(new Chunk3Frame((int) offset, newData));
                    }
                }
            }
        }

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public bool Recompress(bool onlyModified) {
            var framesModified = Chunk3Frames?.Any(x => x.Data.IsModified || x.Data.NeedsRecompression) ?? false;
            var chunksModified = framesModified || ChunkData.Any(x => x != null && (x.IsModified || x.NeedsRecompression));

            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !framesModified && !chunksModified)
                return true;

            // Chunk 3 is made up of several individually-compressed images that need to be recompressed.
            RecompressChunk3Frames(onlyModified);

            // Fetch chunk data. We need to do this before the chunk table is optimized, otherwise we can't fetch
            // existing data that hasn't been copied into ChunkData[] because the offset will be wrong.
            var chunks = ChunkHeader.Rows;

            // Recompress and update the chunk table.
            RebuildChunkTable(onlyModified);
            return true;
        }

        private void RecompressChunk3Frames(bool onlyModified) {
            if (Chunk3Frames == null)
                return;

            // Recompress texture frames and determine the new total size of Chunk[3].
            foreach (var frameDataKv in Chunk3Frames) {
                var frameData = frameDataKv.Data;
                if (!onlyModified || frameData.NeedsRecompression || frameData.IsModified)
                    _ = frameData.Finish();
            }
        }

        private void RebuildChunkTable(bool onlyModified) {
            // We'll need to completely rewrite this file. Start by recompressing chunks.
            var currentChunkPos = 0x2100;

            for (var i = 0; i < ChunkData.Length; i++) {
                var chunk = ChunkHeader.Rows[i];
                if (!chunk.Exists)
                    continue;

                var chunkData = ChunkData[i];
                var chunkByteArray = (ByteArraySegment) chunkData?.Data;

                // Enforce alignment by inserting bytes where necessary before this chunk begins.
                if (chunkByteArray != null && chunkByteArray.Offset < currentChunkPos)
                    chunkByteArray.ParentArray.ExpandOrContractAt(chunkByteArray.Offset, currentChunkPos - chunkByteArray.Offset);

                // Finish compressed chunks.
                if (chunkData != null && chunkData.IsCompressed && (!onlyModified || chunkData.NeedsRecompression || chunkData.IsModified))
                    _ = chunkData.Recompress();

                // Advance chunk position, enforcing an alignment of 4.
                currentChunkPos += chunk.ChunkSize;
                if (currentChunkPos % 4 != 0)
                    currentChunkPos += 4 - currentChunkPos % 4;
            }
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
            => Recompress(onlyModified: true);

        public override void Dispose() {
            base.Dispose();
            if (ChunkData != null) {
                foreach (var cd in ChunkData.Where(x => x != null))
                    cd.Dispose();
            }
            if (Chunk3Frames != null) {
                foreach (var cd in Chunk3Frames)
                    cd.Data.Dispose();
                Chunk3Frames.Clear();
            }
        }

        public IChunkData[] ChunkData { get; private set; }

        public IChunkData SurfaceChunkData => (_surfaceChunkIndex >= 0) ? ChunkData[_surfaceChunkIndex] : null;

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

        public List<Chunk3Frame> Chunk3Frames { get; private set; }

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

        private int _surfaceChunkIndex = -1;
    }
}
