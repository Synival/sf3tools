using SF3.NamedValues;
using SF3.Types;
using SF3.Models.Files.X1;
using SF3.RawData;
using CommonLib.Arrays;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Town_S2 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario2, "X1DUSTY.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.EnterTable;

            Assert.AreEqual(400, table.Rows[0].EnterXPos);
            Assert.AreEqual(400, table.Rows[0].EnterZPos);

            Assert.AreEqual(688, table.Rows[1].EnterXPos);
            Assert.AreEqual(144, table.Rows[1].EnterZPos);

            Assert.AreEqual(496, table.Rows[2].EnterXPos);
            Assert.AreEqual(208, table.Rows[2].EnterZPos);

            Assert.AreEqual(15, table.Rows.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.NpcTable;

            Assert.AreEqual(0x45, table.Rows[0].SpriteID);
            Assert.AreEqual(784, table.Rows[0].NpcXPos);
            Assert.AreEqual(272, table.Rows[0].NpcZPos);

            Assert.AreEqual(0x5F, table.Rows[1].SpriteID);
            Assert.AreEqual(496, table.Rows[1].NpcXPos);
            Assert.AreEqual(272, table.Rows[1].NpcZPos);

            Assert.AreEqual(24, table.Rows.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.TreasureTable;

            Assert.AreEqual(0x00, table.Rows[0].Searched);
            Assert.AreEqual(0x909, table.Rows[0].EventParameter);
            Assert.AreEqual(0x01, table.Rows[0].FlagUse);
            Assert.AreEqual(61, table.Rows[0].EventNumber);

            Assert.AreEqual(0x0, table.Rows[1].Searched);
            Assert.AreEqual(0x90A, table.Rows[1].EventParameter);
            Assert.AreEqual(1, table.Rows[1].FlagUse);
            Assert.AreEqual(62, table.Rows[1].EventNumber);

            Assert.AreEqual(33, table.Rows.Length);
        }

        [TestMethod]
        public void ArrowTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.ArrowTable;

            Assert.AreEqual(0x8C7, table.Rows[0].ArrowText);
            Assert.AreEqual(0x28, table.Rows[0].ArrowWarp);

            Assert.AreEqual(0x8C6, table.Rows[1].ArrowText);
            Assert.AreEqual(0x21, table.Rows[1].ArrowWarp);

            Assert.AreEqual(7, table.Rows.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.WarpTable;

            Assert.AreEqual(0x4D, table.Rows[0].WarpMap);
            Assert.AreEqual(0x00, table.Rows[0].WarpType);

            Assert.AreEqual(0x51, table.Rows[1].WarpMap);
            Assert.AreEqual(0x82, table.Rows[1].WarpType);

            Assert.AreEqual(15, table.Rows.Length);
        }
    }
}
