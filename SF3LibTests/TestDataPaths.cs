using SF3.Types;

namespace SF3.Tests {
    internal static class TestDataPaths {
        public static readonly Dictionary<ScenarioType, string> ScenarioDataPaths = new() {
            { ScenarioType.Scenario1, "D:/" },
            { ScenarioType.Scenario2, "E:/" },
            { ScenarioType.Scenario3, "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        public static string? ResourcePath(ScenarioType scenario, string resource = "")
            => ScenarioDataPaths.TryGetValue(scenario, out string? value) ? (value + resource) : null;
    }
}
