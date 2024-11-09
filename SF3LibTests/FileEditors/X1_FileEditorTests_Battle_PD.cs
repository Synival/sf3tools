using SF3.FileEditors;
using SF3.Types;

namespace SF3.Tests.FileEditors {
    [TestClass]
    public class X1_FileEditorTests_Battle_PD {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.PremiumDisk, "X1BTLP04.BIN");

        [TestMethod]
        public void AITable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var battle = editor.Battles[MapLeaderType.Synbios];
            var table = battle.AITable;

            Assert.AreEqual( 8, table.Rows[0].TargetX);
            Assert.AreEqual(13, table.Rows[0].TargetY);

            Assert.AreEqual( 7, table.Rows[1].TargetX);
            Assert.AreEqual(12, table.Rows[1].TargetY);

            Assert.AreEqual(33, table.Rows.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var battle = editor.Battles[MapLeaderType.Synbios];
            var table = editor.BattlePointersTable;

            Assert.AreEqual(0x6061974, table.Rows[0].BattlePointer);
            Assert.AreEqual(0, table.Rows[1].BattlePointer);
            Assert.AreEqual(0, table.Rows[2].BattlePointer);
            Assert.AreEqual(0, table.Rows[3].BattlePointer);

            Assert.AreEqual(4, table.Rows.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var battle = editor.Battles[MapLeaderType.Synbios];
            var table = battle.CustomMovementTable;

            Assert.AreEqual( 0, table.Rows[0].CustomMovementX1);
            Assert.AreEqual(63, table.Rows[0].CustomMovementZ1);

            Assert.AreEqual( 0, table.Rows[1].CustomMovementX1);
            Assert.AreEqual(63, table.Rows[1].CustomMovementZ1);

            Assert.AreEqual(33, table.Rows.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var battle = editor.Battles[MapLeaderType.Synbios];
            var table = battle.HeaderTable;

            Assert.AreEqual(   0, table.Rows[0].SizeUnknown1);
            Assert.AreEqual(  16, table.Rows[0].TableSize);

            Assert.AreEqual(1, table.Rows.Length);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var battle = editor.Battles[MapLeaderType.Synbios];
            var table = battle.SlotTable;

            Assert.AreEqual(0xFFFF, table.Rows[0].EnemyID);
            Assert.AreEqual( 9, table.Rows[0].EnemyX);
            Assert.AreEqual( 5, table.Rows[0].EnemyY);

            Assert.AreEqual(0x92, table.Rows[12].EnemyID);
            Assert.AreEqual( 8, table.Rows[12].EnemyX);
            Assert.AreEqual(13, table.Rows[12].EnemyY);

            Assert.AreEqual(52, table.Rows.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var battle = editor.Battles[MapLeaderType.Synbios];
            var table = battle.SpawnZoneTable;

            Assert.AreEqual(0x00, table.Rows[0].UnknownAI00);
            Assert.AreEqual(0x00, table.Rows[0].UnknownAI02);
            Assert.AreEqual(0x3F, table.Rows[0].UnknownAI04);
            Assert.AreEqual(0x00, table.Rows[0].UnknownAI06);
            Assert.AreEqual(0x3F, table.Rows[0].UnknownAI08);
            Assert.AreEqual(0x00, table.Rows[0].UnknownAI0A);

            Assert.AreEqual(0x00, table.Rows[1].UnknownAI00);
            Assert.AreEqual(0x00, table.Rows[1].UnknownAI02);
            Assert.AreEqual(0x3F, table.Rows[1].UnknownAI04);
            Assert.AreEqual(0x00, table.Rows[1].UnknownAI06);
            Assert.AreEqual(0x3F, table.Rows[1].UnknownAI08);
            Assert.AreEqual(0x00, table.Rows[1].UnknownAI0A);

            Assert.AreEqual(16, table.Rows.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.TreasureTable;

            // No treasure to be found in PD, as far as I know.
            Assert.AreEqual(0xFFFF, table.Rows[0].Searched);
            Assert.AreEqual(0, table.Rows[0].EventParameter);
            Assert.AreEqual(0, table.Rows[0].FlagUse);
            Assert.AreEqual(0, table.Rows[0].EventNumber);

            Assert.AreEqual(1, table.Rows.Length);
        }

        [TestMethod]
        public void TileMovementTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.TileMovementTable;

            Assert.AreEqual(0x12, table.Rows[0].TileGrassland);
            Assert.AreEqual(0x23, table.Rows[0].TileDarkGrass);
            Assert.AreEqual(0x24, table.Rows[0].TileForest);
            Assert.AreEqual(0xFF, table.Rows[0].TileNoEntry);

            Assert.AreEqual(0x12, table.Rows[1].TileGrassland);
            Assert.AreEqual(0x23, table.Rows[1].TileDarkGrass);
            Assert.AreEqual(0x24, table.Rows[1].TileForest);
            Assert.AreEqual(0xFF, table.Rows[1].TileNoEntry);

            Assert.AreEqual(0x12, table.Rows[2].TileGrassland);
            Assert.AreEqual(0x23, table.Rows[2].TileDarkGrass);
            Assert.AreEqual(0x25, table.Rows[2].TileForest);
            Assert.AreEqual(0xFF, table.Rows[2].TileNoEntry);

            Assert.AreEqual(14, table.Rows.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var editor = new X1_FileEditor(TestCase.Scenario, false);
            Assert.IsTrue(editor.LoadFile(TestCase.Filename));
            var table = editor.WarpTable;

            Assert.AreEqual(0x07, table.Rows[0].WarpMap);
            Assert.AreEqual(0x00, table.Rows[0].WarpType);

            Assert.AreEqual(0x09, table.Rows[1].WarpMap);
            Assert.AreEqual(0x82, table.Rows[1].WarpType);

            Assert.AreEqual(5, table.Rows.Length);
        }
    }
}
