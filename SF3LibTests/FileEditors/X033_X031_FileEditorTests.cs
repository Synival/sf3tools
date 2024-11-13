using SF3.RawEditors;
using SF3.Editors;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X033_X031_FileEditorTests {
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

            public X033_X031_Editor Create()
                => X033_X031_Editor.Create(new ByteEditor(File.ReadAllBytes(Filename)), new NameGetterContext(Scenario), Scenario);

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
                var editor = testCase.Create();

                Assert.AreEqual(0x00, editor.StatsTable.Rows[0].CharacterClass);
                Assert.AreEqual(0x00, editor.StatsTable.Rows[0].CharacterID);
                Assert.AreEqual(12, editor.StatsTable.Rows[0].HPCurve1);
                // TODO: maybe more data?

                Assert.AreEqual(0x01, editor.StatsTable.Rows[1].CharacterClass);
                Assert.AreEqual(0x01, editor.StatsTable.Rows[1].CharacterID);
                Assert.AreEqual(13, editor.StatsTable.Rows[1].HPCurve1);
                // TODO: maybe more data?

                Assert.AreEqual(0x04, editor.StatsTable.Rows[2].CharacterClass);
                Assert.AreEqual(0x02, editor.StatsTable.Rows[2].CharacterID);
                Assert.AreEqual(9, editor.StatsTable.Rows[2].HPCurve1);
                // TODO: maybe more data?

                Assert.AreEqual(testCase.ExpectedStatsRows, editor.StatsTable.Rows.Length);
            });
        }

        [TestMethod]
        public void InitialInfoTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();

                if (testCase.Scenario == ScenarioType.Scenario1) {
                    Assert.AreEqual(0x00, editor.InitialInfoTable.Rows[0].CharacterE);
                    Assert.AreEqual(0x00, editor.InitialInfoTable.Rows[0].CharacterClassE);
                    Assert.AreEqual(1, editor.InitialInfoTable.Rows[0].Level);
                    // TODO: maybe more data?

                    Assert.AreEqual(0x01, editor.InitialInfoTable.Rows[1].CharacterE);
                    Assert.AreEqual(0x01, editor.InitialInfoTable.Rows[1].CharacterClassE);
                    Assert.AreEqual(1, editor.InitialInfoTable.Rows[1].Level);
                    // TODO: maybe more data?
                }
                else {
                    Assert.AreEqual(0x00, editor.InitialInfoTable.Rows[0].CharacterE);
                    Assert.AreEqual(0x20, editor.InitialInfoTable.Rows[0].CharacterClassE);
                    Assert.AreEqual(15, editor.InitialInfoTable.Rows[0].Level);
                    // TODO: maybe more data?

                    Assert.AreEqual(0x01, editor.InitialInfoTable.Rows[1].CharacterE);
                    Assert.AreEqual(0x21, editor.InitialInfoTable.Rows[1].CharacterClassE);
                    Assert.AreEqual(15, editor.InitialInfoTable.Rows[1].Level);
                    // TODO: maybe more data?
                }

                Assert.AreEqual(testCase.ExpectedInitialInfoRows, editor.InitialInfoTable.Rows.Length);
            });
        }

        [TestMethod]
        public void WeaponLevelTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();

                Assert.AreEqual(  70, editor.WeaponLevelTable.Rows[0].WLevel1);
                Assert.AreEqual( 150, editor.WeaponLevelTable.Rows[0].WLevel2);
                Assert.AreEqual( 250, editor.WeaponLevelTable.Rows[0].WLevel3);
                Assert.AreEqual(9999, editor.WeaponLevelTable.Rows[0].WLevel4);

                Assert.AreEqual(1, editor.WeaponLevelTable.Rows.Length);
            });
        }
    }
}
