using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X002_Editor.Models;
using SF3.X002_Editor.Models.Spells;
using SF3.X002_Editor.Models.Presets;
using SF3.X002_Editor.Models.Items;
using SF3.X002_Editor.Models.Loading;
using SF3.X002_Editor.Models.StatBoost;
using SF3.X002_Editor.Models.WeaponRank;
using SF3.X002_Editor.Models.AttackResist;
using SF3.X002_Editor.Models.Warps;
using SF3.X002_Editor.Models.MusicOverride;
using BrightIdeasSoftware;
using SF3.Editor;

/*

*/

namespace SF3.X002_Editor.Forms
{
    public partial class frmMain : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "19";

        private ItemList _itemList = new ItemList();
        private SpellList _spellList = new SpellList();
        private PresetList _presetList = new PresetList();
        private LoadList _loadList = new LoadList();
        private StatList _statList = new StatList();
        private WeaponRankList _weaponRankList = new WeaponRankList();
        private AttackResistList _attackResistList = new AttackResistList();
        private WarpList _warpList = new WarpList();
        private MusicOverrideList _musicOverrideList = new MusicOverrideList();

        public frmMain()
        {
            InitializeComponent();
            frmMonsterEditor_Resize(this, new EventArgs());

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
        private string getSpellName(object target)
        {
            return ((Spell)target).SpellName;
        }
        private string getPresetName(object target)
        {
            return ((Preset)target).PresetName;
        }
        private string getLoadName(object target)
        {
            return ((Loading)target).LoadName;
        }
        private string getStatName(object target)
        {
            return ((StatBoost)target).StatName;
        }
        private string getWeaponRankName(object target)
        {
            return ((WeaponRank)target).WeaponRankName;
        }

        private string getAttackResist(object target)
        {
            return ((AttackResist)target).AttackResistName;
        }

        private string getMusicOverride(object target)
        {
            return ((MusicOverride)target).MusicOverrideName;
        }

        /*
        private string getStoreItemTypeName(object target)
        {
            return ((StoreItemType)target).Name;
        }*/

        private bool initialise()
        {
            saveAsToolStripMenuItem.Enabled = true;
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
            if (!_itemList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            if (!_spellList.Load())
            {
                MessageBox.Show("Could not load Resources/spellList.xml.");
                return false;
            }

            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/spellIndexList.xml.");
                return false;
            }

            if (!_loadList.Load())
            {
                MessageBox.Show("Could not load Resources/loadList.xml.");
                return false;
            }

            if (!_statList.Load())
            {
                MessageBox.Show("Could not load Resources/statList.xml.");
                return false;
            }
            if (!_weaponRankList.Load())
            {
                MessageBox.Show("Could not load Resources/WeaponRankList.xml.");
                return false;
            }
            if (!_attackResistList.Load())
            {
                MessageBox.Show("Could not load Resources/AttackResistList.xml.");
                return false;
            }
            if (Globals.scenario == 1 && !_warpList.Load())
            {
                MessageBox.Show("Could not load Resources/WarpList.xml.");
                return false;
            }
            if (!_musicOverrideList.Load())
            {
                MessageBox.Show("Could not load Resources/MusicOverrideList.xml.");
                return false;
            }

            //BlacksmithList.loadBlacksmithList();
            //StoreItemList.loadStoreItemList();
            //SpellEntryList.loadSpellEntryList();

            //olvBlacksmith.ClearObjects();
            //olvCharacters.ClearObjects();
            //olvMonsters.ClearObjects();

            olvItems.ClearObjects();
            objectListView1.ClearObjects();
            objectListView2.ClearObjects();
            objectListView3.ClearObjects();
            objectListView4.ClearObjects();
            objectListView5.ClearObjects();
            objectListView6.ClearObjects();
            objectListView7.ClearObjects();
            objectListView8.ClearObjects();

            //olvPresets.ClearObjects();
            //olvSpells.ClearObjects();
            //olvSpells.ClearObjects();
            //olvStoreItems.ClearObjects();

            //olvMonsters.AddObjects(MonsterList.getMonsterList());

            olvItems.AddObjects(_itemList.Models);
            objectListView1.AddObjects(_spellList.Models);
            objectListView2.AddObjects(_presetList.Models);
            objectListView3.AddObjects(_loadList.Models);
            objectListView4.AddObjects(_statList.Models);
            objectListView5.AddObjects(_weaponRankList.Models);
            objectListView6.AddObjects(_attackResistList.Models);
            objectListView8.AddObjects(_musicOverrideList.Models);

            if (Globals.scenario == 1)
            {
                objectListView7.AddObjects(_warpList.Models);
            }

            //olvCharacters.AddObjects(CharacterList.getCharacterList());
            //olvBlacksmith.AddObjects(BlacksmithList.getBlacksmithList());
            //olvStoreItems.AddObjects(StoreItemList.getStoreItemList());
            //olvSpells.AddObjects(SpellEntryList.getSpellEntryList());
            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 scn3 data (X002.bin)|X002.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                if (FileEditor.loadFile(openfile.FileName))
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
                }
                else
                {
                    MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                }
            }
        }

        private void frmMonsterEditor_Resize(object sender, EventArgs e)
        {
            Size newsize = ClientSize;
            newsize.Height -= 24;
            tabMain.Size = newsize;
            //olvMonsters.Size = tabMonsters.ClientSize;
            //olvCharacters.Size = tabCharacters.ClientSize;
            olvItems.Size = tabItems.ClientSize;
            objectListView1.Size = tabPage1.ClientSize;
            objectListView2.Size = tabPage2.ClientSize;
            objectListView3.Size = tabPage3.ClientSize;
            objectListView4.Size = tabPage4.ClientSize;
            objectListView5.Size = tabPage5.ClientSize;
            objectListView6.Size = tabPage6.ClientSize;
            objectListView7.Size = tabPage7.ClientSize;
            objectListView8.Size = tabPage8.ClientSize;
            //olvBlacksmith.Size = tabBlacksmith.ClientSize;
            //olvStoreItems.Size = tabShops.ClientSize;
            //olvSpells.Size = tabSpells.ClientSize;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //olvBlacksmith.FinishCellEdit();
            //olvMonsters.FinishCellEdit();
            //olvCharacters.FinishCellEdit();
            olvItems.FinishCellEdit();
            objectListView1.FinishCellEdit();
            objectListView2.FinishCellEdit();
            objectListView3.FinishCellEdit();
            objectListView4.FinishCellEdit();
            objectListView5.FinishCellEdit();
            objectListView6.FinishCellEdit();
            objectListView7.FinishCellEdit();
            objectListView8.FinishCellEdit();
            //olvStoreItems.FinishCellEdit();
            //olvSpells.FinishCellEdit();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x002 (.bin)|X002.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                FileEditor.saveFile(savefile.FileName);
            }
        }

        private void frmMonsterEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*try {
                byte[] data = olvMonsters.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/monsterstate." + Version + ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            } catch (Exception) { }*/
            /*
            try {
                byte[] data = olvItems.SaveState();
                FileStream stream = new FileStream(Application.StartupPath + "/Resources/itemstate." +
                     ".bin", FileMode.Create, FileAccess.Write);
                stream.Write(data, 0, data.Length);
                stream.Close();
            } catch (Exception) { }*/
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
            }
            else if (e.Value is Spell)
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
            }
            else if (e.Value is Preset)
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
            }

            else if (e.Value is Loading)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(LoadList.getLoadList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }

            else if (e.Value is StatBoost)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(StatList.getStatList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }

            else if (e.Value is WeaponRank)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(WeaponRankList.getWeaponRankList());
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }

            else if (e.Value is AttackResist)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(AttackResistList.getAttackResistList());
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
            if (e.Value is Item)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Item value = (Item)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is Spell)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Spell value = (Spell)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is Preset)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Preset value = (Preset)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is Loading)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Loading value = (Loading)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }

            else if (e.Value is StatBoost)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                StatBoost value = (StatBoost)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is WeaponRank)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                WeaponRank value = (WeaponRank)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is AttackResist)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                AttackResist value = (AttackResist)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }

            /*else if (e.Value is CharacterClass) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                CharacterClass value = (CharacterClass)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            } else if (e.Value is StoreItemType) {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                StoreItemType value = (StoreItemType)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }*/
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmMonsterEditor_Resize(this, new EventArgs());
        }

        private void tabpage_Click(object sender, EventArgs e)
        {
            objectListView1.ClearObjects();
            objectListView1.AddObjects(_spellList.Models);
            objectListView4.ClearObjects();
            objectListView4.AddObjects(_statList.Models);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        public static class Globals
        {
            public static int scenario = 1;
            //public static int customOffset = 0x00000000;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Globals.scenario = 1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Globals.scenario = 2;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Globals.scenario = 3;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Globals.scenario = 4;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
