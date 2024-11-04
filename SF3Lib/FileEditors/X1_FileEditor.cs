using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.FileEditors {
    public class X1_FileEditor : SF3FileEditor, IX1_FileEditor {
        public X1_FileEditor(ScenarioType scenario, bool isBTL99) : base(scenario, new NameGetterContext(scenario)) {
            IsBTL99 = isBTL99;
        }

        public override IEnumerable<ITable> MakeTables() {
            var isScn1OrBTL99 = Scenario == ScenarioType.Scenario1 || IsBTL99;

            int sub;
            bool hasLargeEnemyTable;

            int treasureAddress;
            int warpAddress;
            int battlePointersPointerAddress; // the address to the pointer to the table of battle pointers
            int npcAddress;
            int enterAddress;
            int arrowAddress;

            int battlePointersAddress;
            int tileMovementAddress;

            var battlePointersPointerPointerAddress = isScn1OrBTL99 ? 0x0018 : 0x0024;
            sub = IsBTL99 ? 0x06060000 : (Scenario == ScenarioType.Scenario1) ? 0x0605f000 : 0x0605e000;

            battlePointersPointerAddress = GetDouble(battlePointersPointerPointerAddress) - sub;
            battlePointersAddress = GetDouble(battlePointersPointerAddress);

            // A value higher means a pointer is on the expLimitAddress, meaning we are in a battle. If it is not a
            // pointer we are at our destination so we know a town is loaded.
            if ((Scenario == ScenarioType.Scenario1 && battlePointersAddress > 0x0605F000) || battlePointersAddress > 0x0605e000) {
                battlePointersAddress -= sub;
                IsBattle = true;
            }
            else {
                battlePointersAddress = -1;
                IsBattle = false;
            }

            if (isScn1OrBTL99) {
                hasLargeEnemyTable = true;

                treasureAddress      = GetDouble(0x000c) - sub;
                warpAddress          = -1; // X002 editor has Scenario1 WarpTable, and provides the address itself.
                npcAddress           = (IsBattle == true) ? -1 : battlePointersPointerAddress; // same address
                enterAddress         = GetDouble(0x0024) - sub;
                arrowAddress         = -1; // Not present in Scenario1
            }
            else {
                hasLargeEnemyTable = false;

                treasureAddress      = GetDouble(0x000c) - sub;
                warpAddress          = GetDouble(0x0018) - sub;
                npcAddress           = (IsBattle == true) ? -1 : battlePointersPointerAddress; // same address
                enterAddress         = GetDouble(0x0030) - sub;
                arrowAddress         = GetDouble(0x0060) - sub;
            }

            // If this is a battle, we need to get the addresses for a lot of battle-specific stuff.
            // TODO: we should have a table of tables here!!
            if (IsBattle == true) {
                // Load the BattlePointersTable early so we can use it to determine the addresses of other tables.
                BattlePointersTable = new BattlePointersTable(this, ResourceFile("BattlePointersList.xml"), battlePointersAddress);
                BattlePointersTable.Load();

                // Get the address of the selected battle, or, if it's not available, the first available in the BattlePointersTable.
                this.BattleTables = new Dictionary<MapLeaderType, BattleTable>();
                foreach (var mapLeader in (MapLeaderType[]) Enum.GetValues(typeof(MapLeaderType))) {
                    int mapIndex = (int) mapLeader;
                    var battleTableAddress = BattlePointersTable.Rows[mapIndex].BattlePointer;
                    if (battleTableAddress != 0)
                        BattleTables.Add(mapLeader, new BattleTable(this, mapLeader, battleTableAddress - sub, hasLargeEnemyTable));
                }

                // Determine the location of the TileMovementTable, which isn't so straight-forward.
                // This table is not present in Scenario 1.
                if (!isScn1OrBTL99) {
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
                BattleTables = null;
                tileMovementAddress = -1;
            }

            // Add tables present outside of the battle tables.
            var tables = new List<ITable>();
            if (treasureAddress >= 0)
                tables.Add(TreasureTable = new TreasureTable(this, ResourceFile("X1Treasure.xml"), treasureAddress));
            if (warpAddress >= 0)
                tables.Add(WarpTable = new WarpTable(this, null, warpAddress));
            if (battlePointersAddress >= 0)
                tables.Add(BattlePointersTable);
            if (npcAddress >= 0)
                tables.Add(NpcTable = new NpcTable(this, ResourceFile("X1Npc.xml"), npcAddress));
            if (enterAddress >= 0)
                tables.Add(EnterTable = new EnterTable(this, ResourceFile("X1Enter.xml"), enterAddress));
            if (arrowAddress >= 0)
                tables.Add(ArrowTable = new ArrowTable(this, ResourceFile("X1Arrow.xml"), arrowAddress));

            // Add tables for battle tables.
            if (BattleTables != null)
                tables.AddRange(BattleTables.SelectMany(x => x.Value.Tables));

            if (tileMovementAddress >= 0)
                tables.Add(TileMovementTable = new TileMovementTable(this, ResourceFile("MovementTypes.xml"), tileMovementAddress));

            return tables;
        }

        public override void DestroyTables() {
            IsBattle            = null;

            TreasureTable       = null;
            WarpTable           = null;
            BattlePointersTable = null;
            NpcTable            = null;
            EnterTable          = null;
            ArrowTable          = null;

            if (BattleTables != null) {
                BattleTables.Clear();
                BattleTables = null;
            }

            TileMovementTable   = null;
        }

        protected override string BaseTitle => IsLoaded
            ? base.BaseTitle + " (Type: " + (IsBTL99 ? "BTL99" : (IsBattle == true) ? "Battle" : "Town") + ")"
            : base.BaseTitle;

        public bool IsBTL99 { get; }

        private bool? _isBattle;

        public bool? IsBattle {
            get => _isBattle;
            set {
                if (_isBattle != value) {
                    _isBattle = value;
                    UpdateTitle();
                }
            }
        }

        [BulkCopyRecurse]
        public TreasureTable TreasureTable { get; private set; }
        [BulkCopyRecurse]
        public WarpTable WarpTable { get; private set; }
        [BulkCopyRecurse]
        public BattlePointersTable BattlePointersTable { get; private set; }
        [BulkCopyRecurse]
        public NpcTable NpcTable { get; private set; }
        [BulkCopyRecurse]
        public EnterTable EnterTable { get; private set; }
        [BulkCopyRecurse]
        public ArrowTable ArrowTable { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<MapLeaderType, BattleTable> BattleTables { get; private set; }

        [BulkCopyRecurse]
        public TileMovementTable TileMovementTable { get; private set; }
    }
}
