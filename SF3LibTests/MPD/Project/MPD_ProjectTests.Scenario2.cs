using SF3.Types;

namespace SF3.Tests.MPD.Project {
    public partial class MPD_ProjectTests {
        [TestMethod]
        public void Copy_WithScenario2_VOID_ProducesSameMPDAsOriginal()
            => ProducesSameMPDAsOriginal(ScenarioType.Scenario2, "VOID");
    }
}
