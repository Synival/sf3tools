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
    }
}
