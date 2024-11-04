using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests_Town_PD {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.PremiumDisk, "X1DREAM.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.EnterTable;

            Assert.AreEqual(0xFFFF, table.Rows[1].Entered);
            Assert.AreEqual(0, table.Rows[0].EnterXPos);
            Assert.AreEqual(0, table.Rows[0].EnterZPos);

            Assert.AreEqual(0xFFFF, table.Rows[1].Entered);
            Assert.AreEqual(0, table.Rows[1].EnterXPos);
            Assert.AreEqual(0, table.Rows[1].EnterZPos);

            Assert.AreEqual(2, table.Rows.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.NpcTable;

            Assert.AreEqual(0xC7, table.Rows[0].SpriteID);
            Assert.AreEqual(0, table.Rows[0].NpcXPos);
            Assert.AreEqual(0, table.Rows[0].NpcZPos);

            Assert.AreEqual(0xC7, table.Rows[1].SpriteID);
            Assert.AreEqual(65472, table.Rows[1].NpcXPos);
            Assert.AreEqual(0, table.Rows[1].NpcZPos);

            Assert.AreEqual(7, table.Rows.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.TreasureTable;

            Assert.AreEqual(0xFFFF, table.Rows[0].Searched);
            Assert.AreEqual(0, table.Rows[0].TreasureItem);
            Assert.AreEqual(0, table.Rows[0].FlagUse);
            Assert.AreEqual(0, table.Rows[0].EventNumber);

            Assert.AreEqual(1, table.Rows.Length);
        }

        [TestMethod]
        public void ArrowTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.ArrowTable;

            Assert.AreEqual(0xFFFF, table.Rows[0].ArrowText);
            Assert.AreEqual(0x00, table.Rows[0].ArrowWarp);

            Assert.AreEqual(1, table.Rows.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.WarpTable;

            Assert.AreEqual(0x00, table.Rows[0].WarpMap);
            Assert.AreEqual(0x00, table.Rows[0].WarpType);

            Assert.AreEqual(0x00, table.Rows[1].WarpMap);
            Assert.AreEqual(0x82, table.Rows[1].WarpType);

            Assert.AreEqual(7, table.Rows.Length);
        }
    }
}
