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
using SF3.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using SF3.Editor.Forms;
using SF3.Models;

namespace SF3.X1_Editor.Forms
{
    public partial class frmX1_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.34";

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

        private bool _isBattle = true;

        private bool IsBattle
        {
            get => _isBattle;
            set
            {
                _isBattle = value;
                tsmiMapType_BattleToggle.Checked = _isBattle;

                if (_isBattle)
                {
                    this.tsmiMapType_BattleToggle.Text = "Battle toggle: on";
                    _mapType = "battle";
                }
                else
                {
                    this.tsmiMapType_BattleToggle.Text = "Battle toggle: off";
                    _mapType = "town";
                }

                UpdateTitle();
            }
        }

        private string _scn = "1";
        private string _mapType = "none";
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

                switch (Scenario)
                {
                    case ScenarioType.Scenario1: _scn = "1"; break;
                    case ScenarioType.Scenario2: _scn = "2"; break;
                    case ScenarioType.Scenario3: _scn = "3"; break;
                    case ScenarioType.PremiumDisk: _scn = "PD"; break;
                    case ScenarioType.Other: _scn = "BTL99"; break;
                }

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
            int offset = 0;
            int sub = 0;

            if (Scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
            }
            else if (Scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
            }
            else if (Scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
            }
            else if (Scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
            }

            else if (Scenario == ScenarioType.Other /* BTL99 */)
            {
                offset = 0x00000018; //btl99 initial pointer
                sub = 0x06060000;
            }

            offset = FileEditor.GetDouble(offset);

            offset = offset - sub; //first pointer
            offset = FileEditor.GetDouble(offset);

            /*A value higher means a pointer is on the offset, meaning we are in a battle. If it is not a 
              pointer we are at our destination so we know a town is loaded.
            */
            if (Scenario == ScenarioType.Scenario1 && offset > 0x0605F000)
            {
                IsBattle = true;
            }
            else if (offset > 0x0605e000)
            {
                IsBattle = true;
            }
            else
            {
                IsBattle = false;
            }

            UpdateTitle();

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

            var fileEditor = FileEditor as IX1_FileEditor;
            bool isntScn1 = Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other;

            _slotList = new SlotList(fileEditor);
            _headerList = new HeaderList(fileEditor);
            _aiList = new AIList(fileEditor);
            _spawnZoneList = new SpawnZoneList(fileEditor);
            _battlePointersList = new BattlePointersList(fileEditor);
            _customMovementList = new CustomMovementList(fileEditor);
            _treasureList = new TreasureList(fileEditor);
            _warpList = new WarpList(fileEditor);
            _tileList = new TileList(fileEditor);
            _npcList = new NpcList(fileEditor);
            _enterList = new EnterList(fileEditor);
            _arrowList = new ArrowList(fileEditor);

            // Models in all X1 files
            var loadLists = new List<IModelArray>()
            {
                _treasureList,
            };

            if (Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other)
            {
                loadLists.Add(_warpList);
            }

            // Models in only X1 battle files
            if (IsBattle)
            {
                loadLists.AddRange(new List<IModelArray>()
                {
                    _slotList,
                    _headerList,
                    _aiList,
                    _spawnZoneList,
                    _battlePointersList,
                    _customMovementList,
                });

                if (isntScn1)
                {
                    loadLists.Add(_tileList);
                }
            }
            // Models in only X1 town files
            else
            {
                loadLists.AddRange(new List<IModelArray>()
                {
                    _npcList,
                    _enterList,
                });

                if (isntScn1)
                {
                    loadLists.Add(_arrowList);
                }
            }

            foreach (var list in loadLists)
            {
                if (!list.Load())
                {
                    MessageBox.Show("Could not load " + list.ResourceFile);
                    return false;
                }
            }

            ObjectListViews.ForEach(x => x.ClearObjects());

            Action<bool, TabPage, ObjectListView, object[]> conditionallyAddModels = (cond, tab, olv, models) =>
            {
                if (cond)
                {
                    tabMain.TabPages.Add(tab);
                    olv.AddObjects(models);
                    tab.Controls.Add(olv);
                }
            };

            tabMain.SuspendLayout();
            tabMain.TabPages.Clear();

            conditionallyAddModels(IsBattle, tabHeader, olvHeader, _headerList.Models);
            conditionallyAddModels(IsBattle, tabSlotTab1, olvSlotTab1, _slotList.Models);
            conditionallyAddModels(IsBattle, tabSlotTab2, olvSlotTab2, _slotList.Models);
            conditionallyAddModels(IsBattle, tabSlotTab3, olvSlotTab3, _slotList.Models);
            conditionallyAddModels(IsBattle, tabSlotTab4, olvSlotTab4, _slotList.Models);
            conditionallyAddModels(IsBattle, tabAITargetPosition, olvAITargetPosition, _aiList.Models);
            conditionallyAddModels(IsBattle, tabSpawnZones, olvSpawnZones, _spawnZoneList.Models);
            conditionallyAddModels(IsBattle, tabBattlePointers, olvBattlePointers, _battlePointersList.Models);
            conditionallyAddModels(IsBattle, tabScriptedMovement, olvScriptedMovement, _customMovementList.Models);
            conditionallyAddModels(true, tabInteractables, olvInteractables, _treasureList.Models);
            conditionallyAddModels(!IsBattle, tabTownNpcs, olvTownNpcs, _npcList.Models);
            conditionallyAddModels(!IsBattle, tabNonBattleEnter, olvNonBattleEnter, _enterList.Models);
            conditionallyAddModels(!IsBattle && isntScn1, tabArrows, olvArrows, _arrowList.Models);
            conditionallyAddModels(isntScn1, tabWarpTable, olvWarpTable, _warpList.Models);
            conditionallyAddModels(IsBattle && isntScn1, tabTileData, olvTileData, _tileList.Models);

            tabMain.ResumeLayout();

            return true;
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

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

        private void tsmiMapType_BattleToggle_Click(object sender, EventArgs e) => IsBattle = !IsBattle;

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

        protected override string MakeTitle()
        {
            return base.MakeTitle() +
                "            | Current open settings: Scenario: " + _scn + " | Map: " + _map.ToString() + " | MapType: " + _mapType + " | Debug: " + _debug;
        }
    }
}
