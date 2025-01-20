using CommonLib.Arrays;
using SF3.Models.Files.X033_X031;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X033_X031_FileTests {
        private class X033_X031_TestCase : TestCase {
            public X033_X031_TestCase(
                ScenarioType scenario,
                string filename,
                int expectedStatsRows,
                int expectedInitialInfoRows
            )
            : base(scenario, filename) {
                ExpectedStatsRows = expectedStatsRows;
                ExpectedInitialInfoRows = expectedInitialInfoRows;
            }

            public X033_X031_File Create()
                => X033_X031_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

            public int ExpectedStatsRows { get; }
            public int ExpectedInitialInfoRows { get; }
        }

        private static readonly List<X033_X031_TestCase> TestCases = [
            new(ScenarioType.Scenario1, "X033.BIN", 33, 22),
            new(ScenarioType.Scenario1, "X031.BIN", 33, 22),
            new(ScenarioType.Scenario2, "X033.BIN", 66, 41),
            new(ScenarioType.Scenario2, "X031.BIN", 66, 41),
            new(ScenarioType.Scenario3, "X033.BIN", 143, 60),
            new(ScenarioType.Scenario3, "X031.BIN", 143, 60),
            new(ScenarioType.PremiumDisk, "X033.BIN", 144, 61),
            new(ScenarioType.PremiumDisk, "X031.BIN", 144, 61),
        ];

        [TestMethod]
        public void StatsTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                Assert.AreEqual(0x00, file.StatsTable[0].CharacterClass);
                Assert.AreEqual(0x00, file.StatsTable[0].CharacterID);
                Assert.AreEqual(12, file.StatsTable[0].HPCurve1);
                // TODO: maybe more data?

                Assert.AreEqual(0x01, file.StatsTable[1].CharacterClass);
                Assert.AreEqual(0x01, file.StatsTable[1].CharacterID);
                Assert.AreEqual(13, file.StatsTable[1].HPCurve1);
                // TODO: maybe more data?

                Assert.AreEqual(0x04, file.StatsTable[2].CharacterClass);
                Assert.AreEqual(0x02, file.StatsTable[2].CharacterID);
                Assert.AreEqual(9, file.StatsTable[2].HPCurve1);
                // TODO: maybe more data?

                Assert.AreEqual(testCase.ExpectedStatsRows, file.StatsTable.Rows.Length);
            });
        }

        [TestMethod]
        public void InitialInfoTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.AreEqual(0x00, file.InitialInfoTable[0].CharacterE);
                    Assert.AreEqual(0x00, file.InitialInfoTable[0].CharacterClassE);
                    Assert.AreEqual(1, file.InitialInfoTable[0].Level);
                    // TODO: maybe more data?

                    Assert.AreEqual(0x01, file.InitialInfoTable[1].CharacterE);
                    Assert.AreEqual(0x01, file.InitialInfoTable[1].CharacterClassE);
                    Assert.AreEqual(1, file.InitialInfoTable[1].Level);
                    // TODO: maybe more data?
                }
                else {
                    Assert.AreEqual(0x00, file.InitialInfoTable[0].CharacterE);
                    Assert.AreEqual(0x20, file.InitialInfoTable[0].CharacterClassE);
                    Assert.AreEqual(15, file.InitialInfoTable[0].Level);
                    // TODO: maybe more data?

                    Assert.AreEqual(0x01, file.InitialInfoTable[1].CharacterE);
                    Assert.AreEqual(0x21, file.InitialInfoTable[1].CharacterClassE);
                    Assert.AreEqual(15, file.InitialInfoTable[1].Level);
                    // TODO: maybe more data?
                }

                Assert.AreEqual(testCase.ExpectedInitialInfoRows, file.InitialInfoTable.Rows.Length);
            });
        }

        [TestMethod]
        public void WeaponLevelTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                Assert.AreEqual(70, file.WeaponLevelTable[0].WLevel1);
                Assert.AreEqual(150, file.WeaponLevelTable[0].WLevel2);
                Assert.AreEqual(250, file.WeaponLevelTable[0].WLevel3);
                Assert.AreEqual(9999, file.WeaponLevelTable[0].WLevel4);

                Assert.AreEqual(1, file.WeaponLevelTable.Rows.Length);
            });
        }
    }
}
