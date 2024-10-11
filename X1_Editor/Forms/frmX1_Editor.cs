using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SF3.X1_Editor.Models.Presets;
using SF3.X1_Editor.Models.Items;
using SF3.X1_Editor.Models.AI;
using SF3.X1_Editor.Models.UnknownAI;
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

/*

*/

namespace SF3.X1_Editor.Forms
{
    public partial class frmX1_Editor : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "033";
        private bool isBattle = true;

        private ScenarioType _scenario = ScenarioType.Scenario1;
        private int _map = 0x00;
        private string _scn = "1";
        private string _maps = "Synbios";
        private string _battle = "none";
        private string _fileName = "None";
        private string _debug = "off";

        private ItemList _itemList;
        private PresetList _presetList;
        private AIList _aiList;
        private UnknownAIList _unknownAIList;
        private BattlePointersList _battlePointersList;
        private TreasureList _treasureList;
        private CustomMovementList _customMovementList;
        private WarpList _warpList;
        private TileList _tileList;
        private NpcList _npcList;
        private EnterList _enterList;
        private ArrowList _arrowList;

        private IX1FileEditor _fileEditor;

        public frmX1_Editor()
        {
            InitializeComponent();

            /*try {
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/monsterstate." + Version + ".bin", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                olvMonsters.RestoreState(data);
                stream.Close();
            } catch (Exception) { }*/
            /*try {
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/itemstate." + Version + ".bin", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                olvItems.RestoreState(data);
                stream.Close();
            } catch (Exception) { }*/

            /*try
            {
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/spellsstate." + Version + ".bin", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                olvItems.RestoreState(data);
                stream.Close();
            }/*
            catch (Exception) { }
            /*try {
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/blacksmithstate." + Version + ".bin", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                olvBlacksmith.RestoreState(data);
                stream.Close();
            } catch (Exception) { }
            try {
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/storesstate." + Version + ".bin", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                olvStoreItems.RestoreState(data);
                stream.Close();
            } catch (Exception) { }
            /*
            try {
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/spellsstate." + Version + ".bin", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                olvSpells.RestoreState(data);
                stream.Close();
            } catch (Exception) { }*/
            //lvcAction1.AspectToStringConverter = getActionName;
            //lvcAction2.AspectToStringConverter = getActionName;
            //lvcAction3.AspectToStringConverter = getActionName;
            //lvcAction4.AspectToStringConverter = getActionName;
            //lvcAction5.AspectToStringConverter = getActionName;
            //lvcAction6.AspectToStringConverter = getActionName;
            //lvcAction7.AspectToStringConverter = getActionName;
            //lvcAction8.AspectToStringConverter = getActionName;

            //lvcItemStatType1.AspectToStringConverter += getStatTypeName;
            //lvcItemStatType2.AspectToStringConverter += getStatTypeName;
            //lvcItemStatType3.AspectToStringConverter += getStatTypeName;

            //lvcCharacterItem1.AspectToStringConverter += getItemName;
            //lvcCharacterItem2.AspectToStringConverter += getItemName;
            //lvcCharacterItem3.AspectToStringConverter += getItemName;
            //lvcCharacterItem4.AspectToStringConverter += getItemName;
            //lvcCharacterItem5.AspectToStringConverter += getItemName;
            //lvcCharacterItem6.AspectToStringConverter += getItemName;
            //lvcCharacterItem7.AspectToStringConverter += getItemName;
            //lvcCharacterItem8.AspectToStringConverter += getItemName;

            //lvcItem.AspectToStringConverter += getItemName;

            //lvcBlacksmithItem.AspectToStringConverter += getItemName;
            //lvcStoreItem.AspectToStringConverter += getItemName;

            //lvcSpellType.AspectToStringConverter += getSpellName;
            //lvcSpellClass.AspectToStringConverter += getClassName;

            //lvcStoreItemType.AspectToStringConverter += getStoreItemTypeName;

            //Block the putter events for columns that use comboboxes
            //we handle this in the cell edit finishing event to make things a TON easier
            /*lvcItemStatType1.AspectPutter += blocker;
            lvcItemStatType2.AspectPutter += blocker;
            lvcItemStatType3.AspectPutter += blocker;
            lvcAction1.AspectPutter += blocker;
            lvcAction2.AspectPutter += blocker;
            lvcAction3.AspectPutter += blocker;
            lvcAction4.AspectPutter += blocker;
            lvcAction5.AspectPutter += blocker;
            lvcAction6.AspectPutter += blocker;
            lvcAction7.AspectPutter += blocker;
            lvcAction8.AspectPutter += blocker;
            lvcCharacterItem1.AspectPutter += blocker;
            lvcCharacterItem2.AspectPutter += blocker;
            lvcCharacterItem3.AspectPutter += blocker;
            lvcCharacterItem4.AspectPutter += blocker;
            lvcCharacterItem5.AspectPutter += blocker;
            lvcCharacterItem6.AspectPutter += blocker;
            lvcCharacterItem7.AspectPutter += blocker;
            lvcCharacterItem8.AspectPutter += blocker;

            lvcItem.AspectPutter += blocker;
            lvcBlacksmithItem.AspectPutter += blocker;
            lvcStoreItem.AspectPutter += blocker;

            lvcSpellClass.AspectPutter += blocker;
            lvcSpellType.AspectPutter += blocker;
            lvcStoreItemType.AspectPutter += blocker;*/
        }

