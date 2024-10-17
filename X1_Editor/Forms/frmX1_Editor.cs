using System;
using BrightIdeasSoftware;
using SF3.Types;
using System.Collections.Generic;
using SF3.Editor.Forms;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X1_Editor.Forms
{
    public partial class frmX1_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.34";

        private bool _isBTL99 = false;

        public bool IsBTL99
        {
            get => _isBTL99;
            set
            {
                if (_isBTL99 != value)
                {
                    _isBTL99 = value;
                    tsmiScenario_BTL99.Checked = _isBTL99;
                }
            }
        }

        new public IX1_FileEditor FileEditor => base.FileEditor as IX1_FileEditor;

        private MapType _map = MapType.Synbios;

        private MapType Map
        {
            get => _map;
            set
            {
                _map = value;
                tsmiMap_MapSynbios.Checked = (_map == MapType.Synbios);
                tsmiMap_MapMedion.Checked = (_map == MapType.Medion);
                tsmiMap_MapJulian.Checked = (_map == MapType.Julian);
                tsmiMap_MapExtra.Checked = (_map == MapType.Extra);
            }
        }

        public frmX1_Editor()
        {
            InitializeComponent();
            BaseTitle = this.Text;

            this.tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
            Map = 0x00;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(this, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        protected override string FileDialogFilter => "SF3 data (X1*.bin)|X1*.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X1_FileEditor(Scenario, Map, IsBTL99);

        protected override bool OnLoad()
        {
            if (!base.OnLoad())
            {
                return false;
            }

            return tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>()
            {
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

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        private void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        public static class Globals
        {
            public static bool treasureDebug = false;
        }

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario1;
            Map = MapType.Synbios;
        }

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario2;
            Map = MapType.Medion;
        }

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario3;
            Map = MapType.Julian;
        }

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.PremiumDisk;
            Map = MapType.Synbios;
        }

        private void tsmiScenario_BTL99_Click(object sender, EventArgs e)
        {
            IsBTL99 = !IsBTL99;
            if (IsBTL99)
            {
                Map = MapType.Synbios;
            }
        }

        private void tsmiMap_MapSynbios_Click(object sender, EventArgs e) => Map = MapType.Synbios; //map with synbios as lead
        private void tsmiMap_MapMedion_Click(object sender, EventArgs e) => Map = MapType.Medion; //map with medion as lead
        private void tsmiMap_MapJulian_Click(object sender, EventArgs e) => Map = MapType.Julian; //map with julian as lead
        private void tsmiMap_MapExtra_Click(object sender, EventArgs e) => Map = MapType.Extra; //map with no lead or a extra as lead. also for ruins

        private void tsmiHelp_TreasureDebugToggle_Click(object sender, EventArgs e)
        {
            Globals.treasureDebug = !Globals.treasureDebug;
            tsmiHelp_TreasureDebugToggle.Checked = Globals.treasureDebug;
            tsmiHelp_TreasureDebugToggle.Text = "treasureDebug toggle: " + (Globals.treasureDebug ? "on" : "off");
        }
    }
}
