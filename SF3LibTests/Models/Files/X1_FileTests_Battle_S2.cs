using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Battle_S2 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario2, "X1BTL204.BIN");

        [TestMethod]
        public void AITable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Medion];
            var table = battle.AITargetPositionTable;

            Assert.AreEqual(30, table[0].TargetX);
            Assert.AreEqual(24, table[0].TargetZ);

            Assert.AreEqual(21, table[1].TargetX);
            Assert.AreEqual(21, table[1].TargetZ);

            Assert.AreEqual(32, table.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.BattlePointersTable;

            Assert.AreEqual(0, table[0].Pointer);
            Assert.AreEqual(0x6062AF8, table[1].Pointer);
            Assert.AreEqual(0, table[2].Pointer);
            Assert.AreEqual(0, table[3].Pointer);

            Assert.AreEqual(4, table.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Medion];
            var table = battle.ScriptedMovementTable;

            Assert.AreEqual(21, table[0].XPos1);
            Assert.AreEqual(17, table[0].ZPos1);

            Assert.AreEqual(0, table[1].XPos1);
            Assert.AreEqual(63, table[1].ZPos1);

            Assert.AreEqual(32, table.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Medion];
            var header = battle.BattleHeader;

            Assert.AreEqual(27, header.NumSlots);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Medion];
            var table = battle.SlotTable;

            Assert.AreEqual(0xFFFF, table[0].EnemyID);
            Assert.AreEqual(22, table[0].X);
            Assert.AreEqual(8, table[0].Z);

            Assert.AreEqual(0x45, table[12].EnemyID);
            Assert.AreEqual(26, table[12].X);
            Assert.AreEqual(23, table[12].Z);

            Assert.AreEqual(52, table.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Medion];
            var table = battle.SpawnZoneTable;

            Assert.AreEqual(0x04, table[0].NumPoints);
            Assert.AreEqual(0x00, table[0].Padding0x02);
            Assert.AreEqual(0x17, table[0].X1);
            Assert.AreEqual(0x0C, table[0].Z1);
            Assert.AreEqual(0x17, table[0].X2);

            Assert.AreEqual(0x04, table[1].NumPoints);
            Assert.AreEqual(0x00, table[1].Padding0x02);
            Assert.AreEqual(0x1A, table[1].X1);
            Assert.AreEqual(0x0B, table[1].Z1);
            Assert.AreEqual(0x1A, table[1].X2);

            Assert.AreEqual(16, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(    0x0063,  table[0].Trigger);
            Assert.AreEqual(      0x00,  table[0].TriggerFlags);
            Assert.AreEqual(0x01000065u, table[0].Action);

            Assert.AreEqual(2, table.Length);
        }

        [TestMethod]
        public void TileMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.TileMovementTable;

            Assert.AreEqual(0x12, table[0].Grassland);
            Assert.AreEqual(0x23, table[0].DarkGrass);
            Assert.AreEqual(0x24, table[0].Forest);
            Assert.AreEqual(0xFF, table[0].NoEntry);

            Assert.AreEqual(0x12, table[1].Grassland);
            Assert.AreEqual(0x23, table[1].DarkGrass);
            Assert.AreEqual(0x24, table[1].Forest);
            Assert.AreEqual(0xFF, table[1].NoEntry);

            Assert.AreEqual(0x12, table[2].Grassland);
            Assert.AreEqual(0x23, table[2].DarkGrass);
            Assert.AreEqual(0x25, table[2].Forest);
            Assert.AreEqual(0xFF, table[2].NoEntry);

            Assert.AreEqual(14, table.Length);
        }

        [TestMethod]
        public void WarpTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.WarpTable;

            Assert.AreEqual(0, table[0].LocationID);
            Assert.AreEqual(0, table[0].IfFlagUnset);
            Assert.AreEqual(0, table[0].WarpTrigger);
            Assert.AreEqual(10, table[0].LoadID);

            Assert.AreEqual(1, table[1].LocationID);
            Assert.AreEqual(0xFFF, table[1].IfFlagUnset);
            Assert.AreEqual(1, table[1].WarpTrigger);
            Assert.AreEqual(83, table[1].LoadID);

            Assert.AreEqual(4, table.Length);
        }
    }
}
