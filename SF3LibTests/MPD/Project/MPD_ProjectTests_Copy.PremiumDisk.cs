using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests_Copy {
        [TestMethod]
        public void Copy_WithPremiumDisk_MOVSEL_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.PremiumDisk, "MOVSEL");

        [TestMethod]
        public void Copy_WithPremiumDisk_OPPREM_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.PremiumDisk, "OPPREM");

        [TestMethod]
        public void Copy_WithPremiumDisk_NIGI01_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.PremiumDisk, "NIGI01");

        [TestMethod]
        public void Copy_WithPremiumDisk_NIGI02_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.PremiumDisk, "NIGI02");
    }
}
