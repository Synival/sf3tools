using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests {
        [TestMethod]
        public void Copy_WithScenario2_VOID_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "VOID");

        [TestMethod]
        public void Copy_WithScenario2_BLACK_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "BLACK");

        [TestMethod]
        public void Copy_WithScenario2_SNIOKI_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "SNIOKI");

        [TestMethod]
        public void Copy_WithScenario2_BTL43_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "BTL43");

        [TestMethod]
        public void Copy_WithScenario2_AIRO_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "AIRO");

        [TestMethod]
        public void Copy_WithScenario2_SARA22_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "SARA22");

        [TestMethod]
        public void Copy_WithScenario2_SARA23_ProducesVerySpecificDifferences()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "SARA23");

        [TestMethod]
        public void Copy_WithScenario2_MUBAR2_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "MUBAR2");

        [TestMethod]
        public void Copy_WithScenario2_BTL42_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "BTL42");

        [TestMethod]
        public void Copy_WithScenario2_FUNE_T_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "FUNE_T");

        [TestMethod]
        public void Copy_WithScenario2_STAMP2_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "STAMP2");

        [TestMethod]
        public void Copy_WithScenario2_STAMP3_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "STAMP3");

        [TestMethod]
        public void Copy_WithScenario2_ELINB_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "ELINB");

        [TestMethod]
        public void Copy_WithScenario2_ATBTL2_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "ATBTL2");
    }
}
