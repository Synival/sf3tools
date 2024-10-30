using System.Collections.Generic;
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

            // Add tables present for both towns and battles.
            var tables = new List<ITable> {
                (TreasureTable = new TreasureTable(this))
            };

            if (isntScn1OrBTL99)
                tables.Add(WarpTable = new WarpTable(this));

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
                    tables.Add(ArrowTable = new ArrowTable(this));
            }

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

        public SlotTable SlotTable { get; private set; }
        public HeaderTable HeaderTable { get; private set; }
        public AITable AITable { get; private set; }
        public SpawnZoneTable SpawnZoneTable { get; private set; }
        public BattlePointersTable BattlePointersTable { get; private set; }
        public TreasureTable TreasureTable { get; private set; }
        public CustomMovementTable CustomMovementTable { get; private set; }
        public WarpTable WarpTable { get; private set; }
        public TileMovementTable TileMovementTable { get; private set; }
        public NpcTable NpcTable { get; private set; }
        public EnterTable EnterTable { get; private set; }
        public ArrowTable ArrowTable { get; private set; }
    }
}
