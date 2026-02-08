using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Types;
using SF3.Models.Structs.MPD;
using SF3.Models.Structs.MPD.Main;
using SF3.Types;

namespace SF3.Models.Files.MPD {
    public partial class MPD_File {
        public override string[] GetErrors() {
            var errors = base.GetErrors().ToList();

            foreach (var chunk in ChunkData)
                if (chunk?.NeedsRecompression == true)
                    errors.Add($"Chunk[{chunk.Index}] must be recompressed");

            errors.AddRange(GetHeaderPaddingErrors());
            errors.AddRange(GetPaletteErrors());
            errors.AddRange(GetChunkTableErrors());
            errors.AddRange(GetChunkTypeErrors());
            errors.AddRange(GetModelChunkErrors());
            errors.AddRange(GetSurfaceChunkErrors());
            errors.AddRange(GetImageChunkErrors());
            errors.AddRange(GetSurfaceTileErrors());

            return errors.ToArray();
        }

        private string[] GetHeaderPaddingErrors() {
            var header = MPDHeader;
            var errors = new List<string>();

            void CheckPadding(string prop, int value) {
                if (value != 0)
                    errors.Add($"{prop} has non-zero data: {value.ToString("X4")}");
            }

            CheckPadding(nameof(header.Padding1), header.Padding1);
            CheckPadding(nameof(header.Padding2), header.Padding2);
            CheckPadding(nameof(header.Padding3), header.Padding3);
            CheckPadding(nameof(header.Padding4), header.Padding4);

            return errors.ToArray();
        }

        private string[] GetPaletteErrors() {
            var header = MPDHeader;
            var errors = new List<string>();

            // TODO: improve logic -- check for Scenario1+2, and what palettes should be present based on chunks
            // TODO: check palette sizes

            // If the disc is Scenario 3, all palettes should be present.
            if (Scenario >= ScenarioType.Scenario3) {
                if (Planes?.GroundPalette == null)
                    errors.Add("GroundPalette is required for Scenario 3 and above but it is missing");
                if (Planes?.SkyPalette == null)
                    errors.Add("SkyPalette is required for Scenario 3 and above but it is missing");
                if (TexturePalette == null)
                    errors.Add("TexturePalette is required for Scenario 3 and above but it is missing");
            }

            return errors.ToArray();
        }

        private string[] GetChunkTableErrors() {
            var errors = new List<string>();

            ChunkLocation[] GetIntersectingChunks(ChunkLocation loc) {
                if (loc.ChunkRAMAddress == 0)
                    return new ChunkLocation[0];

                var loc1Start = loc.ChunkRAMAddress;
                var loc1Stop = loc.ChunkRAMAddress + loc.ChunkSize;

                var locations = new List<ChunkLocation>();
                foreach (var loc2 in ChunkLocations) {
                    if (loc2 == loc || loc2.ChunkRAMAddress == 0)
                        continue;

                    var loc2Start = loc2.ChunkRAMAddress;
                    var loc2Stop = loc2.ChunkRAMAddress + loc2.ChunkSize;

                    var intersects = (loc1Start < loc2Stop) && (loc1Stop > loc2Start);
                    if (intersects)
                        locations.Add(loc2);
                }

                return locations.ToArray();
            };

            foreach (var chunkLoc in ChunkLocations) {
                var chunk = ChunkData[chunkLoc.ID];

                void AddChunkError(string error)
                    => errors.Add($"Chunk[{chunkLoc.ID}]: {error}");

                if (chunkLoc.ChunkRAMAddress == 0 && chunk != null)
                    AddChunkError($"RAM address in table is 0 but has chunk data (size = 0x{chunk.Length:X4})");
                if (chunkLoc.ChunkRAMAddress == 0 && chunkLoc.ChunkSize != 0)
                    AddChunkError($"RAM address in table is 0 but has size in table (0x{chunkLoc.ChunkSize:X4})");
                if (chunkLoc.ChunkRAMAddress > 0 && chunkLoc.ChunkRAMAddress < c_RamAddress + 0x2100)
                    AddChunkError($"RAM address in table is invalid value (0x{chunkLoc.ChunkRAMAddress:X6})");

                if (chunkLoc.ChunkSize == 0 && chunk != null)
                    AddChunkError($"Size in table is 0 but has chunk data (size = 0x{chunk.Length:X4})");
                if (chunkLoc.ChunkSize != 0 && chunk == null)
                    AddChunkError($"Size in table is non-zero (0x{chunkLoc.ChunkSize:X4}) but has no chunk data");
                if (chunkLoc.ChunkSize != 0 && chunk != null && chunk.Length != chunkLoc.ChunkSize)
                    AddChunkError($"Size in table (0x{chunkLoc.ChunkSize:X4}) does not match chunk data size (0x{chunk.Length:X4})");

                var intersectingChunks = GetIntersectingChunks(chunkLoc);
                foreach (var intersectingChunk in intersectingChunks)
                    AddChunkError($"Intersects with Chunk[{intersectingChunk.ID}]");
            }

            return errors.ToArray();
        }

