using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X019_FileEditorTests {
        private class X019_TestCase : TestCase {
            public X019_TestCase(ScenarioType scenario, string filename, int expectedRows)
            : base(scenario, filename) {
                ExpectedRows = expectedRows;
            }

            public int ExpectedRows { get; }
        }

        private static readonly List<X019_TestCase> TestCases = [
            new(ScenarioType.Scenario1,   "X019.BIN", 142),
            new(ScenarioType.Scenario2,   "X019.BIN", 191),
            new(ScenarioType.Scenario3,   "X019.BIN", 212),
            new(ScenarioType.PremiumDisk, "X019.BIN", 212),
            new(ScenarioType.PremiumDisk, "X044.BIN", 212),
        ];

        [TestMethod]
        public void MonsterTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = new X019_FileEditor(testCase.Scenario);
                Assert.IsTrue(editor.LoadFile(testCase.Filename));

                Assert.AreEqual(0, editor.MonsterTable.Rows[0].MaxHP);
                Assert.AreEqual(0, editor.MonsterTable.Rows[0].MaxMP);
                Assert.AreEqual(0, editor.MonsterTable.Rows[0].Attack);
                // TODO: maybe more data?

                Assert.AreEqual(10, editor.MonsterTable.Rows[1].MaxHP);
                Assert.AreEqual( 0, editor.MonsterTable.Rows[1].MaxMP);
                Assert.AreEqual(12, editor.MonsterTable.Rows[1].Attack);
                // TODO: maybe more data?

                Assert.AreEqual(testCase.ExpectedRows, editor.MonsterTable.Rows.Length);
            });
        }
    }
}
