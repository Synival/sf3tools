using CommonLib.Arrays;
using CommonLib.Tests;
using SF3.Models.Files.X031;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X031_FileTests {
        private class X031_TestCase : SF3FileTestCase {
            public X031_TestCase(
                ScenarioType scenario,
                string filename,
                int expectedStatsRows,
                int expectedInitialInfoRows
            )
            : base(scenario, filename) {
                ExpectedStatsRows = expectedStatsRows;
                ExpectedInitialInfoRows = expectedInitialInfoRows;
            }

            public X031_File Create()
                => X031_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

            public int ExpectedStatsRows { get; }
            public int ExpectedInitialInfoRows { get; }
        }

        private static readonly List<X031_TestCase> TestCases = [
            new(ScenarioType.Scenario1,   "X031.BIN", 33, 21),
            new(ScenarioType.Scenario2,   "X031.BIN", 66, 40),
            new(ScenarioType.Scenario3,   "X031.BIN", 143, 59),
            new(ScenarioType.PremiumDisk, "X031.BIN", 144, 60),
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

                Assert.AreEqual(testCase.ExpectedStatsRows, file.StatsTable.Length);
            });
        }

        [TestMethod]
        public void InitialInfoTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.AreEqual(0x00, file.InitialInfoTable[0].CharacterID);
                    Assert.AreEqual(0x00, file.InitialInfoTable[0].CharacterClass);
                    Assert.AreEqual(1, file.InitialInfoTable[0].Level);
                    // TODO: maybe more data?

                    Assert.AreEqual(0x01, file.InitialInfoTable[1].CharacterID);
                    Assert.AreEqual(0x01, file.InitialInfoTable[1].CharacterClass);
                    Assert.AreEqual(1, file.InitialInfoTable[1].Level);
                    // TODO: maybe more data?
                }
                else {
                    Assert.AreEqual(0x00, file.InitialInfoTable[0].CharacterID);
                    Assert.AreEqual(0x20, file.InitialInfoTable[0].CharacterClass);
                    Assert.AreEqual(15, file.InitialInfoTable[0].Level);
                    // TODO: maybe more data?

                    Assert.AreEqual(0x01, file.InitialInfoTable[1].CharacterID);
                    Assert.AreEqual(0x21, file.InitialInfoTable[1].CharacterClass);
                    Assert.AreEqual(15, file.InitialInfoTable[1].Level);
                    // TODO: maybe more data?
                }

                Assert.AreEqual(testCase.ExpectedInitialInfoRows, file.InitialInfoTable.Length);
            });
        }

        [TestMethod]
        public void WeaponLevelTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                var data = file.WeaponLevelExp;
                Assert.AreEqual(  70, data.WLevel1);
                Assert.AreEqual( 150, data.WLevel2);
                Assert.AreEqual( 250, data.WLevel3);
                Assert.AreEqual(9999, data.WLevel4);
            });
        }
    }
}
