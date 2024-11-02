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
            new(ScenarioType.Scenario1,   "X1BTL104.BIN", MapLeaderType.Synbios),
            new(ScenarioType.Scenario2,   "X1BTL201.BIN", MapLeaderType.Medion),
            new(ScenarioType.Scenario3,   "X1BTL301.BIN", MapLeaderType.Julian),
            new(ScenarioType.PremiumDisk, "X1BTLP01.BIN", MapLeaderType.Synbios),
        ];

        private static readonly List<X1_TestCase> TownTestCases = [
            new(ScenarioType.Scenario1,   "X1BAL_3.BIN", MapLeaderType.Synbios),
            new(ScenarioType.Scenario2,   "X1DUSTY.BIN", MapLeaderType.Medion),
            new(ScenarioType.Scenario3,   "X1BEER.BIN",  MapLeaderType.Julian),
            new(ScenarioType.PremiumDisk, "X1DREAM.BIN", MapLeaderType.Synbios),
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
                else {
                    Assert.IsNotNull(editor.TileMovementTable);
                    Assert.IsNotNull(editor.WarpTable);
                }
            });
        }

        [TestMethod]
        public void TownFiles_HaveExpectedTables() {
            TestCase.Run(TownTestCases, testCase => {
                var editor = new X1_FileEditor(testCase.Scenario, testCase.MapLeader, false);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.IsNull(editor.AITable);
                Assert.IsNull(editor.BattlePointersTable);
                Assert.IsNull(editor.CustomMovementTable);
                Assert.IsNull(editor.HeaderTable);
                Assert.IsNull(editor.SlotTable);
                Assert.IsNull(editor.SpawnZoneTable);
                Assert.IsNull(editor.TileMovementTable);

                Assert.IsNotNull(editor.EnterTable);
                Assert.IsNotNull(editor.NpcTable);
                Assert.IsNotNull(editor.TreasureTable);

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNull(editor.ArrowTable);
                    Assert.IsNull(editor.WarpTable);
                }
                else {
                    Assert.IsNotNull(editor.ArrowTable);
                    Assert.IsNotNull(editor.WarpTable);
                }
            });
        }
    }
}
