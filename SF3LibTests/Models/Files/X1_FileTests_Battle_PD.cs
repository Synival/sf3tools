using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Battle_PD {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.PremiumDisk, "X1BTLP04.BIN");

        [TestMethod]
        public void AITable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.AITable;

            Assert.AreEqual(8, table[0].TargetX);
            Assert.AreEqual(13, table[0].TargetY);

            Assert.AreEqual(7, table[1].TargetX);
            Assert.AreEqual(12, table[1].TargetY);

            Assert.AreEqual(33, table.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = file.BattlePointersTable;

            Assert.AreEqual(0x6061974, table[0].BattlePointer);
            Assert.AreEqual(0, table[1].BattlePointer);
            Assert.AreEqual(0, table[2].BattlePointer);
            Assert.AreEqual(0, table[3].BattlePointer);

            Assert.AreEqual(4, table.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.CustomMovementTable;

            Assert.AreEqual(0, table[0].CustomMovementX1);
            Assert.AreEqual(63, table[0].CustomMovementZ1);

            Assert.AreEqual(0, table[1].CustomMovementX1);
            Assert.AreEqual(63, table[1].CustomMovementZ1);

            Assert.AreEqual(33, table.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.BattleHeaderTable;

            Assert.AreEqual(0, table[0].SizeUnknown1);
            Assert.AreEqual(16, table[0].TableSize);

            Assert.AreEqual(1, table.Length);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.SlotTable;

            Assert.AreEqual(0xFFFF, table[0].EnemyID);
            Assert.AreEqual(9, table[0].EnemyX);
            Assert.AreEqual(5, table[0].EnemyY);

            Assert.AreEqual(0x92, table[12].EnemyID);
            Assert.AreEqual(8, table[12].EnemyX);
            Assert.AreEqual(13, table[12].EnemyY);

            Assert.AreEqual(52, table.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.SpawnZoneTable;

            Assert.AreEqual(0x00, table[0].UnknownAI00);
            Assert.AreEqual(0x00, table[0].UnknownAI02);
            Assert.AreEqual(0x3F, table[0].UnknownAI04);
            Assert.AreEqual(0x00, table[0].UnknownAI06);
            Assert.AreEqual(0x3F, table[0].UnknownAI08);
            Assert.AreEqual(0x00, table[0].UnknownAI0A);

            Assert.AreEqual(0x00, table[1].UnknownAI00);
            Assert.AreEqual(0x00, table[1].UnknownAI02);
            Assert.AreEqual(0x3F, table[1].UnknownAI04);
            Assert.AreEqual(0x00, table[1].UnknownAI06);
            Assert.AreEqual(0x3F, table[1].UnknownAI08);
            Assert.AreEqual(0x00, table[1].UnknownAI0A);

            Assert.AreEqual(16, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.TreasureTable;

            // No treasure to be found in PD, as far as I know.
            Assert.AreEqual(0xFFFF, table[0].Searched);
            Assert.AreEqual(0, table[0].EventParameter);
            Assert.AreEqual(0, table[0].FlagUse);
            Assert.AreEqual(0, table[0].EventNumber);

            Assert.AreEqual(1, table.Length);
        }

        [TestMethod]
        public void TileMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.TileMovementTable;

            Assert.AreEqual(0x12, table[0].TileGrassland);
            Assert.AreEqual(0x23, table[0].TileDarkGrass);
            Assert.AreEqual(0x24, table[0].TileForest);
            Assert.AreEqual(0xFF, table[0].TileNoEntry);

            Assert.AreEqual(0x12, table[1].TileGrassland);
            Assert.AreEqual(0x23, table[1].TileDarkGrass);
            Assert.AreEqual(0x24, table[1].TileForest);
            Assert.AreEqual(0xFF, table[1].TileNoEntry);

            Assert.AreEqual(0x12, table[2].TileGrassland);
            Assert.AreEqual(0x23, table[2].TileDarkGrass);
            Assert.AreEqual(0x25, table[2].TileForest);
            Assert.AreEqual(0xFF, table[2].TileNoEntry);

            Assert.AreEqual(14, table.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.WarpTable;

            Assert.AreEqual(0x07, table[0].WarpMap);
            Assert.AreEqual(0x00, table[0].WarpType);

            Assert.AreEqual(0x09, table[1].WarpMap);
            Assert.AreEqual(0x82, table[1].WarpType);

            Assert.AreEqual(5, table.Length);
        }
    }
}
