using SF3.Analysis;
using SF3.Types;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD.Writer {
    public partial class MPD_WriterTests {
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
            ]);
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
        public void WriteMPD_WithScenario1_BAL_3_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BAL_3", performByteComparison: false, null);

        [TestMethod]
        public void WriteMPD_WithScenario1_BALSA_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BALSA", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_DAM_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "DAM", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_MUCHUR_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "MUCHUR", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_BTL03_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BTL03", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_BTL02_CanBeLoaded()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BTL02", performByteComparison: false);

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
        public void WriteMPD_WithScenario1_JOUSAI_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "JOUSAI", performByteComparison: false, null);

        [TestMethod]
        public void WriteMPD_WithScenario1_YAKA3_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "YAKA3", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_SARA03_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "SARA03", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_SARA04_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "SARA04", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_FED06_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "FED06", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_BAKA2_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "BAKA2", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario1_HRRAIL_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario1, "HRRAIL", performByteComparison: false);

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
    }
}
