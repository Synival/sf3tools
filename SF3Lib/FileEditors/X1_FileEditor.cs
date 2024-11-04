using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.NamedValues;
using SF3.Tables;
using SF3.Tables.X1;
using SF3.Types;

namespace SF3.FileEditors {
    public class X1_FileEditor : SF3FileEditor, IX1_FileEditor {
        public X1_FileEditor(ScenarioType scenario, MapLeaderType mapLeader, bool isBTL99) : base(scenario, new NameGetterContext(scenario)) {
            MapLeader = mapLeader;
            IsBTL99 = isBTL99;
        }

        public override bool OnLoadBeforeMakeTables() {
            var offset = 0;
            var sub = 0;

            if (IsBTL99) {
                offset = 0x00000018; //btl99 initial pointer
                sub = 0x06060000;
            }
            else if (Scenario == ScenarioType.Scenario1) {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
            }
            else if (Scenario == ScenarioType.Scenario2) {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
            }
            else if (Scenario == ScenarioType.Scenario3) {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
            }
            else if (Scenario == ScenarioType.PremiumDisk) {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
            }

            offset = GetDouble(offset);

            offset -= sub; //first pointer
            offset = GetDouble(offset);

            /*A value higher means a pointer is on the offset, meaning we are in a battle. If it is not a 
              pointer we are at our destination so we know a town is loaded. */
            IsBattle = (Scenario == ScenarioType.Scenario1 && offset > 0x0605F000) || offset > 0x0605e000;

            return true;
        }

        public override IEnumerable<ITable> MakeTables() {
            var isntScn1OrBTL99 = Scenario != ScenarioType.Scenario1 && !IsBTL99;

            int sub;

            int arrowAddress;
            int treasureAddress;
            int warpAddress;

            // X1BTL99.BIN is a special case. It has the same layout for each scenario.
            if (IsBTL99) {
                arrowAddress    = 0; // Not present
                treasureAddress = GetDouble(0x000c) - 0x06060000;
                warpAddress     = 0; // Not present
            }
            else {
                switch (Scenario) {
                    case ScenarioType.Scenario1:
                        arrowAddress    = 0; // Not present in Scenario1
                        warpAddress     = 0; // X002 editor has Scenario1 WarpTable, and provides the address itself.
                        treasureAddress = GetDouble(0x000c) - 0x0605f000;
                        break;

                    case ScenarioType.Scenario2:
                        arrowAddress    = GetDouble(0x0060) - 0x0605e000;
                        warpAddress     = GetDouble(0x0018) - 0x0605e000;
                        treasureAddress = GetDouble(0x000c) - 0x0605e000;
                        break;

                    case ScenarioType.Scenario3:
                        arrowAddress    = GetDouble(0x0060) - 0x0605e000;
                        warpAddress     = GetDouble(0x0018) - 0x0605e000;
                        treasureAddress = GetDouble(0x000c) - 0x0605e000;
                        break;

                    case ScenarioType.PremiumDisk:
                        arrowAddress    = GetDouble(0x0060) - 0x0605e000;
                        warpAddress     = GetDouble(0x0018) - 0x0605e000;
                        treasureAddress = GetDouble(0x000c) - 0x0605e000;
                        break;

                    default:
                        throw new ArgumentException(nameof(Scenario));
                }
            }

            // Add tables present for both towns and battles.
            var tables = new List<ITable> {
                (TreasureTable = new TreasureTable(this, treasureAddress))
            };

            // Add tables only present for battles.
            if (IsBattle) {
                tables.AddRange(new List<ITable>() {
                    (HeaderTable = new HeaderTable(this)),
                    (SlotTable = new SlotTable(this)),
                    (AITable = new AITable(this)),
                    (SpawnZoneTable = new SpawnZoneTable(this)),
                    (BattlePointersTable = new BattlePointersTable(this)),
                    (CustomMovementTable = new CustomMovementTable(this)),
                });

                if (isntScn1OrBTL99)
                    tables.Add(TileMovementTable = new TileMovementTable(this));
            }

            // Add tables only present for towns.
            if (!IsBattle) {
                tables.AddRange(new List<ITable>() {
                    (NpcTable = new NpcTable(this)),
                    (EnterTable = new EnterTable(this))
                });

                if (isntScn1OrBTL99)
                    tables.Add(ArrowTable = new ArrowTable(this, arrowAddress));
            }

            if (isntScn1OrBTL99)
                tables.Add(WarpTable = new WarpTable(this, warpAddress));

            return tables;
        }

        public override void DestroyTables() {
            SlotTable = null;
            HeaderTable = null;
            AITable = null;
            SpawnZoneTable = null;
            BattlePointersTable = null;
            TreasureTable = null;
            CustomMovementTable = null;
            WarpTable = null;
            TileMovementTable = null;
            NpcTable = null;
            EnterTable = null;
            ArrowTable = null;
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

        public int MapOffset => (int) MapLeader;

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