        private void blocker(object target, object newvalue) { }

        /*private string getActionName(object target)
        {
            return ((Action)target).Name;
        }*/
        /*private string getStatTypeName(object target)
        {
            return ((StatType)target).Name;
        }*/
        private string getItemName(object target)
        {
            return ((Item)target).Name;
        }
        /*private string getSpellName(object target)
        {
            return ((Spell)target).SpellName;
        }*/
        private string getPresetName(object target)
        {
            return ((Preset)target).SizeName;
        }
        private string getAIName(object target)
        {
            return ((AI)target).AIName;
        }

        private string getUnknownAIName(object target)
        {
            return ((UnknownAI)target).UnknownAIName;
        }
        private string getBattlePointersName(object target)
        {
            return ((BattlePointers)target).BattleName;
        }
        private string getTreasureName(object target)
        {
            return ((Treasure)target).TreasureName;
        }
        private string getCustomMovementName(object target)
        {
            return ((CustomMovement)target).CustomMovementName;
        }
        private string getWarpName(object target)
        {
            return ((Warp)target).WarpName;
        }
        private string getNpcName(object target)
        {
            return ((Npc)target).NpcName;
        }

        private string getEnterName(object target)
        {
            return ((Enter)target).EnterName;
        }

        private string getArrowName(object target)
        {
            return ((Arrow)target).ArrowName;
        }
        /*
        private string getStoreItemTypeName(object target)
        {
            return ((StoreItemType)target).Name;
        }*/

