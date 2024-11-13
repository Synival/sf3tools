using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.RawEditors;
using SF3.Tables;
using SF3.Tables.X1.Battle;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.X1.Battle {
    public class Battle {
        public Battle(IRawEditor editor, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable) {
            Editor    = editor;
            MapLeader = mapLeader;
            Address   = address;

            var enemySpawnTableSize = hasLargeEnemyTable ? 0xe9a : 0xa8a;

            // Determine addresses of sub-tables.
            var headerAddress         = address;
            var slotAddress           = headerAddress + 0x0a;
            var spawnZoneAddress      = slotAddress + enemySpawnTableSize + 0x06;
            var aiAddress             = spawnZoneAddress + 0x120;
            var customMovementAddress = aiAddress + 0x84;

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

        public IRawEditor Editor { get; }
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
