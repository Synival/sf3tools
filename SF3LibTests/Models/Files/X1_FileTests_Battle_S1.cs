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
            Assert.AreEqual(43, table[0].TargetZ);

            Assert.AreEqual(32, table[1].TargetX);
            Assert.AreEqual(33, table[1].TargetZ);

            Assert.AreEqual(32, table.Length);
        }

        [TestMethod]
        public void BattlePointersTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.BattlePointersTable;

            Assert.AreEqual(0x6061170, table[0].Pointer);
            Assert.AreEqual(0, table[1].Pointer);
            Assert.AreEqual(0, table[2].Pointer);
            Assert.AreEqual(0, table[3].Pointer);

            Assert.AreEqual(4, table.Length);
        }

        [TestMethod]
        public void CustomMovementTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.ScriptedMovementTable;

            Assert.AreEqual(27, table[0].XPos1);
            Assert.AreEqual(41, table[0].ZPos1);

            Assert.AreEqual(32, table[1].XPos1);
            Assert.AreEqual(31, table[1].ZPos1);

            Assert.AreEqual(32, table.Length);
        }

        [TestMethod]
        public void HeaderTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var header = battle.BattleHeader;

            Assert.AreEqual(26, header.NumSlots);
        }

        [TestMethod]
        public void SlotTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.SlotTable;

            Assert.AreEqual(0xFFFF, table[0].EnemyID);
            Assert.AreEqual(33, table[0].X);
            Assert.AreEqual(17, table[0].Z);

            Assert.AreEqual(0x13, table[12].EnemyID);
            Assert.AreEqual(29, table[12].X);
            Assert.AreEqual(43, table[12].Z);

            Assert.AreEqual(72, table.Length);
        }

        [TestMethod]
        public void SpawnZoneTable_HasExpectedData() {
            var file = TestCase.Create();
            var battle = file.Battles[MapLeaderType.Synbios];
            var table = battle.SpawnZoneTable;

            Assert.AreEqual(0x04, table[0].NumPoints);
            Assert.AreEqual(0x00, table[0].Padding0x02);
            Assert.AreEqual(0x1C, table[0].X1);
            Assert.AreEqual(0x2C, table[0].Z1);
            Assert.AreEqual(0x1C, table[0].X2);

            Assert.AreEqual(0x04, table[1].NumPoints);
            Assert.AreEqual(0x00, table[1].Padding0x02);
            Assert.AreEqual(0x15, table[1].X1);
            Assert.AreEqual(0x20, table[1].Z1);
            Assert.AreEqual(0x15, table[1].X2);

            Assert.AreEqual(16, table.Length);
        }

        [TestMethod]
        public void TreasureTable_HasExpectedData() {
            var file = TestCase.Create();
            var table = file.InteractableTable;

            Assert.AreEqual(    0x8013,  table[0].Trigger);
            Assert.AreEqual(      0x00,  table[0].TriggerFlags);
            Assert.AreEqual(0x01000053u, table[0].Action);

            Assert.AreEqual(1, table.Length);
        }
    }
}
