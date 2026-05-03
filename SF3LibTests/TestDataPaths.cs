using SF3.Types;

namespace SF3.Tests {
    internal static class TestDataPaths {
        public static readonly Dictionary<ScenarioType, string> ScenarioDataPaths = new() {
            { ScenarioType.Scenario1, "C:/SF3/Scenario1/" },
            { ScenarioType.Scenario2, "C:/SF3/Scenario2/" },
            { ScenarioType.Scenario3, "C:/SF3/Scenario3/" },
            { ScenarioType.PremiumDisk, "C:/SF3/PremiumDisk/" },
        };

        public static string? ResourcePath(ScenarioType scenario, string resource = "")
            => ScenarioDataPaths.TryGetValue(scenario, out string? value) ? (value + resource) : null;
    }
}
