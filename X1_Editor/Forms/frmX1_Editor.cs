using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X1_Editor.Forms {
    public partial class frmX1_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.36";

        private bool _isBTL99 = false;

        public bool IsBTL99 {
            get => _isBTL99;
            set {
                if (_isBTL99 != value) {
                    _isBTL99 = value;
                    tsmiScenario_BTL99.Checked = _isBTL99;
                }
            }
        }

        public new IX1_FileEditor FileEditor => base.FileEditor as IX1_FileEditor;

        private MapLeaderType _mapLeader = (MapLeaderType) (-1); // uninitialized value

        private MapLeaderType MapLeader {
            get => _mapLeader;
            set {
                if (_mapLeader != value) {
                    _mapLeader = value;
                    tsmiMap_MapSynbios.Checked = _mapLeader == MapLeaderType.Synbios;
                    tsmiMap_MapMedion.Checked  = _mapLeader == MapLeaderType.Medion;
                    tsmiMap_MapJulian.Checked  = _mapLeader == MapLeaderType.Julian;
                    tsmiMap_MapExtra.Checked   = _mapLeader == MapLeaderType.Extra;
                }
            }
        }

        public frmX1_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip1);
            MapLeader = MapLeaderType.Synbios;
        }

        protected override string FileDialogFilter
            => (IsBTL99 ? "SF3 Data (X1BTL99.BIN)|X1BTL99.BIN|" : "SF3 Data (X1*.BIN)|X1*.BIN|") + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X1_FileEditor(Scenario, MapLeader, IsBTL99);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
                new PopulateTabConfig(tabHeader, olvHeader, FileEditor.HeaderList),
                new PopulateTabConfig(tabSlotTab1, olvSlotTab1, FileEditor.SlotList),
                new PopulateTabConfig(tabSlotTab2, olvSlotTab2, FileEditor.SlotList),
                new PopulateTabConfig(tabSlotTab3, olvSlotTab3, FileEditor.SlotList),
                new PopulateTabConfig(tabSlotTab4, olvSlotTab4, FileEditor.SlotList),
                new PopulateTabConfig(tabAITargetPosition, olvAITargetPosition, FileEditor.AIList),
                new PopulateTabConfig(tabSpawnZones, olvSpawnZones, FileEditor.SpawnZoneList),
                new PopulateTabConfig(tabBattlePointers, olvBattlePointers, FileEditor.BattlePointersList),
                new PopulateTabConfig(tabScriptedMovement, olvScriptedMovement, FileEditor.CustomMovementList),
                new PopulateTabConfig(tabInteractables, olvInteractables, FileEditor.TreasureList),
                new PopulateTabConfig(tabTownNpcs, olvTownNpcs, FileEditor.NpcList),
                new PopulateTabConfig(tabNonBattleEnter, olvNonBattleEnter, FileEditor.EnterList),
                new PopulateTabConfig(tabArrows, olvArrows, FileEditor.ArrowList),
                new PopulateTabConfig(tabWarpTable, olvWarpTable, FileEditor.WarpList),
                new PopulateTabConfig(tabTileData, olvTileData, FileEditor.TileList),
            });
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        protected override void tsmiScenario_Scenario1_Click(object sender, EventArgs e) {
            base.tsmiScenario_Scenario1_Click(sender, e);
            if (!IsBTL99)
                MapLeader = MapLeaderType.Synbios;
        }

        protected override void tsmiScenario_Scenario2_Click(object sender, EventArgs e) {
            base.tsmiScenario_Scenario2_Click(sender, e);
            if (!IsBTL99)
                MapLeader = MapLeaderType.Medion;
        }

        protected override void tsmiScenario_Scenario3_Click(object sender, EventArgs e) {
            base.tsmiScenario_Scenario3_Click(sender, e);
            if (!IsBTL99)
                MapLeader = MapLeaderType.Julian;
        }

        protected override void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) {
            base.tsmiScenario_PremiumDisk_Click(sender, e);
            if (!IsBTL99)
                MapLeader = MapLeaderType.Synbios;
        }

        private void tsmiScenario_BTL99_Click(object sender, EventArgs e) {
            IsBTL99 = !IsBTL99;
            if (IsBTL99)
                MapLeader = MapLeaderType.Synbios;
        }

        private void tsmiMap_MapSynbios_Click(object sender, EventArgs e) => MapLeader = MapLeaderType.Synbios; //map with synbios as lead
        private void tsmiMap_MapMedion_Click(object sender, EventArgs e) => MapLeader = MapLeaderType.Medion; //map with medion as lead
        private void tsmiMap_MapJulian_Click(object sender, EventArgs e) => MapLeader = MapLeaderType.Julian; //map with julian as lead
        private void tsmiMap_MapExtra_Click(object sender, EventArgs e) => MapLeader = MapLeaderType.Extra; //map with no lead or a extra as lead. also for ruins

        private void tsmiHelp_TreasureDebugToggle_Click(object sender, EventArgs e) {
            Tables.X1.Treasure.TreasureTable.Debug = !Tables.X1.Treasure.TreasureTable.Debug;
            tsmiHelp_TreasureDebugToggle.Checked = Tables.X1.Treasure.TreasureTable.Debug;
        }
    }
}
