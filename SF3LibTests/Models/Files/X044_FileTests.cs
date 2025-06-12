using CommonLib.Arrays;
using SF3.Models.Files.X044;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X044_FileTests {
        private class X044_TestCase : TestCase {
            public X044_TestCase(ScenarioType scenario, string filename, int expectedRows)
            : base(scenario, filename) {
                ExpectedRows = expectedRows;
            }

            public X044_File Create()
                => X044_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario);

            public int ExpectedRows { get; }
        }

        private static readonly List<X044_TestCase> TestCases = [
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

                Assert.AreEqual(testCase.ExpectedRows, file.MonsterTable.Length);
            });
        }
    }
}
