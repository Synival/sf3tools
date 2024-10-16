using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SF3.X1_Editor.Models.Headers;
using SF3.X1_Editor.Models.Slots;
using SF3.X1_Editor.Models.AI;
using SF3.X1_Editor.Models.SpawnZones;
using SF3.X1_Editor.Models.BattlePointers;
using SF3.X1_Editor.Models.Treasures;
using SF3.X1_Editor.Models.CustomMovement;
using SF3.X1_Editor.Models.Warps;
using SF3.X1_Editor.Models.Tiles;
using SF3.X1_Editor.Models.Npcs;
using SF3.X1_Editor.Models.Enters;
using SF3.X1_Editor.Models.Arrows;
using BrightIdeasSoftware;
using SF3.Types;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;
using SF3.Models;
using SF3.Editor.Extensions;
using static SF3.Editor.Extensions.TabControlExtensions;

namespace SF3.X1_Editor.Forms
{
    public partial class frmX1_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.34";

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
                UpdateTitle();
            }
        }

        private string _debug = "off";

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
                tsmiScenario_BTL99.Checked = (Scenario == ScenarioType.Other);
                UpdateTitle();
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        protected override string FileDialogFilter => "SF3 data (X1*.bin)|X1*.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X1_FileEditor(Scenario, Map);

        protected override bool LoadOpenedFile()
        {
            bool isntScn1 = Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other;

            return tabMain.PopulateAndToggleTabs(new List<PopulateAndToggleTabConfig>()
            {
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabHeader, olvHeader, FileEditor.HeaderList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab1, olvSlotTab1, FileEditor.SlotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab2, olvSlotTab2, FileEditor.SlotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab3, olvSlotTab3, FileEditor.SlotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab4, olvSlotTab4, FileEditor.SlotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabAITargetPosition, olvAITargetPosition, FileEditor.AIList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSpawnZones, olvSpawnZones, FileEditor.SpawnZoneList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabBattlePointers, olvBattlePointers, FileEditor.BattlePointersList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabScriptedMovement, olvScriptedMovement, FileEditor.CustomMovementList),
                new PopulateAndToggleTabConfig(true, tabInteractables, olvInteractables, FileEditor.TreasureList),
                new PopulateAndToggleTabConfig(!FileEditor.IsBattle, tabTownNpcs, olvTownNpcs, FileEditor.NpcList),
                new PopulateAndToggleTabConfig(!FileEditor.IsBattle, tabNonBattleEnter, olvNonBattleEnter, FileEditor.EnterList),
                new PopulateAndToggleTabConfig(!FileEditor.IsBattle && isntScn1, tabArrows, olvArrows, FileEditor.ArrowList),
                new PopulateAndToggleTabConfig(isntScn1, tabWarpTable, olvWarpTable, FileEditor.WarpList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle && isntScn1, tabTileData, olvTileData, FileEditor.TileList),
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
            Scenario = ScenarioType.Other;
            Map = MapType.Synbios;
        }

        private void tsmiMap_MapSynbios_Click(object sender, EventArgs e) => Map = MapType.Synbios; //map with synbios as lead
        private void tsmiMap_MapMedion_Click(object sender, EventArgs e) => Map = MapType.Medion; //map with medion as lead
        private void tsmiMap_MapJulian_Click(object sender, EventArgs e) => Map = MapType.Julian; //map with julian as lead
        private void tsmiMap_MapExtra_Click(object sender, EventArgs e) => Map = MapType.Extra; //map with no lead or a extra as lead. also for ruins

        private void tsmiHelp_TreasureDebugToggle_Click(object sender, EventArgs e)
        {
            Globals.treasureDebug = !Globals.treasureDebug;
            tsmiHelp_TreasureDebugToggle.Checked = Globals.treasureDebug;

            if (Globals.treasureDebug)
            {
                this.tsmiHelp_TreasureDebugToggle.Text = "treasureDebug toggle: on";
                _debug = "on";
            }
            else
            {
                this.tsmiHelp_TreasureDebugToggle.Text = "treasureDebug toggle: off";
                _debug = "off";
            }

            UpdateTitle();
        }
    }
}
