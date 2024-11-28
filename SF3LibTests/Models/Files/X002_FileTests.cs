using SF3.RawEditors;
using SF3.NamedValues;
using SF3.Types;
using SF3.Models.Files.X002;

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
                => X002_File.Create(new ByteEditor(File.ReadAllBytes(Filename)), new NameGetterContext(Scenario), Scenario);

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
                var editor = testCase.Create();
                var table = editor.ItemTable;

                Assert.AreEqual(0, table.Rows[0].Price);
                Assert.AreEqual(0, table.Rows[0].WeaponType);
                Assert.AreEqual(0, table.Rows[0].SpellUse);
                Assert.AreEqual(0, table.Rows[0].StatType1);
                Assert.AreEqual(0, table.Rows[0].Range);
                Assert.AreEqual(0, table.Rows[0].Attack);

                Assert.AreEqual(50, table.Rows[1].Price);
                Assert.AreEqual(10, table.Rows[1].WeaponType);
                Assert.AreEqual(0, table.Rows[1].SpellUse);
                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 20 : 0, table.Rows[1].StatType1);
                Assert.AreEqual(0x21, table.Rows[1].Range);
                Assert.AreEqual(3, table.Rows[1].Attack);

                Assert.AreEqual(250, table.Rows[2].Price);
                Assert.AreEqual(10, table.Rows[2].WeaponType);
                Assert.AreEqual(0, table.Rows[2].SpellUse);
                Assert.AreEqual(testCase.Scenario == ScenarioType.Scenario1 ? 20 : 0, table.Rows[2].StatType1);
                Assert.AreEqual(0x21, table.Rows[2].Range);
                Assert.AreEqual(7, table.Rows[2].Attack);

                Assert.AreEqual(testCase.ExpectedItems, table.Rows.Length);
            });
        }

        [TestMethod]
        public void SpellTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.SpellTable;

                Assert.AreEqual(0, table.Rows[0].Element);
                Assert.AreEqual(0, table.Rows[0].Lv1Cost);
                Assert.AreEqual(0, table.Rows[0].Lv1Damage);
                Assert.AreEqual(0, table.Rows[0].Lv1Distance);
                Assert.AreEqual(1, table.Rows[0].Lv1Targets);

                Assert.AreEqual(1, table.Rows[1].Element);
                Assert.AreEqual(2, table.Rows[1].Lv1Cost);
                Assert.AreEqual(8, table.Rows[1].Lv1Damage);
                Assert.AreEqual(0x21, table.Rows[1].Lv1Distance);
                Assert.AreEqual(1, table.Rows[1].Lv1Targets);

                Assert.AreEqual(2, table.Rows[2].Element);
                Assert.AreEqual(3, table.Rows[2].Lv1Cost);
                Assert.AreEqual(10, table.Rows[2].Lv1Damage);
                Assert.AreEqual(0x21, table.Rows[2].Lv1Distance);
                Assert.AreEqual(1, table.Rows[2].Lv1Targets);

                Assert.AreEqual(testCase.ExpectedSpells, table.Rows.Length);
            });
        }

        [TestMethod]
        public void WeaponSpellTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.WeaponSpellTable;

                Assert.AreEqual(0, table.Rows[0].SpellID2);
                Assert.AreEqual(0, table.Rows[0].Weapon0);
                Assert.AreEqual(0, table.Rows[0].Weapon1);
                Assert.AreEqual(0, table.Rows[0].Weapon2);
                Assert.AreEqual(0, table.Rows[0].Weapon3);

                Assert.AreEqual(0x27, table.Rows[1].SpellID2);
                Assert.AreEqual(1, table.Rows[1].Weapon0);
                Assert.AreEqual(1, table.Rows[1].Weapon1);
                Assert.AreEqual(1, table.Rows[1].Weapon2);
                Assert.AreEqual(2, table.Rows[1].Weapon3);

                Assert.AreEqual(0x12, table.Rows[2].SpellID2);
                Assert.AreEqual(1, table.Rows[2].Weapon0);
                Assert.AreEqual(1, table.Rows[2].Weapon1);
                Assert.AreEqual(2, table.Rows[2].Weapon2);
                Assert.AreEqual(2, table.Rows[2].Weapon3);

                Assert.AreEqual(testCase.ExpectedWeaponSpells, table.Rows.Length);
            });
        }

        [TestMethod]
        public void LoadingTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.LoadingTable;

                Assert.AreEqual(1, table.Rows[0].LocationID);
                Assert.AreEqual(testCase.Scenario <= ScenarioType.Scenario2 ? 27 : 0, table.Rows[0].Music);

                Assert.AreEqual(2, table.Rows[1].LocationID);
                Assert.AreEqual(testCase.Scenario <= ScenarioType.Scenario2 ? 29 : 12, table.Rows[1].Music);

                Assert.AreEqual(3, table.Rows[2].LocationID);
                Assert.AreEqual(testCase.Scenario <= ScenarioType.Scenario2 ? 0 : 12, table.Rows[2].Music);

                Assert.AreEqual(testCase.ExpectedLoads, table.Rows.Length);
            });
        }

        [TestMethod]
        public void StatBoostTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.StatBoostTable;

                Assert.AreEqual(3, table.Rows[0].Stat);
                Assert.AreEqual(3, table.Rows[1].Stat);
                Assert.AreEqual(3, table.Rows[2].Stat);
                Assert.AreEqual(2, table.Rows[3].Stat);
                Assert.AreEqual(2, table.Rows[4].Stat);
                Assert.AreEqual(3, table.Rows[5].Stat);
                Assert.AreEqual(3, table.Rows[6].Stat);

                Assert.AreEqual(7, table.Rows.Length);
            });
        }

        [TestMethod]
        public void WeaponRankTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.WeaponRankTable;

                Assert.AreEqual(0, table.Rows[0].Skill0);
                Assert.AreEqual(3, table.Rows[1].Skill1);
                Assert.AreEqual(7, table.Rows[2].Skill2);
                Assert.AreEqual(15, table.Rows[3].Skill3);

                Assert.AreEqual(5, table.Rows.Length);
            });
        }

        [TestMethod]
        public void AttackResistTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.AttackResistTable;

                Assert.AreEqual(15, table.Rows[0].Attack);
                Assert.AreEqual(30, table.Rows[0].Resist);

                Assert.AreEqual(1, table.Rows.Length);
            });
        }

        [TestMethod]
        public void LoadedOverrideTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.LoadedOverrideTable;

                switch (editor.Scenario) {
                    case ScenarioType.Scenario1:
                        Assert.AreEqual(0, table.Rows[0].SynChr);
                        Assert.AreEqual(0x1D, table.Rows[0].SynMpd);
                        Assert.AreEqual(0x20, table.Rows[0].SynMusic);
                        break;

                    case ScenarioType.Scenario2:
                        Assert.AreEqual(0, table.Rows[0].MedChr);
                        Assert.AreEqual(0x1A, table.Rows[0].MedMpd);
                        Assert.AreEqual(0x20, table.Rows[0].MedMusic);
                        break;

                    case ScenarioType.Scenario3:
                        Assert.AreEqual(0, table.Rows[0].JulChr);
                        Assert.AreEqual(0xE6, table.Rows[0].JulMpd);
                        Assert.AreEqual(0x3F, table.Rows[0].JulMusic);
                        break;

                    case ScenarioType.PremiumDisk:
                        // No LoadedOverrides. Assert here just to make this test future-proof.
                        Assert.AreEqual(0, table.Rows.Length);
                        break;
                }

                Assert.AreEqual(testCase.ExpectedLoadedOverrides, table.Rows.Length);
            });
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = testCase.Create();
                var table = editor.WarpTable;

                // Only Scenario1 has a warp table in X002.BIN
                if (testCase.Scenario != ScenarioType.Scenario1) {
                    Assert.IsNull(table);
                    return;
                }

                Assert.AreEqual(0, table.Rows[0].WarpType);
                Assert.AreEqual(0, table.Rows[0].WarpUnknown1);
                Assert.AreEqual(0, table.Rows[0].WarpUnknown2);

                Assert.AreEqual(130, table.Rows[1].WarpType);
                Assert.AreEqual(15, table.Rows[1].WarpUnknown1);
                Assert.AreEqual(255, table.Rows[1].WarpUnknown2);

                Assert.AreEqual(780, table.Rows.Length);
            });
        }
    }
}
