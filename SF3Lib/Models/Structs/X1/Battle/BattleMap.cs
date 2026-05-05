using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.X1.Battle;
using SF3.Scenes;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class BattleMap : Struct, IScene, ITableContainer {
        public BattleMap(IByteData data, int id, string name, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable, ScenarioType scenario, BattleHeader battleHeader)
        : base(data, id, name, address, 0x14) {
            MapLeader  = mapLeader;
            Address    = address;
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario   = scenario;
            BattleHeader = battleHeader;
            SceneName  = $"Battle ({mapLeader})";

            var enemyTableSize = HasLargeEnemyTable ? 0xea0 : 0xa90;

            // Determine addresses of sub-tables.
            var headerAddress        = Address;
            var unitsAddress         = headerAddress    + 0x0a;
            var zonesAddress         = unitsAddress     + enemyTableSize;
            var locationsAddress     = zonesAddress     + 0x120;
            var pathsAddress         = locationsAddress + 0x80;
            var mapMoveCoordsAddress = pathsAddress     + 0x2C0;

            Header = new BattleMapHeader(Data, 0, nameof(BattleMapHeader), headerAddress);

            Tables = new List<ITable>() {
                (UnitTable          = UnitTable.Create        (Data, "Units",         unitsAddress, HasLargeEnemyTable ? 72 : 52, Scenario, BattleHeader, MapLeader)),
                (ZoneTable          = ZoneTable.Create        (Data, "Zones",         zonesAddress)),
                (LocationTable      = LocationTable.Create    (Data, "Locations",     locationsAddress)),
                (PathTable          = PathTable.Create        (Data, "Paths",         pathsAddress)),
                (MapMoveTargetTable = MapMoveCoordTable.Create(Data, "MapMoveCoords", mapMoveCoordsAddress, MapLeader, BattleHeader)),
            };
        }

        public MapLeaderType MapLeader { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public BattleHeader BattleHeader { get; }

        [BulkCopyRecurse]
        public BattleMapHeader Header { get; private set; }
        [BulkCopyRecurse]
        public UnitTable UnitTable { get; private set; }
        [BulkCopyRecurse]
        public ZoneTable ZoneTable { get; private set; }
        [BulkCopyRecurse]
        public LocationTable LocationTable { get; private set; }
        [BulkCopyRecurse]
        public PathTable PathTable { get; private set; }
        [BulkCopyRecurse]
        public MapMoveCoordTable MapMoveTargetTable { get; private set; }

        public bool IsBattle => true;
        public string SceneName { get; }
        public int NumActors => Header.NumUnits;
        public IReadOnlyList<IActor> Actors => UnitTable;
        public int NumZones => Header.NumZones;
        public IReadOnlyList<Zone> Zones => ZoneTable;

        public IEnumerable<ITable> Tables { get; }
    }
}
