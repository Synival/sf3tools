using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Extensions;
using SF3.Win.Extensions;
using SF3.Win.Forms;
using SF3.ModelLoaders;
using SF3.NamedValues;
using SF3.Types;
using SF3.X1_Editor.Controls;
using static SF3.Win.Extensions.TabControlExtensions;
using SF3.Models.Files;
using SF3.Models.Files.X1;

namespace SF3.X1_Editor.Forms {
    public partial class frmX1_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.38";

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

        public IX1_File File => base.FileLoader.Model as IX1_File;

        public frmX1_Editor() {
            InitializeComponent();

            // Gather all tables in our battleEditors.
            // TODO: InitializeEditor() should do just do this recursively
            var battleEditors = new List<BattleEditorControl>() {
                becBattle_Synbios,
                becBattle_Medion,
                becBattle_Julian,
                becBattle_Extra
            };
            var battleEditorOLVs = battleEditors.SelectMany(x => x.GetAllObjectsOfTypeInFields<ObjectListView>(false)).ToList();

            // Synchronize the tabs in the battle editors
            void tabSyncFunc(object sender, TabControlEventArgs e) {
                foreach (var bec in battleEditors)
                    bec.Tabs.SelectedIndex = e.TabPageIndex;
            };
            foreach (var bec in battleEditors)
                bec.Tabs.Selected += tabSyncFunc;

            InitializeEditor(menuStrip2, battleEditorOLVs);
        }

        protected override string FileDialogFilter
            => (IsBTL99 ? "SF3 Data (X1BTL99.BIN)|X1BTL99.BIN|" : "SF3 Data (X1*.BIN)|X1*.BIN|") + base.FileDialogFilter;

        protected override IBaseFile CreateModel(IModelFileLoader loader)
            => X1_File.Create(loader.ByteData, new NameGetterContext(Scenario), Scenario, IsBTL99);

        private class PopulateBattleTabConfig : IPopulateTabConfig {
            public PopulateBattleTabConfig(TabPage tabPage, Dictionary<MapLeaderType, X1_FileBattle> battleTable, MapLeaderType mapLeader) {
                TabPage = tabPage;
                BattleTable = battleTable != null && battleTable.ContainsKey(mapLeader) ? battleTable[mapLeader] : null;
            }

            public TabPage TabPage { get; }
            public X1_FileBattle BattleTable { get; }

            public bool CanPopulate => BattleTable != null;

            public bool Populate() {
                var bec = TabPage.Controls[0] as BattleEditorControl;
                return bec.Tabs.PopulateTabs(new List<IPopulateTabConfig>() {
                    new PopulateOLVTabConfig(bec.TabHeader,           bec.OLVHeader,           BattleTable.BattleHeaderTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab1,         bec.OLVSlotTab1,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab2,         bec.OLVSlotTab2,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab3,         bec.OLVSlotTab3,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSlotTab4,         bec.OLVSlotTab4,         BattleTable.SlotTable),
                    new PopulateOLVTabConfig(bec.TabSpawnZones,       bec.OLVSpawnZones,       BattleTable.SpawnZoneTable),
                    new PopulateOLVTabConfig(bec.TabAITargetPosition, bec.OLVAITargetPosition, BattleTable.AITable),
                    new PopulateOLVTabConfig(bec.TabScriptedMovement, bec.OLVScriptedMovement, BattleTable.CustomMovementTable)
                });
            }
        }

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            return tabMain.PopulateAndToggleTabs(new List<IPopulateTabConfig>() {
                new PopulateOLVTabConfig(tabInteractables, olvInteractables, File.TreasureTable),
                new PopulateOLVTabConfig(tabBattlePointers, olvBattlePointers, File.BattlePointersTable),
                new PopulateOLVTabConfig(tabTownNpcs, olvTownNpcs, File.NpcTable),
                new PopulateOLVTabConfig(tabNonBattleEnter, olvNonBattleEnter, File.EnterTable),
                new PopulateOLVTabConfig(tabWarpTable, olvWarpTable, File.WarpTable),
                new PopulateOLVTabConfig(tabArrows, olvArrows, File.ArrowTable),
                new PopulateOLVTabConfig(tabTileData, olvTileData, File.TileMovementTable),
                new PopulateBattleTabConfig(tabBattle_Synbios, File.Battles, MapLeaderType.Synbios),
                new PopulateBattleTabConfig(tabBattle_Medion , File.Battles, MapLeaderType.Medion),
                new PopulateBattleTabConfig(tabBattle_Julian , File.Battles, MapLeaderType.Julian),
                new PopulateBattleTabConfig(tabBattle_Extra,   File.Battles, MapLeaderType.Extra),
            });
        }

        private void tsmiScenario_BTL99_Click(object sender, EventArgs e) => IsBTL99 = !IsBTL99;
    }
}
