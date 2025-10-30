using SF3.Types;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests {
    public class SF3FileTestCase : CommonLib.Tests.TestCase {
        public SF3FileTestCase(ScenarioType scenario, string filename)
        : base((ResourcePath(scenario, filename) ?? "<not found>") + " (" + scenario.ToString() + ")") {
            Scenario = scenario;
            Filename = ResourcePath(scenario, filename) ?? "<not found>";
        }

        public ScenarioType Scenario { get; }
        public string Filename { get; }
    }
}
