using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Town_PD {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.PremiumDisk, "X1DREAM.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.EnterTable;

            Assert.AreEqual(0xFFFF, table[1].SceneNumber);
            Assert.AreEqual(0, table[0].XPos);
            Assert.AreEqual(0, table[0].ZPos);

            Assert.AreEqual(0xFFFF, table[1].SceneNumber);
            Assert.AreEqual(0, table[1].XPos);
            Assert.AreEqual(0, table[1].ZPos);

            Assert.AreEqual(2, table.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.NpcTable;

            Assert.AreEqual(0xC7, table[0].SpriteID);
            Assert.AreEqual(0, table[0].XPos);
            Assert.AreEqual(0, table[0].ZPos);

            Assert.AreEqual(0xC7, table[1].SpriteID);
            Assert.AreEqual(65472, table[1].XPos);
            Assert.AreEqual(0, table[1].ZPos);

            Assert.AreEqual(7, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(0xFFFF, table[0].Searched);
            Assert.AreEqual(0, table[0].EventParameter);
            Assert.AreEqual(0, table[0].FlagUsed);
            Assert.AreEqual(0, table[0].EventNumber);

            Assert.AreEqual(1, table.Length);
        }

        [TestMethod]
        public void ArrowTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.ArrowTable;

            Assert.AreEqual(0xFFFF, table[0].TextID);
            Assert.AreEqual(0x00, table[0].PointToWarpMPD);

            Assert.AreEqual(1, table.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.WarpTable;

            Assert.AreEqual(0x00, table[0].WarpMap);
            Assert.AreEqual(0x00, table[0].WarpType);

            Assert.AreEqual(0x00, table[1].WarpMap);
            Assert.AreEqual(0x82, table[1].WarpType);

            Assert.AreEqual(7, table.Length);
        }
    }
}
