using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X1.Battle;
using SF3.Scenes;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class Battle : Struct, IScene, ITableContainer {
        public Battle(IByteData data, int id, string name, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable, ScenarioType scenario, BattlePointerTable battles)
        : base(data, id, name, address, 0x14) {
            MapLeader  = mapLeader;
            Address    = address;
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario   = scenario;
            Battles    = battles;
            SceneName  = $"Battle ({mapLeader})";

            var enemyTableSize = HasLargeEnemyTable ? 0xea0 : 0xa90;

            // Determine addresses of sub-tables.
            var headerAddress         = Address;
            var slotAddress           = headerAddress + 0x0a;
            var zoneAddress           = slotAddress + enemyTableSize;
            var aiAddress             = zoneAddress + 0x120;
            var customMovementAddress = aiAddress + 0x80;
            var mapMoveCoordsAddr     = customMovementAddress + 0x2C0;

            BattleHeader = new BattleHeader(Data, 0, "BattleHeader", headerAddress);

            Tables = new List<ITable>() {
                (SlotTable             = SlotTable.Create            (Data, "Slots",          slotAddress, HasLargeEnemyTable ? 72 : 52, Scenario, Battles, MapLeader)),
                (ZoneTable             = ZoneTable.Create            (Data, "Zones",          zoneAddress)),
                (AITargetPositionTable = AITargetPositionTable.Create(Data, "Positions",      aiAddress)),
                (AITarrgetPathTable    = AITargetPathTable.Create    (Data, "Paths",          customMovementAddress)),
                (MapMoveCoordTable     = MapMoveCoordTable.Create    (Data, "MapMoveCoords",  mapMoveCoordsAddr)),
            };
        }

        public MapLeaderType MapLeader { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public BattlePointerTable Battles { get; }

        [BulkCopyRecurse]
        public BattleHeader BattleHeader { get; private set; }
        [BulkCopyRecurse]
        public SlotTable SlotTable { get; private set; }
        [BulkCopyRecurse]
        public ZoneTable ZoneTable { get; private set; }
        [BulkCopyRecurse]
        public AITargetPositionTable AITargetPositionTable { get; private set; }
        [BulkCopyRecurse]
        public AITargetPathTable AITarrgetPathTable { get; private set; }
        [BulkCopyRecurse]
        public MapMoveCoordTable MapMoveCoordTable { get; private set; }

        public bool IsBattle => true;
        public string SceneName { get; }
        public int NumActors => BattleHeader.NumSlots;
        public IReadOnlyList<IActor> Actors => SlotTable;
        public int NumZones => BattleHeader.NumZones;
        public IReadOnlyList<Zone> Zones => ZoneTable;

        public IEnumerable<ITable> Tables { get; }
    }
}
