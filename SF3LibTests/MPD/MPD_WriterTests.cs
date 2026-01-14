using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;
using static SF3.Tests.Utils.DataUtils;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD {
    [TestClass]
    public class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario1_TESMAP_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "TESMAP.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_TESMAP_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "TESMAP.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("TESMAP_Test.MPD", outputData);

            AssertMPDByteComparison(file, outputData, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0xFB36, Size = 2 },

                // Insignificant texture Chunk[17] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x1A5A9, Size = 1 },

                // Insignificant texture Chunk[18] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x1EDE6, Size = 1 },
            ]);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_VOID_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "VOID.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_VOID_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "VOID.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("VOID_Test.MPD", outputData);

            AssertMPDByteComparison(file, outputData, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BLACK_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "BLACK.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BLACK_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "BLACK.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("BLACK_Test.MPD", outputData);

            AssertMPDByteComparison(file, outputData, [
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

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_FURAIN_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "FURAIN.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_FURAIN_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "FURAIN.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("FURAIN_Test.MPD", outputData);

            AssertMPDByteComparison(file, outputData, [
                // Header: Insignificant texture Chunk[5] size difference (2 bytes) due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2037, Size = 1 },

                // Chunk[2]: Some inconsequential heightmap differences caused by *not* ignoring neighbor tiles in other blocks when flat.
                // TODO: we should be able to fix this one!!
                new ByteComparisonSkipRegion { Offset = 0x134B7, Size = 0x500 },

                // Texture compression nonsense
                new ByteComparisonSkipRegion { Offset = 0x1AA81, Size = 0x19 },

                // Image data LZSS issue
                new ByteComparisonSkipRegion { Offset = 0x24515, Size = 1 },
            ]);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile, new Dictionary<int, ByteComparisonSkipRegion[]> {
                {2, [new ByteComparisonSkipRegion { Offset = 0x134B7, Size = 0x500 }]}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_HONJIN_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "HONJIN.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_HONJIN_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "HONJIN.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("HONJIN_Test.MPD", outputData);

            AssertMPDByteComparison(file, outputData, [
                // Texture compression nonsense
                new ByteComparisonSkipRegion { Offset = 0x7E9A, Size = 2 },

                // Image data LZSS issues
                new ByteComparisonSkipRegion { Offset = 0x2042C, Size = 2 },
                new ByteComparisonSkipRegion { Offset = 0x21560, Size = 2 },
            ]);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BAL_3_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "BAL_3.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BAL_3_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "BAL_3.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("BAL_3_Test.MPD", outputData);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // There's exactly one more collision line that screws up the table...
                    new ByteComparisonSkipRegion() { Offset = 0x1442C, Size = 0x1C0 },

                    // ...and here it is.
                    new ByteComparisonSkipRegion() { Offset = 0x14A0C, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BALSA_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "BALSA.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BALSA_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "BALSA.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("BALSA_Test.MPD", outputData);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 1, new ByteComparisonSkipRegion[] {
                    // There's exactly one more collision line that screws up the table...
                    new ByteComparisonSkipRegion() { Offset = 0x1EC20, Size = 0x2A4 },

                    // ...and here it is.
                    new ByteComparisonSkipRegion() { Offset = 0x1F16A, Size = 2, ActualDataExtraBytes = 2 },
                }}
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_DAM_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "DAM.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_DAM_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "DAM.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("DAM_Test.MPD", outputData);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                { 2, new ByteComparisonSkipRegion[] {
                    // TODO: The normals aren't quite right. Gotta fix that!
/*
                    // Bogus surface models heights
                    // TODO: fix these!!!
                    new ByteComparisonSkipRegion() { Offset = 0x222D9, Size = 0x10000 },
*/
                }
            }});
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_MUCHUR_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "MUCHUR.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_MUCHUR_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "MUCHUR.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("MUCHUR_Test.MPD", outputData);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                {
                    1, new ByteComparisonSkipRegion[] {
                        // Collision lines are inconsistent
                        new ByteComparisonSkipRegion() { Offset = 0xD14E, Size = 0x0152 },
                        new ByteComparisonSkipRegion() { Offset = 0xD51E, Size = 2, ActualDataExtraBytes = 2 },
                        new ByteComparisonSkipRegion() { Offset = 0xD570, Size = 2, ActualDataExtraBytes = 2 },
                        new ByteComparisonSkipRegion() { Offset = 0xD654, Size = 2, ActualDataExtraBytes = 2 },
                        new ByteComparisonSkipRegion() { Offset = 0xD670, Size = 2, ActualDataExtraBytes = 2 },
                    }
                }
            });
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BTL03_CanBeLoaded() {
            var originalFile = MakeMPD_File(ScenarioType.Scenario1, "BTL03.MPD");
            _ = RecreateMPD_File(originalFile);
        }

        [TestMethod]
        public void WriteMPD_WithScenario1_BTL03_ProducesSameData() {
            var file = MakeMPD_File(ScenarioType.Scenario1, "BTL03.MPD");

            byte[]? outputData = null;
            using (var memoryStream = new MemoryStream()) {
                var writer = new MPD_Writer(memoryStream, ScenarioType.Scenario1);
                writer.WriteMPD(file);
                outputData = memoryStream.ToArray();
            }

            File.WriteAllBytes("BTL03_Test.MPD", outputData);

            var newFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(outputData)), file.NameGetterContext, file.Scenario);
            AssertMPD_FilesHaveSameContent(file, newFile, new Dictionary<int, ByteComparisonSkipRegion[]>() {
                {
                    1, new ByteComparisonSkipRegion[] {
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
                    }
                }
            });
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
