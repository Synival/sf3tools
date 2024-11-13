using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.RawEditors;
using SF3.Tables;
using SF3.Tables.X1.Battle;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Editors.X1 {
    public class BattleEditor : TableEditor {
        protected BattleEditor(IRawEditor editor, INameGetterContext nameContext, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable)
        : base(editor, nameContext) {
            MapLeader = mapLeader;
            Address   = address;
            HasLargeEnemyTable = hasLargeEnemyTable;
        }

        public static BattleEditor Create(IRawEditor editor, INameGetterContext nameContext, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable) {
            var newEditor = new BattleEditor(editor, nameContext, mapLeader, address, hasLargeEnemyTable);
            _ = newEditor.Init();
            return newEditor;
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
                (HeaderTable         = new HeaderTable(Editor, ResourceFile("X1Top.xml"), headerAddress)),
                (SlotTable           = new SlotTable(Editor, ResourceFile(HasLargeEnemyTable ? "X1List.xml" : "X1OtherList.xml"), slotAddress)),
                (SpawnZoneTable      = new SpawnZoneTable(Editor, ResourceFile("UnknownAIList.xml"), spawnZoneAddress)),
                (AITable             = new AITable(Editor, ResourceFile("X1AI.xml"), aiAddress)),
                (CustomMovementTable = new CustomMovementTable(Editor, ResourceFile("X1AI.xml"), customMovementAddress)),
            };
        }

        [BulkCopyRowName]
        public string Name => MapLeader.ToString() + " Battle";

        public MapLeaderType MapLeader { get; }
        public int Address { get; }
        public bool HasLargeEnemyTable { get; }

        [BulkCopyRecurse]
        public HeaderTable HeaderTable { get; private set; }
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
