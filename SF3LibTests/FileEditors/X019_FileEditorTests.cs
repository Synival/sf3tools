using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X019_FileEditorTests {
        private class X019_TestCase : TestCase {
            public X019_TestCase(ScenarioType scenario, string filename, bool isX044, int expectedRows)
            : base(scenario, filename) {
                IsX044 = isX044;
                ExpectedRows = expectedRows;
            }

            public override string Name => base.Name + (IsX044 ? " (X044)" : "");
            public bool IsX044 { get; }
            public int ExpectedRows { get; }
        }

        private readonly static List<X019_TestCase> TestCases = [
            new(ScenarioType.Scenario1, "X019.BIN", false, 142),
            new(ScenarioType.Scenario2, "X019.BIN", false, 191),
            new(ScenarioType.Scenario3, "X019.BIN", false, 212),
            new(ScenarioType.PremiumDisk, "X019.BIN", false, 212),
            new(ScenarioType.PremiumDisk, "X044.BIN", true, 212),
        ];

        [TestMethod]
        public void MonsterTable_HasExpectedData() {
            TestCase.Run(TestCases, testCase => {
                var editor = new X019_FileEditor(testCase.Scenario, testCase.IsX044);
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
