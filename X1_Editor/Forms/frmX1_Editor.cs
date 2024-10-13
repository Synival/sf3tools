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

namespace SF3.X1_Editor.Forms
{
    public partial class frmX1_Editor : Form
    {
        // Used to display version in the application
        private string Version = "0.34";

        private ScenarioType _scenario = (ScenarioType)(-1); // uninitialized value

        private ScenarioType Scenario
        {
            get => _scenario;
            set
            {
                _scenario = value;
                tsmiScenario_Scenario1.Checked = (_scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (_scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (_scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (_scenario == ScenarioType.PremiumDisk);
                tsmiScenario_BTL99.Checked = (_scenario == ScenarioType.Other);

                switch (_scenario)
                {
                    case ScenarioType.Scenario1:   _scn = "1";     break;
                    case ScenarioType.Scenario2:   _scn = "2";     break;
                    case ScenarioType.Scenario3:   _scn = "3";     break;
                    case ScenarioType.PremiumDisk: _scn = "PD";    break;
                    case ScenarioType.Other:       _scn = "BTL99"; break;
                }

                updateText();
            }
        }

        private int _map = 0x00;

        private int Map
        {
            get => _map;
            set
            {
                _map = value;
                tsmiMap_MapSynbios.Checked = (_map == 0x00);
                tsmiMap_MapMedion.Checked = (_map == 0x04);
                tsmiMap_MapJulian.Checked = (_map == 0x08);
                tsmiMap_MapExtra.Checked = (_map == 0x0C);

                switch (_map)
                {
                    case 0x00: _maps = "Synbios"; break;
                    case 0x04: _maps = "Medion";  break;
                    case 0x08: _maps = "Julian";  break;
                    case 0x0C: _maps = "Extra";   break;
                }

                updateText();
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

                updateText();
            }
        }

        private string _scn = "1";
        private string _maps = "Synbios";
        private string _mapType = "none";
        private string _fileName = "None";
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

        private IX1_FileEditor _fileEditor;

        public frmX1_Editor()
        {
            InitializeComponent();
            this.tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
            Map = 0x00;
        }

        private bool initialise()
        {
            tsmiFile_SaveAs.Enabled = true;

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

            else if (Scenario == ScenarioType.Other)
            {
                offset = 0x00000018; //btl99 initial pointer
                sub = 0x06060000;
            }

            offset = _fileEditor.GetDouble(offset);

            offset = offset - sub; //first pointer
            offset = _fileEditor.GetDouble(offset);

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

            updateText();

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

            _slotList = new SlotList(_fileEditor);
            if (IsBattle && !_slotList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            _headerList = new HeaderList(_fileEditor);
            if (IsBattle && !_headerList.Load())
            {
                MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }

            _aiList = new AIList(_fileEditor);
            if (IsBattle && !_aiList.Load())
            {
                MessageBox.Show("Could not load Resources/AI.xml.");
                return false;
            }

            _spawnZoneList = new SpawnZoneList(_fileEditor);
            if (IsBattle && !_spawnZoneList.Load())
            {
                MessageBox.Show("Could not load Resources/UnknownAI.xml.");
                return false;
            }

            _battlePointersList = new BattlePointersList(_fileEditor);
            if (IsBattle && !_battlePointersList.Load())
            {
                MessageBox.Show("Could not load Resources/BattlePointersList.xml.");
                return false;
            }

            _treasureList = new TreasureList(_fileEditor);
            if (!_treasureList.Load())
            {
                MessageBox.Show("Could not load Resources/X1Treasure.xml.");
                return false;
            }

            _customMovementList = new CustomMovementList(_fileEditor);
            if (IsBattle && !_customMovementList.Load())
            {
                MessageBox.Show("Could not load Resources/X1AI.xml.");
                return false;
            }

            _warpList = new WarpList(_fileEditor);
            if (Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other && !_warpList.Load())
            {
                MessageBox.Show("Could not load Resources/X1Warp.xml.");
                return false;
            }

            _tileList = new TileList(_fileEditor);
            if (IsBattle && Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other && !_tileList.Load())
            {
                MessageBox.Show("Could not load Resources/MovementTypes.xml.");
                return false;
            }

            _npcList = new NpcList(_fileEditor);
            if (!IsBattle && !_npcList.Load())
            {
                MessageBox.Show("Could not load Resources/NpcList.xml.");
                return false;
            }

            _enterList = new EnterList(_fileEditor);
            if (!IsBattle && !_enterList.Load())
            {
                MessageBox.Show("Could not load Resources/EnterList.xml.");
                return false;
            }

            _arrowList = new ArrowList(_fileEditor);
            if (!IsBattle && Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other && !_arrowList.Load())
            {
                MessageBox.Show("Could not load Resources/ArrowList.xml.");
                return false;
            }

            olvHeader.ClearObjects();
            olvSlotTab1.ClearObjects();
            olvSlotTab2.ClearObjects();
            olvSlotTab3.ClearObjects();
            olvSlotTab4.ClearObjects();
            olvAITargetPosition.ClearObjects();
            olvSpawnZones.ClearObjects();
            olvBattlePointers.ClearObjects();
            olvInteractables.ClearObjects();
            olvScriptedMovement.ClearObjects();
            olvWarpTable.ClearObjects();
            olvTileData.ClearObjects();
            olvTownNpcs.ClearObjects();
            olvNonBattleEnter.ClearObjects();
            olvArrows.ClearObjects();

            if (IsBattle)
            {
                olvHeader.AddObjects(_headerList.Models);
                olvSlotTab1.AddObjects(_slotList.Models);
                olvSlotTab2.AddObjects(_slotList.Models);
                olvSlotTab3.AddObjects(_slotList.Models);
                olvSlotTab4.AddObjects(_slotList.Models);
                olvAITargetPosition.AddObjects(_aiList.Models);
                olvSpawnZones.AddObjects(_spawnZoneList.Models);
                olvBattlePointers.AddObjects(_battlePointersList.Models);
                olvScriptedMovement.AddObjects(_customMovementList.Models);
            }

            olvInteractables.AddObjects(_treasureList.Models);

            if (!IsBattle)
            {
                olvTownNpcs.AddObjects(_npcList.Models);
                olvNonBattleEnter.AddObjects(_enterList.Models);
            }

            if (!IsBattle && Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other)
            {
                olvArrows.AddObjects(_arrowList.Models);
            }

            if (Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other)
            {
                olvWarpTable.AddObjects(_warpList.Models);
            }

            if (IsBattle && Scenario != ScenarioType.Scenario1 && Scenario != ScenarioType.Other)
            {
                olvTileData.AddObjects(_tileList.Models);
            }

            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e)
        {
            string[] words = new[] { "" };
            string lastWord = "";

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X1*.bin)|X1*.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor = new X1_FileEditor(Scenario, Map);
                if (_fileEditor.LoadFile(openfile.FileName))
                {
                    try
                    {
                        initialise();
                    }
                    catch (System.Reflection.TargetInvocationException)
                    {
                        //wrong file was selected
                        MessageBox.Show("Failed to read file:\n" +
                                        "    " + openfile.FileName);
                    }
                    catch (FileEditorReadException)
                    {
                        //wrong file was selected
                        MessageBox.Show("Data appears corrupt or invalid:\n" +
                                        "    " + openfile.FileName + "\n\n" +
                                        "Is this the correct type of file?");
                    }

                    words = openfile.FileName.Split('\\');
                    lastWord = words[words.Length - 1];
                    _fileName = lastWord;
                    updateText();
                }
                else
                {
                    MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                }
            }
        }

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e)
        {
            if (_fileEditor == null)
            {
                return;
            }

            olvHeader.FinishCellEdit();
            olvSlotTab1.FinishCellEdit();
            olvSlotTab2.FinishCellEdit();
            olvSlotTab3.FinishCellEdit();
            olvSlotTab4.FinishCellEdit();
            olvAITargetPosition.FinishCellEdit();
            olvSpawnZones.FinishCellEdit();
            olvBattlePointers.FinishCellEdit();
            olvInteractables.FinishCellEdit();
            olvScriptedMovement.FinishCellEdit();
            olvWarpTable.FinishCellEdit();
            olvTileData.FinishCellEdit();
            olvTownNpcs.FinishCellEdit();
            olvNonBattleEnter.FinishCellEdit();
            olvArrows.FinishCellEdit();

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 X1* (.bin)|X1.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(_fileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

        public static class Globals
        {
            public static bool treasureDebug = false;
        }

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario1;
            Map = 0x00; //synbios lead by default
        }

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario2;
            Map = 0x04; //medion lead by default
        }

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Scenario3;
            Map = 0x08; //julian lead by default
        }

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.PremiumDisk;
            Map = 0x00; //synbios lead by default
        }

