using SF3.RawEditors;
using SF3.Editors;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests_Town_S3 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_Editor Create()
                => X1_Editor.Create(new ByteEditor(File.ReadAllBytes(Filename)), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario3, "X1BEER.BIN");

        [TestMethod]
        public void EnterTable_HasExpectedData() {
            var editor = TestCase.Create();
            var table = editor.EnterTable;

            Assert.AreEqual(304, table.Rows[0].EnterXPos);
            Assert.AreEqual(976, table.Rows[0].EnterZPos);

            Assert.AreEqual(816, table.Rows[1].EnterXPos);
            Assert.AreEqual(848, table.Rows[1].EnterZPos);

            Assert.AreEqual(528, table.Rows[2].EnterXPos);
            Assert.AreEqual(912, table.Rows[2].EnterZPos);

            Assert.AreEqual(13, table.Rows.Length);
        }

        [TestMethod]
        public void NpcTable_HasExpectedData() {
            var editor = TestCase.Create();
            var table = editor.NpcTable;

            Assert.AreEqual(0x216, table.Rows[0].SpriteID);
            Assert.AreEqual(240, table.Rows[0].NpcXPos);
            Assert.AreEqual(688, table.Rows[0].NpcZPos);

            Assert.AreEqual(0x217, table.Rows[1].SpriteID);
            Assert.AreEqual(368, table.Rows[1].NpcXPos);
            Assert.AreEqual(880, table.Rows[1].NpcZPos);

            Assert.AreEqual(15, table.Rows.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var editor = TestCase.Create();
            var table = editor.TreasureTable;

            Assert.AreEqual(0x00, table.Rows[0].Searched);
            Assert.AreEqual(0x97C, table.Rows[0].EventParameter);
            Assert.AreEqual(0xFFFF, table.Rows[0].FlagUse);
            Assert.AreEqual(61, table.Rows[0].EventNumber);

            Assert.AreEqual(0x00, table.Rows[1].Searched);
            Assert.AreEqual(0xE06C, table.Rows[1].EventParameter);
            Assert.AreEqual(0xFFFF, table.Rows[1].FlagUse);
            Assert.AreEqual(62, table.Rows[1].EventNumber);

            Assert.AreEqual(28, table.Rows.Length);
        }

        [TestMethod]
        public void ArrowTable_HasExpectedData() {
            var editor = TestCase.Create();
            var table = editor.ArrowTable;

            Assert.AreEqual(0x960, table.Rows[0].ArrowText);
            Assert.AreEqual(0x22, table.Rows[0].ArrowWarp);

            Assert.AreEqual(0x961, table.Rows[1].ArrowText);
            Assert.AreEqual(0x23, table.Rows[1].ArrowWarp);

            Assert.AreEqual(9, table.Rows.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var editor = TestCase.Create();
            var table = editor.WarpTable;

            Assert.AreEqual(0x41, table.Rows[0].WarpMap);
            Assert.AreEqual(0x00, table.Rows[0].WarpType);

            Assert.AreEqual(0x87, table.Rows[1].WarpMap);
            Assert.AreEqual(0x82, table.Rows[1].WarpType);

            Assert.AreEqual(14, table.Rows.Length);
        }
    }
}
