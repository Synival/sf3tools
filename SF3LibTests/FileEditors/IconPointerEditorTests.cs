using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class IconPointerEditorTests {
        private class IPETestCase : TestCase {
            public IPETestCase(ScenarioType scenario, string filename, bool isX026, int itemIconRows, int spellIconRows)
            : base(scenario, filename) {
                IsX026 = isX026;
                ExpectedItemIconRows = itemIconRows;
                ExpectedSpellIconRows = spellIconRows;
            }

            public override string Name => base.Name + (IsX026 ? " (X026)" : "");
            public bool IsX026 { get; }
            public int ExpectedItemIconRows { get; }
            public int ExpectedSpellIconRows { get; }
        }

        private readonly static List<IPETestCase> TestCases = new()
        {
            new(ScenarioType.Scenario1, "X011.BIN", false, 256, 51),
            new(ScenarioType.Scenario1, "X021.BIN", false, 256, 51),
            new(ScenarioType.Scenario1, "X026.BIN", true, 256, 51),

            new(ScenarioType.Scenario2, "X011.BIN", false, 256, 61),
            new(ScenarioType.Scenario2, "X021.BIN", false, 256, 61),
            new(ScenarioType.Scenario2, "X026.BIN", true, 256, 61),

            new(ScenarioType.Scenario3, "X011.BIN", false, 300, 91),
            new(ScenarioType.Scenario3, "X021.BIN", false, 300, 91),
            new(ScenarioType.Scenario3, "X026.BIN", true, 300, 91),

            new(ScenarioType.PremiumDisk, "X011.BIN", false, 300, 93),
            new(ScenarioType.PremiumDisk, "X021.BIN", false, 300, 93),
            new(ScenarioType.PremiumDisk, "X026.BIN", true, 300, 93)
        };

        [TestMethod]
        public void ItemIconTable_X11_X21_X26_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = new IconPointerFileEditor(testCase.Scenario, testCase.IsX026);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.AreEqual(0x00, editor.ItemIconList.Rows[0].TheItemIcon);
                Assert.AreEqual(0x26, editor.ItemIconList.Rows[1].TheItemIcon);
                Assert.AreEqual(testCase.ExpectedItemIconRows, editor.ItemIconList.Rows.Length);
            });
        }

        [TestMethod]
        public void SpellIconTable_X11_X21_X26_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = new IconPointerFileEditor(testCase.Scenario, testCase.IsX026);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.AreEqual(0x00, editor.SpellIconList.Rows[0].TheSpellIcon);
                Assert.AreEqual(0x18, editor.SpellIconList.Rows[1].TheSpellIcon);
                Assert.AreEqual(testCase.ExpectedSpellIconRows, editor.SpellIconList.Rows.Length);
            });
        }
    }
}
