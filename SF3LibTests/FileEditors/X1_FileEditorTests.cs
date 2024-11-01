using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename,
                MapLeaderType mapLeader)
            : base(scenario, filename) {
                MapLeader = mapLeader;
            }

            public MapLeaderType MapLeader { get; }
        }

        private static readonly List<X1_TestCase> BattleTestCases = [
            new(ScenarioType.Scenario1,   "X1BTL101.BIN", MapLeaderType.Synbios),
            new(ScenarioType.Scenario2,   "X1BTL201.BIN", MapLeaderType.Medion),
            new(ScenarioType.Scenario3,   "X1BTL301.BIN", MapLeaderType.Julian),
            new(ScenarioType.PremiumDisk, "X1BTLP01.BIN", MapLeaderType.Synbios),
        ];

        [TestMethod]
        public void BattleFiles_HaveExpectedTables() {
            TestCase.Run(BattleTestCases, testCase => {
                var editor = new X1_FileEditor(testCase.Scenario, testCase.MapLeader, false);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.IsNotNull(editor.AITable);
                Assert.IsNotNull(editor.BattlePointersTable);
                Assert.IsNotNull(editor.CustomMovementTable);
                Assert.IsNotNull(editor.HeaderTable);
                Assert.IsNotNull(editor.SlotTable);
                Assert.IsNotNull(editor.SpawnZoneTable);
                Assert.IsNotNull(editor.TreasureTable);

                Assert.IsNull(editor.ArrowTable);
                Assert.IsNull(editor.EnterTable);
                Assert.IsNull(editor.NpcTable);

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNull(editor.TileMovementTable);
                    Assert.IsNull(editor.WarpTable);
                }
                else if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNotNull(editor.TileMovementTable);
                    Assert.IsNotNull(editor.WarpTable);
                }
            });
        }
    }
}