        private string[] GetChunkTypeErrors() {
            var chunkHeaders = ChunkLocations;
            var errors = new List<string>();

            // Chunk[0] and Chunk[4] should always be empty.
            if (chunkHeaders[0].Exists)
                errors.Add("Chunk[0] exists -- this chunk should always be empty");
            if (chunkHeaders[4].Exists)
                errors.Add("Chunk[4] exists -- this chunk should always be empty");

            // Anything Scenario 2 or higher should have addresses for Chunk[20] and Chunk[21].
            bool shouldHaveChunk20_21 = Scenario >= ScenarioType.Scenario2;
            bool hasChunk20 = chunkHeaders[20].ChunkRAMAddress > 0;
            bool hasChunk21 = chunkHeaders[21].ChunkRAMAddress > 0;

            if (shouldHaveChunk20_21 != hasChunk20)
                errors.Add("Chunk[20] error: ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk20);
            if (shouldHaveChunk20_21 != hasChunk21)
                errors.Add("Chunk[21] errors: ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk21);

            // Make sure the header indicates the correct chunks.
            var flags = (MPD_FlagsFromHeader) Flags;
            if (flags.Chunk1Type != chunkHeaders[1].ChunkType)
                errors.Add($"Chunk[1] type should be '{flags.Chunk1Type}', but is '{chunkHeaders[1].ChunkType}'");
            if (flags.Chunk2Type != chunkHeaders[2].ChunkType)
                errors.Add($"Chunk[2] type should be '{flags.Chunk2Type}', but is '{chunkHeaders[2].ChunkType}'");
            if (flags.Chunk20Type != chunkHeaders[20].ChunkType)
                errors.Add($"Chunk[20] type should be '{flags.Chunk20Type}', but is '{chunkHeaders[20].ChunkType}'");

            if (flags.Bit_0x0100_HasModels && chunkHeaders[flags.ModelsChunkIndex].ChunkType != ChunkType.Models)
                errors.Add($"Model chunk ({flags.ModelsChunkIndex}) type should be 'Models' but is '{chunkHeaders[flags.ModelsChunkIndex].ChunkType}'");
            if (flags.Bit_0x0200_HasSurfaceModel && chunkHeaders[flags.SurfaceModelChunkIndex].ChunkType != ChunkType.SurfaceModel)
                errors.Add($"Surface model chunk ({flags.SurfaceModelChunkIndex}) type should be 'SurfaceModel' but is '{chunkHeaders[flags.SurfaceModelChunkIndex].ChunkType}'");

            return errors.ToArray();
        }

        private string[] GetModelChunkErrors() {
            MemoryLocationType? GetMemoryLocationOfPointers(ModelChunk mc) {
                if (mc == null)
                    return null;
                return (mc.PDatasByMemoryAddress.Values.Count == 0)
                    ? (MemoryLocationType?) null
                    : mc.PDatasByMemoryAddress.Values.First().RamAddress >= 0x0600_0000
                        ? MemoryLocationType.HighMemory
                        : MemoryLocationType.LowMemory;
            }

            var flags = (MPD_FlagsFromHeader) Flags;
            var errors = new List<string>();

            var modelChunks = ModelCollections.Values.Select(x => x as ModelChunk).Where(x => x != null).ToArray();
            var mc1  = modelChunks.FirstOrDefault(x => x.ChunkIndex == 1);
            var mc20 = modelChunks.FirstOrDefault(x => x.ChunkIndex == 20);

            if (mc1 != null) {
                var expected = flags.Chunk1PointersMemoryLocation;
                var actual   = GetMemoryLocationOfPointers(mc1);

                if (expected != actual)
                    errors.Add($"Chunk[1] memory location is '{actual}' but should be '{expected}'");
            }

            if (mc20 != null) {
                var actual = GetMemoryLocationOfPointers(mc20);
                if (actual != MemoryLocationType.HighMemory)
                    errors.Add($"Chunk[20] memory location is '{actual}' but should be '{nameof(MemoryLocationType.HighMemory)}'");
            }

            return errors.ToArray();
        }

        private string[] GetSurfaceChunkErrors() {
            var flags = (MPD_FlagsFromHeader) Flags;
            var chunkHeaders = ChunkLocations;
            var errors = new List<string>();

            var expectedIndex = (flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel && chunkHeaders[20].Exists) ? 20 : 2;
            var chunk2LooksLikeSurfaceChunk  = chunkHeaders[2].Exists  && chunkHeaders[2].ChunkSize  == 0xCF00;
            var chunk20LooksLikeSurfaceChunk = chunkHeaders[20].Exists && chunkHeaders[20].ChunkSize == 0xCF00;

            if (flags.Bit_0x0200_HasSurfaceModel) {
                if (SurfaceModelChunk == null) {
                    errors.Add("HasSurfaceModel flag is set but no surface model was created");
                    if (chunk2LooksLikeSurfaceChunk)
                        errors.Add($"  (Chunk[2] (expected={expectedIndex}) looks like one -- this could be an SF3Lib error)");
                    if (chunk20LooksLikeSurfaceChunk)
                        errors.Add($"  (Chunk[20] (expected={expectedIndex}) looks like one -- this could be an SF3Lib error)");
                }
                else if (flags.SurfaceModelChunkIndex != expectedIndex)
                    errors.Add($"(Maybe not an error?) SurfaceModel in unexpected index. Expected in Chunk[{expectedIndex}] but found at Chunk[{flags.SurfaceModelChunkIndex}]");
            }
            else {
                if (flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                    errors.Add("HasSurfaceModel flag is unset but Chunk20IsSurfaceModelIfExists flag is set");
                if (chunk2LooksLikeSurfaceChunk)
                    errors.Add($"HasSurfaceModel flag is unset but Chunk[2] (expected={expectedIndex}) looks like one");
                if (chunk20LooksLikeSurfaceChunk)
                    errors.Add($"HasSurfaceModel flag is unset but Chunk[20] (expected={expectedIndex}) looks like one");
            }

            if (chunk2LooksLikeSurfaceChunk && chunk20LooksLikeSurfaceChunk)
                errors.Add("Only one surface chunk should be present but both Chunk[2] and Chunk[20] look like one");

            return errors.ToArray();
        }

        private string[] GetImageChunkErrors() {
            var flags = Flags;
            var chunkHeaders = ChunkLocations;
            var errors = new List<string>();

            var chunkUses = new Dictionary<int, List<string>>() {
                { 14, new List<string>() },
                { 15, new List<string>() },
                { 16, new List<string>() },
                { 17, new List<string>() },
                { 18, new List<string>() },
                { 19, new List<string>() },
            };

            var typicalUse = new Dictionary<int, string>() {
                { 14, "GroundImageTop[Tiles]" },
                { 15, "GroundImageBottom[Tiles]" },
                { 16, "GroundImageTopTileMap" },
                { 17, "SkyImageTop" },
                { 18, "SkyImageBottom" },
                { 19, "GroundImageBottomTileMap" },
            };

            if (flags.Bit_0x0400_HasGroundImage) {
                chunkUses[14].Add("GroundImageTop");
                chunkUses[15].Add("GroundImageBottom");
            }

            if (flags.Bit_0x1000_HasTileBasedGroundImage) {
                chunkUses[14].Add("GroundImageTopTiles");
                chunkUses[15].Add("GroundImageBottomTiles");
                chunkUses[16].Add("GroundImageTopTileMap");
                chunkUses[19].Add("GroundImageBottomTileMap");
            }

            if (flags.HasAnySky) {
                chunkUses[17].Add("SkyImageTop");
                chunkUses[18].Add("SkyImageBottom");
            }

            if (flags.Bit_0x0040_HasBackgroundImage) {
                chunkUses[14].Add("BackgroundImageTop");
                chunkUses[15].Add("BackgroundImageBottom");
            }

            if (flags.Bit_0x0010_HasTileBasedForegroundImage) {
                chunkUses[17].Add("ForegroundImageTopTiles");
                chunkUses[18].Add("ForegroundImageBottomTiles");
                chunkUses[19].Add("ForegroundImageTileMap");
            }

            if (flags.Bit_0x0080_HasChunk19ModelWithChunk10Textures)
                chunkUses[19].Add("ExtraModel");

            foreach (var cu in chunkUses) {
                if (cu.Value.Count == 0) {
                    if (chunkHeaders[cu.Key].Exists)
                        errors.Add($"Image Chunk[{cu.Key}] exists but has no flag to indicate its use (probably {typicalUse[cu.Key]})");
                }
                else {
                    var usesStr = string.Join(", ", cu.Value);
                    if (cu.Value.Count > 1)
                        errors.Add($"Image Chunk[{cu.Key}] has multiple uses indicated: {usesStr}");

                    if (!chunkHeaders[cu.Key].Exists) {
                        // The sky is allowed to be missing from Scenario 2 onward.
                        if (!(usesStr.StartsWith("SkyImage") && Scenario >= ScenarioType.Scenario2))
                            errors.Add($"{usesStr} Chunk[{cu.Key}] is missing");
                    }
                }
            }

            return errors.ToArray();
        }

        private string[] GetSurfaceTileErrors() {
            if (SurfaceModelChunk == null)
                return new string[0];

            var errors = new List<string>();
            var flags = Flags;
            var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));

            foreach (var tile in Surface.GetAllTiles()) {
                if (tile is SurfaceTile fileTile) {
                    // Report irregularities in the heightmap.
                    var dataHeights = corners.ToDictionary(c => c, fileTile.GetSurfaceDataVertexHeight);
                    if (tile.IsFlat) {
                        var br = CornerType.BottomRight;
                        foreach (var c in corners) {
                            if (c == br)
                                continue;
                            if (dataHeights[c] != dataHeights[br])
                                errors.Add($"Flat tile ({tile.X}, {tile.Y}) corner '{c}' height ({dataHeights[c]}) doesn't match bottom-right corner height ({dataHeights[br]})");
                        }
                    }
                    else {
                        var modelHeights = corners.ToDictionary(c => c, x => fileTile.GetSurfaceModelVertexHeight(x) * 16.0f);
                        foreach (var c in corners) {
                            if (dataHeights[c] != modelHeights[c])
                                errors.Add($"Non-flat tile ({tile.X}, {tile.Y}) corner '{c}' height ({dataHeights[c]}) doesn't match surface model height ({modelHeights[c]})");
                        }
                    }
                }

                // Report unknown or unhandled tile flags. Only Scenario 3+ has rotation flags 0x01 and 0x02.
                var weirdTexFlags = tile.TextureFlags & ~0xB0;
                if (Scenario >= ScenarioType.Scenario3)
                    weirdTexFlags &= ~0x03;
                if (weirdTexFlags != 0x00)
                    errors.Add("Tile (" + tile.X + ", " + tile.Y + ") has unhandled texture flag 0x" + weirdTexFlags.ToString("X2"));
                if (Scenario >= ScenarioType.Scenario3 && !flags.Bit_0x0002_HasSurfaceTextureRotation && (tile.TextureFlags & 0x03) != 0)
                    errors.Add("HasSurfaceTextureRotation flag is off but tile (" + tile.X + ", " + tile.Y + ") has rotation flags 0x" + (weirdTexFlags & 0x03).ToString("X2"));
            }

            return errors.ToArray();
        }
    }
}
