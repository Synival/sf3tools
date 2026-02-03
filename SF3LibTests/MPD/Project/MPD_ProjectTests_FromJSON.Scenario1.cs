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
    }
}
