using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests_Battle_S1 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename,
                MapLeaderType mapLeader)
            : base(scenario, filename) {
                MapLeader = mapLeader;
            }

            public MapLeaderType MapLeader { get; }
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario1, "X1BTL104.BIN", MapLeaderType.Synbios);

        [TestMethod]
        public void AITable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.AITable;

            Assert.AreEqual(29, table.Rows[0].TargetX);
            Assert.AreEqual(43, table.Rows[0].TargetY);

            Assert.AreEqual(32, table.Rows[1].TargetX);
            Assert.AreEqual(33, table.Rows[1].TargetY);

            Assert.AreEqual(33, table.Rows.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.BattlePointersTable;

            Assert.AreEqual(0x6061170, table.Rows[0].BattlePointer);
            Assert.AreEqual(0, table.Rows[1].BattlePointer);
            Assert.AreEqual(0, table.Rows[2].BattlePointer);
            Assert.AreEqual(0, table.Rows[3].BattlePointer);

            Assert.AreEqual(4, table.Rows.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.CustomMovementTable;

            Assert.AreEqual(27, table.Rows[0].CustomMovementX1);
            Assert.AreEqual(41, table.Rows[0].CustomMovementZ1);

            Assert.AreEqual(32, table.Rows[1].CustomMovementX1);
            Assert.AreEqual(31, table.Rows[1].CustomMovementZ1);

            Assert.AreEqual(33, table.Rows.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.HeaderTable;

            Assert.AreEqual( 0, table.Rows[0].SizeUnknown1);
            Assert.AreEqual( 0, table.Rows[0].BattlePointersTableOffset);
            Assert.AreEqual(26, table.Rows[0].TableSize);

            Assert.AreEqual(1, table.Rows.Length);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.SlotTable;

            Assert.AreEqual(0xFFFF, table.Rows[0].EnemyID);
            Assert.AreEqual(33, table.Rows[0].EnemyX);
            Assert.AreEqual(17, table.Rows[0].EnemyY);

            Assert.AreEqual(0x13, table.Rows[12].EnemyID);
            Assert.AreEqual(29, table.Rows[12].EnemyX);
            Assert.AreEqual(43, table.Rows[12].EnemyY);

            Assert.AreEqual(72, table.Rows.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.SpawnZoneTable;

            Assert.AreEqual(0x400, table.Rows[0].UnknownAI00);
            Assert.AreEqual(0x1C, table.Rows[0].UnknownAI02);
            Assert.AreEqual(0x2C, table.Rows[0].UnknownAI04);
            Assert.AreEqual(0x1C, table.Rows[0].UnknownAI06);

            Assert.AreEqual(0x400, table.Rows[1].UnknownAI00);
            Assert.AreEqual(0x15, table.Rows[1].UnknownAI02);
            Assert.AreEqual(0x20, table.Rows[1].UnknownAI04);
            Assert.AreEqual(0x15, table.Rows[1].UnknownAI06);

            Assert.AreEqual(16, table.Rows.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, TestCase.MapLeader, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.TreasureTable;

            Assert.AreEqual(0x8013, table.Rows[0].Searched);
            Assert.AreEqual(0x53, table.Rows[0].TreasureItem);
            Assert.AreEqual(0x70E, table.Rows[0].FlagUse);
            Assert.AreEqual(0x0F, table.Rows[0].EventNumber);

            Assert.AreEqual(0xFFFF, table.Rows[1].Searched);
            Assert.AreEqual(0, table.Rows[1].TreasureItem);
            Assert.AreEqual(0, table.Rows[1].FlagUse);
            Assert.AreEqual(0, table.Rows[1].EventNumber);

            Assert.AreEqual(2, table.Rows.Length);
        }
    }
}
