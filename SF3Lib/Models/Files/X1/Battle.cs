using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.Actors;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Models.Tables;
using SF3.Models.Tables.X1.Battle;
using SF3.Scenes;
using SF3.Types;

namespace SF3.Models.Files.X1 {
    public class Battle : TableFile, IScene {
        protected Battle(IByteData data, INameGetterContext nameContext, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable, ScenarioType scenario, Battle prevBattle)
        : base(data, nameContext) {
            MapLeader  = mapLeader;
            Address    = address;
            HasLargeEnemyTable = hasLargeEnemyTable;
            Scenario   = scenario;
            PrevBattle = prevBattle;
            SceneName  = $"Battle ({mapLeader})";
        }

        public static Battle Create(IByteData data, INameGetterContext nameContext, MapLeaderType mapLeader, int address, bool hasLargeEnemyTable, ScenarioType scenario, Battle prevBattle) {
            var newFile = new Battle(data, nameContext, mapLeader, address, hasLargeEnemyTable, scenario, prevBattle);
            _ = newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var enemyTableSize = HasLargeEnemyTable ? 0xe9a : 0xa8a;

            // Determine addresses of sub-tables.
            var headerAddress         = Address;
            var slotAddress           = headerAddress + 0x0a;
            var zoneAddress           = slotAddress + enemyTableSize + 0x06;
            var aiAddress             = zoneAddress + 0x120;
            var customMovementAddress = aiAddress + 0x80;

            BattleHeader = new BattleHeader(Data, 0, "BattleHeader", headerAddress);

            return new List<ITable>() {
                (SlotTable             = SlotTable.Create            (Data, "Slots",          slotAddress, HasLargeEnemyTable ? 72 : 52, Scenario, PrevBattle?.SlotTable?.Rows?.Last())),
                (ZoneTable             = ZoneTable.Create            (Data, "Zones",          zoneAddress)),
                (AITargetPositionTable = AITargetPositionTable.Create(Data, "AI",             aiAddress)),
                (ScriptedMovementTable = ScriptedMovementTable.Create(Data, "CustomMovement", customMovementAddress)),
            };
        }

        [BulkCopyRowName]
        public string Name => MapLeader.ToString() + " Battle";

        public MapLeaderType MapLeader { get; }
        public int Address { get; }
        public bool HasLargeEnemyTable { get; }
        public ScenarioType Scenario { get; }
        public Battle PrevBattle { get; }

        [BulkCopyRecurse]
        public BattleHeader BattleHeader { get; private set; }
        [BulkCopyRecurse]
        public SlotTable SlotTable { get; private set; }
        [BulkCopyRecurse]
        public ZoneTable ZoneTable { get; private set; }
        [BulkCopyRecurse]
        public AITargetPositionTable AITargetPositionTable { get; private set; }
        [BulkCopyRecurse]
        public ScriptedMovementTable ScriptedMovementTable { get; private set; }

        public bool IsBattle => true;
        public string SceneName { get; }
        public int NumActors => BattleHeader.NumSlots;
        public IReadOnlyList<IActor> Actors => SlotTable;
        public int NumZones => BattleHeader.NumZones;
        public IReadOnlyList<Zone> Zones => ZoneTable;
    }
}
