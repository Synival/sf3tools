using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X1.Battle;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Files.X1 {
    public class Battle : TableFile {
        protected Battle(IByteData data, INameGetterContext nameContext, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable)
        : base(data, nameContext) {
            MapLeader = mapLeader;
            Address   = address;
            HasLargeEnemyTable = hasLargeEnemyTable;
        }

        public static Battle Create(IByteData data, INameGetterContext nameContext, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable) {
            var newFile = new Battle(data, nameContext, mapLeader, address, hasLargeEnemyTable);
            _ = newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var enemySpawnTableSize = HasLargeEnemyTable ? 0xe9a : 0xa8a;

            // Determine addresses of sub-tables.
            var headerAddress         = Address;
            var slotAddress           = headerAddress + 0x0a;
            var spawnZoneAddress      = slotAddress + enemySpawnTableSize + 0x06;
            var aiAddress             = spawnZoneAddress + 0x120;
            var customMovementAddress = aiAddress + 0x84;

            return new List<ITable>() {
                (BattleHeaderTable   = BattleHeaderTable.Create  (Data, "BattleHeader",   ResourceFile("X1Top.xml"), headerAddress)),
                (SlotTable           = SlotTable.Create          (Data, "Slots",          ResourceFile(HasLargeEnemyTable ? "X1List.xml" : "X1OtherList.xml"), slotAddress)),
                (SpawnZoneTable      = SpawnZoneTable.Create     (Data, "SpawnZones",     ResourceFile("UnknownAIList.xml"), spawnZoneAddress)),
                (AITable             = AITable.Create            (Data, "AI",             ResourceFile("X1AI.xml"), aiAddress)),
                (CustomMovementTable = CustomMovementTable.Create(Data, "CustomMovement", ResourceFile("X1AI.xml"), customMovementAddress)),
            };
        }

        [BulkCopyRowName]
        public string Name => MapLeader.ToString() + " Battle";

        public MapLeaderType MapLeader { get; }
        public int Address { get; }
        public bool HasLargeEnemyTable { get; }

        [BulkCopyRecurse]
        public BattleHeaderTable BattleHeaderTable { get; private set; }
        [BulkCopyRecurse]
        public SlotTable SlotTable { get; private set; }
        [BulkCopyRecurse]
        public SpawnZoneTable SpawnZoneTable { get; private set; }
        [BulkCopyRecurse]
        public AITable AITable { get; private set; }
        [BulkCopyRecurse]
        public CustomMovementTable CustomMovementTable { get; private set; }
    }
}
