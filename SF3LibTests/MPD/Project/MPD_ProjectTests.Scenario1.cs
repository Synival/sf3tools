using SF3.MPD;
using SF3.MPD.Project;
using SF3.Types;
using SF3.Utils;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD.Project {
    [TestClass]
    public class MPD_ProjectTests {
        [TestMethod]
        public void Copy_WithScenario1_VOID_ProducesSameMPDAsOriginal() {
            var mpdOriginal = MakeMPD_File(ScenarioType.Scenario1, "VOID.MPD");
            var mpdCopy = new MPD_Project(mpdOriginal);

            byte[] exportOriginal;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, ScenarioType.Scenario1);
                writer.WriteMPD(mpdOriginal);
                exportOriginal = stream.ToArray();
            }

            byte[] exportCopy;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, ScenarioType.Scenario1);
                writer.WriteMPD(mpdCopy);
                exportCopy = stream.ToArray();
            }

            File.WriteAllBytes($"Project_S1_VOID_Test.MPD", exportCopy);

            var errors = AnalysisUtils.GetByteComparisonErrors(exportOriginal, exportCopy);
            if (errors?.Length > 0)
                Assert.Fail("\r\n============================================\r\n" + string.Join("\r\n", errors));
        }
    }
}
