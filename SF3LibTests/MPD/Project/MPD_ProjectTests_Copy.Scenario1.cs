using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests_Copy {
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

        [TestMethod]
        public void Copy_WithScenario1_BAL_3_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "BAL_3");

        [TestMethod]
        public void Copy_WithScenario1_BALSA_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "BALSA");

        [TestMethod]
        public void Copy_WithScenario1_DAM_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "DAM");

        [TestMethod]
        public void Copy_WithScenario1_MUCHUR_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "MUCHUR");

        [TestMethod]
        public void Copy_WithScenario1_BTL03_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "BTL03");

        [TestMethod]
        public void Copy_WithScenario1_BTL02_CanBeLoaded()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "BTL02");

        [TestMethod]
        public void Copy_WithScenario1_Z_AS_ProducesSameData()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "Z_AS");

        [TestMethod]
        public void Copy_WithScenario1_CHOU00_ProducesSameData()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "CHOU00");

        [TestMethod]
        public void Copy_WithScenario1_GDI_ProducesSameData()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "GDI");

        [TestMethod]
        public void Copy_WithScenario1_MGMA00_ProducesSameData()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "MGMA00");

        [TestMethod]
        public void Copy_WithScenario1_MGMA01_ProducesSameData()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "MGMA01");

        [TestMethod]
        public void Copy_WithScenario1_JOUSAI_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "JOUSAI");

        [TestMethod]
        public void Copy_WithScenario1_YAKA3_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "YAKA3");

        [TestMethod]
        public void Copy_WithScenario1_SARA03_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "SARA03");

        [TestMethod]
        public void Copy_WithScenario1_SARA04_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "SARA04");

        [TestMethod]
        public void Copy_WithScenario1_FED06_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "FED06");

        [TestMethod]
        public void Copy_WithScenario1_BAKA2_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "BAKA2");

        [TestMethod]
        public void Copy_WithScenario1_HRRAIL_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario1, "HRRAIL");
    }
}
