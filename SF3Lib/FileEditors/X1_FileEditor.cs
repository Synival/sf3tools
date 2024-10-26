using System.Collections.Generic;
using SF3.Models;
using SF3.Models.X1.AI;
using SF3.Models.X1.Arrow;
using SF3.Models.X1.BattlePointers;
using SF3.Models.X1.CustomMovement;
using SF3.Models.X1.Enter;
using SF3.Models.X1.Headers;
using SF3.Models.X1.Npc;
using SF3.Models.X1.Slots;
using SF3.Models.X1.SpawnZones;
using SF3.Models.X1.TileMovement;
using SF3.Models.X1.Treasure;
using SF3.Models.X1.Warp;
using SF3.Types;

namespace SF3.FileEditors {
    public class X1_FileEditor : SF3FileEditor, IX1_FileEditor {
        public X1_FileEditor(ScenarioType scenario, MapLeaderType mapLeader, bool isBTL99) : base(scenario) {
            MapLeader = mapLeader;
            IsBTL99 = isBTL99;
        }

        public override bool OnLoadBeforeMakeModelArrays() {
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

        public override IEnumerable<IModelArray> MakeModelArrays() {
            var isntScn1OrBTL99 = Scenario != ScenarioType.Scenario1 && !IsBTL99;

            // Add models present for both towns and battles.
            var modelArrays = new List<IModelArray> {
                (TreasureList = new TreasureList(this))
            };

            if (isntScn1OrBTL99)
                modelArrays.Add(WarpList = new WarpList(this));

            // Add models only present for battles.
            if (IsBattle) {
                modelArrays.AddRange(new List<IModelArray>() {
                    (HeaderList = new HeaderList(this)),
                    (SlotList = new SlotList(this)),
                    (AIList = new AIList(this)),
                    (SpawnZoneList = new SpawnZoneList(this)),
                    (BattlePointersList = new BattlePointersList(this)),
                    (CustomMovementList = new CustomMovementList(this)),
                });

                if (isntScn1OrBTL99)
                    modelArrays.Add(TileList = new TileList(this));
            }

            // Add models only present for towns.
            if (!IsBattle) {
                modelArrays.AddRange(new List<IModelArray>() {
                    (NpcList = new NpcList(this)),
                    (EnterList = new EnterList(this))
                });

                if (isntScn1OrBTL99)
                    modelArrays.Add(ArrowList = new ArrowList(this));
            }

            return modelArrays;
        }

        public override void DestroyModelArrays() {
            SlotList = null;
            HeaderList = null;
            AIList = null;
            SpawnZoneList = null;
            BattlePointersList = null;
            TreasureList = null;
            CustomMovementList = null;
            WarpList = null;
            TileList = null;
            NpcList = null;
            EnterList = null;
            ArrowList = null;
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

        public SlotList SlotList { get; private set; }
        public HeaderList HeaderList { get; private set; }
        public AIList AIList { get; private set; }
        public SpawnZoneList SpawnZoneList { get; private set; }
        public BattlePointersList BattlePointersList { get; private set; }
        public TreasureList TreasureList { get; private set; }
        public CustomMovementList CustomMovementList { get; private set; }
        public WarpList WarpList { get; private set; }
        public TileList TileList { get; private set; }
        public NpcList NpcList { get; private set; }
        public EnterList EnterList { get; private set; }
        public ArrowList ArrowList { get; private set; }
    }
}
