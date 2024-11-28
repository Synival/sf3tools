using SF3.RawEditors;
using SF3.NamedValues;
using SF3.Types;
using SF3.Models.Files.X013;

namespace SF3.Tests.Editors {
    [TestClass]
    public class X013_EditorTests {
        private class X013_TestCase : TestCase {
            public X013_TestCase(
                ScenarioType scenario,
                string filename,
                int expectedMagicBonuses,
                int expectedSpecials,
                int expectedSupportTypes)
            : base(scenario, filename) {
                ExpectedMagicBonuses = expectedMagicBonuses;
                ExpectedSpecials = expectedSpecials;
                ExpectedSupportTypes = expectedSupportTypes;
            }

            public X013_Editor Create()
                => X013_Editor.Create(new ByteEditor(File.ReadAllBytes(Filename)), new NameGetterContext(Scenario), Scenario);

            public int ExpectedMagicBonuses { get; }
            public int ExpectedSpecials { get; }
            public int ExpectedSupportTypes { get; }
        }

        private static readonly List<X013_TestCase> TestCases = [
            new(ScenarioType.Scenario1,   "X013.BIN", 20, 188, 24),
            new(ScenarioType.Scenario2,   "X013.BIN", 32, 201, 60),
            new(ScenarioType.Scenario3,   "X013.BIN", 75, 256, 60),
            new(ScenarioType.PremiumDisk, "X013.BIN", 76, 256, 60),
        ];

        [TestMethod]
        public void CritModTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.CritModTable;

