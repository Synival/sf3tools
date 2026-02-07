using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests_FromJSON {
        [TestMethod]
        public void FromJSON_WithScenario1_VOID_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "VOID");

        [TestMethod]
        public void FromJSON_WithScenario1_TESMAP_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "TESMAP");

        [TestMethod]
        public void FromJSON_WithScenario1_BLACK_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BLACK");

        [TestMethod]
        public void FromJSON_WithScenario1_FURAIN_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "FURAIN");

        [TestMethod]
        public void FromJSON_WithScenario1_CHOU00_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "CHOU00");

        [TestMethod]
        public void FromJSON_WithScenario1_HONJIN_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "HONJIN");

        [TestMethod]
        public void FromJSON_WithScenario1_BAL_3_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BAL_3");

        [TestMethod]
        public void FromJSON_WithScenario1_BALSA_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BALSA");

        [TestMethod]
        public void FromJSON_WithScenario1_BTL02_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BTL02");
    }
}
