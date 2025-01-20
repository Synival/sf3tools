using CommonLib.Arrays;
using SF3.Models.Files.X019;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X019_FileTests {
        private class X019_TestCase : TestCase {
            public X019_TestCase(ScenarioType scenario, string filename, int expectedRows)
            : base(scenario, filename) {
                ExpectedRows = expectedRows;
            }

            public X019_File Create()
                => X019_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

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
                var file = testCase.Create();

                Assert.AreEqual(0, file.MonsterTable[0].MaxHP);
                Assert.AreEqual(0, file.MonsterTable[0].MaxMP);
                Assert.AreEqual(0, file.MonsterTable[0].Attack);
                // TODO: maybe more data?

                Assert.AreEqual(10, file.MonsterTable[1].MaxHP);
                Assert.AreEqual(0, file.MonsterTable[1].MaxMP);
                Assert.AreEqual(12, file.MonsterTable[1].Attack);
                // TODO: maybe more data?

                Assert.AreEqual(testCase.ExpectedRows, file.MonsterTable.Rows.Length);
            });
        }
    }
}
