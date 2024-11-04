using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.FileEditors {
    public class X1_FileEditor : SF3FileEditor, IX1_FileEditor {
        public X1_FileEditor(ScenarioType scenario, MapLeaderType mapLeader, bool isBTL99) : base(scenario, new NameGetterContext(scenario)) {
            MapLeader = mapLeader;
            IsBTL99 = isBTL99;
        }

        public override bool OnLoadBeforeMakeTables() {
            var battlePointersPointerPointerAddress = (IsBTL99 || Scenario == ScenarioType.Scenario1) ? 0x0018 : 0x0024;
            var sub = IsBTL99 ? 0x06060000 : (Scenario == ScenarioType.Scenario1) ? 0x0605f000 : 0x0605e000;

            var battlePointersPointerAddress = GetDouble(battlePointersPointerPointerAddress) - sub;
            var battlePointersAddress = GetDouble(battlePointersPointerAddress);

            // A value higher means a pointer is on the expLimitAddress, meaning we are in a battle. If it is not a
            // pointer we are at our destination so we know a town is loaded.
            IsBattle = (Scenario == ScenarioType.Scenario1 && battlePointersAddress > 0x0605F000) || battlePointersAddress > 0x0605e000;
            return true;
        }

        public override IEnumerable<ITable> MakeTables() {
            var isScn1OrBTL99 = Scenario == ScenarioType.Scenario1 || IsBTL99;

            int sub;
            int enemySpawnTableSize;

            int treasureAddress;
            int warpAddress;
            int battlePointersPointerAddress; // the address to the pointer to the table of battle pointers
            int npcAddress;
            int enterAddress;
            int arrowAddress;

            int battlePointersAddress;
            int battleAddress;

            int headerAddress;
            int slotAddress;
            int spawnZoneAddress;
            int aiAddress;
            int customMovementAddress;

            int tileMovementAddress;

            if (isScn1OrBTL99) {
                sub = IsBTL99 ? 0x06060000 : 0x0605f000;
                enemySpawnTableSize  = 0xe9a;
                treasureAddress      = GetDouble(0x000c) - sub;
                warpAddress          = -1; // X002 editor has Scenario1 WarpTable, and provides the address itself.
                battlePointersPointerAddress = GetDouble(0x0018) - sub;
                npcAddress           = battlePointersPointerAddress; // same address
                enterAddress         = GetDouble(0x0024) - sub;
                arrowAddress         = -1; // Not present in Scenario1
            }
            else {
                sub = 0x0605e000;
                enemySpawnTableSize = 0xa8a;

                treasureAddress      = GetDouble(0x000c) - sub;
                warpAddress          = GetDouble(0x0018) - sub;
                battlePointersPointerAddress = GetDouble(0x0024) - sub;
                npcAddress           = battlePointersPointerAddress; // same address
                enterAddress         = GetDouble(0x0030) - sub;
                arrowAddress         = GetDouble(0x0060) - sub;
            }

            // If this is a battle, we need to get the addresses for a lot of battle-specific stuff.
            // TODO: we should have a table of tables here!!
            int mapIndex;
            if (IsBattle) {
                // Load the BattlePointersTable early so we can use it to determine the addresses of other tables.
                battlePointersAddress = GetDouble(battlePointersPointerAddress) - sub;
                BattlePointersTable = new BattlePointersTable(this, ResourceFile("BattlePointersList.xml"), battlePointersAddress);
                BattlePointersTable.Load();

                // Get the address of the selected battle, or, if it's not available, the first available in the BattlePointersTable.
                mapIndex = (BattlePointersTable.Rows[MapIndex].BattlePointer != 0)
                    ? MapIndex
                    : BattlePointersTable.Rows.Select((v, i) => new { v, i }).First(x => x.v.BattlePointer != 0).i;

                battleAddress = BattlePointersTable.Rows[mapIndex].BattlePointer - sub;

                // Determine addresses of other tables.
                headerAddress         = battleAddress;
                slotAddress           = headerAddress + 0x0a;
                spawnZoneAddress      = slotAddress + enemySpawnTableSize + 0x06;
                aiAddress             = spawnZoneAddress + 0x120;
                customMovementAddress = aiAddress + 0x84;

                // Determine the location of the TileMovementTable, which isn't so straight-forward.
                // This table is not present in Scenario 1.
                if (Scenario != ScenarioType.Scenario1) {
                    // First, look inside a function for its address.
                    // The value we want is 0xac bytes later always (except for X1BTL330-339 and X1BTLP05)
                    int tileMovementAddressPointer = GetDouble(0x000001c4) - sub + 0x00ac;

                    // No problems with this method in Scenario 2.
                    if (Scenario == ScenarioType.Scenario2)
                        tileMovementAddress = GetDouble(tileMovementAddressPointer) - sub;
                    else {
                        tileMovementAddress = GetDouble(tileMovementAddressPointer);

                        // Is this a valid pointer to memory?
                        if (tileMovementAddress < 0x06070000 && tileMovementAddress > 0)
                            tileMovementAddress -= sub;
                        // If not, emply the workaround for X1BTL330-339 and X1BTLP05 not being consistant with everything else
                        // and locate the table directly.
                        // TODO: does this pointer exist in other X1BTL* files?
                        else
                            tileMovementAddress = GetDouble(0x0024) - sub + 0x14;
                    }
                }
                else
                    tileMovementAddress = -1;
            }
            else {
                // No battle, so none of these tables exist.
                battlePointersAddress = -1;
                mapIndex              = -1;
                battleAddress         = -1;

                headerAddress         = -1;
                slotAddress           = -1;
                spawnZoneAddress      = -1;
                aiAddress             = -1;
                customMovementAddress = -1;

                tileMovementAddress = -1;
            }

            // Add tables present for both towns and battles.
            var tables = new List<ITable> {
                (TreasureTable = new TreasureTable(this, ResourceFile("X1Treasure.xml"), treasureAddress))
            };

            // Add tables only present for battles.
            if (IsBattle) {
                tables.AddRange(new List<ITable>() {
                    (HeaderTable         = new HeaderTable(this, ResourceFile("X1Top.xml"), headerAddress, mapIndex * 0x04)),
                    (SlotTable           = new SlotTable(this, ResourceFile(Scenario == ScenarioType.Scenario1 ? "X1List.xml" : "X1OtherList.xml"), slotAddress)),
                    (AITable             = new AITable(this, ResourceFile("X1AI.xml"), aiAddress)),
                    (SpawnZoneTable      = new SpawnZoneTable(this, ResourceFile("UnknownAIList.xml"), spawnZoneAddress)),
                    BattlePointersTable,
                    (CustomMovementTable = new CustomMovementTable(this, ResourceFile("X1AI.xml"), customMovementAddress)),
                });

                if (!isScn1OrBTL99)
                    tables.Add(TileMovementTable = new TileMovementTable(this, ResourceFile("MovementTypes.xml"), tileMovementAddress));
            }
            // Add tables only present outside of battle.
            else {
                tables.AddRange(new List<ITable>() {
                    (NpcTable = new NpcTable(this, ResourceFile("X1Npc.xml"), npcAddress)),
                    (EnterTable = new EnterTable(this, ResourceFile("X1Enter.xml"), enterAddress))
                });

                if (!isScn1OrBTL99)
                    tables.Add(ArrowTable = new ArrowTable(this, ResourceFile("X1Arrow.xml"), arrowAddress));
            }

            if (!isScn1OrBTL99)
                tables.Add(WarpTable = new WarpTable(this, null, warpAddress));

            return tables;
        }

        public override void DestroyTables() {
            SlotTable           = null;
            HeaderTable         = null;
            AITable             = null;
            SpawnZoneTable      = null;
            BattlePointersTable = null;
            TreasureTable       = null;
            CustomMovementTable = null;
            WarpTable           = null;
            TileMovementTable   = null;
            NpcTable            = null;
            EnterTable          = null;
            ArrowTable          = null;
        }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (Map: " + MapLeader.ToString() + ") (Type: " + (IsBTL99 ? "BTL99" : IsBattle ? "Battle" : "Town") + ")"
            : base.BaseTitle;

        private MapLeaderType _mapLeader;

        public MapLeaderType MapLeader {
            get => _mapLeader;
            set {
                if (_mapLeader != value) {
                    _mapLeader = value;
                    UpdateTitle();
                }
            }
        }

        public int MapIndex => (int) MapLeader;
        public int MapOffset => MapIndex * 4;

        private bool _isBattle;

        public bool IsBattle {
            get => _isBattle;
            set {
                if (_isBattle != value) {
                    _isBattle = value;
                    UpdateTitle();
                }
            }
        }

        public bool IsBTL99 { get; }

        [BulkCopyRecurse]
        public SlotTable SlotTable { get; private set; }
        [BulkCopyRecurse]
        public HeaderTable HeaderTable { get; private set; }
        [BulkCopyRecurse]
        public AITable AITable { get; private set; }
        [BulkCopyRecurse]
        public SpawnZoneTable SpawnZoneTable { get; private set; }
        [BulkCopyRecurse]
        public BattlePointersTable BattlePointersTable { get; private set; }
        [BulkCopyRecurse]
        public TreasureTable TreasureTable { get; private set; }
        [BulkCopyRecurse]
        public CustomMovementTable CustomMovementTable { get; private set; }
        [BulkCopyRecurse]
        public WarpTable WarpTable { get; private set; }
        [BulkCopyRecurse]
        public TileMovementTable TileMovementTable { get; private set; }
        [BulkCopyRecurse]
        public NpcTable NpcTable { get; private set; }
        [BulkCopyRecurse]
        public EnterTable EnterTable { get; private set; }
        [BulkCopyRecurse]
        public ArrowTable ArrowTable { get; private set; }
    }
}
