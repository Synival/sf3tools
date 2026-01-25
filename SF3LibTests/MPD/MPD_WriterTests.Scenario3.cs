using SF3.Analysis;
using SF3.Types;

namespace SF3.Tests.MPD {
    public partial class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithScenario3_VOID3_ProducesSameLoadableData() {
            ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "VOID3", performByteComparison: true, [
                // Insignificant surface data Chunk[5] difference due to LZSS compression differences
                new ByteComparisonSkipRegion { Offset = 0x2C36, Size = 2 },
            ]);
        }

        [TestMethod]
        public void WriteMPD_WithScenario3_AHIRU_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "AHIRU", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_AHIRU2_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "AHIRU2", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_BLACK3_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "BLACK3", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_BTL95_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "BTL95", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_BEER_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "BEER", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_B_KOYA_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "B_KOYA", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_FEDEND_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "FEDEND", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_IWAOKA_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "IWAOKA", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_HNSNOP_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "HNSNOP", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_SNRK00_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "SNRK00", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithScenario3_DAIDAI_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.Scenario3, "DAIDAI", performByteComparison: false);
    }
}
