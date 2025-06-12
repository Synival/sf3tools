using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

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
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario2, "X1DUSTY.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.EnterTable;

            Assert.AreEqual(400, table[0].XPos);
            Assert.AreEqual(400, table[0].ZPos);

            Assert.AreEqual(688, table[1].XPos);
            Assert.AreEqual(144, table[1].ZPos);

            Assert.AreEqual(496, table[2].XPos);
            Assert.AreEqual(208, table[2].ZPos);

            Assert.AreEqual(14, table.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.NpcTable;

            Assert.AreEqual(0x45, table[0].SpriteID);
            Assert.AreEqual(784, table[0].XPosDec);
            Assert.AreEqual(272, table[0].ZPosDec);

            Assert.AreEqual(0x5F, table[1].SpriteID);
            Assert.AreEqual(496, table[1].XPosDec);
            Assert.AreEqual(272, table[1].ZPosDec);

            Assert.AreEqual(23, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(    0x0000,  table[0].Trigger);
            Assert.AreEqual(      0x00,  table[0].TriggerFlags);
            Assert.AreEqual(0x00000909u, table[0].Action);

            Assert.AreEqual(32, table.Length);
        }

        [TestMethod]
        public void ArrowTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.ArrowTable;

            Assert.AreEqual(0x8C7, table[0].TextID);
            Assert.AreEqual(0x28, table[0].PointToWarpMPD);

            Assert.AreEqual(0x8C6, table[1].TextID);
            Assert.AreEqual(0x21, table[1].PointToWarpMPD);

            Assert.AreEqual(6, table.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.WarpTable;

            Assert.AreEqual(0, table[0].LocationID);
            Assert.AreEqual(0, table[0].IfFlagUnset);
            Assert.AreEqual(0, table[0].WarpTrigger);
            Assert.AreEqual(77, table[0].LoadID);

            Assert.AreEqual(1, table[1].LocationID);
            Assert.AreEqual(0xFFF, table[1].IfFlagUnset);
            Assert.AreEqual(1, table[1].WarpTrigger);
            Assert.AreEqual(81, table[1].LoadID);

            Assert.AreEqual(14, table.Length);
        }
    }
}
