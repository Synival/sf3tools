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

        public static void AssertByteComparison(byte[] fileData, byte[] outputData, ByteComparisonSkipRegion[]? skipRegions = null, float acceptablePercentage = 100.0f) {
            var errors = ByteComparisonErrors(fileData, outputData, out var percentageCorrect, skipRegions) ?? [];
            if (percentageCorrect >= acceptablePercentage) {
                foreach (var error in errors)
                    System.Diagnostics.Debug.WriteLine(error);
            }
            else if (errors.Count > 0)
                Assert.Fail(string.Join("\r\n", errors));
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
    }
}
