using CommonLib.Arrays;
using CommonLib.Tests;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests {
        private class X1_TestCase : SF3FileTestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename,
                MapLeaderType? mapLeader,
                int? expectedNpcTableCount,
                int? expectedBattleCount)
            : base(scenario, filename) {
                MapLeader = mapLeader;
                ExpectedNPCTableCount = expectedNpcTableCount;
                ExpectedBattleCount   = expectedBattleCount;
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);

            public MapLeaderType? MapLeader { get; }
            public int? ExpectedNPCTableCount { get; }
            public int? ExpectedBattleCount { get; }
        }

        private static readonly List<X1_TestCase> BattleTestCases = [
            new(ScenarioType.Scenario1,   "X1BTL104.BIN", MapLeaderType.Synbios, 2, 1),
            new(ScenarioType.Scenario2,   "X1BTL201.BIN", MapLeaderType.Medion,  0, 1),
            new(ScenarioType.Scenario3,   "X1BTL301.BIN", MapLeaderType.Julian,  0, 1),
            new(ScenarioType.PremiumDisk, "X1BTLP01.BIN", MapLeaderType.Synbios, 1, 1),
        ];

        private static readonly List<X1_TestCase> TownTestCases = [
            new(ScenarioType.Scenario1,   "X1BAL_3.BIN", null, 1, null),
            new(ScenarioType.Scenario2,   "X1DUSTY.BIN", null, 1, null),
            new(ScenarioType.Scenario3,   "X1BEER.BIN",  null, 1, null),
            new(ScenarioType.PremiumDisk, "X1DREAM.BIN", null, 1, null),
        ];

        [TestMethod]
        public void BattleFiles_HaveExpectedTables() {
            TestCase.Run(BattleTestCases, testCase => {
                var file = testCase.Create();

                Assert.IsTrue(file.InteractableTables.Any());
                Assert.IsNotNull(file.BattleHeader);
                Assert.IsNotNull(file.BattleHeader.BattleMapPointerTable);
                Assert.AreEqual(testCase.ExpectedNPCTableCount, file.NpcTables.Count());
                Assert.IsNull(file.EnterTable);
                Assert.IsNull(file.ArrowTable);

                var battles = file.GetBattleMaps();
                Assert.AreEqual(testCase.ExpectedBattleCount, battles.Count);

                if (testCase.MapLeader != null) {
                    Assert.IsTrue(battles.ContainsKey((MapLeaderType) testCase.MapLeader));
                    var battle = battles[(MapLeaderType) testCase.MapLeader];

                    Assert.IsNotNull(battle.Header);
                    Assert.IsNotNull(battle.SlotTable);
                    Assert.IsNotNull(battle.ZoneTable);
                    Assert.IsNotNull(battle.AITargetPositionTable);
                    Assert.IsNotNull(battle.AITarrgetPathTable);
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

                Assert.IsTrue(file.InteractableTables.Any());
                Assert.IsNull(file.BattleHeader);
                Assert.AreEqual(testCase.ExpectedNPCTableCount, file.NpcTables.Count());
                Assert.IsNotNull(file.EnterTable);

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
