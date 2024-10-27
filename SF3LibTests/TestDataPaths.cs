﻿using SF3.Types;

namespace SF3.Tests {
    internal static class TestDataPaths {
        private static readonly Dictionary<ScenarioType, string> ScenarioDataPath = new() {
            { ScenarioType.Scenario1, "D:/" },
            { ScenarioType.Scenario2, "E:/" },
            { ScenarioType.Scenario3, "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        internal static string ResourcePath(ScenarioType scenario, string resource)
            => ScenarioDataPath[scenario] + resource;
    }
}
