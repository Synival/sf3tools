using CommonLib.Arrays;
using SF3.Models.Files.X013;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X013_FileTests {
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

            public X013_File Create()
                => X013_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

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
                var file = testCase.Create();
                var table = file.CritModTable;

                Assert.AreEqual(20, table[0].Advantage);
                Assert.AreEqual(236, table[0].Disadvantage);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void CritrateTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.CritrateTable;

                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 0 : 3, table[0].NoSpecial);
                Assert.AreEqual(20, table[0].OneSpecial);
                Assert.AreEqual(30, table[0].TwoSpecial);
                Assert.AreEqual(40, table[0].ThreeSpecial);
                Assert.AreEqual(50, table[0].FourSpecial);
                Assert.AreEqual(50, table[0].FiveSpecial);

                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 0 : 3, table[0].NoSpecial);
                Assert.AreEqual(20, table[1].OneSpecial);
                Assert.AreEqual(30, table[1].TwoSpecial);
                Assert.AreEqual(40, table[1].ThreeSpecial);
                Assert.AreEqual(50, table[1].FourSpecial);
                Assert.AreEqual(0, table[1].FiveSpecial);

                Assert.AreEqual(2, table.Rows.Length);
            });
        }

        [TestMethod]
        public void ExpLimitTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.ExpLimitTable;

                Assert.AreEqual(49, table[0].ExpCheck);
                Assert.AreEqual(49, table[0].ExpReplacement);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void FriendshipExpTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.FriendshipExpTable;

                Assert.AreEqual(0, table[0].SLvl0);
                Assert.AreEqual(10, table[0].SLvl1);
                Assert.AreEqual(20, table[0].SLvl2);
                Assert.AreEqual(30, table[0].SLvl3);
                Assert.AreEqual(45, table[0].SLvl4);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void HealExpTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.HealExpTable;

                Assert.AreEqual(10, table[0].HealBonus);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void MagicBonusTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.MagicBonusTable;

                Assert.AreEqual(0, table[0].EarthBonus);
                Assert.AreEqual(0, table[0].FireBonus);
                Assert.AreEqual(0, table[0].IceBonus);
                Assert.AreEqual(0, table[0].WindBonus);
                Assert.AreEqual(0, table[0].SparkBonus);
                Assert.AreEqual(0, table[0].LightBonus);
                Assert.AreEqual(0, table[0].DarkBonus);
                Assert.AreEqual(0, table[0].UnknownBonus);

                Assert.AreEqual(0, table[1].EarthBonus);
                Assert.AreEqual(0, table[1].FireBonus);
                Assert.AreEqual(0, table[1].IceBonus);
                Assert.AreEqual(0, table[1].WindBonus);
                Assert.AreEqual(1, table[1].SparkBonus);
                Assert.AreEqual(1, table[1].LightBonus);
                Assert.AreEqual(0, table[1].DarkBonus);
                Assert.AreEqual(0, table[1].UnknownBonus);

                Assert.AreEqual(0, table[2].EarthBonus);
                Assert.AreEqual(2, table[2].FireBonus);
                Assert.AreEqual(2, table[2].IceBonus);
                Assert.AreEqual(1, table[2].WindBonus);
                Assert.AreEqual(4, table[2].SparkBonus);
                Assert.AreEqual(3, table[2].LightBonus);
                Assert.AreEqual(2, table[2].DarkBonus);
                Assert.AreEqual(0, table[2].UnknownBonus);

                Assert.AreEqual(testCase.ExpectedMagicBonuses, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SoulfailTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SoulfailTable;

                Assert.AreEqual(246, table[0].ExpLost);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SoulmateTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SoulmateTable;

                Assert.AreEqual(5, table[0].Chance);
                Assert.AreEqual(15, table[1].Chance);
                Assert.AreEqual(15, table[2].Chance);
                Assert.AreEqual(15, table[3].Chance);
                Assert.AreEqual(15, table[4].Chance);
                Assert.AreEqual(5, table[5].Chance);

                Assert.AreEqual(1770, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SpecialChanceTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SpecialChanceTable;

                Assert.AreEqual(59, table[0].TwoSpecials2);
                Assert.AreEqual(69, table[0].ThreeSpecials2);
                Assert.AreEqual(39, table[0].ThreeSpecials3);
                Assert.AreEqual(79, table[0].FourSpecials2);
                Assert.AreEqual(59, table[0].FourSpecials3);
                Assert.AreEqual(34, table[0].FourSpecials4);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SpecialsTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SpecialsTable;

                Assert.AreEqual(0, table[0].Unknown1);
                Assert.AreEqual(0, table[0].DamageCalc);
                Assert.AreEqual(0, table[0].ExtraPow);
                Assert.AreEqual(0, table[0].Pow);

                Assert.AreEqual(1, table[1].Unknown1);
                Assert.AreEqual(3, table[1].DamageCalc);
                Assert.AreEqual(4, table[1].ExtraPow);
                Assert.AreEqual(5, table[1].Pow);

                Assert.AreEqual(2, table[2].Unknown1);
                Assert.AreEqual(0, table[2].DamageCalc);
                Assert.AreEqual(0, table[2].ExtraPow);
                Assert.AreEqual(0, table[2].Pow);

                Assert.AreEqual(testCase.ExpectedSpecials, table.Rows.Length);
            });
        }

        [TestMethod]
        public void StatusEffectTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.StatusEffectTable;

                Assert.AreEqual(100, table[0].StatusLuck0);
                Assert.AreEqual(95, table[0].StatusLuck1);
                Assert.AreEqual(91, table[0].StatusLuck2);
                Assert.AreEqual(87, table[0].StatusLuck3);
                Assert.AreEqual(83, table[0].StatusLuck4);
                Assert.AreEqual(79, table[0].StatusLuck5);
                Assert.AreEqual(76, table[0].StatusLuck6);
                Assert.AreEqual(73, table[0].StatusLuck7);
                Assert.AreEqual(70, table[0].StatusLuck8);
                Assert.AreEqual(60, table[0].StatusLuck9);

                Assert.AreEqual(100, table[1].StatusLuck0);
                Assert.AreEqual(90, table[1].StatusLuck1);
                Assert.AreEqual(85, table[1].StatusLuck2);
                Assert.AreEqual(80, table[1].StatusLuck3);
                Assert.AreEqual(75, table[1].StatusLuck4);
                Assert.AreEqual(70, table[1].StatusLuck5);
                Assert.AreEqual(65, table[1].StatusLuck6);
                Assert.AreEqual(60, table[1].StatusLuck7);
                Assert.AreEqual(55, table[1].StatusLuck8);
                Assert.AreEqual(0, table[1].StatusLuck9);

                Assert.AreEqual(100, table[2].StatusLuck0);
                Assert.AreEqual(80, table[2].StatusLuck1);
                Assert.AreEqual(74, table[2].StatusLuck2);
                Assert.AreEqual(68, table[2].StatusLuck3);
                Assert.AreEqual(62, table[2].StatusLuck4);
                Assert.AreEqual(56, table[2].StatusLuck5);
                Assert.AreEqual(50, table[2].StatusLuck6);
                Assert.AreEqual(45, table[2].StatusLuck7);
                Assert.AreEqual(40, table[2].StatusLuck8);
                Assert.AreEqual(0, table[2].StatusLuck9);

                Assert.AreEqual(100, table[3].StatusLuck0);
                Assert.AreEqual(75, table[3].StatusLuck1);
                Assert.AreEqual(65, table[3].StatusLuck2);
                Assert.AreEqual(55, table[3].StatusLuck3);
                Assert.AreEqual(45, table[3].StatusLuck4);
                Assert.AreEqual(35, table[3].StatusLuck5);
                Assert.AreEqual(25, table[3].StatusLuck6);
                Assert.AreEqual(15, table[3].StatusLuck7);
                Assert.AreEqual(0, table[3].StatusLuck8);
                Assert.AreEqual(0, table[3].StatusLuck9);

                Assert.AreEqual(4, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SupportStatsTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SupportStatsTable;

                Assert.AreEqual(5, table[0].SLvlStat1);
                Assert.AreEqual(7, table[0].SLvlStat2);
                Assert.AreEqual(10, table[0].SLvlStat3);
                Assert.AreEqual(15, table[0].SLvlStat4);

                Assert.AreEqual(15, table[1].SLvlStat1);
                Assert.AreEqual(17, table[1].SLvlStat2);
                Assert.AreEqual(20, table[1].SLvlStat3);
                Assert.AreEqual(25, table[1].SLvlStat4);

                Assert.AreEqual(8, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SupportTypeTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SupportTypeTable;

                Assert.AreEqual(1, table[0].SupportA);
                Assert.AreEqual(0, table[0].SupportB);

                Assert.AreEqual(8, table[1].SupportA);
                Assert.AreEqual(0, table[1].SupportB);

                Assert.AreEqual(3, table[2].SupportA);
                Assert.AreEqual(0, table[2].SupportB);

                Assert.AreEqual(testCase.ExpectedSupportTypes, table.Rows.Length);
            });
        }

        [TestMethod]
        public void WeaponSpellRankTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.WeaponSpellRankTable;

                Assert.AreEqual(0, table[0].RankNone);
                Assert.AreEqual(0, table[0].RankC);
                Assert.AreEqual(0, table[0].RankB);
                Assert.AreEqual(0, table[0].RankA);
                Assert.AreEqual(0, table[0].RankS);

                Assert.AreEqual(0, table[1].RankNone);
                Assert.AreEqual(2, table[1].RankC);
                Assert.AreEqual(3, table[1].RankB);
                Assert.AreEqual(3, table[1].RankA);
                Assert.AreEqual(4, table[1].RankS);

                Assert.AreEqual(4, table.Rows.Length);
            });
        }
    }
}
