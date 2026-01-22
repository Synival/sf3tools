using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Structs.MPD;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        public void RecompressChunks(bool onlyModified) {
            var chunksModified = ChunkData.Any(x => x != null && (x.IsModified || x.NeedsRecompression));

            // Don't bother doing anything if no chunks have been modified.
            if (onlyModified && !chunksModified)
                return;

            // Perform recompression.
            foreach (var chunkData in ChunkData) {
                if (chunkData == null || !chunkData.IsCompressed)
                    continue;
                if (chunkData.IsModified || !onlyModified)
                    chunkData.Recompress();
            }

            return;
        }

        public void RebuildChunkTable() {
            // Chunks always start at file offset 0x2100.
            int nextChunkOffset = 0x2100;

            foreach (var loc in ChunkLocations) {
                if (loc.ChunkRAMAddress == 0)
                    break;

                var chunkData = ChunkData[loc.ID];
                if (chunkData == null) {
                    loc.ChunkFileAddress = nextChunkOffset;
                    loc.ChunkSize = 0;
                }
                else {
                    loc.ChunkFileAddress = nextChunkOffset;
                    loc.ChunkSize = chunkData.Length;
                    nextChunkOffset += (int) (Math.Ceiling(loc.ChunkSize / 4.0) * 4.0);
                }
            }
        }

        public void CommitChunks() {
            // We need to copy chunk data into the file -- get the new file size.
            var maxChunkEnd = ChunkLocations.Max(x => x.ChunkFileAddress + x.ChunkSize);
            var newFileSize = (int) (Math.Ceiling(maxChunkEnd / 4.0) * 4.0);

            // Copy all the chunk data into a clean buffer.
            var newChunkData = new byte[newFileSize - 0x2100];
            foreach (var chunk in ChunkData) {
                if (chunk == null)
                    continue;
                var copyToOffset = ChunkLocations[chunk.Index].ChunkFileAddress - 0x2100;
                if (copyToOffset >= 0)
                    chunk.GetDataCopyOrReference().CopyTo(newChunkData, copyToOffset);
                else
                    CommonLib.Logging.Logger.WriteLine($"Chunk[{chunk.Index}] position (0x{copyToOffset + 0x2100:X4}) is < 0x2100; not writing", CommonLib.Types.LogType.Error);
            }

            // Resize and update our file.
            Data.Data.Resize(newFileSize);
            Data.Data.SetDataAtTo(0x2100, newChunkData.Length, newChunkData);
        }

        public IChunkData MakeChunkData(int chunkIndex, ChunkType type, CompressionType compressionType) {
            if (ChunkData[chunkIndex] != null)
                throw new ArgumentException($"Chunk[{nameof(chunkIndex)}] already has data");

            var isCompressed = (compressionType == CompressionType.Compressed);
            ByteArray byteArray = null;
            ChunkData chunkData = null;

            try {
                byteArray = new ByteArray(Data.Data.GetDataCopyAt(ChunkLocations[chunkIndex].ChunkFileAddress, ChunkLocations[chunkIndex].ChunkSize));
                chunkData = new ChunkData(byteArray, isCompressed, chunkIndex);
            }
            catch {
                // TODO: what to do???
                return null;
            }
            var chunkLocation = ChunkLocations[chunkIndex];

            chunkLocation.DecompressedSize = chunkData.DecompressedData.Length;
            chunkData.DecompressedData.Data.RangeModified += (s, a) => {
                if (a.Resized)
                    chunkLocation.DecompressedSize = chunkData.DecompressedData.Length;
            };

            chunkData.Data.RangeModified += (s, a) => {
                // If the data hasn't been modified, do nothing.
                if (chunkLocation.ChunkSize == chunkData.Length)
                    return;

                // Determine how much the next chunks should be moved by.
                var oldNextChunkOffset = (int) (Math.Ceiling((chunkLocation.ChunkRAMAddress + chunkLocation.ChunkSize) / 4.0) * 4.0);
                var newNextChunkOffset = (int) (Math.Ceiling((chunkLocation.ChunkRAMAddress + chunkData.Length) / 4.0) * 4.0);

                // Set the new chunk size.
                chunkLocation.ChunkSize = chunkData.Length;

                // Don't move proceeding chunks if not requested.
                if (!UpdateChunkTableOnChunkResize)
                    return;

                // Adjust the offset/address of every chunk after this one.
                var nextChunkOffsetDelta = newNextChunkOffset - oldNextChunkOffset;
                if (nextChunkOffsetDelta != 0) {
                    var thisRamAddr = chunkLocation.ChunkRAMAddress;
                    var thisIndex = chunkLocation.ID;
                    foreach (var loc in ChunkLocations) {
                        var ramAddr = loc.ChunkRAMAddress;
                        var index = loc.ID;

                        // Offset the chunk if either:
                        //   1) its offset/address is later, or
                        //   2) it's the same but is a higher ID (this happens when the earlier chunk has a size of 0)
                        if (ramAddr > thisRamAddr || (ramAddr == thisRamAddr && index > thisIndex))
                            loc.ChunkRAMAddress += nextChunkOffsetDelta;
                    }
                }
            };

            chunkLocation.ChunkType = type;
            chunkLocation.CompressionType = compressionType;

            ChunkData[chunkIndex] = chunkData;
            return chunkData;
        }

        private void DetermineChunkIndices() {
            PrimaryTextureChunksFirstIndex = 6;
            PrimaryTextureChunksLastIndex  = PrimaryTextureChunksFirstIndex +
                ((Scenario >= ScenarioType.Prototype) ? 4 : 3);

            MeshTextureChunksFirstIndex = PrimaryTextureChunksLastIndex + 1;

            // Ship2 chunks are in a very order.
            if (Scenario <= ScenarioType.Ship2) {
                MeshTextureChunksLastIndex = MeshTextureChunksFirstIndex;
                PlaneChunksFirstIndex = MeshTextureChunksLastIndex + 1;
                PlaneChunksLastIndex  = PlaneChunksFirstIndex + 5;

                GroundImageChunk1Index = 12;
                GroundImageChunk2Index = 13;

                ForegroundTilesetChunk1Index = 15;
                ForegroundTilesetChunk2Index = 16;

                // TODO: This isn't quite right for Ship2
                ForegroundTileAssignmentChunkIndex = 17;
            }
            else {
                MeshTextureChunksLastIndex = MeshTextureChunksFirstIndex +
                    ((Scenario >= ScenarioType.Scenario1) ? 2 : 1);
                PlaneChunksFirstIndex = MeshTextureChunksLastIndex + 1;
                PlaneChunksLastIndex  = PlaneChunksFirstIndex + 5;

                // Chunk[14] (Scenario 1+ for all)
                GroundImageChunk1Index   = MeshTextureChunksLastIndex + 1;
                GroundTilesetChunk1Index = GroundImageChunk1Index;
                BackgroundChunk1Index    = GroundImageChunk1Index;

                // Chunk[15]
                GroundImageChunk2Index   = GroundImageChunk1Index + 1;
                GroundTilesetChunk2Index = GroundImageChunk2Index;
                BackgroundChunk2Index    = GroundImageChunk2Index;

                // Chunk[16]
                GroundTileAssignmentChunk1Index = GroundImageChunk2Index + 1;

                // Chunk[17]
                SkyChunk1Index = GroundTileAssignmentChunk1Index + 1;
                ForegroundTilesetChunk1Index = SkyChunk1Index;

                // Chunk[18]
                SkyChunk2Index = SkyChunk1Index + 1;
                ForegroundTilesetChunk2Index = SkyChunk2Index;

                // Chunk[19] (also used by the Titan model in Scenario 1 Z_AS.MPD)
                GroundTileAssignmentChunk2Index    = SkyChunk2Index + 1;
                ForegroundTileAssignmentChunkIndex = GroundTileAssignmentChunk2Index;
            }
        }

        private IChunkData[] MakeChunkDatas(ChunkLocation[] chunks) {
            ChunkData = new IChunkData[chunks.Length];

            // Surface model chunk
            SurfaceModelChunkIndex = GetSurfaceModelChunkIndex(chunks);
            if (SurfaceModelChunkIndex != null)
                _ = MakeChunkData(SurfaceModelChunkIndex.Value, ChunkType.SurfaceModel, CompressionType.Uncompressed);

            // All model chunks
            ModelChunkIndices = GetModelChunkIndices(chunks);
            var modelChunksList = new List<IChunkData>();
            foreach (var i in ModelChunkIndices) {
                _ = MakeChunkData(i, ChunkType.Models, CompressionType.Uncompressed);
                modelChunksList.Add(ChunkData[i]);
            }
            ModelChunkDatas = modelChunksList.ToArray();

            // Animated textures chunk
            if (chunks[3].Exists)
                _ = MakeChunkData(3, ChunkType.AnimationFrames, CompressionType.IndividuallyCompressed);

            // Surface chunk (heightmap, terrain, event IDs)
            if (chunks[5].Exists)
                _ = MakeChunkData(5, ChunkType.Surface, CompressionType.Compressed);

            // Texture data, in chunks (6...13)
            for (var i = PrimaryTextureChunksFirstIndex; i <= MeshTextureChunksLastIndex; i++)
                if (chunks[i].Exists)
                    _ = MakeChunkData(i, ChunkType.Textures, CompressionType.Compressed);

            // In Scenario 2+, Chunk[21] is a set of extra textures for things like the Kraken.
            if (chunks[21].Exists)
                _ = MakeChunkData(21, ChunkType.Textures, CompressionType.Compressed);

            var groundImageChunks   = new List<IChunkData>();
            var groundTilesetChunks = new List<IChunkData>();
            var backgroundChunks    = new List<IChunkData>();

            // 512x256 image ground planes
            if (Flags.Bit_0x0400_HasGroundImage) {
                if (chunks[GroundImageChunk1Index].Exists)
                    groundImageChunks.Add(_ = MakeChunkData(GroundImageChunk1Index, ChunkType.GroundPaletteImage, CompressionType.Compressed));
                if (chunks[GroundImageChunk2Index].Exists)
                    groundImageChunks.Add(_ = MakeChunkData(GroundImageChunk2Index, ChunkType.GroundPaletteImage, CompressionType.Compressed));
            }
            // Tiled-based ground plane tilesets
            else if (Flags.Bit_0x1000_HasTileBasedGroundImage) {
                if (chunks[GroundTilesetChunk1Index].Exists)
                    groundTilesetChunks.Add(_ = MakeChunkData(GroundTilesetChunk1Index, ChunkType.TiledGroundTiles, CompressionType.Compressed));
                if (chunks[GroundTilesetChunk2Index].Exists)
                    groundTilesetChunks.Add(_ = MakeChunkData(GroundTilesetChunk2Index, ChunkType.TiledGroundTiles, CompressionType.Compressed));
            }
            // Background image
            else if (Flags.Bit_0x0040_HasBackgroundImage) {
                if (chunks[BackgroundChunk1Index].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(BackgroundChunk1Index, ChunkType.GroundPaletteImage, CompressionType.Compressed));
                if (chunks[BackgroundChunk2Index].Exists)
                    backgroundChunks.Add(_ = MakeChunkData(BackgroundChunk2Index, ChunkType.GroundPaletteImage, CompressionType.Compressed));
            }
            // If no flags are set, and these chunks exist, try to figure out what they could be.
            else {
                bool looksLikeTiledGroundPlane = chunks[GroundTileAssignmentChunk1Index].Exists;
                var unhandledGroundChunks = looksLikeTiledGroundPlane ? groundTilesetChunks : groundImageChunks;
                if (chunks[GroundImageChunk1Index].Exists)
                    unhandledGroundChunks.Add(_ = MakeChunkData(GroundImageChunk1Index, ChunkType.GroundPaletteImage, CompressionType.Compressed));
                if (chunks[GroundImageChunk2Index].Exists)
                    unhandledGroundChunks.Add(_ = MakeChunkData(GroundImageChunk2Index, ChunkType.GroundPaletteImage, CompressionType.Compressed));

            }

            GroundImageChunkDatas   = groundImageChunks.ToArray();
            GroundTilesetChunkDatas = groundTilesetChunks.ToArray();
            BackgroundChunkDatas    = backgroundChunks.ToArray();

            // Check for tile-based ground plane tileset mappings, regardless of whether the bit exists.
            // (Sometimes these chunks exist even though they're unused and would be broken anyway)
            var groundTileAssignmentChunks = new List<IChunkData>();
            if (chunks[GroundTileAssignmentChunk1Index].Exists)
                groundTileAssignmentChunks.Add(_ = MakeChunkData(GroundTileAssignmentChunk1Index, ChunkType.TiledGroundMap, CompressionType.Compressed));
            // Chunks 'GroundTileAssignmentChunk2Index', 'ForegroundTileAssignmentChunkIndex', and the Titan model are shared. Don't steal a chunk!
            if (chunks[GroundTileAssignmentChunk2Index].Exists && !Flags.Bit_0x0010_HasTileBasedForegroundImage && !Flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures)
                groundTileAssignmentChunks.Add(_ = MakeChunkData(GroundTileAssignmentChunk2Index, ChunkType.TiledGroundMap, CompressionType.Compressed));
            GroundTileAssignmentChunkDatas = groundTileAssignmentChunks.ToArray();

            var skyChunks            = new List<IChunkData>();
            var foregroundTileChunks = new List<IChunkData>();
            IChunkData foregroundTileAssignmentChunk = null;

            // 512x256 sky images. Always assume the chunks here are for a sky unless a tiled foreground image is explicitly stated.
            if (Flags.HasAnySky || !Flags.Bit_0x0010_HasTileBasedForegroundImage) {
                if (chunks[SkyChunk1Index].Exists)
                    skyChunks.Add(_ = MakeChunkData(SkyChunk1Index, ChunkType.SkyPaletteImage, CompressionType.Compressed));
                if (chunks[SkyChunk2Index].Exists)
                    skyChunks.Add(_ = MakeChunkData(SkyChunk2Index, ChunkType.SkyPaletteImage, CompressionType.Compressed));
            }
            // Foreground image tiles
            else {
                if (chunks[ForegroundTilesetChunk1Index].Exists)
                    foregroundTileChunks.Add(_ = MakeChunkData(ForegroundTilesetChunk1Index, ChunkType.ForegroundTiles, CompressionType.Compressed));
                if (chunks[ForegroundTilesetChunk2Index].Exists)
                    foregroundTileChunks.Add(_ = MakeChunkData(ForegroundTilesetChunk2Index, ChunkType.ForegroundTiles, CompressionType.Compressed));
            }

            if (Flags.Bit_0x0010_HasTileBasedForegroundImage && chunks[ForegroundTileAssignmentChunkIndex].Exists)
                foregroundTileAssignmentChunk = MakeChunkData(ForegroundTileAssignmentChunkIndex, ChunkType.ForegroundMap, CompressionType.Compressed);

            SkyChunkDatas            = skyChunks.ToArray();
            ForegroundTileChunkDatas = foregroundTileChunks.ToArray();
            ForegroundTileAssignmentChunkData = foregroundTileAssignmentChunk;

            // Add remaining unhandled chunks.
            for (var i = 0; i < chunks.Length; i++)
                if (ChunkData[i] == null && chunks[i].Exists)
                    _ = MakeChunkData(i, ChunkType.Unknown, CompressionType.Uncompressed);

            return ChunkData;
        }

        private int[] GetModelChunkIndices(ChunkLocation[] chunks) {
            var flags = Flags;
            var indices = new List<int>();

            if (chunks[1].Exists && flags.Chunk1Type == ChunkType.Models)
                indices.Add(1);
            if (chunks[20].Exists && flags.Chunk20Type == ChunkType.Models)
                indices.Add(20);
            if (chunks[19].Exists && flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures)
                indices.Add(19);

            return indices.ToArray();
        }

        private int? GetSurfaceModelChunkIndex(ChunkLocation[] chunks) {
            var smci = Flags.SurfaceModelChunkIndex;
            return (smci.HasValue && chunks[smci.Value].Exists) ? smci : null;
        }

        public int PrimaryTextureChunksFirstIndex { get; private set; }
        public int PrimaryTextureChunksLastIndex { get; private set; }

        public int MeshTextureChunksFirstIndex { get;private set;  }
        public int MeshTextureChunksLastIndex { get; private set; }

        public int PlaneChunksFirstIndex { get;private set;  }
        public int PlaneChunksLastIndex { get; private set; }

        public int GroundImageChunk1Index { get; private set; }
        public int GroundImageChunk2Index { get; private set; }
        public int GroundTilesetChunk1Index { get; private set; }
        public int GroundTilesetChunk2Index { get; private set; }
        public int GroundTileAssignmentChunk1Index { get; private set; }
        public int GroundTileAssignmentChunk2Index { get; private set; }
        public int BackgroundChunk1Index { get; private set; }
        public int BackgroundChunk2Index { get; private set; }

        public int SkyChunk1Index { get; private set; }
        public int SkyChunk2Index { get; private set; }
        public int ForegroundTilesetChunk1Index { get; private set; }
        public int ForegroundTilesetChunk2Index { get; private set; }
        public int ForegroundTileAssignmentChunkIndex { get; private set; }

        public IChunkData[] ChunkData { get; private set; }

        public IChunkData[] ModelChunkDatas { get; private set; }

        public IChunkData SurfaceChunkData => (SurfaceModelChunkIndex.HasValue) ? ChunkData[SurfaceModelChunkIndex.Value] : null;

        public IChunkData[] GroundImageChunkDatas { get; private set; }
        public IChunkData[] GroundTilesetChunkDatas { get; private set; }
        public IChunkData[] GroundTileAssignmentChunkDatas { get; private set; }
        public IChunkData[] BackgroundChunkDatas { get; private set; }

        public IChunkData[] SkyChunkDatas { get; private set; }
        public IChunkData[] ForegroundTileChunkDatas { get; private set; }
        public IChunkData ForegroundTileAssignmentChunkData { get; private set; }
    }
}
