using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests {
        [TestMethod]
        public void Copy_WithScenario1_VOID_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "VOID");

        [TestMethod]
        public void Copy_WithScenario1_TESMAP_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "TESMAP");

        [TestMethod]
        public void Copy_WithScenario1_BLACK_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "BLACK");

        [TestMethod]
        public void Copy_WithScenario1_FURAIN_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "FURAIN");

        [TestMethod]
        public void Copy_WithScenario1_CHOU00_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "CHOU00");

        [TestMethod]
        public void Copy_WithScenario1_HONJIN_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "HONJIN");

    }
}
