using SF3.MPD.Project;
using SF3.MPD.Writer;
using SF3.Types;
using SF3.Utils;
using static SF3.Tests.Utils.MPD_TestUtils;

namespace SF3.Tests.MPD.Project {
    [TestClass]
    public partial class MPD_ProjectTests {
        private void ProducesSameMPDAsOriginal(ScenarioType scenario, string file) {
            var mpdOriginal = MakeMPD_File(scenario, file + ".MPD");
            var mpdCopy = new MPD_Project(mpdOriginal);

            byte[] exportOriginal;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, scenario);
                writer.WriteMPD(mpdOriginal);
                exportOriginal = stream.ToArray();
            }

            byte[] exportCopy;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, scenario);
                writer.WriteMPD(mpdCopy);
                exportCopy = stream.ToArray();
            }

            var scenarioPrefix =
                (scenario == ScenarioType.Scenario1) ? "S1" :
                (scenario == ScenarioType.Scenario2) ? "S2" :
                (scenario == ScenarioType.Scenario3) ? "S3" :
                (scenario == ScenarioType.Scenario3) ? "PD" :
                                                       "Unknown";

            File.WriteAllBytes($"Project_{scenarioPrefix}_{file}_Test.MPD", exportCopy);

            var errors = AnalysisUtils.GetByteComparisonErrors(exportOriginal, exportCopy);
            if (errors?.Length > 0)
                Assert.Fail("\r\n============================================\r\n" + string.Join("\r\n", errors));
        }
    }
}