        private bool initialise()
        {
            tsmiFile_SaveAs.Enabled = true;

            int offset = 0;
            int sub = 0;

            if (_scenario == ScenarioType.Scenario1)
            {
                offset = 0x00000018; //scn1 initial pointer
                sub = 0x0605f000;
            }
            else if (_scenario == ScenarioType.Scenario2)
            {
                offset = 0x00000024; //scn2 initial pointer
                sub = 0x0605e000;
            }
            else if (_scenario == ScenarioType.Scenario3)
            {
                offset = 0x00000024; //scn3 initial pointer
                sub = 0x0605e000;
            }
            else if (_scenario == ScenarioType.PremiumDisk)
            {
                offset = 0x00000024; //pd initial pointer
                sub = 0x0605e000;
            }

            else if (_scenario == ScenarioType.Other)
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
            if (_scenario == ScenarioType.Scenario1 && offset > 0x0605F000)
            {
                isBattle = true;
                //Console.WriteLine(offset.ToString("X"));

                this.tsmiMapType_BattleToggle.Text = "Battle toggle: on";
                _battle = "battle";
            }
            else if (offset > 0x0605e000)
            {
                isBattle = true;
                //Console.WriteLine(offset.ToString("X"));

                this.tsmiMapType_BattleToggle.Text = "Battle toggle: on";
                _battle = "battle";
            }
            else
            {
                isBattle = false;
                //Console.WriteLine(offset.ToString("X"));
                this.tsmiMapType_BattleToggle.Text = "Battle toggle: off";
                _battle = "town";
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

            /*if (!StoreItemTypeList.loadStoreItemTypeList()) {
                MessageBox.Show("Could not load Resources/storeitemtypes.xml.");
                return false;
            }
            if (!SpellList.loadSpellList()) {
                MessageBox.Show("Could not load Resources/spells.xml.");
                return false;
            }
            if (!CharacterClassList.loadCharacterClassList()) {
                MessageBox.Show("Could not load Resources/classes.xml.");
                return false;
            }*/
            /*if (!ActionList.loadActionList()) {
                MessageBox.Show("Could not load Resources/actions.xml.");
                return false;
            }*/
            /*if (!StatTypeList.loadStatTypeList()) {
                MessageBox.Show("Could not load Resources/stattypes.xml.");
                return false;
            }*/
            _itemList = new ItemList(_fileEditor);
            if (isBattle && !_itemList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            /*if (!SpellList.loadSpellList()) {
                MessageBox.Show("Could not load Resources/spellList.xml.");
                return false;
            }*/

            _presetList = new PresetList(_fileEditor);
            if (isBattle && !_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }

            _aiList = new AIList(_fileEditor);
            if (isBattle && !_aiList.Load())
            {
                MessageBox.Show("Could not load Resources/AI.xml.");
                return false;
            }

            _unknownAIList = new UnknownAIList(_fileEditor);
            if (isBattle && !_unknownAIList.Load())
            {
                MessageBox.Show("Could not load Resources/UnknownAI.xml.");
                return false;
            }

            _battlePointersList = new BattlePointersList(_fileEditor);
            if (isBattle && !_battlePointersList.Load())
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
            if (isBattle && !_customMovementList.Load())
            {
                MessageBox.Show("Could not load Resources/X1AI.xml.");
                return false;
            }
            _warpList = new WarpList(_fileEditor);
            if (_scenario != ScenarioType.Scenario1 && _scenario != ScenarioType.Other && !_warpList.Load())
            {
                MessageBox.Show("Could not load Resources/X1Warp.xml.");
                return false;
            }
            _tileList = new TileList(_fileEditor);
            if (isBattle && _scenario != ScenarioType.Scenario1 && _scenario != ScenarioType.Other && !_tileList.Load())
            {
                MessageBox.Show("Could not load Resources/MovementTypes.xml.");
                return false;
            }

            _npcList = new NpcList(_fileEditor);
            if (!isBattle && !_npcList.Load())
            {
                MessageBox.Show("Could not load Resources/NpcList.xml.");
                return false;
            }

            _enterList = new EnterList(_fileEditor);
            if (!isBattle && !_enterList.Load())
            {
                MessageBox.Show("Could not load Resources/EnterList.xml.");
                return false;
            }
            _arrowList = new ArrowList(_fileEditor);
            if (!isBattle && _scenario != ScenarioType.Scenario1 && _scenario != ScenarioType.Other && !_arrowList.Load())
            {
                MessageBox.Show("Could not load Resources/ArrowList.xml.");
                return false;
            }

            //BlacksmithList.loadBlacksmithList();
            //StoreItemList.loadStoreItemList();
            //SpellEntryList.loadSpellEntryList();

            //olvBlacksmith.ClearObjects();
            //olvCharacters.ClearObjects();
            //olvMonsters.ClearObjects();

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

            //olvPresets.ClearObjects();
            //olvSpells.ClearObjects();
            //olvSpells.ClearObjects();
            //olvStoreItems.ClearObjects();

            //olvMonsters.AddObjects(MonsterList.getMonsterList());

            if (isBattle)
            {
                olvHeader.AddObjects(_presetList.Models);
                olvSlotTab1.AddObjects(_itemList.Models);
                olvSlotTab2.AddObjects(_itemList.Models);
                olvSlotTab3.AddObjects(_itemList.Models);
                olvSlotTab4.AddObjects(_itemList.Models);
                olvAITargetPosition.AddObjects(_aiList.Models);
                olvSpawnZones.AddObjects(_unknownAIList.Models);
                olvBattlePointers.AddObjects(_battlePointersList.Models);
                olvScriptedMovement.AddObjects(_customMovementList.Models);
            }

            olvInteractables.AddObjects(_treasureList.Models);

            if (!isBattle)
            {
                olvTownNpcs.AddObjects(_npcList.Models);
                olvNonBattleEnter.AddObjects(_enterList.Models);
            }

            if (!isBattle && _scenario != ScenarioType.Scenario1 && _scenario != ScenarioType.Other)
            {
                olvArrows.AddObjects(_arrowList.Models);
            }

            if (_scenario != ScenarioType.Scenario1 && _scenario != ScenarioType.Other)
            {
                olvWarpTable.AddObjects(_warpList.Models);
            }
            if (isBattle && _scenario != ScenarioType.Scenario1 && _scenario != ScenarioType.Other)
            {
                olvTileData.AddObjects(_tileList.Models);
            }

            //olvCharacters.AddObjects(CharacterList.getCharacterList());
            //olvBlacksmith.AddObjects(BlacksmithList.getBlacksmithList());
            //olvStoreItems.AddObjects(StoreItemList.getStoreItemList());
            //olvSpells.AddObjects(SpellEntryList.getSpellEntryList());
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
                _fileEditor = new X1FileEditor(_scenario, _map);
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

                    //string[] words = openfile.FileName.Split('\\');
                    //string lastWord = words[words.Length - 1];
                    words = openfile.FileName.Split('\\');
                    lastWord = words[words.Length - 1];
                    _fileName = lastWord;
                    updateText();
                    //Console.WriteLine(lastWord);
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

            //olvBlacksmith.FinishCellEdit();
            //olvMonsters.FinishCellEdit();
            //olvCharacters.FinishCellEdit();
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
            //objectListView1.FinishCellEdit();
            //objectListView2.FinishCellEdit();
            //olvStoreItems.FinishCellEdit();
            //olvSpells.FinishCellEdit();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 X1* (.bin)|X1.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void frmX1_Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*try {
                byte[] data = olvMonsters.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/monsterstate." + Version + ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            } catch (Exception) { }*/
            try
            {
                byte[] data = olvHeader.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/itemstate." +
                     ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            catch (Exception) { }
            /*try {
                byte[] data = olvBlacksmith.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/blacksmithstate." + Version + ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            } catch (Exception) { }
            try {
                byte[] data = olvStoreItems.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/storesstate." + Version + ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            } catch (Exception) { }
            try {
                byte[] data = olvSpells.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/spellsstate." + Version + ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            } catch (Exception) { }*/
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.Column.AspectToStringFormat == "{0:X}")
            {
                NumericUpDown control = (NumericUpDown)e.Control;
                control.Hexadecimal = true;
            }
            /*else if (e.Column.AspectToStringFormat == "{0:1}")
            {
                NumericUpDown control = (NumericUpDown)e.Control;
                control.binary? = true;
            } */
            /*else if (e.Value is Item) {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(ItemList.getItemList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }*/
            /*else if (e.Value is Spell)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "SpellName";
                cb.Items.AddRange(SpellList.getSpellList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }*/
            /*else if (e.Value is Preset)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(PresetList.getPresetList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }*/

            /*else if (e.Value is StatType) {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(StatTypeList.getStatTypeList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            } else if (e.Value is Spell) {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(SpellList.getSpellList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            } else if (e.Value is CharacterClass) {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(CharacterClassList.getCharacterClassList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            } else if (e.Value is StoreItemType) {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(StoreItemTypeList.getStoreItemTypeList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }*/

            Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);
        }

        private void olvCellEditFinishing(object sender, CellEditEventArgs e)
        {
            /*if (e.Value is Action) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Action value = (Action)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            } else*/
            /*if (e.Value is Item) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Item value = (Item)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);*/
            /*} else if (e.Value is Spell) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Spell value = (Spell)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);*/
            /*} else if (e.Value is Preset) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Preset value = (Preset)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }*/ /*else if (e.Value is CharacterClass) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                CharacterClass value = (CharacterClass)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            } else if (e.Value is StoreItemType) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                StoreItemType value = (StoreItemType)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }*/
        }

        public static class Globals
        {
            public static bool treasureDebug = false;
        }

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Scenario1;
            _map = 0x00; //synbios lead by default
            _scn = "1";
            _maps = "Synbios";
            updateText();
        }

        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Scenario2;
            _map = 0x04; //medion lead by default
            _scn = "2";
            _maps = "Medion";
            updateText();
        }

        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Scenario3;
            _map = 0x08; //julian lead by default
            _scn = "3";
            _maps = "Julian";
            updateText();
        }

        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.PremiumDisk;
            _map = 0x00; //synbios lead by default
            _scn = "PD";
            _maps = "Synbios";

            updateText();
        }
        private void tsmiScenario_BTL99_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Other;
            _scn = "BTL99";
            _maps = "Synbios";

            updateText();
        }

        private void tsmiMap_MapSynbios_Click(object sender, EventArgs e)
        {
            _map = 0x00; //map with synbios as lead
            _maps = "Synbios";
            updateText();
        }

        private void tsmiMap_MapMedion_Click(object sender, EventArgs e)
        {
            _map = 0x04; //map with medion as lead
            _maps = "Medion";
            updateText();
        }

        private void tsmiMap_MapJulian_Click(object sender, EventArgs e)
        {
            _map = 0x08; //map with julian as lead
            _maps = "Julian";
            updateText();
        }

        private void tsmiMap_MapExtra_Click(object sender, EventArgs e)
        {
            _map = 0x0C; //map with no lead or a extra as lead. also for ruins
            _maps = "Extra";
            updateText();
        }

        private void tsmiMapType_BattleToggle_Click(object sender, EventArgs e)
        {
            //battle
            if (isBattle == false)
            {
                isBattle = true;
                this.tsmiMapType_BattleToggle.Text = "Battle toggle: on";
                _battle = "battle";
            }
            else
            {
                isBattle = false;
                this.tsmiMapType_BattleToggle.Text = "Battle toggle: off";
                _battle = "town";
            }
            updateText();
        }

        private void tsmiHelp_TreasureDebugToggle_Click(object sender, EventArgs e)
        {
            if (Globals.treasureDebug == true)
            {
                Globals.treasureDebug = false;
                this.tsmiHelp_TreasureDebugToggle.Text = "treasureDebug toggle: off";
                _debug = "off";
            }
            else
            {
                Globals.treasureDebug = true;
                this.tsmiHelp_TreasureDebugToggle.Text = "treasureDebug toggle: on";
                _debug = "on";
            }
            updateText();
        }

        private void updateText()
        {
            //this.toolStripMenuItem12.Text = "Current Loading info. Map: " + _maps + " Scenario " + _scn + " MapType: " + _battle + " debug: " + _debug;
            //this.Text = "Sf3 X1 editor" + "          " + "|OpenedFile: " + _fileName + "|          Current Loading info: Scenario: " + _scn + " | Map: " + _maps + " | MapType: " + _battle + " | debug: " + _debug;
            this.Text = "Sf3 X1 editor" + "          " + "|OpenedFile: " + _fileName + "|          Current open settings: Scenario: " + _scn + " | Map: " + _maps + " | MapType: " + _battle + " | debug: " + _debug;
        }
    }
}
