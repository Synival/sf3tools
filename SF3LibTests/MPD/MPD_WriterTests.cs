using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;
using static SF3.Tests.Utils.DataUtils;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD {
    [TestClass]
    public partial class MPD_WriterTests {
        private void ProducesSameLoadableDataTestBase(
            ScenarioType scenario,
            string mpdName,
            bool performByteComparison,
            ByteComparisonSkipRegion[]? rawDataSkipRegions = null,
            Dictionary<int, ByteComparisonSkipRegion[]>? chunkSkipRegions = null
        ) {
            var mpdFile = MakeMPD_File(scenario, mpdName + ".MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, scenario);
                writer.WriteMPD(mpdFile);
                outputData = memoryStream.ToArray();
            }

            var scenarioPrefix =
                (scenario == ScenarioType.Scenario1) ? "S1_" :
                (scenario == ScenarioType.Scenario2) ? "S2_" :
                                                       "S3_";

            File.WriteAllBytes($"{scenarioPrefix}{mpdName}_Test.MPD", outputData);

            if (performByteComparison)
                AssertMPDByteComparison(mpdFile, outputData, rawDataSkipRegions);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), mpdFile.NameGetterContext, scenario);
            AssertMPD_FilesHaveSameContent(mpdFile, newFile, chunkSkipRegions);
        }

        private void TestMPDTextures(IMPD_File originalFile, MPD_CollectionType collection) {
            var newFile = RecreateMPD_File(originalFile);

            var primaryTextureCollections = originalFile.TextureChunks
                .Where(x => x != null && x.Collection == collection)
                .OrderBy(x => x.ChunkIndex)
                .ToArray();

            var errors = new List<string>();
            foreach (var origTexCollection in primaryTextureCollections) {
                var chunkIndex = origTexCollection.ChunkIndex;

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
    }
}
