using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.IconPointerEditor.Models.Presets;
using SF3.IconPointerEditor.Models.Items;
using BrightIdeasSoftware;
using SF3.Types;
using SF3.Exceptions;

/*

*/

namespace SF3.IconPointerEditor.Forms
{
    public partial class frmMain : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "007";

        private ScenarioType _scenario = ScenarioType.Scenario1;

        private ItemList _itemList;
        private PresetList _presetList;

        private IFileEditor _fileEditor;

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
        /*private string getSpellName(object target)
        {
            return ((Spell)target).SpellName;
        }*/
        private string getPresetName(object target)
        {
            return ((Preset)target).SizeName;
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
            _itemList = new ItemList(_fileEditor, _scenario);
            if (!_itemList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            /*if (!SpellList.loadSpellList()) {
                MessageBox.Show("Could not load Resources/spellList.xml.");
                return false;
            }*/

            _presetList = new PresetList(_fileEditor, _scenario);
            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/spellIndexList.xml.");
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

            //olvPresets.ClearObjects();
            //olvSpells.ClearObjects();
            //olvSpells.ClearObjects();
            //olvStoreItems.ClearObjects();

            //olvMonsters.AddObjects(MonsterList.getMonsterList());

            olvItems.AddObjects(_presetList.Models);
            objectListView1.AddObjects(_itemList.Models);

            //olvCharacters.AddObjects(CharacterList.getCharacterList());
            //olvBlacksmith.AddObjects(BlacksmithList.getBlacksmithList());
            //olvStoreItems.AddObjects(StoreItemList.getStoreItemList());
            //olvSpells.AddObjects(SpellEntryList.getSpellEntryList());
            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X011*.bin)|X011*.bin|SF3 data (X021*.bin)|X021*.bin|SF3 data (X026*.bin)|X026*.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor = new FileEditor();
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
            //olvBlacksmith.Size = tabBlacksmith.ClientSize;
            //olvStoreItems.Size = tabShops.ClientSize;
            //olvSpells.Size = tabSpells.ClientSize;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_fileEditor == null)
            {
                return;
            }

            //olvBlacksmith.FinishCellEdit();
            //olvMonsters.FinishCellEdit();
            //olvCharacters.FinishCellEdit();
            olvItems.FinishCellEdit();
            objectListView1.FinishCellEdit();
            //objectListView1.FinishCellEdit();
            //objectListView2.FinishCellEdit();
            //olvStoreItems.FinishCellEdit();
            //olvSpells.FinishCellEdit();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 X011* (.bin)|X011.bin|Sf3 X021* (.bin)|X021.bin|Sf3 X026* (.bin)|X026.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
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
            else if (e.Value is Item)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(_itemList.Models);
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }
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
            else if (e.Value is Preset)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(_presetList.Models);
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }

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
                /*} else if (e.Value is Spell) {
                    PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                    Spell value = (Spell)((ComboBox)e.Control).SelectedItem;
                    property.SetValue(e.RowObject, value, null);*/
            }
            else if (e.Value is Preset)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Preset value = (Preset)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            } /*else if (e.Value is CharacterClass) {
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

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        public static class Globals
        {
            //public static int customOffset = 0x00000000;
            public static bool x026 = false;
            public static string toggle = "Off";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Scenario1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Scenario2;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Scenario3;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.PremiumDisk;
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            _scenario = ScenarioType.Other;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem5_Click_1(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem6_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show("Could not load Resources/itemList.xml.");
            if (Globals.x026 == true)
            {
                Globals.x026 = false;
                //this.Text = "Sf3 Icon pointer Editor" + "          " + "X026 mode: Off";
                Globals.toggle = "Off";
                updateText();
            }
            else
            {
                Globals.x026 = true;
                //this.Text = "Sf3 Icon pointer Editor" + "          " + "X026 mode: On";
                Globals.toggle = "On";
                updateText();
            }
        }

        private void updateText()
        {
            //this.toolStripMenuItem12.Text = "Current Loading info. Map: " + Globals.maps + " Scenario " + Globals.scn + " MapType: " + Globals.battle + " debug: " + Globals.debug;
            //this.Text = "Sf3 X1 editor" + "          " + "|OpenedFile: " + Globals.fileName + "|          Current Loading info: Scenario: " + Globals.scn + " | Map: " + Globals.maps + " | MapType: " + Globals.battle + " | debug: " + Globals.debug;
            this.Text = "Sf3 Icon pointer Editor" + "          " + "X026 mode: " + Globals.toggle;
        }
    }
}
