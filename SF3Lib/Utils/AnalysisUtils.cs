using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using SF3.Analysis;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.Utils {
    public static class AnalysisUtils {
        public static string[] GetByteComparisonErrors(MPD_File expectedMpd, byte[] actualMpdData, ByteComparisonSkipRegion[] skipRegions = null, float acceptablePercentage = 100.0f) {
            skipRegions = GetKnownAcceptableInconsistenciesForMPD_File(expectedMpd)
                .Concat(skipRegions ?? new ByteComparisonSkipRegion[0])
                .OrderBy(x => x.Offset)
                .GroupBy(x => x.Offset)
                .Select(x => x.First())
                .ToArray();

            return GetByteComparisonErrors(expectedMpd.Data.GetDataCopyOrReference(), actualMpdData, skipRegions, acceptablePercentage);
        }

        /// <summary>
        /// There are several specific things that the MPD_Writer can't get right, 99% of which are extremely minor
        /// inconsistencies in LZSS compression. This will fetch them so they don't have to be added manually every
        /// time.
        /// </summary>
        public static ByteComparisonSkipRegion[] GetKnownAcceptableInconsistenciesForMPD_File(IMPD_File file) {
            var chunk13Errors = new ByteComparisonSkipRegion[0];
            var chunk13Info = file.ChunkLocations[13];
            if (chunk13Info.Exists && chunk13Info.ChunkSize == 0x494) {
                var fileAddr = chunk13Info.ChunkFileAddress;
                chunk13Errors = new ByteComparisonSkipRegion[] {
                    // Header: Insignificant texture Chunk[13] size difference (2 bytes) due to LZSS compression differences
                    new ByteComparisonSkipRegion { Offset = 0x206F, Size = 1 },

                    // Insignificant texture Chunk[13] difference due to LZSS compression differences
                    new ByteComparisonSkipRegion { Offset = fileAddr + 0x484, Size = 1 },
                    new ByteComparisonSkipRegion { Offset = fileAddr + 0x48e, Size = 4 },
                };
            }

            return chunk13Errors;
        }

        public static string[] GetByteComparisonErrors(byte[] expected, byte[] actual, int reportingOffset, string reportInfo, out float percentageCorrect, ByteComparisonSkipRegion[] skipRegions = null) {
            var errors = new List<string>();

            var expectedSizeDiff = (skipRegions != null) ? -skipRegions.Sum(x => x.ActualDataExtraBytes) : 0;
            var expectedLength = expected.Length + expectedSizeDiff;

            if (expectedLength != actual.Length)
                errors.Add($"Length is wrong: should be {expectedLength} (0x{expectedLength:X5}), is {actual.Length} (0x{actual.Length:X5})");
            (uint ExpectedOffset, uint ActualOffset)? firstWrongByte = null;
            int wrongBytes = 0;
            int bytesToCompare = Math.Min(expected.Length, actual.Length);

            // Sort the skip regions.
            skipRegions = (skipRegions ?? new ByteComparisonSkipRegion[0]).OrderBy(x => x.Offset).ToArray();
            int skipRegionIndex = 0;
            var skipRegion = skipRegions.Length > skipRegionIndex ? skipRegions[skipRegionIndex] : (ByteComparisonSkipRegion?) null;

            for (int i = 0, j = 0; i < expected.Length && j < actual.Length; i++, j++) {
                if (i + reportingOffset == skipRegion?.Offset) {
                    i += skipRegion.Value.Size - 1;
                    j += skipRegion.Value.Size - 1;
                    i -= skipRegion.Value.ActualDataExtraBytes;
                    skipRegionIndex++;
                    skipRegion = skipRegions.Length > skipRegionIndex ? skipRegions[skipRegionIndex] : (ByteComparisonSkipRegion?) null;
                    continue;
                }

                if (expected[i] != actual[j]) {
                    if (!firstWrongByte.HasValue)
                        firstWrongByte = ((uint) i, (uint) j);
                    wrongBytes++;
                }
            }

            percentageCorrect = (float) Math.Floor((float) (bytesToCompare - wrongBytes) / bytesToCompare * 10000.0f) / 100.0f;
            if (wrongBytes > 0) {
                errors.Add($"Comparable data is wrong: {percentageCorrect:0.00}% accurate ({wrongBytes} wrong bytes)");

                var rightByte = expected[firstWrongByte.Value.ExpectedOffset];
                var wrongByte = actual[firstWrongByte.Value.ActualOffset];

                var fwbOffset = firstWrongByte.Value.ExpectedOffset + reportingOffset;

                errors.Add($"First wrong byte is at {fwbOffset} (0x{fwbOffset:X4}):");
                errors.Add($"  Should be {rightByte} (0x{rightByte:X2}), is {wrongByte} (0x{wrongByte:X2})");
            }

            if (errors.Count > 0 && reportInfo != null && reportInfo != "") {
                var newErrors = new List<string> { reportInfo + ":" };
                newErrors.AddRange(errors.Select(x => "  " + x));
                errors = newErrors;
            }

            return errors.ToArray();
        }

        public static string[] GetByteComparisonErrors(byte[] expected, byte[] actual, ByteComparisonSkipRegion[] skipRegions = null, float acceptablePercentage = 100.0f)
            => GetByteComparisonErrors(expected, actual, 0, null, skipRegions, acceptablePercentage);

        public static string[] GetByteComparisonErrors(byte[] expected, byte[] actual, int reportingOffset, string reportInfo, ByteComparisonSkipRegion[] skipRegions = null, float acceptablePercentage = 100.0f) {
            var errors = GetByteComparisonErrors(expected, actual, reportingOffset, reportInfo, out var percentageCorrect, skipRegions) ?? new string[0];
            return percentageCorrect < acceptablePercentage ? errors : new string[0];
        }

        private class TextureDataWithIndex {
            public TextureDataWithIndex(byte[] data, int index) {
                Data = data;
                Index = index;
            }

            public readonly byte[] Data;
            public readonly int Index;
        }

        public static string[] GetMPDContentComparisonErrors(MPD_File expectedFile, MPD_File actualFile, Dictionary<int, ByteComparisonSkipRegion[]> skipRegionsByChunk = null) {
            var errors = new List<string>();
            void Assert(bool mustBeTrue, string failMessage) {
                if (!mustBeTrue)
                    errors.Add(failMessage);
            }

            skipRegionsByChunk = skipRegionsByChunk ?? new Dictionary<int, ByteComparisonSkipRegion[]>();

            var expectedPrimaryChunk = expectedFile.ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var mcExpOut) ? mcExpOut as ModelChunk : null;
            var actualPrimaryChunk   = actualFile  .ModelCollections.TryGetValue(MPD_CollectionType.Primary, out var mcActOut) ? mcActOut as ModelChunk : null;

            var eitt = expectedFile.IndexedTextureTable;
            var aitt = actualFile.IndexedTextureTable;
            var eanims = expectedFile.Animations;
            var aanims = actualFile.Animations;

            var animTablesOccupySameSpace = eanims != null && aanims != null && eanims.Address == aanims.Address && eanims.SizeInBytes == aanims.SizeInBytes;
            var indexedTexturesTablesOccupySameSpace = eitt != null && aitt != null && eitt.Address == aitt.Address && eitt.SizeInBytes == aitt.SizeInBytes;

            ByteComparisonSkipRegion[] GetSkipRegions(int chunk) {
                var regionList = skipRegionsByChunk.TryGetValue(chunk, out var bcsr) ? bcsr.ToList() : new List<ByteComparisonSkipRegion>();

                // Chunk -1 is the "main" region from 0x0000 - 0x2000.
                if (chunk == -1) {
                    // Skip byte-for-byte comparisons of texture-related tables that aren't going to be in the same, somewhat arbitrary order.
                    if (animTablesOccupySameSpace)
                        regionList.Add(new ByteComparisonSkipRegion() { Offset = eanims.Address, Size = eanims.SizeInBytes });
                    if (indexedTexturesTablesOccupySameSpace)
                        regionList.Add(new ByteComparisonSkipRegion() { Offset = eitt.Address, Size = eitt.SizeInBytes });
                }

                // If checking the primary chunk, skip the collision tables.
                // They're not quite accurate (and probably can't ever be), and confirmed elsewhere that they're good.
                if (chunk == expectedPrimaryChunk?.ChunkIndex && chunk == actualPrimaryChunk?.ChunkIndex) {
                    var collisionBlocksActual = actualPrimaryChunk?.ModelsHeader.CollisionBlocksOffset;
                    if (collisionBlocksActual > 0) {
                        var expectedSize = expectedFile.ChunkLocations[chunk].ChunkSize;

                        var actualAddr   = actualFile.ChunkLocations[chunk].ChunkFileAddress;
                        var actualSize   = actualFile.ChunkLocations[chunk].ChunkSize;

                        var actualFileRamAddition = expectedFile.Flags.ModelsMemoryLocation == MemoryLocationType.HighMemory
                            ? (0x60A0000 - actualAddr)
                            : 0x0290000;
                        var collisionBlocksStart = collisionBlocksActual.Value - actualFileRamAddition;
                        var collisionBlocksSize = actualAddr + actualSize - collisionBlocksStart;

                        var newRegion = new ByteComparisonSkipRegion() {
                            Offset = (int) collisionBlocksStart,
                            Size   = (int) collisionBlocksSize,
                            ActualDataExtraBytes = actualSize - expectedSize
                        };

                        regionList.Add(newRegion);
                    }
                }

                return regionList.ToArray();
            }

            // Scenario should be the same.
            Assert(expectedFile.Scenario == actualFile.Scenario, $"Scenario is different: expected={expectedFile.Scenario}, actual={actualFile.Scenario}");

            // Main/header content from 0x0000 - 0x2000 should be identical.
            errors.AddRange(AnalysisUtils.GetByteComparisonErrors(
                expectedFile.Data.GetDataCopyAt(0, 0x2000),
                actualFile.Data.GetDataCopyAt(0, 0x2000),
                0, "Main/Header Region",
                GetSkipRegions(-1) // Chunk -1 is a big dumb hack for the main/header area.
            ));

            // Compare animated textures table.
            if (indexedTexturesTablesOccupySameSpace) {
                var expectedAnimatedTextures = eanims.Select(x => x.TextureID).OrderBy(x => x).ToArray();
                var actualAnimatedTextures   = aanims.Select(x => x.TextureID).OrderBy(x => x).ToArray();
                // TODO: Actually compare the things!!
            }

            // Compare indexed textures table.
            if (indexedTexturesTablesOccupySameSpace) {
                var expectedOrderedTextures = eitt.Select(x => x.TextureID).OrderBy(x => x).ToArray();
                var actualOrderedTextures   = aitt.Select(x => x.TextureID).OrderBy(x => x).ToArray();
                Assert(Enumerable.SequenceEqual(expectedOrderedTextures, actualOrderedTextures), "Indexed texture tables are different");
            }

            // Go chunk by chunk.
            var expectedChunkCount = expectedFile.ChunkData.Length;
            var actualChunkCount   = actualFile.ChunkData.Length;
            Assert(expectedChunkCount == actualChunkCount, $"ChunkData.Length's are different: should be {expectedChunkCount}, is {actualChunkCount}");

            for (int i = 0; i < expectedChunkCount; i++) {
                var expectedChunkInfo = expectedFile.ChunkLocations[i];
                var actualChunkInfo   = actualFile.ChunkLocations[i];

                Assert(expectedChunkInfo.Exists == actualChunkInfo.Exists, $"Inconsistent Chunk[{i}].Exists: expected={expectedChunkInfo.Exists}, actual={actualChunkInfo.Exists}");
                if (!expectedChunkInfo.Exists || actualChunkInfo.Exists == false)
                    continue;

                Assert(expectedChunkInfo.ChunkType == actualChunkInfo.ChunkType, $"Inconsistent Chunk[{i}].ChunkType: expected={expectedChunkInfo.ChunkType}, actual={actualChunkInfo.ChunkType}");

                var expectedChunkData = expectedFile.ChunkData[i];
                var actualChunkData   = actualFile.ChunkData[i];
                Assert(expectedChunkData != null, $"Internal logic error: {nameof(expectedChunkData)} should not be null!");
                Assert(actualChunkData   != null, $"Internal logic error: {nameof(actualChunkData)} should not be null!");
                if (expectedChunkData == null || actualChunkData == null)
                    continue;

                var expectedChunkByteData = expectedChunkData.DecompressedData.Data.GetDataCopyOrReference();
                var actualChunkByteData   = actualChunkData.DecompressedData.Data.GetDataCopyOrReference();

                // Chunk[3] is special; it has *individually compressed* images. Compare image-by-image.
                if (i == 3) {
                    // Textures aren't necessarily in the same order; just make sure they're all present.
                    var expectedTextures = new List<TextureDataWithIndex>();
                    var actualTextures   = new List<TextureDataWithIndex>();

                    // Get expected textures.
                    int pos = 0;
                    for (int index = 0; pos < expectedChunkByteData.Length - 2; index++) {
                        var textureData = CommonLib.Utils.Compression.DecompressLZSS(expectedChunkByteData, pos, null, out var bytesRead, out var endDataFound);
                        Assert(endDataFound, $"Existing MPD error: Chunk[3] expected texture 0x{index:X2} error: {nameof(endDataFound)} == false");
                        expectedTextures.Add(new TextureDataWithIndex(textureData, index));

                        pos += bytesRead;
                        if ((pos % 4) != 0)
                            pos += (4 - (pos % 4));
                    }

                    // Get actual textures.
                    pos = 0;
                    for (int index = 0; pos < actualChunkByteData.Length - 2; index++) {
                        var textureData = CommonLib.Utils.Compression.DecompressLZSS(actualChunkByteData, pos, null, out var bytesRead, out var endDataFound);
                        Assert(endDataFound, $"Chunk[3] actual texture 0x{index:X2} error: {nameof(endDataFound)} == false");
                        actualTextures.Add(new TextureDataWithIndex(textureData, index));

                        pos += bytesRead;
                        if ((pos % 4) != 0)
                            pos += (4 - (pos % 4));
                    }

                    // Find textures that are contained in 'actualTextures' but not 'expectedTextures' and vice-versa.
                    var extraTextures   = new List<TextureDataWithIndex>();
                    var missingTextures = new List<TextureDataWithIndex>(expectedTextures);
                    var uniqueActualTextures = actualTextures.GroupBy(x => x.Data.CreateTextureHash()).Select(x => x.First()).ToArray();

                    foreach (var actualTexture in uniqueActualTextures) {
                        var matchingTexturesFound = missingTextures.Where(x => Enumerable.SequenceEqual(actualTexture.Data, x.Data)).ToArray();
                        if (matchingTexturesFound.Length > 0) {
                            foreach (var tex in matchingTexturesFound)
                                missingTextures.Remove(tex);
                        }
                        else
                            extraTextures.Add(actualTexture);
                    }

                    foreach (var tex in extraTextures)
                        errors.Add($"Chunk[3] actual texture 0x{tex.Index:X2} is extra (not found in expected MPD)");
                    foreach (var tex in missingTextures)
                        errors.Add($"Chunk[3] expected texture 0x{tex.Index:X2} missing (not found in actual MPD)");
                }
                // For other chunks, just compare the decompressed data.
                else {
                    var skipRegions = GetSkipRegions(i);
                    var expectedSize = expectedChunkInfo.DecompressedSize + ((skipRegions != null) ? skipRegions.Sum(x => x.ActualDataExtraBytes) : 0);
                    var actualSize   = actualChunkInfo.DecompressedSize;

                    Assert(expectedSize == actualSize,
                        $"Inconsistent Chunk[{i}].DecompressedSize: expected={expectedSize} ({expectedSize:X4}), actual={actualSize} ({actualSize:X4})");

                    var reportOffset = actualChunkData.IsCompressed ? 0 : actualChunkInfo.ChunkFileAddress;
                    var reportInfo = actualChunkData.IsCompressed
                        ? $"Chunk[{i}] (compressed data)"
                        : $"Chunk[{i}] (uncompressed data -- actual offset is 0x{reportOffset:X4}";

                    errors.AddRange(AnalysisUtils.GetByteComparisonErrors(
                        expectedChunkByteData,
                        actualChunkByteData,
                        reportOffset, reportInfo,
                        skipRegions
                    ));
                }
            }

            return errors.ToArray();
        }
    }
}
