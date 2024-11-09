using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename,
                MapLeaderType? mapLeader,
                int? expectedBattleCount)
            : base(scenario, filename) {
                MapLeader = mapLeader;
                ExpectedBattleCount = expectedBattleCount;
            }

            public MapLeaderType? MapLeader { get; }
            public int? ExpectedBattleCount { get; }
        }

        private static readonly List<X1_TestCase> BattleTestCases = [
            new(ScenarioType.Scenario1,   "X1BTL104.BIN", MapLeaderType.Synbios, 1),
            new(ScenarioType.Scenario2,   "X1BTL201.BIN", MapLeaderType.Medion,  1),
            new(ScenarioType.Scenario3,   "X1BTL301.BIN", MapLeaderType.Julian,  1),
            new(ScenarioType.PremiumDisk, "X1BTLP01.BIN", MapLeaderType.Synbios, 1),
        ];

        private static readonly List<X1_TestCase> TownTestCases = [
            new(ScenarioType.Scenario1,   "X1BAL_3.BIN", null, null),
            new(ScenarioType.Scenario2,   "X1DUSTY.BIN", null, null),
            new(ScenarioType.Scenario3,   "X1BEER.BIN",  null, null),
            new(ScenarioType.PremiumDisk, "X1DREAM.BIN", null, null),
        ];

        [TestMethod]
        public void BattleFiles_HaveExpectedTables() {
            TestCase.Run(BattleTestCases, testCase => {
                var editor = new X1_FileEditor(testCase.Scenario, false);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.IsNotNull(editor.TreasureTable);
                Assert.IsNotNull(editor.BattlePointersTable);
                Assert.IsNull(editor.NpcTable);
                Assert.IsNull(editor.EnterTable);
                Assert.IsNull(editor.ArrowTable);

                Assert.AreEqual(testCase.ExpectedBattleCount, editor.BattleTables?.Count);

                if (testCase.MapLeader != null) {
                    Assert.IsNotNull(editor.BattleTables);
                    Assert.IsTrue(editor.BattleTables.ContainsKey((MapLeaderType) testCase.MapLeader));
                    var battle = editor.BattleTables[(MapLeaderType) testCase.MapLeader];

                    Assert.IsNotNull(battle.HeaderTable);
                    Assert.IsNotNull(battle.SlotTable);
                    Assert.IsNotNull(battle.SpawnZoneTable);
                    Assert.IsNotNull(battle.AITable);
                    Assert.IsNotNull(battle.CustomMovementTable);
                }

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNull(editor.WarpTable);
                    Assert.IsNull(editor.TileMovementTable);
                }
                else {
                    Assert.IsNotNull(editor.WarpTable);
                    Assert.IsNotNull(editor.TileMovementTable);
                }
            });
        }

        [TestMethod]
        public void TownFiles_HaveExpectedTables() {
            TestCase.Run(TownTestCases, testCase => {
                var editor = new X1_FileEditor(testCase.Scenario, false);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.IsNotNull(editor.TreasureTable);
                Assert.IsNull(editor.BattlePointersTable);
                Assert.IsNotNull(editor.NpcTable);
                Assert.IsNotNull(editor.EnterTable);

                Assert.IsNull(editor.BattleTables);

                Assert.IsNull(editor.TileMovementTable);

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNull(editor.WarpTable);
                    Assert.IsNull(editor.ArrowTable);
                }
                else {
                    Assert.IsNotNull(editor.WarpTable);
                    Assert.IsNotNull(editor.ArrowTable);
                }
            });
        }
    }
}
