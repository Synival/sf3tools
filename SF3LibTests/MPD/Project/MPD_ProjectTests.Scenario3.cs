using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests {
        [TestMethod]
        public void Copy_WithScenario3_VOID3_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "VOID3");

        [TestMethod]
        public void Copy_WithScenario3_AHIRU_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "AHIRU");

        [TestMethod]
        public void Copy_WithScenario3_AHIRU2_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "AHIRU2");
/*
        [TestMethod]
        public void Copy_WithScenario3_BLACK3_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "BLACK3");
*/
        [TestMethod]
        public void Copy_WithScenario3_BTL95_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "BTL95");
/*
        [TestMethod]
        public void Copy_WithScenario3_BEER_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "BEER");

        [TestMethod]
        public void Copy_WithScenario3_B_KOYA_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "B_KOYA");

        [TestMethod]
        public void Copy_WithScenario3_FEDEND_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "FEDEND");

        [TestMethod]
        public void Copy_WithScenario3_IWAOKA_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "IWAOKA");

        [TestMethod]
        public void Copy_WithScenario3_HNSNOP_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "HNSNOP");
*/
        [TestMethod]
        public void Copy_WithScenario3_SNRK00_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "SNRK00");
/*
        [TestMethod]
        public void Copy_WithScenario3_DAIDAI_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "DAIDAI");
*/
        [TestMethod]
        public void Copy_WithScenario3_ATBTL2_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario3, "ATBTL2");
    }
}
