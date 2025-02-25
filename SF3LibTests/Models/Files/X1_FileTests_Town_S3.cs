using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Town_S3 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario3, "X1BEER.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.EnterTable;

            Assert.AreEqual(304, table[0].XPos);
            Assert.AreEqual(976, table[0].ZPos);

            Assert.AreEqual(816, table[1].XPos);
            Assert.AreEqual(848, table[1].ZPos);

            Assert.AreEqual(528, table[2].XPos);
            Assert.AreEqual(912, table[2].ZPos);

            Assert.AreEqual(13, table.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.NpcTable;

            Assert.AreEqual(0x216, table[0].SpriteID);
            Assert.AreEqual(240, table[0].XPos);
            Assert.AreEqual(688, table[0].ZPos);

            Assert.AreEqual(0x217, table[1].SpriteID);
            Assert.AreEqual(368, table[1].XPos);
            Assert.AreEqual(880, table[1].ZPos);

            Assert.AreEqual(15, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(0x00, table[0].Searched);
            Assert.AreEqual(0x97C, table[0].EventParameter);
            Assert.AreEqual(0xFFFF, table[0].FlagUsed);
            Assert.AreEqual(61, table[0].EventNumber);

            Assert.AreEqual(0x00, table[1].Searched);
            Assert.AreEqual(0xE06C, table[1].EventParameter);
            Assert.AreEqual(0xFFFF, table[1].FlagUsed);
            Assert.AreEqual(62, table[1].EventNumber);

            Assert.AreEqual(28, table.Length);
        }

        [TestMethod]
        public void ArrowTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.ArrowTable;

            Assert.AreEqual(0x960, table[0].TextID);
            Assert.AreEqual(0x22, table[0].PointToWarpMPD);

            Assert.AreEqual(0x961, table[1].TextID);
            Assert.AreEqual(0x23, table[1].PointToWarpMPD);

            Assert.AreEqual(9, table.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.WarpTable;

            Assert.AreEqual(0x41, table[0].WarpMap);
            Assert.AreEqual(0x00, table[0].WarpType);

            Assert.AreEqual(0x87, table[1].WarpMap);
            Assert.AreEqual(0x82, table[1].WarpType);

            Assert.AreEqual(14, table.Length);
        }
    }
}
