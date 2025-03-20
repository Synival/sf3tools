using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Town_S1 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario1, "X1BAL_3.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.EnterTable;

            Assert.AreEqual(1472, table[0].XPos);
            Assert.AreEqual(480, table[0].ZPos);

            Assert.AreEqual(1472, table[1].XPos);
            Assert.AreEqual(480, table[1].ZPos);

            Assert.AreEqual(1008, table[2].XPos);
            Assert.AreEqual(1184, table[2].ZPos);

            Assert.AreEqual(6, table.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.NpcTable;

            Assert.AreEqual(0x42, table[0].SpriteID);
            Assert.AreEqual(1592, table[0].XPos);
            Assert.AreEqual(544, table[0].ZPos);

            Assert.AreEqual(0x3E, table[1].SpriteID);
            Assert.AreEqual(1568, table[1].XPos);
            Assert.AreEqual(448, table[1].ZPos);

            Assert.AreEqual(24, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(0xC302, table[0].Searched);
            Assert.AreEqual(0xF2D0, table[0].EventParameter);
            Assert.AreEqual(0x0201, table[0].FlagUsed);
            Assert.AreEqual(3, table[0].EventNumber);

            Assert.AreEqual(0x0302, table[1].Searched);
            Assert.AreEqual(0xF2EC, table[1].EventParameter);
            Assert.AreEqual(0x0202, table[1].FlagUsed);
            Assert.AreEqual(4, table[1].EventNumber);

            Assert.AreEqual(31, table.Length);
        }
    }
}