        private void tsmiScenario_BTL99_Click(object sender, EventArgs e)
        {
            Scenario = ScenarioType.Other;
            Map = 0x00; //synbios lead by default
        }

        private void tsmiMap_MapSynbios_Click(object sender, EventArgs e) => Map = 0x00; //map with synbios as lead
        private void tsmiMap_MapMedion_Click(object sender, EventArgs e) => Map = 0x04; //map with medion as lead
        private void tsmiMap_MapJulian_Click(object sender, EventArgs e) => Map = 0x08; //map with julian as lead
        private void tsmiMap_MapExtra_Click(object sender, EventArgs e) => Map = 0x0C; //map with no lead or a extra as lead. also for ruins

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

            updateText();
        }

        private void updateText()
        {
            //this.toolStripMenuItem12.Text = "Current Loading info. Map: " + _maps + " Scenario " + _scn + " MapType: " + _mapType + " debug: " + _debug;
            //this.Text = "Sf3 X1 editor" + "          " + "|OpenedFile: " + _fileName + "|          Current Loading info: Scenario: " + _scn + " | Map: " + _maps + " | MapType: " + _mapType + " | debug: " + _debug;
            this.Text = "Sf3 X1 editor" + "          " + "|OpenedFile: " + _fileName + "|          Current open settings: Scenario: " + _scn + " | Map: " + _maps + " | MapType: " + _mapType + " | debug: " + _debug;
        }
    }
}
