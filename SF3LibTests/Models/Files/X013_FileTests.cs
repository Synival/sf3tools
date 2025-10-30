using CommonLib.Arrays;
using CommonLib.Tests;
using SF3.Models.Files.X013;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X013_FileTests {
        private class X013_TestCase : SF3FileTestCase {
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
        public void SignificantValues_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var data = file.SignificantValues;

                Assert.AreEqual( 20, data.AttackAdvantageCritMod);
                Assert.AreEqual(-20, data.AttackDisadvantageCritMod);
                Assert.AreEqual( 49, data.MaxExpCheckedValue);
                Assert.AreEqual( 49, data.MaxExpReplacementValue);
                Assert.AreEqual(  0, data.FriendshipExp_Lvl0_Ally);
                Assert.AreEqual( 10, data.FriendshipExp_Lvl1_Partner);
                Assert.AreEqual( 20, data.FriendshipExp_Lvl2_Friend);
                Assert.AreEqual( 30, data.FriendshipExp_Lvl3_Trusted);
                Assert.AreEqual( 45, data.FriendshipExp_Lvl4_Soulmate);
                Assert.AreEqual( 10, data.HealBonusExp);
                Assert.AreEqual(-10, data.SoulmateFailExpMod);
                Assert.AreEqual( 59, data.SpecialChances2Specials2);
                Assert.AreEqual( 69, data.SpecialChances3Specials2);
                Assert.AreEqual( 39, data.SpecialChances3Specials3);
                Assert.AreEqual( 79, data.SpecialChances4Specials2);
                Assert.AreEqual( 59, data.SpecialChances4Specials3);
                Assert.AreEqual( 34, data.SpecialChances4Specials4);
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

                Assert.AreEqual(2, table.Length);
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

                Assert.AreEqual(testCase.ExpectedMagicBonuses, table.Length);
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

                Assert.AreEqual(1770, table.Length);
            });
        }

        [TestMethod]
        public void SpecialsTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SpecialsTable;

                Assert.AreEqual(0, table[0].Type);
                Assert.AreEqual(0, table[0].LowPow);
                Assert.AreEqual(0, table[0].MidPow);
                Assert.AreEqual(0, table[0].MaxPow);

                Assert.AreEqual(1, table[1].Type);
                Assert.AreEqual(3, table[1].LowPow);
                Assert.AreEqual(4, table[1].MidPow);
                Assert.AreEqual(5, table[1].MaxPow);

                Assert.AreEqual(2, table[2].Type);
                Assert.AreEqual(0, table[2].LowPow);
                Assert.AreEqual(0, table[2].MidPow);
                Assert.AreEqual(0, table[2].MaxPow);

                Assert.AreEqual(testCase.ExpectedSpecials, table.Length);
            });
        }

        [TestMethod]
        public void StatusEffectTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.StatusEffectTable;

                Assert.AreEqual(100, table[0].Luck0Chance);
                Assert.AreEqual(95, table[0].Luck1Chance);
                Assert.AreEqual(91, table[0].Luck2Chance);
                Assert.AreEqual(87, table[0].Luck3Chance);
                Assert.AreEqual(83, table[0].Luck4Chance);
                Assert.AreEqual(79, table[0].Luck5Chance);
                Assert.AreEqual(76, table[0].Luck6Chance);
                Assert.AreEqual(73, table[0].Luck7Chance);
                Assert.AreEqual(70, table[0].Luck8Chance);
                Assert.AreEqual(60, table[0].Luck9Chance);

                Assert.AreEqual(100, table[1].Luck0Chance);
                Assert.AreEqual(90, table[1].Luck1Chance);
                Assert.AreEqual(85, table[1].Luck2Chance);
                Assert.AreEqual(80, table[1].Luck3Chance);
                Assert.AreEqual(75, table[1].Luck4Chance);
                Assert.AreEqual(70, table[1].Luck5Chance);
                Assert.AreEqual(65, table[1].Luck6Chance);
                Assert.AreEqual(60, table[1].Luck7Chance);
                Assert.AreEqual(55, table[1].Luck8Chance);
                Assert.AreEqual(0, table[1].Luck9Chance);

                Assert.AreEqual(100, table[2].Luck0Chance);
                Assert.AreEqual(80, table[2].Luck1Chance);
                Assert.AreEqual(74, table[2].Luck2Chance);
                Assert.AreEqual(68, table[2].Luck3Chance);
                Assert.AreEqual(62, table[2].Luck4Chance);
                Assert.AreEqual(56, table[2].Luck5Chance);
                Assert.AreEqual(50, table[2].Luck6Chance);
                Assert.AreEqual(45, table[2].Luck7Chance);
                Assert.AreEqual(40, table[2].Luck8Chance);
                Assert.AreEqual(0, table[2].Luck9Chance);

                Assert.AreEqual(100, table[3].Luck0Chance);
                Assert.AreEqual(75, table[3].Luck1Chance);
                Assert.AreEqual(65, table[3].Luck2Chance);
                Assert.AreEqual(55, table[3].Luck3Chance);
                Assert.AreEqual(45, table[3].Luck4Chance);
                Assert.AreEqual(35, table[3].Luck5Chance);
                Assert.AreEqual(25, table[3].Luck6Chance);
                Assert.AreEqual(15, table[3].Luck7Chance);
                Assert.AreEqual(0, table[3].Luck8Chance);
                Assert.AreEqual(0, table[3].Luck9Chance);

                Assert.AreEqual(4, table.Length);
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

                Assert.AreEqual(8, table.Length);
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

                Assert.AreEqual(testCase.ExpectedSupportTypes, table.Length);
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

                Assert.AreEqual(4, table.Length);
            });
        }
    }
}
