using SF3.NamedValues;
using SF3.Types;
using SF3.Models.Files.X1;
using SF3.ByteData;
using CommonLib.Arrays;

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

            Assert.AreEqual(1472, table.Rows[0].EnterXPos);
            Assert.AreEqual(480, table.Rows[0].EnterZPos);

            Assert.AreEqual(1472, table.Rows[1].EnterXPos);
            Assert.AreEqual(480, table.Rows[1].EnterZPos);

            Assert.AreEqual(1008, table.Rows[2].EnterXPos);
            Assert.AreEqual(1184, table.Rows[2].EnterZPos);

            Assert.AreEqual(7, table.Rows.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.NpcTable;

            Assert.AreEqual(0x42, table.Rows[0].SpriteID);
            Assert.AreEqual(1592, table.Rows[0].NpcXPos);
            Assert.AreEqual(544, table.Rows[0].NpcZPos);

            Assert.AreEqual(0x3E, table.Rows[1].SpriteID);
            Assert.AreEqual(1568, table.Rows[1].NpcXPos);
            Assert.AreEqual(448, table.Rows[1].NpcZPos);

            Assert.AreEqual(25, table.Rows.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.TreasureTable;

            Assert.AreEqual(0xC302, table.Rows[0].Searched);
            Assert.AreEqual(0xF2D0, table.Rows[0].EventParameter);
            Assert.AreEqual(0x0201, table.Rows[0].FlagUse);
            Assert.AreEqual(3, table.Rows[0].EventNumber);

            Assert.AreEqual(0x0302, table.Rows[1].Searched);
            Assert.AreEqual(0xF2EC, table.Rows[1].EventParameter);
            Assert.AreEqual(0x0202, table.Rows[1].FlagUse);
            Assert.AreEqual(4, table.Rows[1].EventNumber);

            Assert.AreEqual(32, table.Rows.Length);
        }
    }
}
