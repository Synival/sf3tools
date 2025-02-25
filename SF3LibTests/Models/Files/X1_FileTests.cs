using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests {
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

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);

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
                var file = testCase.Create();

                Assert.IsNotNull(file.InteractableTable);
                Assert.IsNotNull(file.BattlePointersTable);
                Assert.IsNull(file.NpcTable);
                Assert.IsNull(file.EnterTable);
                Assert.IsNull(file.ArrowTable);

                Assert.AreEqual(testCase.ExpectedBattleCount, file.Battles?.Count);

                if (testCase.MapLeader != null) {
                    Assert.IsNotNull(file.Battles);
                    Assert.IsTrue(file.Battles.ContainsKey((MapLeaderType) testCase.MapLeader));
                    var battle = file.Battles[(MapLeaderType) testCase.MapLeader];

                    Assert.IsNotNull(battle.BattleHeader);
                    Assert.IsNotNull(battle.SlotTable);
                    Assert.IsNotNull(battle.SpawnZoneTable);
                    Assert.IsNotNull(battle.AITargetPositionTable);
                    Assert.IsNotNull(battle.ScriptedMovementTable);
                }

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNull(file.WarpTable);
                    Assert.IsNull(file.TileMovementTable);
                }
                else {
                    Assert.IsNotNull(file.WarpTable);
                    Assert.IsNotNull(file.TileMovementTable);
                }
            });
        }

        [TestMethod]
        public void TownFiles_HaveExpectedTables() {
            TestCase.Run(TownTestCases, testCase => {
                var file = testCase.Create();

                Assert.IsNotNull(file.InteractableTable);
                Assert.IsNull(file.BattlePointersTable);
                Assert.IsNotNull(file.NpcTable);
                Assert.IsNotNull(file.EnterTable);

                Assert.IsNull(file.Battles);

                Assert.IsNull(file.TileMovementTable);

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.IsNull(file.WarpTable);
                    Assert.IsNull(file.ArrowTable);
                }
                else {
                    Assert.IsNotNull(file.WarpTable);
                    Assert.IsNotNull(file.ArrowTable);
                }
            });
        }
    }
}
