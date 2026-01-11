using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.NamedValues;
using SF3.Types;
using static SF3.Tests.Utils.DataUtils;

namespace SF3.Tests.Utils {
    public static class MPD_TestUtils {
        public static MPD_File MakeMPD_File(ScenarioType scenario, string filename) {
            var filePath = TestDataPaths.ResourcePath(scenario, Path.GetFileName(filename))!;
            var fileData = File.ReadAllBytes(filePath);
            return MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(fileData)), new NameGetterContext(scenario), scenario);
        }

        public static void AssertMPDByteComparison(MPD_File file, byte[] outputData, ByteComparisonSkipRegion[]? skipRegions = null, float acceptablePercentage = 100.0f) {
            skipRegions = GetKnownAcceptableInconsistenciesForMPD_File(file)
                .Concat(skipRegions ?? [])
                .OrderBy(x => x.Offset)
                .GroupBy(x => x.Offset)
                .Select(x => x.First())
                .ToArray();

            AssertByteComparison(file.Data.GetDataCopyOrReference(), outputData, skipRegions, acceptablePercentage);
        }

        /// <summary>
        /// There are several specific things that the MPD_Writer can't get right, 99% of which are extremely minor
        /// inconsistencies in LZSS compression. This will fetch them so they don't have to be added manually every
        /// time.
        /// </summary>
        public static ByteComparisonSkipRegion[] GetKnownAcceptableInconsistenciesForMPD_File(IMPD_File file) {
            var muhSize = file.ChunkLocations[13].ChunkSize;

            ByteComparisonSkipRegion[] chunk13Errors = [];
            var chunk13Info = file.ChunkLocations[13];
            if (chunk13Info.Exists && chunk13Info.ChunkSize == 0x494) {
                var fileAddr = chunk13Info.ChunkFileAddress;
                chunk13Errors = [
                    // Header: Insignificant texture Chunk[13] size difference (2 bytes) due to LZSS compression differences
                    new ByteComparisonSkipRegion { Offset = 0x206F, Size = 1 },

                    // Insignificant texture Chunk[13] difference due to LZSS compression differences
                    new ByteComparisonSkipRegion { Offset = fileAddr + 0x484, Size = 1 },
                    new ByteComparisonSkipRegion { Offset = fileAddr + 0x48e, Size = 4 },
                ];
            }

            return chunk13Errors;
        }

        public static IMPD_File RecreateMPD_File(IMPD_File mpd) {
            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(mpd);
                outputData = memoryStream.ToArray();
            }

            return MPD_File.Create(
                new SF3.ByteData.ByteData(new ByteArray(outputData)),
                mpd.NameGetterContext,
                mpd.Scenario
            );
        }

        public static void ForEachMPD_File(ScenarioType scenario, Action<IMPD_File> action) {
            var path = TestDataPaths.ScenarioDataPaths[scenario];
            var mpdFiles = Directory
                .GetFiles(path, "*.MPD")
                .Select(x => Path.GetFileName(x))
                .Where(x => x != "SHIP2.MPD")
                .ToArray();
            var testCases = mpdFiles
                .Select(x => new SF3FileTestCase(scenario, x))
                .ToArray();

            SF3FileTestCase.Run(testCases, testCase => {
                var mpd = MakeMPD_File(scenario, testCase.Filename);
                action(mpd);
            });
        }

        public static void AssertMPD_FilesHaveSameContent(MPD_File expectedFile, MPD_File actualFile, Dictionary<int, ByteComparisonSkipRegion[]> skipRegionsByChunk = null) {
            var exceptionsCaught = new List<Exception>();
            void CollectException(Action action) {
                try {
                    action();
                }
                catch (Exception ex) {
                    exceptionsCaught.Add(ex);
                }
            }

            ByteComparisonSkipRegion[]? GetSkipRegions(int chunk) {
                if (skipRegionsByChunk == null)
                    return null;
                return skipRegionsByChunk.TryGetValue(chunk, out var bcsr) ? bcsr : null;
            }

            // Main/header content from 0x0000 - 0x2000 should be identical.
            CollectException(() => AssertByteComparison(
                expectedFile.Data.GetDataCopyAt(0, 0x2000),
                actualFile.Data.GetDataCopyAt(0, 0x2000),
                0, "Main/Header Region",
                GetSkipRegions(-1) // Chunk -1 is a big dumb hack for the main/header area.
            ));

            // Scenario should be the same.
            CollectException(() => Assert.IsTrue(expectedFile.Scenario == actualFile.Scenario, $"Scenario is different: expected={expectedFile.Scenario}, actual={actualFile.Scenario}"));

            // Go chunk by chunk.
            var expectedChunkCount = expectedFile.ChunkData.Length;
            var actualChunkCount   = actualFile.ChunkData.Length;
            CollectException(() => Assert.IsTrue(expectedChunkCount == actualChunkCount, $"ChunkData.Length's are different: should be {expectedChunkCount}, is {actualChunkCount}"));

            for (int i = 0; i < expectedChunkCount; i++) {
                var expectedChunkInfo = expectedFile.ChunkLocations[i];
                var actualChunkInfo   = actualFile.ChunkLocations[i];

                CollectException(() => Assert.IsTrue(expectedChunkInfo.Exists == actualChunkInfo.Exists, $"Inconsistent Chunk[{i}].Exists: expected={expectedChunkInfo.Exists}, actual={actualChunkInfo.Exists}"));
                if (!expectedChunkInfo.Exists || actualChunkInfo.Exists == false)
                    continue;

                CollectException(() => Assert.IsTrue(expectedChunkInfo.ChunkType == actualChunkInfo.ChunkType, $"Inconsistent Chunk[{i}].ChunkType: expected={expectedChunkInfo.ChunkType}, actual={actualChunkInfo.ChunkType}"));

                var skipRegions = GetSkipRegions(i);
                var expectedSize = expectedChunkInfo.DecompressedSize + ((skipRegions != null) ? skipRegions.Sum(x => x.ActualDataExtraBytes) : 0);
                var actualSize   = actualChunkInfo.DecompressedSize;

                CollectException(() => Assert.IsTrue(expectedSize == actualSize,
                    $"Inconsistent Chunk[{i}].DecompressedSize: expected={expectedSize} ({expectedSize:X4}), actual={actualSize} ({actualSize:X4})"));

                var expectedChunkData = expectedFile.ChunkData[i];
                var actualChunkData   = actualFile.ChunkData[i];
                Assert.IsNotNull(expectedChunkData, $"Internal logic error: {nameof(expectedChunkData)} should not be null!");
                Assert.IsNotNull(actualChunkData,   $"Internal logic error: {nameof(actualChunkData)} should not be null!");

                var expectedChunkByteData = expectedChunkData.DecompressedData.Data.GetDataCopyOrReference();
                var actualChunkByteData   = actualChunkData.DecompressedData.Data.GetDataCopyOrReference();
                var reportOffset = actualChunkData.IsCompressed ? 0 : actualChunkInfo.ChunkFileAddress;
                var reportInfo = actualChunkData.IsCompressed
                    ? $"Chunk[{i}] (compressed data)"
                    : $"Chunk[{i}] (uncompressed data -- actual offset is 0x{reportOffset:X4}";

                CollectException(() => AssertByteComparison(
                    expectedChunkByteData,
                    actualChunkByteData,
                    reportOffset, reportInfo,
                    skipRegions
                ));
            }

            if (exceptionsCaught.Count == 1)
                throw exceptionsCaught[0];
            else if (exceptionsCaught.Count > 1)
                throw new AggregateException("\r\n" + string.Join("\r\n", exceptionsCaught.Select(x => x.Message)));
        }
    }
}
