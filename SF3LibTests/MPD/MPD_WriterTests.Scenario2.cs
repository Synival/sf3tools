using SF3.Analysis;
using SF3.Types;

namespace SF3.Tests.MPD {
    public partial class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario2_VOID_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "VOID", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario2_BLACK_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "BLACK", performByteComparison: true, [
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
        public void WriteMPD_WithScenario2_SNIOKI_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "SNIOKI", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_BTL43_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "BTL43", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_AIRO_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "AIRO", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_SARA23_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "SARA23", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_BTL42_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "BTL42", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_FUNE_T_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "FUNE_T", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_STAMP2_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "STAMP2", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario2_STAMP3_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario2, "STAMP3", performByteComparison: false);
    }
}
