using SF3.RawEditors;
using SF3.NamedValues;
using SF3.Types;
using SF3.Editors.X019;

namespace SF3.Tests.Editors {
    [TestClass]
    public class X019_EditorTests {
        private class X019_TestCase : TestCase {
            public X019_TestCase(ScenarioType scenario, string filename, int expectedRows)
            : base(scenario, filename) {
                ExpectedRows = expectedRows;
            }

            public X019_Editor Create()
                => X019_Editor.Create(new ByteEditor(File.ReadAllBytes(Filename)), new NameGetterContext(Scenario), Scenario);

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
                var editor = testCase.Create();

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
