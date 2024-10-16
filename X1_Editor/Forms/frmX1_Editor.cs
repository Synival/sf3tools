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

        private SlotList _slotList;
        private HeaderList _headerList;
        private AIList _aiList;
        private SpawnZoneList _spawnZoneList;
        private BattlePointersList _battlePointersList;
        private TreasureList _treasureList;
        private CustomMovementList _customMovementList;
        private WarpList _warpList;
        private TileList _tileList;
        private NpcList _npcList;
        private EnterList _enterList;
        private ArrowList _arrowList;

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
            //attempt to detect scenario that failed
            /*if (offset > 0x06067fff || offset < 0x0605e000)
            {
                //Console.WriteLine(offset.ToString("X"));

                olvItems.ClearObjects();
                objectListView1.ClearObjects();
                objectListView2.ClearObjects();
                objectListView3.ClearObjects();
                objectListView4.ClearObjects();
                objectListView5.ClearObjects();
                objectListView6.ClearObjects();
                objectListView7.ClearObjects();
                objectListView8.ClearObjects();
                objectListView9.ClearObjects();
                objectListView10.ClearObjects();
                objectListView11.ClearObjects();
                objectListView12.ClearObjects();
                objectListView13.ClearObjects();
                return false;
            }    
            else*/

            bool isntScn1 = Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other;

            _slotList = new SlotList(FileEditor);
            _headerList = new HeaderList(FileEditor);
            _aiList = new AIList(FileEditor);
            _spawnZoneList = new SpawnZoneList(FileEditor);
            _battlePointersList = new BattlePointersList(FileEditor);
            _customMovementList = new CustomMovementList(FileEditor);
            _treasureList = new TreasureList(FileEditor);
            _warpList = new WarpList(FileEditor);
            _tileList = new TileList(FileEditor);
            _npcList = new NpcList(FileEditor);
            _enterList = new EnterList(FileEditor);
            _arrowList = new ArrowList(FileEditor);

            return tabMain.PopulateAndToggleTabs(new List<PopulateAndToggleTabConfig>()
            {
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabHeader, olvHeader, _headerList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab1, olvSlotTab1, _slotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab2, olvSlotTab2, _slotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab3, olvSlotTab3, _slotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSlotTab4, olvSlotTab4, _slotList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabAITargetPosition, olvAITargetPosition, _aiList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabSpawnZones, olvSpawnZones, _spawnZoneList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabBattlePointers, olvBattlePointers, _battlePointersList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle, tabScriptedMovement, olvScriptedMovement, _customMovementList),
                new PopulateAndToggleTabConfig(true, tabInteractables, olvInteractables, _treasureList),
                new PopulateAndToggleTabConfig(!FileEditor.IsBattle, tabTownNpcs, olvTownNpcs, _npcList),
                new PopulateAndToggleTabConfig(!FileEditor.IsBattle, tabNonBattleEnter, olvNonBattleEnter, _enterList),
                new PopulateAndToggleTabConfig(!FileEditor.IsBattle && isntScn1, tabArrows, olvArrows, _arrowList),
                new PopulateAndToggleTabConfig(isntScn1, tabWarpTable, olvWarpTable, _warpList),
                new PopulateAndToggleTabConfig(FileEditor.IsBattle && isntScn1, tabTileData, olvTileData, _tileList),
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
