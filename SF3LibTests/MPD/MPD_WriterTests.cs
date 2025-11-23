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

            AssertByteComparison(fileData, outputData);
        }

        [Ignore("Works great but takes too long!")]
        [TestMethod]
        public void WriteMPD_WithAllScenario1MPDs_HasSamePrimaryTextureChunks() {
            ForEachMPD(ScenarioType.Scenario1, originalFile => {
                TestMPDTextures(originalFile, TextureCollectionType.PrimaryTextures);
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_HasCorrectPrimaryTextures() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "Z_AS.MPD");
            TestMPDTextures(originalFile, TextureCollectionType.PrimaryTextures);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_HasCorrectExtraTextures() {
            var originalFile = MakeFile(ScenarioType.Scenario1, "Z_AS.MPD");
            TestMPDTextures(originalFile, TextureCollectionType.Chunk19ModelTextures);
        }

        private void TestMPDTextures(IMPD_File originalFile, TextureCollectionType collectionType) {
            var newFile = RecreateMPD(originalFile);

            var primaryTextureCollections = originalFile.TextureCollections
                .Where(x => x != null && x.Collection == collectionType)
                .OrderBy(x => x.ChunkIndex)
                .ToArray();

            var errors = new List<string>();
            foreach (var origTexCollection in primaryTextureCollections) {
                var chunkIndex = origTexCollection.ChunkIndex!.Value;

                var origLoc = originalFile.ChunkLocations[chunkIndex];
                var newLoc  = newFile.ChunkLocations[chunkIndex];
                var newTexCollection  = newFile.TextureCollections.FirstOrDefault(x => x.ChunkIndex == chunkIndex);

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

        private void AssertByteComparison(byte[] fileData, byte[] outputData) {
            var errors = ByteComparisonErrors(fileData, outputData) ?? [];
            if (errors.Count > 0)
                Assert.Fail(string.Join("\r\n", errors));
        }

        private List<string> ByteComparisonErrors(byte[] expected, byte[] actual) {
            var errors = new List<string>();

            if (expected.Length != actual.Length)
                errors.Add($"Length is wrong: should be {expected.Length} (0x{expected.Length:X5}), is {actual.Length} (0x{actual.Length:X5})");
            uint? firstWrongByte = null;
            int wrongBytes = 0;
            int bytesToCompare = Math.Min(expected.Length, actual.Length);

            for (uint i = 0; i < bytesToCompare; i++) {
                if (expected[i] != actual[i]) {
                    if (!firstWrongByte.HasValue)
                        firstWrongByte = i;
                    wrongBytes++;
                }
            }

            if (wrongBytes > 0) {
                var percent = (float) (bytesToCompare - wrongBytes) / bytesToCompare * 100.0f;
                errors.Add($"Comparable data is wrong: {percent:0.00}% accurate");

                var rightByte = expected[firstWrongByte!.Value];
                var wrongByte = actual[firstWrongByte!.Value];
                errors.Add($"First wrong byte is at {firstWrongByte!.Value} (0x{firstWrongByte:X4}):");
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
