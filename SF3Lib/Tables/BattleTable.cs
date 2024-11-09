using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Tables {
    public class BattleTable {
        public BattleTable(IByteEditor editor, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable) {
            Editor    = editor;
            MapLeader = mapLeader;
            Address   = address;

            int enemySpawnTableSize = hasLargeEnemyTable ? 0xe9a : 0xa8a;

            // Determine addresses of sub-tables.
            int headerAddress         = address;
            int slotAddress           = headerAddress + 0x0a;
            int spawnZoneAddress      = slotAddress + enemySpawnTableSize + 0x06;
            int aiAddress             = spawnZoneAddress + 0x120;
            int customMovementAddress = aiAddress + 0x84;

            HeaderTable         = new HeaderTable(editor, ResourceFile("X1Top.xml"), headerAddress);
            SlotTable           = new SlotTable(editor, ResourceFile(hasLargeEnemyTable ? "X1List.xml" : "X1OtherList.xml"), slotAddress);
            SpawnZoneTable      = new SpawnZoneTable(editor, ResourceFile("UnknownAIList.xml"), spawnZoneAddress);
            AITable             = new AITable(editor, ResourceFile("X1AI.xml"), aiAddress);
            CustomMovementTable = new CustomMovementTable(editor, ResourceFile("X1AI.xml"), customMovementAddress);

            Tables = new List<ITable>() {
                HeaderTable, SlotTable, SpawnZoneTable, AITable, CustomMovementTable
            };
        }

        [BulkCopyRowName]
        public string Name => MapLeader.ToString() + " Battle";

        public IByteEditor Editor { get; }
        public MapLeaderType MapLeader { get; }
        public int Address { get; }

        public List<ITable> Tables { get; }

        [BulkCopyRecurse]
        public HeaderTable HeaderTable { get; }
        [BulkCopyRecurse]
        public SlotTable SlotTable { get; }
        [BulkCopyRecurse]
        public SpawnZoneTable SpawnZoneTable { get; }
        [BulkCopyRecurse]
        public AITable AITable { get; }
        [BulkCopyRecurse]
        public CustomMovementTable CustomMovementTable { get; }
    }
}
