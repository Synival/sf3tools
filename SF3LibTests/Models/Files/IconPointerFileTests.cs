using SF3.NamedValues;
using SF3.Types;
using SF3.Models.Files.IconPointer;
using SF3.ByteData;
using CommonLib.Arrays;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class IconPointerFileTests {
        private class IPETestCase : TestCase {
            public IPETestCase(ScenarioType scenario, string filename, int itemIconRows, int spellIconRows, int spellRealOffsetStart)
            : base(scenario, filename) {
                ExpectedItemIconRows = itemIconRows;
                ExpectedSpellIconRows = spellIconRows;
                ExpectedSpellRealOffsetStart = spellRealOffsetStart;
            }

            public IconPointerFile Create()
                => IconPointerFile.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

            public int ExpectedItemIconRows { get; }
            public int ExpectedSpellIconRows { get; }
            public int ExpectedSpellRealOffsetStart { get; }
        }

        private static readonly List<IPETestCase> TestCases = [
            new(ScenarioType.Scenario1, "X011.BIN", 256, 51, 65422),
            new(ScenarioType.Scenario1, "X021.BIN", 256, 51, 65422),
            new(ScenarioType.Scenario1, "X026.BIN", 256, 51, 65422),

            new(ScenarioType.Scenario2, "X011.BIN", 256, 61, 64646),
            new(ScenarioType.Scenario2, "X021.BIN", 256, 61, 64646),
            new(ScenarioType.Scenario2, "X026.BIN", 256, 61, 64646),

            new(ScenarioType.Scenario3, "X011.BIN", 300, 91, 76360),
            new(ScenarioType.Scenario3, "X021.BIN", 300, 91, 76360),
            new(ScenarioType.Scenario3, "X026.BIN", 300, 91, 76360),

            new(ScenarioType.PremiumDisk, "X011.BIN", 300, 93, 76338),
            new(ScenarioType.PremiumDisk, "X021.BIN", 300, 93, 76338),
            new(ScenarioType.PremiumDisk, "X026.BIN", 300, 93, 76338)
        ];

        [TestMethod]
        public void ItemIconTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                Assert.AreEqual(0x00, file.ItemIconTable.Rows[0].TheItemIcon);
                Assert.AreEqual(0x26, file.ItemIconTable.Rows[1].TheItemIcon);
                Assert.AreEqual(testCase.ExpectedItemIconRows, file.ItemIconTable.Rows.Length);
            });
        }

        [TestMethod]
        public void SpellIconTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var file = testCase.Create();

                Assert.AreEqual(0x00, file.SpellIconTable.Rows[0].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellRealOffsetStart + file.SpellIconTable.Rows[0].TheSpellIcon, file.SpellIconTable.Rows[0].RealOffset);

                Assert.AreEqual(0x18, file.SpellIconTable.Rows[1].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellRealOffsetStart + file.SpellIconTable.Rows[1].TheSpellIcon, file.SpellIconTable.Rows[1].RealOffset);

                Assert.AreEqual(0x176, file.SpellIconTable.Rows[2].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellRealOffsetStart + file.SpellIconTable.Rows[2].TheSpellIcon, file.SpellIconTable.Rows[2].RealOffset);

                Assert.AreEqual(testCase.ExpectedSpellIconRows, file.SpellIconTable.Rows.Length);
            });
        }
    }
}
