using SF3.FileEditors;
using SF3.Types;
using static SF3Lib.Tests.Utils;

namespace SF3LibTests.FileEditors {
    [TestClass]
    public class IconPointerEditorTests {
        class TestCase {
            public TestCase(string filename, ScenarioType scenario, bool isX066) {
                Filename = "../../../" + filename;
                Scenario = scenario;
                IsX066 = isX066;
            }

            public readonly string Filename;
            public readonly ScenarioType Scenario;
            public readonly bool IsX066;
        }

        [TestMethod]
        public void ItemIcons_X11_X21_HaveExpectedFirstFewRows() {
            var testCases = new List<TestCase>()
            {
                new TestCase("TestData/S1US/X011.BIN", ScenarioType.Scenario1, false),
                new TestCase("TestData/S1US/X021.BIN", ScenarioType.Scenario1, false),
                new TestCase("TestData/S2/X011.BIN", ScenarioType.Scenario2, false),
                new TestCase("TestData/S2/X021.BIN", ScenarioType.Scenario2, false),
                new TestCase("TestData/S3/X011.BIN", ScenarioType.Scenario3, false),
                new TestCase("TestData/S3/X021.BIN", ScenarioType.Scenario3, false),
                new TestCase("TestData/PD/X011.BIN", ScenarioType.PremiumDisk, false),
                new TestCase("TestData/PD/X021.BIN", ScenarioType.PremiumDisk, false),
            };

            RunTestCases(testCases, testCase => {
                var editor = new IconPointerFileEditor(testCase.Scenario, testCase.IsX066);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));
                Assert.AreEqual(0x26, editor.ItemIconList.Models[1].TheItemIcon);
                Assert.AreEqual(0x18, editor.SpellIconList.Models[1].TheSpellIcon);
            });
        }

        [TestMethod]
        public void ItemIcons_X26_HasExpectedFirstFewRows() {
            var testCases = new List<TestCase>()
            {
                new TestCase("TestData/S1US/X026.BIN", ScenarioType.Scenario1, true),
                new TestCase("TestData/S1JP/X026.BIN", ScenarioType.Scenario1, true),
                new TestCase("TestData/S2/X026.BIN", ScenarioType.Scenario2, true),
                new TestCase("TestData/S3/X026.BIN", ScenarioType.Scenario3, true),
                new TestCase("TestData/PD/X026.BIN", ScenarioType.PremiumDisk, true),
            };

            RunTestCases(testCases, testCase => {
                var editor = new IconPointerFileEditor(testCase.Scenario, testCase.IsX066);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));
                Assert.AreEqual(0x26, editor.ItemIconList.Models[1].TheItemIcon);
                Assert.AreEqual(0x18, editor.SpellIconList.Models[1].TheSpellIcon);
            });
        }
    }
}
