using CommonLib.Arrays;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class X1_FileTests_Battle_S1 {
        private class X1_TestCase : TestCase {
            public X1_TestCase(
                ScenarioType scenario,
                string filename)
            : base(scenario, filename) {
            }

            public X1_File Create()
                => X1_File.Create(new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(Filename))), new NameGetterContext(Scenario), Scenario, false);
        }

        private static readonly X1_TestCase TestCase = new X1_TestCase(ScenarioType.Scenario1, "X1BTL104.BIN");

        [TestMethod]
        public void AITable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.AITargetPositionTable;

            Assert.AreEqual(29, table[0].TargetX);
            Assert.AreEqual(43, table[0].TargetY);

            Assert.AreEqual(32, table[1].TargetX);
            Assert.AreEqual(33, table[1].TargetY);

            Assert.AreEqual(33, table.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.BattlePointersTable;

            Assert.AreEqual(0x6061170, table[0].BattlePointer);
            Assert.AreEqual(0, table[1].BattlePointer);
            Assert.AreEqual(0, table[2].BattlePointer);
            Assert.AreEqual(0, table[3].BattlePointer);

            Assert.AreEqual(4, table.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.ScriptedMovementTable;

            Assert.AreEqual(27, table[0].CustomMovementX1);
            Assert.AreEqual(41, table[0].CustomMovementZ1);

            Assert.AreEqual(32, table[1].CustomMovementX1);
            Assert.AreEqual(31, table[1].CustomMovementZ1);

            Assert.AreEqual(33, table.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var header = battle.BattleHeader;

            Assert.AreEqual(0, header.Unknown0x00);
            Assert.AreEqual(26, header.NumSlots);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.SlotTable;

            Assert.AreEqual(0xFFFF, table[0].EnemyID);
            Assert.AreEqual(33, table[0].EnemyX);
            Assert.AreEqual(17, table[0].EnemyY);

            Assert.AreEqual(0x13, table[12].EnemyID);
            Assert.AreEqual(29, table[12].EnemyX);
            Assert.AreEqual(43, table[12].EnemyY);

            Assert.AreEqual(72, table.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.SpawnZoneTable;

            Assert.AreEqual(0x400, table[0].UnknownAI00);
            Assert.AreEqual(0x1C, table[0].UnknownAI02);
            Assert.AreEqual(0x2C, table[0].UnknownAI04);
            Assert.AreEqual(0x1C, table[0].UnknownAI06);

            Assert.AreEqual(0x400, table[1].UnknownAI00);
            Assert.AreEqual(0x15, table[1].UnknownAI02);
            Assert.AreEqual(0x20, table[1].UnknownAI04);
            Assert.AreEqual(0x15, table[1].UnknownAI06);

            Assert.AreEqual(16, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.TreasureTable;

            Assert.AreEqual(0x8013, table[0].Searched);
            Assert.AreEqual(0x53, table[0].EventParameter);
            Assert.AreEqual(0x70E, table[0].FlagUsed);
            Assert.AreEqual(0x0F, table[0].EventNumber);

            Assert.AreEqual(0xFFFF, table[1].Searched);
            Assert.AreEqual(0, table[1].EventParameter);
            Assert.AreEqual(0, table[1].FlagUsed);
            Assert.AreEqual(0, table[1].EventNumber);

            Assert.AreEqual(2, table.Length);
        }
    }
}
