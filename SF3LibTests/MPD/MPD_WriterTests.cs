using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;
using static SF3.Tests.Utils.DataUtils;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD {
    [TestClass]
    public class MPD_WriterTests {
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

            File.WriteAllBytes(mpdName + "_Test.MPD", outputData);

            if (performByteComparison)
                AssertMPDByteComparison(mpdFile, outputData, rawDataSkipRegions);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), mpdFile.NameGetterContext, scenario);
            AssertMPD_FilesHaveSameContent(mpdFile, newFile, chunkSkipRegions);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_TESMAP_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "TESMAP", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0xFB36, Size = 2 },

                // Insignificant texture Chunk[17] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x1A5A9, Size = 1 },

                // Insignificant texture Chunk[18] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x1EDE6, Size = 1 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_VOID_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "VOID", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BLACK_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BLACK", performByteComparison: true, [
                // PDATA's have the wrong addresses in the original file!! Just skip it!!
                new ByteComparisonSkipRegion { Offset = 0x2100, Size = 0x798 },

                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },

                // Whole buncha other random LZSS inconsistencies that are totally fine.
                new ByteComparisonSkipRegion { Offset = 0x0B267, Size = 1 },
                new ByteComparisonSkipRegion { Offset = 0x0F1FC, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x1577A, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x18576, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x18DDD, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x18DF2, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_FURAIN_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "FURAIN", performByteComparison: true, [
                // Header: Insignificant texture Chunk[5] size difference (2 bytes) due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2037, Size = 1 },

                // Texture compression nonsense
                new ByteComparisonSkipRegion { Offset = 0x1AA81, Size = 0x19 },

                // Image data LZSS issue
                new ByteComparisonSkipRegion { Offset = 0x24515, Size = 1 },
            ],
            new Dictionary<int, ByteComparisonSkipRegion[]> {
                {2, [new ByteComparisonSkipRegion { Offset = 0x134B7, Size = 0x500 }]}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_HONJIN_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "HONJIN", performByteComparison: true, [
                // Texture compression nonsense
                new ByteComparisonSkipRegion { Offset = 0x7E9A, Size = 2 },

                // Image data LZSS issues
                new ByteComparisonSkipRegion { Offset = 0x2042C, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x21560, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BAL_3_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BAL_3", performByteComparison: false, null, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // There's exactly one more collision line that screws up the table...
                    new ByteComparisonSkipRegion() { Offset = 0x1442C, Size = 0x1C0 },

                    // ...and here it is.
                    new ByteComparisonSkipRegion() { Offset = 0x14A0C, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BALSA_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BALSA", performByteComparison: false, null, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // There's exactly one more collision line that screws up the table...
                    new ByteComparisonSkipRegion() { Offset = 0x1EC20, Size = 0x2A4 },

                    // ...and here it is.
                    new ByteComparisonSkipRegion() { Offset = 0x1F16A, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_DAM_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "DAM", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_MUCHUR_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "MUCHUR", performByteComparison: false, null, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // Collision lines are inconsistent
                    new ByteComparisonSkipRegion() { Offset = 0xD14E, Size = 0x0152 },
                    new ByteComparisonSkipRegion() { Offset = 0xD51E, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0xD570, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0xD654, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0xD670, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BTL03_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BTL03", performByteComparison: false, null, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // Collision lines are inconsistent
                    new ByteComparisonSkipRegion() { Offset = 0x5203, Size = 0x31E },
                    new ByteComparisonSkipRegion() { Offset = 0x559C, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x5610, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x562C, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x56C6, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x5724, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x5792, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x57C8, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x57FE, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x580E, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x5910, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x5B24, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x5B36, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BTL02_CanBeLoaded() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BTL02", performByteComparison: false, null, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // Collision lines are inconsistent
                    new ByteComparisonSkipRegion() { Offset = 0x1105A, Size = 0x2DA },
                    new ByteComparisonSkipRegion() { Offset = 0x1151A, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x115C6, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x11696, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x11720, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x119A8, Size = 2, ActualDataExtraBytes = 2 },
                    new ByteComparisonSkipRegion() { Offset = 0x119B4, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_ProducesSameData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "Z_AS", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_CHOU00_ProducesSameData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "CHOU00", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_GDI_ProducesSameData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "GDI", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_MGMA00_ProducesSameData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "MGMA00", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_MGMA01_ProducesSameData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "MGMA01", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_Scenario2ToScenario1_BTL43_ProducesExpectedMPD() {
            var mpdFile = MakeMPD_File(ScenarioType.Scenario2, "BTL47.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(mpdFile);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("BTL47_Scn1_Test.MPD", outputData);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), mpdFile.NameGetterContext, ScenarioType.Scenario1);

            // TODO: Check a whole buncha stuff!
            Assert.Fail();
        }

        [Ignore("Works great but takes too long!")]
        [TestMethod]
        public void WriteMPD_WithAllScenario1MPDs_HasSamePrimaryTextureChunks() {
            ForEachMPD_File(ScenarioType.Scenario1, originalFile => {
                TestMPDTextures(originalFile, MPD_CollectionType.Primary);
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_HasCorrectPrimaryTextures() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "Z_AS.MPD");
            TestMPDTextures(originalFile, MPD_CollectionType.Primary);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_Z_AS_HasCorrectExtraTextures() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "Z_AS.MPD");
            TestMPDTextures(originalFile, MPD_CollectionType.ExtraModels);
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
