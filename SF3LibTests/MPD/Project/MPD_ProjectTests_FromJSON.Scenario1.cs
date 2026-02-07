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
        public void FromJSON_WithScenario1_DAM_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "DAM");

        [TestMethod]
        public void FromJSON_WithScenario1_MUCHUR_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "MUCHUR");

        [TestMethod]
        public void FromJSON_WithScenario1_BTL03_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BTL03");

        [TestMethod]
        public void FromJSON_WithScenario1_BTL02_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BTL02");

        [TestMethod]
        public void FromJSON_WithScenario1_Z_AS_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "Z_AS");

        [TestMethod]
        public void FromJSON_WithScenario1_GDI_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "GDI");

        [TestMethod]
        public void FromJSON_WithScenario1_MGMA00_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "MGMA00");

        [TestMethod]
        public void FromJSON_WithScenario1_MGMA01_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "MGMA01");

        [TestMethod]
        public void FromJSON_WithScenario1_JOUSAI_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "JOUSAI");

        [TestMethod]
        public void FromJSON_WithScenario1_YAKA3_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "YAKA3");

        [TestMethod]
        public void FromJSON_WithScenario1_SARA03_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "SARA03");

        [TestMethod]
        public void FromJSON_WithScenario1_SARA04_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "SARA04");

        [TestMethod]
        public void FromJSON_WithScenario1_FED06_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "FED06");

        [TestMethod]
        public void FromJSON_WithScenario1_BAKA2_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "BAKA2");

        [TestMethod]
        public void FromJSON_WithScenario1_HRRAIL_ProducesSameMPDAsProjectCopy()
            => ProducesSameMPDAsProjectCopyTestBase(ScenarioType.Scenario1, "HRRAIL");
    }
}
