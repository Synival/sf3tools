using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }
        }

        private static readonly List<X1_TestCase> BattleTestCases = [
            new(ScenarioType.Scenario1,   "X1BTL104.BIN"),
            new(ScenarioType.Scenario2,   "X1BTL201.BIN"),
            new(ScenarioType.Scenario3,   "X1BTL301.BIN"),
            new(ScenarioType.PremiumDisk, "X1BTLP01.BIN"),
        ];

        private static readonly List<X1_TestCase> TownTestCases = [
            new(ScenarioType.Scenario1,   "X1BAL_3.BIN"),
            new(ScenarioType.Scenario2,   "X1DUSTY.BIN"),
            new(ScenarioType.Scenario3,   "X1BEER.BIN"),
            new(ScenarioType.PremiumDisk, "X1DREAM.BIN"),
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

                Assert.IsNotNull(editor.HeaderTable);
                Assert.IsNotNull(editor.SlotTable);
                Assert.IsNotNull(editor.SpawnZoneTable);
                Assert.IsNotNull(editor.AITable);
                Assert.IsNotNull(editor.CustomMovementTable);

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

                Assert.IsNull(editor.HeaderTable);
                Assert.IsNull(editor.SlotTable);
                Assert.IsNull(editor.SpawnZoneTable);
                Assert.IsNull(editor.AITable);
                Assert.IsNull(editor.CustomMovementTable);

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