                Assert.AreEqual( 20, table.Rows[0].Advantage);
                Assert.AreEqual(236, table.Rows[0].Disadvantage);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void CritrateTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.CritrateTable;

                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 0 : 3, table.Rows[0].NoSpecial);
                Assert.AreEqual(20, table.Rows[0].OneSpecial);
                Assert.AreEqual(30, table.Rows[0].TwoSpecial);
                Assert.AreEqual(40, table.Rows[0].ThreeSpecial);
                Assert.AreEqual(50, table.Rows[0].FourSpecial);
                Assert.AreEqual(50, table.Rows[0].FiveSpecial);

                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 0 : 3, table.Rows[0].NoSpecial);
                Assert.AreEqual(20, table.Rows[1].OneSpecial);
                Assert.AreEqual(30, table.Rows[1].TwoSpecial);
                Assert.AreEqual(40, table.Rows[1].ThreeSpecial);
                Assert.AreEqual(50, table.Rows[1].FourSpecial);
                Assert.AreEqual( 0, table.Rows[1].FiveSpecial);

                Assert.AreEqual(2, table.Rows.Length);
            });
        }

        [TestMethod]
        public void ExpLimitTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.ExpLimitTable;

                Assert.AreEqual(49, table.Rows[0].ExpCheck);
                Assert.AreEqual(49, table.Rows[0].ExpReplacement);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void FriendshipExpTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.FriendshipExpTable;

                Assert.AreEqual( 0, table.Rows[0].SLvl0);
                Assert.AreEqual(10, table.Rows[0].SLvl1);
                Assert.AreEqual(20, table.Rows[0].SLvl2);
                Assert.AreEqual(30, table.Rows[0].SLvl3);
                Assert.AreEqual(45, table.Rows[0].SLvl4);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void HealExpTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.HealExpTable;

                Assert.AreEqual(10, table.Rows[0].HealBonus);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void MagicBonusTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.MagicBonusTable;

                Assert.AreEqual(0, table.Rows[0].EarthBonus);
                Assert.AreEqual(0, table.Rows[0].FireBonus);
                Assert.AreEqual(0, table.Rows[0].IceBonus);
                Assert.AreEqual(0, table.Rows[0].WindBonus);
                Assert.AreEqual(0, table.Rows[0].SparkBonus);
                Assert.AreEqual(0, table.Rows[0].LightBonus);
                Assert.AreEqual(0, table.Rows[0].DarkBonus);
                Assert.AreEqual(0, table.Rows[0].UnknownBonus);

                Assert.AreEqual(0, table.Rows[1].EarthBonus);
                Assert.AreEqual(0, table.Rows[1].FireBonus);
                Assert.AreEqual(0, table.Rows[1].IceBonus);
                Assert.AreEqual(0, table.Rows[1].WindBonus);
                Assert.AreEqual(1, table.Rows[1].SparkBonus);
                Assert.AreEqual(1, table.Rows[1].LightBonus);
                Assert.AreEqual(0, table.Rows[1].DarkBonus);
                Assert.AreEqual(0, table.Rows[1].UnknownBonus);

                Assert.AreEqual(0, table.Rows[2].EarthBonus);
                Assert.AreEqual(2, table.Rows[2].FireBonus);
                Assert.AreEqual(2, table.Rows[2].IceBonus);
                Assert.AreEqual(1, table.Rows[2].WindBonus);
                Assert.AreEqual(4, table.Rows[2].SparkBonus);
                Assert.AreEqual(3, table.Rows[2].LightBonus);
                Assert.AreEqual(2, table.Rows[2].DarkBonus);
                Assert.AreEqual(0, table.Rows[2].UnknownBonus);

                Assert.AreEqual(testCase.ExpectedMagicBonuses, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SoulfailTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SoulfailTable;

                Assert.AreEqual(246, table.Rows[0].ExpLost);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SoulmateTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SoulmateTable;

                Assert.AreEqual( 5, table.Rows[0].Chance);
                Assert.AreEqual(15, table.Rows[1].Chance);
                Assert.AreEqual(15, table.Rows[2].Chance);
                Assert.AreEqual(15, table.Rows[3].Chance);
                Assert.AreEqual(15, table.Rows[4].Chance);
                Assert.AreEqual( 5, table.Rows[5].Chance);

                Assert.AreEqual(1770, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SpecialChanceTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SpecialChanceTable;

                Assert.AreEqual(59, table.Rows[0].TwoSpecials2);
                Assert.AreEqual(69, table.Rows[0].ThreeSpecials2);
                Assert.AreEqual(39, table.Rows[0].ThreeSpecials3);
                Assert.AreEqual(79, table.Rows[0].FourSpecials2);
                Assert.AreEqual(59, table.Rows[0].FourSpecials3);
                Assert.AreEqual(34, table.Rows[0].FourSpecials4);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SpecialsTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SpecialsTable;

                Assert.AreEqual(0, table.Rows[0].Unknown1);
                Assert.AreEqual(0, table.Rows[0].DamageCalc);
                Assert.AreEqual(0, table.Rows[0].ExtraPow);
                Assert.AreEqual(0, table.Rows[0].Pow);

                Assert.AreEqual(1, table.Rows[1].Unknown1);
                Assert.AreEqual(3, table.Rows[1].DamageCalc);
                Assert.AreEqual(4, table.Rows[1].ExtraPow);
                Assert.AreEqual(5, table.Rows[1].Pow);

                Assert.AreEqual(2, table.Rows[2].Unknown1);
                Assert.AreEqual(0, table.Rows[2].DamageCalc);
                Assert.AreEqual(0, table.Rows[2].ExtraPow);
                Assert.AreEqual(0, table.Rows[2].Pow);

                Assert.AreEqual(testCase.ExpectedSpecials, table.Rows.Length);
            });
        }

        [TestMethod]
        public void StatusEffectTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.StatusEffectTable;

                Assert.AreEqual(100, table.Rows[0].StatusLuck0);
                Assert.AreEqual( 95, table.Rows[0].StatusLuck1);
                Assert.AreEqual( 91, table.Rows[0].StatusLuck2);
                Assert.AreEqual( 87, table.Rows[0].StatusLuck3);
                Assert.AreEqual( 83, table.Rows[0].StatusLuck4);
                Assert.AreEqual( 79, table.Rows[0].StatusLuck5);
                Assert.AreEqual( 76, table.Rows[0].StatusLuck6);
                Assert.AreEqual( 73, table.Rows[0].StatusLuck7);
                Assert.AreEqual( 70, table.Rows[0].StatusLuck8);
                Assert.AreEqual( 60, table.Rows[0].StatusLuck9);
 
                Assert.AreEqual(100, table.Rows[1].StatusLuck0);
                Assert.AreEqual( 90, table.Rows[1].StatusLuck1);
                Assert.AreEqual( 85, table.Rows[1].StatusLuck2);
                Assert.AreEqual( 80, table.Rows[1].StatusLuck3);
                Assert.AreEqual( 75, table.Rows[1].StatusLuck4);
                Assert.AreEqual( 70, table.Rows[1].StatusLuck5);
                Assert.AreEqual( 65, table.Rows[1].StatusLuck6);
                Assert.AreEqual( 60, table.Rows[1].StatusLuck7);
                Assert.AreEqual( 55, table.Rows[1].StatusLuck8);
                Assert.AreEqual(  0, table.Rows[1].StatusLuck9);

                Assert.AreEqual(100, table.Rows[2].StatusLuck0);
                Assert.AreEqual( 80, table.Rows[2].StatusLuck1);
                Assert.AreEqual( 74, table.Rows[2].StatusLuck2);
                Assert.AreEqual( 68, table.Rows[2].StatusLuck3);
                Assert.AreEqual( 62, table.Rows[2].StatusLuck4);
                Assert.AreEqual( 56, table.Rows[2].StatusLuck5);
                Assert.AreEqual( 50, table.Rows[2].StatusLuck6);
                Assert.AreEqual( 45, table.Rows[2].StatusLuck7);
                Assert.AreEqual( 40, table.Rows[2].StatusLuck8);
                Assert.AreEqual(  0, table.Rows[2].StatusLuck9);

                Assert.AreEqual(100, table.Rows[3].StatusLuck0);
                Assert.AreEqual( 75, table.Rows[3].StatusLuck1);
                Assert.AreEqual( 65, table.Rows[3].StatusLuck2);
                Assert.AreEqual( 55, table.Rows[3].StatusLuck3);
                Assert.AreEqual( 45, table.Rows[3].StatusLuck4);
                Assert.AreEqual( 35, table.Rows[3].StatusLuck5);
                Assert.AreEqual( 25, table.Rows[3].StatusLuck6);
                Assert.AreEqual( 15, table.Rows[3].StatusLuck7);
                Assert.AreEqual(  0, table.Rows[3].StatusLuck8);
                Assert.AreEqual(  0, table.Rows[3].StatusLuck9);

                Assert.AreEqual(4, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SupportStatsTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SupportStatsTable;

                Assert.AreEqual( 5, table.Rows[0].SLvlStat1);
                Assert.AreEqual( 7, table.Rows[0].SLvlStat2);
                Assert.AreEqual(10, table.Rows[0].SLvlStat3);
                Assert.AreEqual(15, table.Rows[0].SLvlStat4);

                Assert.AreEqual(15, table.Rows[1].SLvlStat1);
                Assert.AreEqual(17, table.Rows[1].SLvlStat2);
                Assert.AreEqual(20, table.Rows[1].SLvlStat3);
                Assert.AreEqual(25, table.Rows[1].SLvlStat4);

                Assert.AreEqual(8, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SupportTypeTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SupportTypeTable;

                Assert.AreEqual(1, table.Rows[0].SupportA);
                Assert.AreEqual(0, table.Rows[0].SupportB);

                Assert.AreEqual(8, table.Rows[1].SupportA);
                Assert.AreEqual(0, table.Rows[1].SupportB);

                Assert.AreEqual(3, table.Rows[2].SupportA);
                Assert.AreEqual(0, table.Rows[2].SupportB);

                Assert.AreEqual(testCase.ExpectedSupportTypes, table.Rows.Length);
            });
        }

        [TestMethod]
        public void WeaponSpellRankTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.WeaponSpellRankTable;

                Assert.AreEqual(0, table.Rows[0].RankNone);
                Assert.AreEqual(0, table.Rows[0].RankC);
                Assert.AreEqual(0, table.Rows[0].RankB);
                Assert.AreEqual(0, table.Rows[0].RankA);
                Assert.AreEqual(0, table.Rows[0].RankS);

                Assert.AreEqual(0, table.Rows[1].RankNone);
                Assert.AreEqual(2, table.Rows[1].RankC);
                Assert.AreEqual(3, table.Rows[1].RankB);
                Assert.AreEqual(3, table.Rows[1].RankA);
                Assert.AreEqual(4, table.Rows[1].RankS);

                Assert.AreEqual(4, table.Rows.Length);
            });
        }
    }
}
