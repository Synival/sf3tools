using SF3.Types;

namespace SF3.Tests.MPD {
    public partial class MPD_WriterTests {
        [TestMethod]
        public void WriteMPD_WithPremiumDisk_MOVSEL_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.PremiumDisk, "MOVSEL", performByteComparison: false);

        [TestMethod]
        public void WriteMPD_WithPremiumDisk_OPPREM_ProducesSameLoadableData()
            => ProducesSameLoadableDataTestBase(ScenarioType.PremiumDisk, "OPPREM", performByteComparison: false);
    }
}
