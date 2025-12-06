using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.MPD {
    [TestClass]
    public class MPD_WriterTests {
        private static MPD_File MakeFile(ScenarioType scenario, string filename) {
            var filePath = TestDataPaths.ResourcePath(scenario, Path.GetFileName(filename))!;
            var fileData = File.ReadAllBytes(filePath);
            return MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(fileData)), new NameGetterContext(scenario), scenario);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_TESMAP_CanBeLoaded() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "TESMAP.MPD");
            _ = RecreateMPD(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_TESMAP_ProducesSameData() {
            var file = MakeFile(ScenarioType.Scenario1, "TESMAP.MPD");
            var fileData = file.Data.GetDataCopyOrReference();

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("TESMAP_Test.MPD", outputData);

            AssertByteComparison(fileData, outputData, [
                // Header: Insignificant texture Chunk[13] size difference (2 bytes) due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x206F, Size = 1 },

                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0xFB36, Size = 2 },

                // Insignificant texture Chunk[13] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x11A78, Size = 1 },
                new ByteComparisonSkipRegion { Offset = 0x11A82, Size = 4 },

                // Insignificant texture Chunk[17] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x1A5A9, Size = 1 },

                // Insignificant texture Chunk[18] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x1EDE6, Size = 1 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_VOID_CanBeLoaded() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "VOID.MPD");
            _ = RecreateMPD(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_VOID_ProducesSameData() {
            var file = MakeFile(ScenarioType.Scenario1, "VOID.MPD");
            var fileData = file.Data.GetDataCopyOrReference();

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("VOID_Test.MPD", outputData);

            AssertByteComparison(fileData, outputData, [
                // Header: Insignificant texture Chunk[13] size difference (2 bytes) due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x206F, Size = 1 },

                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },

                // Insignificant texture Chunk[13] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x4B78, Size = 1 },
                new ByteComparisonSkipRegion { Offset = 0x4B82, Size = 4 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BLACK_CanBeLoaded() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "BLACK.MPD");
            _ = RecreateMPD(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BLACK_ProducesSameData() {
            var file = MakeFile(ScenarioType.Scenario1, "BLACK.MPD");
            var fileData = file.Data.GetDataCopyOrReference();

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("BLACK_Test.MPD", outputData);

            AssertByteComparison(fileData, outputData, [
/*
                // Header: Insignificant texture Chunk[13] size difference (2 bytes) due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x206F, Size = 1 },

                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },

                // Insignificant texture Chunk[13] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x4B78, Size = 1 },
                new ByteComparisonSkipRegion { Offset = 0x4B82, Size = 4 },
*/
            ]);
        }

        [Ignore("Works great but takes too long!")]
        [TestMethod]
        public void WriteMPD_WithAllScenario1MPDs_HasSamePrimaryTextureChunks() {
            ForEachMPD(ScenarioType.Scenario1, originalFile => {
                TestMPDTextures(originalFile, CollectionType.Primary);
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_HasCorrectPrimaryTextures() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "Z_AS.MPD");
            TestMPDTextures(originalFile, CollectionType.Primary);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_HasCorrectExtraTextures() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "Z_AS.MPD");
            TestMPDTextures(originalFile, CollectionType.ExtraModel);
        }

        private void TestMPDTextures(IMPD_File originalFile, CollectionType collection) {
            var newFile = RecreateMPD(originalFile);

            var primaryTextureCollections = originalFile.TextureChunks
                .Where(x => x != null && x.Collection == collection)
                .OrderBy(x => x.ChunkIndex)
                .ToArray();

            var errors = new List<string>();
            foreach (var origTexCollection in primaryTextureCollections) {
                var chunkIndex = origTexCollection.ChunkIndex!.Value;

                var origLoc = originalFile.ChunkLocations[chunkIndex];
                var newLoc  = newFile.ChunkLocations[chunkIndex];
                var newTexCollection  = newFile.TextureChunks.FirstOrDefault(x => x.ChunkIndex == chunkIndex);

                // Compare chunk size, allowing a 0x04 reduction tolerance to account for the ever-so-slightly more efficient LZSS algorithm.
                var chunkSizeMin = newLoc.ChunkSize - 0x04;
                if (newLoc.ChunkSize < chunkSizeMin || newLoc.ChunkSize > origLoc.ChunkSize)
                    errors.Add($"Chunk[{chunkIndex}] size is wrong: should be 0x{origLoc.ChunkSize:X5}, is 0x{newLoc.ChunkSize:X5}");

                // Decompressed size should always be exactly the same.
                if (origLoc.DecompressedSize != newLoc.DecompressedSize)
                    errors.Add($"Chunk[{chunkIndex}] decompressed size is wrong: should be 0x{origLoc.DecompressedSize:X5}, is 0x{newLoc.DecompressedSize:X5}");

                if (newTexCollection == null) {
                    errors.Add($"Chunk[{chunkIndex}] texture collection is missing");
                    continue;
                }
                else {
                    if (origTexCollection.TextureHeaderTable[0].TextureIdStart != newTexCollection.TextureHeaderTable[0].TextureIdStart)
                        errors.Add($"Chunk[{chunkIndex}] first ID is wrong: should be 0x{origTexCollection.TextureHeaderTable[0].TextureIdStart:X2}, is 0x{newTexCollection.TextureHeaderTable[0].TextureIdStart:X2}");
                    if (origTexCollection.TextureTable.Length != newTexCollection.TextureTable.Length)
                        errors.Add($"Chunk[{chunkIndex}] texture count is wrong: should be {origTexCollection.TextureTable.Length}, is {newTexCollection.TextureTable.Length}");
                }
            }

            if (errors.Count > 0)
                Assert.Fail(string.Join("\r\n", errors));
        }

        private struct ByteComparisonSkipRegion {
            public int Offset;
            public int Size;
            public int ExpectedIndexAdjustment;
        }

        private void AssertByteComparison(byte[] fileData, byte[] outputData, ByteComparisonSkipRegion[]? skipRegions = null, float acceptablePercentage = 100.0f) {
            var errors = ByteComparisonErrors(fileData, outputData, out var percentageCorrect, skipRegions) ?? [];
            if (percentageCorrect >= acceptablePercentage) {
                foreach (var error in errors)
                    System.Diagnostics.Debug.WriteLine(error);
            }
            else if (errors.Count > 0)
                Assert.Fail(string.Join("\r\n", errors));
        }

        private List<string> ByteComparisonErrors(byte[] expected, byte[] actual, out float percentageCorrect, ByteComparisonSkipRegion[]? skipRegions = null) {
            var errors = new List<string>();

            if (expected.Length != actual.Length)
                errors.Add($"Length is wrong: should be {expected.Length} (0x{expected.Length:X5}), is {actual.Length} (0x{actual.Length:X5})");
            (uint ExpectedOffset, uint ActualOffset)? firstWrongByte = null;
            int wrongBytes = 0;
            int bytesToCompare = Math.Min(expected.Length, actual.Length);

            // Sort the skip regions.
            skipRegions = (skipRegions ?? []).OrderBy(x => x.Offset).ToArray();
            int skipRegionIndex = 0;
            var skipRegion = skipRegions.Length > skipRegionIndex ? skipRegions[skipRegionIndex] : (ByteComparisonSkipRegion?) null;

            for (int i = 0, j = 0; i < expected.Length && j < actual.Length; i++, j++) {
                if (i == skipRegion?.Offset) {
                    i += skipRegion.Value.Size - 1;
                    j += skipRegion.Value.Size - 1;
                    i += skipRegion.Value.ExpectedIndexAdjustment;
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
                errors.Add($"Comparable data is wrong: {percentageCorrect:0.00}% accurate");

                var rightByte = expected[firstWrongByte!.Value.ExpectedOffset];
                var wrongByte = actual[firstWrongByte!.Value.ActualOffset];
                errors.Add($"First wrong byte is at {firstWrongByte!.Value.ExpectedOffset} (0x{firstWrongByte!.Value.ExpectedOffset:X4}):");
                errors.Add($"  Should be {rightByte} (0x{rightByte:X2}), is {wrongByte} (0x{wrongByte:X2})");
            }

            return errors;
        }

        private IMPD_File RecreateMPD(IMPD_File mpd) {
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

        private void ForEachMPD(ScenarioType scenario, Action<IMPD_File> action) {
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
                var mpd = MakeFile(scenario, testCase.Filename);
                action(mpd);
            });
        }
    }
}
