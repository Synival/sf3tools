using CommonLib.Arrays;
using SF3.Models.Files.X002;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X002_FileTests {
        private class X002_TestCase : TestCase {
            public X002_TestCase(
                ScenarioType scenario,
                string filename,
                int expectedItems,
                int expectedSpells,
                int expectedWeaponSpells,
                int expectedLoads,
                int expectedLoadedOverrides)
            : base(scenario, filename) {
                ExpectedItems   = expectedItems;
                ExpectedSpells  = expectedSpells;
                ExpectedWeaponSpells = expectedWeaponSpells;
                ExpectedLoads   = expectedLoads;
                ExpectedLoadedOverrides = expectedLoadedOverrides;
            }

            public X002_File Create()
                => X002_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

            public int ExpectedItems { get; }
            public int ExpectedSpells { get; }
            public int ExpectedWeaponSpells { get; }
            public int ExpectedLoads { get; }
            public int ExpectedLoadedOverrides { get; }
        }

        private static readonly List<X002_TestCase> TestCases = [
            new(ScenarioType.Scenario1,   "X002.BIN", 256, 52, 21, 173,  8),
            new(ScenarioType.Scenario2,   "X002.BIN", 256, 61, 24, 206, 11),
            new(ScenarioType.Scenario3,   "X002.BIN", 300, 75, 31, 169,  9),
            new(ScenarioType.PremiumDisk, "X002.BIN", 300, 78, 31,  30,  0),
        ];

        [TestMethod]
        public void ItemTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.ItemTable;

                Assert.AreEqual(0, table[0].Price);
                Assert.AreEqual(0, table[0].WeaponEqType);
                Assert.AreEqual(0, table[0].UseSpell);
                Assert.AreEqual(0, table[0].Stat1Type);
                Assert.AreEqual(0, table[0].RangeMax);
                Assert.AreEqual(0, table[0].RangeMin);
                Assert.AreEqual(0, table[0].Attack);

                Assert.AreEqual(50, table[1].Price);
                Assert.AreEqual(10, table[1].WeaponEqType);
                Assert.AreEqual(0, table[1].UseSpell);
                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 20 : 0, table[1].Stat1Type);
                Assert.AreEqual(0x02, table[1].RangeMax);
                Assert.AreEqual(0x01, table[1].RangeMin);
                Assert.AreEqual(3, table[1].Attack);

                Assert.AreEqual(250, table[2].Price);
                Assert.AreEqual(10, table[2].WeaponEqType);
                Assert.AreEqual(0, table[2].UseSpell);
                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 20 : 0, table[2].Stat1Type);
                Assert.AreEqual(0x02, table[2].RangeMax);
                Assert.AreEqual(0x01, table[2].RangeMin);
                Assert.AreEqual(7, table[2].Attack);

                Assert.AreEqual(testCase.ExpectedItems, table.Length);
            });
        }

        [TestMethod]
        public void SpellTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.SpellTable;

                Assert.AreEqual(0, table[0].Element);
                Assert.AreEqual(0, table[0].Lv1Cost);
                Assert.AreEqual(0, table[0].Lv1Damage);
                Assert.AreEqual(0, table[0].Lv1Distance);
                Assert.AreEqual(1, table[0].Lv1Targets);

                Assert.AreEqual(1, table[1].Element);
                Assert.AreEqual(2, table[1].Lv1Cost);
                Assert.AreEqual(8, table[1].Lv1Damage);
                Assert.AreEqual(0x21, table[1].Lv1Distance);
                Assert.AreEqual(1, table[1].Lv1Targets);

                Assert.AreEqual(2, table[2].Element);
                Assert.AreEqual(3, table[2].Lv1Cost);
                Assert.AreEqual(10, table[2].Lv1Damage);
                Assert.AreEqual(0x21, table[2].Lv1Distance);
                Assert.AreEqual(1, table[2].Lv1Targets);

                Assert.AreEqual(testCase.ExpectedSpells, table.Length);
            });
        }

        [TestMethod]
        public void WeaponSpellTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.WeaponSpellTable;

                Assert.AreEqual(0, table[0].SpellID2);
                Assert.AreEqual(0, table[0].Weapon0);
                Assert.AreEqual(0, table[0].Weapon1);
                Assert.AreEqual(0, table[0].Weapon2);
                Assert.AreEqual(0, table[0].Weapon3);

                Assert.AreEqual(0x27, table[1].SpellID2);
                Assert.AreEqual(1, table[1].Weapon0);
                Assert.AreEqual(1, table[1].Weapon1);
                Assert.AreEqual(1, table[1].Weapon2);
                Assert.AreEqual(2, table[1].Weapon3);

                Assert.AreEqual(0x12, table[2].SpellID2);
                Assert.AreEqual(1, table[2].Weapon0);
                Assert.AreEqual(1, table[2].Weapon1);
                Assert.AreEqual(2, table[2].Weapon2);
                Assert.AreEqual(2, table[2].Weapon3);

                Assert.AreEqual(testCase.ExpectedWeaponSpells, table.Length);
            });
        }

        [TestMethod]
        public void LoadingTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.LoadingTable;

                Assert.AreEqual(1, table[0].SceneID);
                Assert.AreEqual(testCase.Scenario <= ScenarioType.Scenario2 ? 27 : 0, table[0].Music);

                Assert.AreEqual(2, table[1].SceneID);
                Assert.AreEqual(testCase.Scenario <= ScenarioType.Scenario2 ? 29 : 12, table[1].Music);

                Assert.AreEqual(3, table[2].SceneID);
                Assert.AreEqual(testCase.Scenario <= ScenarioType.Scenario2 ? 0 : 12, table[2].Music);

                Assert.AreEqual(testCase.ExpectedLoads, table.Length);
            });
        }

        [TestMethod]
        public void StatBoostTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.StatBoostTable;

                Assert.AreEqual(3, table[0].Stat);
                Assert.AreEqual(3, table[1].Stat);
                Assert.AreEqual(3, table[2].Stat);
                Assert.AreEqual(2, table[3].Stat);
                Assert.AreEqual(2, table[4].Stat);
                Assert.AreEqual(3, table[5].Stat);
                Assert.AreEqual(3, table[6].Stat);

                Assert.AreEqual(7, table.Length);
            });
        }

        [TestMethod]
        public void WeaponRankTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.WeaponRankTable;

                Assert.AreEqual(0, table[0].Skill0);
                Assert.AreEqual(3, table[1].Skill1);
                Assert.AreEqual(7, table[2].Skill2);
                Assert.AreEqual(15, table[3].Skill3);

                Assert.AreEqual(5, table.Length);
            });
        }

        [TestMethod]
        public void AttackResistTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var data = file.AttackResist;

                Assert.AreEqual(15, data.Attack);
                Assert.AreEqual(30, data.Resist);
            });
        }

        [TestMethod]
        public void LoadedOverrideTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.LoadingOverrideTable;

                switch (file.Scenario) {
                    case ScenarioType.Scenario1:
                        Assert.AreEqual(0, table[0].SynCHR);
                        Assert.AreEqual(0x1D, table[0].SynMPD);
                        Assert.AreEqual(0x20, table[0].SynMusic);
                        break;

                    case ScenarioType.Scenario2:
                        Assert.AreEqual(0, table[0].MedCHR);
                        Assert.AreEqual(0x1A, table[0].MedMPD);
                        Assert.AreEqual(0x20, table[0].MedMusic);
                        break;

                    case ScenarioType.Scenario3:
                        Assert.AreEqual(0, table[0].JulCHR);
                        Assert.AreEqual(0xE6, table[0].JulMPD);
                        Assert.AreEqual(0x3F, table[0].JulMusic);
                        break;

                    case ScenarioType.PremiumDisk:
                        // No LoadedOverrides. Assert here just to make this test future-proof.
                        Assert.AreEqual(0, table.Length);
                        break;
                }

                Assert.AreEqual(testCase.ExpectedLoadedOverrides, table.Length);
            });
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();
                var table = file.WarpTable;

                // Only Scenario1 has a warp table in X002.BIN
                if (testCase.Scenario != ScenarioType.Scenario1) {
                    Assert.IsNull(table);
                    return;
                }

                Assert.AreEqual(0, table[0].SceneID);
                Assert.AreEqual(0, table[0].IfFlagUnset);
                Assert.AreEqual(0, table[0].WarpTrigger);
                Assert.AreEqual(3, table[0].LoadID);

                Assert.AreEqual(1, table[1].SceneID);
                Assert.AreEqual(0xFFF, table[1].IfFlagUnset);
                Assert.AreEqual(1, table[1].WarpTrigger);
                Assert.AreEqual(0x02, table[1].LoadID);

                Assert.AreEqual(779, table.Length);
            });
        }
    }
}
