using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Battle_S3 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario3, "X1BTL304.BIN");
        private static readonly X1_TestCase TestCase2 = new X1_TestCase(ScenarioType.Scenario3, "X1BTL305.BIN");

        [TestMethod]
        public void AITable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Julian];
            var table = battle.AITargetPositionTable;

            Assert.AreEqual(12, table[0].TargetX);
            Assert.AreEqual(7, table[0].TargetY);

            Assert.AreEqual(5, table[1].TargetX);
            Assert.AreEqual(35, table[1].TargetY);

            Assert.AreEqual(33, table.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.BattlePointersTable;

            Assert.AreEqual(0, table[0].Pointer);
            Assert.AreEqual(0, table[1].Pointer);
            Assert.AreEqual(0x6063E54, table[2].Pointer);
            Assert.AreEqual(0, table[3].Pointer);

            Assert.AreEqual(4, table.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Julian];
            var table = battle.ScriptedMovementTable;

            Assert.AreEqual(0, table[0].XPos1);
            Assert.AreEqual(63, table[0].ZPos1);

            Assert.AreEqual(0, table[1].XPos1);
            Assert.AreEqual(63, table[1].ZPos1);

            Assert.AreEqual(33, table.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Julian];
            var header = battle.BattleHeader;

            Assert.AreEqual(0, header.Unknown0x00);
            Assert.AreEqual(27, header.NumSlots);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Julian];
            var table = battle.SlotTable;

            Assert.AreEqual(0xFFFF, table[0].EnemyID);
            Assert.AreEqual(12, table[0].EnemyX);
            Assert.AreEqual(37, table[0].EnemyY);

            Assert.AreEqual(0x98, table[12].EnemyID);
            Assert.AreEqual(12, table[12].EnemyX);
            Assert.AreEqual(8, table[12].EnemyY);

            Assert.AreEqual(52, table.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Julian];
            var table = battle.SpawnZoneTable;

            Assert.AreEqual(0x400, table[0].Unknown0x00);
            Assert.AreEqual(0x00, table[0].BottomLeftX);
            Assert.AreEqual(0x00, table[0].BottomLeftZ);
            Assert.AreEqual(0x00, table[0].TopLeftX);
            Assert.AreEqual(0x12, table[0].TopLeftZ);
            Assert.AreEqual(0x18, table[0].TopRightX);

            Assert.AreEqual(0x400, table[1].Unknown0x00);
            Assert.AreEqual(0x00, table[1].BottomLeftX);
            Assert.AreEqual(0x00, table[1].BottomLeftZ);
            Assert.AreEqual(0x00, table[1].TopLeftX);
            Assert.AreEqual(0x1E, table[1].TopLeftZ);
            Assert.AreEqual(0x18, table[1].TopRightX);

            Assert.AreEqual(16, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase2.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(0x13, table[0].Searched);
            Assert.AreEqual(0xEA, table[0].EventParameter);
            Assert.AreEqual(0xF0F, table[0].FlagUsed);
            Assert.AreEqual(0x0F, table[0].EventNumber);

            Assert.AreEqual(0xC013, table[1].Searched);
            Assert.AreEqual(0x65, table[1].EventParameter);
            Assert.AreEqual(0xF10, table[1].FlagUsed);
            Assert.AreEqual(14, table[1].EventNumber);

            Assert.AreEqual(0xFFFF, table[3].Searched);
            Assert.AreEqual(0, table[3].EventParameter);
            Assert.AreEqual(0, table[3].FlagUsed);
            Assert.AreEqual(0, table[3].EventNumber);

            Assert.AreEqual(4, table.Length);
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

            Assert.AreEqual(0, table[0].EntranceID);
            Assert.AreEqual(0, table[0].Flag);
            Assert.AreEqual(0, table[0].WarpTrigger);
            Assert.AreEqual(7, table[0].LoadID);

            Assert.AreEqual(5, table[1].EntranceID);
            Assert.AreEqual(0xFFF, table[1].Flag);
            Assert.AreEqual(1, table[1].WarpTrigger);
            Assert.AreEqual(126, table[1].LoadID);

            Assert.AreEqual(5, table.Length);
        }
    }
}
