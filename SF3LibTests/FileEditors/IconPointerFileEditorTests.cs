using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class IconPointerFileEditorTests {
        private class IPETestCase : TestCase {
            public IPETestCase(ScenarioType scenario, string filename, bool isX026, int itemIconRows, int spellIconRows, int spellRealOffsetStart)
            : base(scenario, filename) {
                IsX026 = isX026;
                ExpectedItemIconRows = itemIconRows;
                ExpectedSpellIconRows = spellIconRows;
                ExpectedSpellRealOffsetStart = spellRealOffsetStart;
            }

            public override string Name => base.Name + (IsX026 ? " (X026)" : "");
            public bool IsX026 { get; }
            public int ExpectedItemIconRows { get; }
            public int ExpectedSpellIconRows { get; }
            public int ExpectedSpellRealOffsetStart { get; }
        }

        private readonly static List<IPETestCase> TestCases = new()
        {
            new(ScenarioType.Scenario1, "X011.BIN", false, 256, 51, 65422),
            new(ScenarioType.Scenario1, "X021.BIN", false, 256, 51, 65422),
            new(ScenarioType.Scenario1, "X026.BIN", true, 256, 51, 65422),

            new(ScenarioType.Scenario2, "X011.BIN", false, 256, 61, 64646),
            new(ScenarioType.Scenario2, "X021.BIN", false, 256, 61, 64646),
            new(ScenarioType.Scenario2, "X026.BIN", true, 256, 61, 64646),

            new(ScenarioType.Scenario3, "X011.BIN", false, 300, 91, 76360),
            new(ScenarioType.Scenario3, "X021.BIN", false, 300, 91, 76360),
            new(ScenarioType.Scenario3, "X026.BIN", true, 300, 91, 76360),

            new(ScenarioType.PremiumDisk, "X011.BIN", false, 300, 93, 76338),
            new(ScenarioType.PremiumDisk, "X021.BIN", false, 300, 93, 76338),
            new(ScenarioType.PremiumDisk, "X026.BIN", true, 300, 93, 76338)
        };

        [TestMethod]
        public void ItemIconTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = new IconPointerFileEditor(testCase.Scenario, testCase.IsX026);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.AreEqual(0x00, editor.ItemIconTable.Rows[0].TheItemIcon);
                Assert.AreEqual(0x26, editor.ItemIconTable.Rows[1].TheItemIcon);
                Assert.AreEqual(testCase.ExpectedItemIconRows, editor.ItemIconTable.Rows.Length);
            });
        }

        [TestMethod]
        public void SpellIconTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = new IconPointerFileEditor(testCase.Scenario, testCase.IsX026);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.AreEqual(0x00, editor.SpellIconTable.Rows[0].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellRealOffsetStart + editor.SpellIconTable.Rows[0].TheSpellIcon, editor.SpellIconTable.Rows[0].RealOffset);

                Assert.AreEqual(0x18, editor.SpellIconTable.Rows[1].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellRealOffsetStart + editor.SpellIconTable.Rows[1].TheSpellIcon, editor.SpellIconTable.Rows[1].RealOffset);

                Assert.AreEqual(0x176, editor.SpellIconTable.Rows[2].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellRealOffsetStart + editor.SpellIconTable.Rows[2].TheSpellIcon, editor.SpellIconTable.Rows[2].RealOffset);

                Assert.AreEqual(testCase.ExpectedSpellIconRows, editor.SpellIconTable.Rows.Length);
            });
        }
    }
}
