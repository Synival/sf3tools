using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Types;
using System.Linq;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;
using SF3.RawData;
using CommonLib.Arrays;
using SF3.Models.Structs.MPD.TextureChunk;
using CommonLib.SGL;
using CommonLib.Types;
using static CommonLib.Utils.BlockHelpers;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Files.MPD {
    public class MPD_File : ScenarioTableFile, IMPD_File {
        private const int c_RamOffset = 0x290000;
        private const int c_SurfaceChunkSize = 0xCF00;

        protected MPD_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) { }

        public static MPD_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new MPD_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var areAnimatedTextures32Bit = Scenario >= ScenarioType.Scenario3;

            // Load root headers
            var header = MakeHeaderTable().Rows[0];
            var headerTables = MakeHeaderTables(header, areAnimatedTextures32Bit);

            // Load chunks
            var chunks = MakeChunkHeaderTable().Rows;
            var chunkDatas = MakeChunkDatas(chunks);
            var chunkTables = MakeChunkTables(chunks, chunkDatas, SurfaceChunkData);

            // Add two-way communication between 'Modified' events from the root IByteData and its children.
            WireChildDataModifiedEvents();

            // Build a list of all data tables.
            var tables = new List<ITable>() {
                MPDHeader,
                ChunkHeader,
            };
            tables.AddRange(headerTables);
            tables.AddRange(chunkTables);

            return tables;
        }

        private MPDHeaderTable MakeHeaderTable() {
            var headerAddrPtr = Data.GetDouble(0x0000) - c_RamOffset;
            var headerAddr = Data.GetDouble(headerAddrPtr) - c_RamOffset;
            MPDHeader = MPDHeaderTable.Create(Data, headerAddr, hasPalette3: Scenario >= ScenarioType.Scenario3);
            return MPDHeader;
        }

        private ITable[] MakeHeaderTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            tables.AddRange(MakeLightingTables(header));
            tables.AddRange(MakeTexturePaletteTables(header));
            tables.AddRange(MakeTextureAnimationTables(header, areAnimatedTextures32Bit));
            tables.AddRange(MakeUnknownTables(header));

            return tables.ToArray();
        }

        private ChunkHeaderTable MakeChunkHeaderTable() {
            // Create chunk data
            ChunkHeader = ChunkHeaderTable.Create(Data, 0x2000);
            foreach (var chunkHeader in ChunkHeader.Rows)
                chunkHeader.CompressionType = "--";

            return ChunkHeader;
        }

        private ITable[] MakeTexturePaletteTables(MPDHeaderModel header) {
            TexturePalettes = new ColorTable[3];
            var headerRamAddr = header.Address + c_RamOffset;

            if (header.OffsetPal1 >= c_RamOffset && header.OffsetPal1 < headerRamAddr)
                TexturePalettes[0] = ColorTable.Create(Data, header.OffsetPal1 - c_RamOffset, Math.Min(256, (headerRamAddr - header.OffsetPal1) / 2));
            if (header.OffsetPal2 >= c_RamOffset && header.OffsetPal2 < headerRamAddr)
                TexturePalettes[1] = ColorTable.Create(Data, header.OffsetPal2 - c_RamOffset, Math.Min(256, (headerRamAddr - header.OffsetPal2) / 2));
            if (Scenario >= ScenarioType.Scenario3 && header.OffsetPal3 >= c_RamOffset && header.OffsetPal3 < headerRamAddr)
                TexturePalettes[2] = ColorTable.Create(Data, header.OffsetPal3 - c_RamOffset, Math.Min(256, (headerRamAddr - header.OffsetPal3) / 2));

            return TexturePalettes.Where(x => x != null).ToArray();
        }

        private ITable[] MakeLightingTables(MPDHeaderModel header) {
            var tables = new List<ITable>();

            if (header.OffsetLightPal != 0)
                tables.Add(LightPalette = ColorTable.Create(Data, header.OffsetLightPal - c_RamOffset, 32));
            if (header.OffsetLightDir != 0)
                tables.Add(LightDirectionTable = LightDirectionTable.Create(Data, header.OffsetLightDir - c_RamOffset));

            return tables.ToArray();
        }

        private ITable[] MakeTextureAnimationTables(MPDHeaderModel header, bool areAnimatedTextures32Bit) {
            var tables = new List<ITable>();

            if (header.OffsetTextureAnimations != 0)
                tables.Add(TextureAnimations = TextureAnimationTable.Create(Data, header.OffsetTextureAnimations - c_RamOffset, areAnimatedTextures32Bit));

            return tables.ToArray();
        }

        private ITable[] MakeUnknownTables(MPDHeaderModel header) {
            var tables = new List<ITable>();

            if (header.Offset3 != 0)
                tables.Add(Offset3Table = UnknownUInt16Table.Create(Data, header.Offset3 - c_RamOffset, 32));
            if (header.Offset4 != 0)
                tables.Add(Offset4Table = Offset4Table.Create(Data, header.Offset4 - c_RamOffset));

            return tables.ToArray();
        }

        private IChunkData[] MakeChunkDatas(ChunkHeader[] chunks) {
            ChunkData = new IChunkData[chunks.Length];

            _surfaceChunkIndex = GetSurfaceChunkIndex(chunks);
            if (_surfaceChunkIndex != null)
                ChunkData[_surfaceChunkIndex.Value] = MakeChunkData(_surfaceChunkIndex.Value, false);

            if (chunks[3].Exists)
                ChunkData[3] = MakeChunkData(3, false, "Individually Compressed");
            if (chunks[5].Exists)
                ChunkData[5] = MakeChunkData(5, true);

            // Texture data, in chunks (6...10)
            for (var i = 6; i <= 10; i++) {
                try {
                    ChunkData[i] = MakeChunkData(i, true);
                }
                catch {
                    // TODO: This is likely failing because the texture is the wrong encoding.
                    //       Finding a table that determines whether this is 16- or 8-bit would be great.
                }
            }

            // Add remaining unhandled chunks.
            for (var i = 0; i < chunks.Length; i++)
                if (ChunkData[i] == null && chunks[i].Exists)
                    ChunkData[i] = MakeChunkData(i, false, "(WIP)");

            return ChunkData;
        }

        private IChunkData MakeChunkData(int chunkIndex, bool chunkIsCompressed, string compressionType = null) {
            var newChunk = new ChunkData(new ByteArraySegment(Data.Data, ChunkHeader.Rows[chunkIndex].ChunkAddress - c_RamOffset, ChunkHeader.Rows[chunkIndex].ChunkSize), chunkIsCompressed);
            var chunkHeader = ChunkHeader.Rows[chunkIndex];

            chunkHeader.DecompressedSize = newChunk.DecompressedData.Length;
            newChunk.DecompressedData.Data.RangeModified += (s, a) => {
                if (a.Resized)
                    chunkHeader.DecompressedSize = newChunk.DecompressedData.Length;
            };

            newChunk.Data.RangeModified += (s, a) => {
                if (a.Moved) {
                    var byteArraySegment = (ByteArraySegment) newChunk.Data;

                    // Figure out how much the offset has changed.
                    var oldOffset = ChunkHeader.Rows[chunkIndex].ChunkAddress - c_RamOffset;
                    var newOffset = byteArraySegment.Offset;
                    var offsetDelta = newOffset - oldOffset;
                    if (offsetDelta == 0)
                        return;

                    // Update the address in the chunk table.
                    ChunkHeader.Rows[chunkIndex].ChunkAddress = newOffset + c_RamOffset;

                    // Chunks after this one with something assigned to ChunkData[] will have their
                    // ChunkAddress updated automatically. For chunks without a ChunkData[] after this one
                    // (but before the next ChunkData), update addresses manually.
                    for (var j = chunkIndex + 1; j < ChunkHeader.Rows.Length; j++) {
                        if (ChunkData[j] != null)
                            break;

                        var ch = ChunkHeader.Rows[j];
                        if (ch != null && ch.ChunkAddress != 0)
                            ch.ChunkAddress += offsetDelta;
                    }
                }
                if (a.Resized)
                    chunkHeader.ChunkSize = newChunk.Data.Length;
            };

            chunkHeader.CompressionType = compressionType ?? (chunkIsCompressed ? "Compressed" : "Uncompressed");
            return newChunk;
        }

        private int? GetSurfaceChunkIndex(ChunkHeader[] chunks) {
            if (chunks[2].Exists && chunks[2].ChunkSize == c_SurfaceChunkSize)
                return 2;
            else if (chunks[20].Exists && chunks[20].ChunkSize == c_SurfaceChunkSize)
                return 20;
            else
                return null;
        }

        private ITable[] MakeChunkTables(ChunkHeader[] chunkHeaders, IChunkData[] chunkDatas, IChunkData surfaceModelChunk) {
            var tables = new List<ITable>();

            if (chunkDatas[5] != null)
                tables.AddRange(MakeTileChunkTables(chunkDatas[5].DecompressedData));
            if (surfaceModelChunk != null)
                tables.AddRange(MakeSurfaceModelChunkTables(surfaceModelChunk.DecompressedData));

            // TODO: refactor MPD_FileTextureChunk
            TextureChunks = new MPD_FileTextureChunk[5];
            for (var i = 0; i < TextureChunks.Length; i++) {
                var chunkIndex = i + 6;
                if (chunkDatas[chunkIndex]?.Length > 0) {
                    TextureChunks[i] = MPD_FileTextureChunk.Create(chunkDatas[chunkIndex].DecompressedData, NameGetterContext, 0x00, "TextureChunk" + (i + 1));
                    tables.AddRange(TextureChunks[i].Tables);
                }
            }

            // Now that textures are loaded, build the texture animation frame data.
            // TODO: This function is a MESS. Please refactor it!!
            BuildTextureAnimFrameData();

            return tables.ToArray();
        }

        private ITable[] MakeTileChunkTables(IByteData data) {
            return new ITable[] {
                (TileSurfaceHeightmapRows = TileSurfaceHeightmapRowTable.Create(data, 0x0000)),
                (TileHeightTerrainRows    = TileHeightTerrainRowTable.Create   (data, 0x4000)),
                (TileItemRows             = TileItemRowTable.Create            (data, 0x6000)),
            };
        }

        private ITable[] MakeSurfaceModelChunkTables(IByteData data) {
            return new ITable[] {
                (TileSurfaceCharacterRows          = TileSurfaceCharacterRowTable.Create     (data, 0x0000)),
                (TileSurfaceVertexNormalMeshBlocks = TileSurfaceVertexNormalMeshBlocks.Create(data, 0x2000)),
                (TileSurfaceVertexHeightMeshBlocks = TileSurfaceVertexHeightMeshBlocks.Create(data, 0xB600)),
            };
        }

        private TextureModel GetTextureModelByID(int textureId) {
            if (TextureChunks == null)
                return null;
            return TextureChunks.Where(x => x != null).Select(x => x.TextureTable).SelectMany(x => x.Rows).FirstOrDefault(x => x.ID == textureId);
        }

        // TODO: refactor this mess!!
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

            // Add triggers to update texture animation frames when their offsets or content are modified.
            foreach (var c3frameLoop in Chunk3Frames) {
                var c3frame = c3frameLoop;
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

        private void WireChildDataModifiedEvents() {
            // Add some callbacks to all child data.
            var allData = ChunkData
                .Where(x => x != null)
                .Cast<IByteData>()
                .Concat((Chunk3Frames != null) ? Chunk3Frames.Select(x => x.Data) : new CompressedData[0])
                .ToArray();

            foreach (var d in allData) {
                // If the data is marked as unmodified (such as after a save), mark child data as unmodified as well.
                Data.IsModifiedChanged += (s, e) => d.IsModified &= Data.IsModified;

                // If any of the child data is marked as modified, mark the parent data as modified as well.
                d.IsModifiedChanged += (s, e) => Data.IsModified |= d.IsModified;
            }
        }

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
            int expectedRamAddr = 0x292100;
            int lastChunkEndRamAddr = expectedRamAddr;

            for (var i = 0; i < ChunkData.Length; i++) {
                var chunk = ChunkHeader.Rows[i];
                if (chunk.ChunkAddress == 0)
                    continue;

                var chunkData = ChunkData[i];
                var chunkByteArray = (ByteArraySegment) chunkData?.Data;

                // Enforce alignment by inserting bytes where necessary before this chunk begins.
                if (chunk.ChunkAddress != expectedRamAddr) {
                    Data.Data.ResizeAt(
                        lastChunkEndRamAddr - c_RamOffset,
                        Math.Min(chunk.ChunkAddress - lastChunkEndRamAddr, Data.Data.Length),
                        expectedRamAddr - lastChunkEndRamAddr
                    );
                }

                // Finish compressed chunks.
                if (chunkData != null && chunkData.IsCompressed && (!onlyModified || chunkData.NeedsRecompression || chunkData.IsModified))
                    _ = chunkData.Recompress();

                // Move forward. Make sure the next chunk starts at an alignment of 4 bytes.
                expectedRamAddr += chunk.ChunkSize;
                if (expectedRamAddr % 4 != 0)
                    expectedRamAddr += 4 - (expectedRamAddr % 4);

                // Keep track of where the last chunk ended. We'll add or remove zeroes here to enforce alignemnt.
                lastChunkEndRamAddr = chunk.ChunkAddress + chunk.ChunkSize;
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

        public VECTOR CalculateSurfaceVertexAbnormal(int tileX, int tileY, CornerType corner, bool useMoreAccurateCalculations)
            => CalculateSurfaceVertexAbnormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), useMoreAccurateCalculations);

        public VECTOR CalculateSurfaceVertexAbnormal(int vertexX, int vertexY, bool useMoreAccurateCalculations) {
            var heightmap = TileSurfaceHeightmapRows?.Rows;
            if (heightmap == null) {
                return new VECTOR(
                    new FIXED(0, true),
                    new FIXED(1, true),
                    new FIXED(0, true)
                );
            }

            // Determine the normals of the 4 quads surrounding the vertex.
            var sumNormals = new List<VECTOR>();

            void TryAddQuadNormal(int vx, int vy) {
                if (vx >= 0 && vy >= 0 && vx <= 63 && vy <= 63) {
                    var heights = heightmap[63 - vy].GetHeights(vx);
                    var quad = new POLYGON(new VECTOR[] {
                        new VECTOR(0.00f, heights[0], 0.00f),
                        new VECTOR(1.00f, heights[1], 0.00f),
                        new VECTOR(1.00f, heights[2], 1.00f),
                        new VECTOR(0.00f, heights[3], 1.00f)
                    });
                    sumNormals.Add(quad.GetNormal(useMoreAccurateCalculations));
                }
            }

            // Gather a list of all quad normals to use for averaging the vertex normal.
            // On the edges of maps, there are fewer adjected polys to the vertex,
            // so only add normals if they exist.
            TryAddQuadNormal(vertexX - 1, vertexY - 1);
            TryAddQuadNormal(vertexX - 0, vertexY - 1);
            TryAddQuadNormal(vertexX - 0, vertexY - 0);
            TryAddQuadNormal(vertexX - 1, vertexY - 0);

            // Return the average of each normal (normalized) for Gouraud shading.
            return VECTOR.GetAbnormalFromNormals(sumNormals.ToArray());
        }

        public void UpdateSurfaceVertexAbnormal(int tileX, int tileY, CornerType corner, bool useMoreAccurateCalculations)
            => UpdateSurfaceVertexAbnormal(TileToVertexX(tileX, corner), TileToVertexY(tileY, corner), useMoreAccurateCalculations);

        public void UpdateSurfaceVertexAbnormal(int vertexX, int vertexY, bool useMoreAccurateCalculations) {
            if (TileSurfaceHeightmapRows == null || TileSurfaceVertexNormalMeshBlocks == null)
                return;

            var abnormal = CalculateSurfaceVertexAbnormal(vertexX, vertexY, useMoreAccurateCalculations);
            var locations = GetBlockLocations(vertexX, vertexY);
            UpdateSurfaceVertexAbnormals(locations, abnormal);
        }

        public void UpdateSurfaceVertexAbnormals(BlockVertexLocation[] locations, VECTOR abnormal) {
            if (TileSurfaceVertexNormalMeshBlocks == null)
                return;

            var blocks = TileSurfaceVertexNormalMeshBlocks.Rows;
            for (var i = 0; i < locations.Length; i++)
                blocks[locations[i].Num][locations[i].X, locations[i].Y] = abnormal;
        }

        public void UpdateSurfaceVertexAbnormals(int tileX, int tileY, bool useMoreAccurateCalculations) {
            if (TileSurfaceHeightmapRows == null || TileSurfaceVertexNormalMeshBlocks == null)
                return;

            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.TopLeft,     useMoreAccurateCalculations);
            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.TopRight,    useMoreAccurateCalculations);
            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.BottomRight, useMoreAccurateCalculations);
            UpdateSurfaceVertexAbnormal(tileX, tileY, CornerType.BottomLeft,  useMoreAccurateCalculations);
        }

        public void UpdateSurfaceVertexAbnormals(bool useMoreAccurateCalculations) {
            if (TileSurfaceHeightmapRows == null || TileSurfaceVertexNormalMeshBlocks == null)
                return;
            for (var y = 0; y < 65; y++)
                for (var x = 0; x < 65; x++)
                    UpdateSurfaceVertexAbnormal(x, y, CornerType.TopLeft, useMoreAccurateCalculations);
        }

        public IChunkData[] ChunkData { get; private set; }

        public IChunkData SurfaceChunkData => (_surfaceChunkIndex.HasValue) ? ChunkData[_surfaceChunkIndex.Value] : null;

        [BulkCopyRecurse]
        public MPDHeaderTable MPDHeader { get; private set; }

        [BulkCopyRecurse]
        public ColorTable[] TexturePalettes { get; private set; }

        [BulkCopyRecurse]
        public ChunkHeaderTable ChunkHeader { get; private set; }

        [BulkCopyRecurse]
        public ColorTable LightPalette { get; private set; }

        [BulkCopyRecurse]
        public LightDirectionTable LightDirectionTable { get; private set; }

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
        public TileSurfaceVertexNormalMeshBlocks TileSurfaceVertexNormalMeshBlocks { get; private set; }

        [BulkCopyRecurse]
        public TileSurfaceVertexHeightMeshBlocks TileSurfaceVertexHeightMeshBlocks { get; private set; }

        [BulkCopyRecurse]
        public TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; private set; }

        [BulkCopyRecurse]
        public TileHeightTerrainRowTable TileHeightTerrainRows { get; private set; }

        [BulkCopyRecurse]
        public TileItemRowTable TileItemRows { get; private set; }

        [BulkCopyRecurse]
        public MPD_FileTextureChunk[] TextureChunks { get; private set; }

        private int? _surfaceChunkIndex = null;
    }
}
