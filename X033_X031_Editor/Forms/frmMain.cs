using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X033_X031_Editor.Models;
using SF3.X033_X031_Editor.Models.InitialInfos;
using SF3.X033_X031_Editor.Models.Stats;
using SF3.X033_X031_Editor.Models.WeaponLevel;
using BrightIdeasSoftware;
using SF3.Editor;
using System.Linq;
using System.Collections.Generic;

/*

*/

namespace SF3.X033_X031_Editor.Forms
{
    public partial class frmMain : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "018";

        private StatsList _statsList = new StatsList();
        private InitialInfoList _initialInfoList = new InitialInfoList();
        private WeaponLevelList _weaponLevelList = new WeaponLevelList();

        public class CurveGraphDataPoint
        {
            public CurveGraphDataPoint(int level, int target)
            {
                Level = level;
                Target = target;
            }

            public int Level { get; set; }
            public int Target { get; set; }
        }

        public frmMain()
        {
            InitializeComponent();
            frmMain_Resize(this, new EventArgs());

            // Set up curve graph controls
            cbCurveGraphCharacter.DataSource = _statsList.Models;
            cbCurveGraphCharacter.DisplayMember = "Name";

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
            return ((Stats)target).Name;
        }
        private string getPresetName(object target)
        {
            return ((InitialInfo)target).PresetName;
        }

        private string getWeaponLevelName(object target)
        {
            return ((WeaponLevel)target).WeaponLevelName;
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
            if (!_statsList.Load())
            {
                MessageBox.Show("Could not load Resources/classList.xml.");
                return false;
            }

            /*
            if (!SpellList.loadSpellList()) {
                MessageBox.Show("Could not load Resources/classEquip.xml.");
                return false;
            }*/

            if (!_initialInfoList.Load())
            {
                MessageBox.Show("Could not load Resources/classEquip.xml.");
                return false;
            }

            if (!_weaponLevelList.Load())
            {
                MessageBox.Show("Could not load Resources/WeaponLevel.xml.");
                return false;
            }

            //BlacksmithList.loadBlacksmithList();
            //StoreItemList.loadStoreItemList();
            //SpellEntryList.loadSpellEntryList();

            //olvBlacksmith.ClearObjects();
            //olvCharacters.ClearObjects();
            //olvMonsters.ClearObjects();

            olvStats.ClearObjects();
            olvSpells.ClearObjects();
            olvEquipStatistics.ClearObjects();
            olvMiscellaneous.ClearObjects();
            olvInitialInfo.ClearObjects();
            olvWeaponLevelReq.ClearObjects();
            olvCurveCalc.ClearObjects();

            //olvPresets.ClearObjects();
            //olvSpells.ClearObjects();
            //olvSpells.ClearObjects();
            //olvStoreItems.ClearObjects();

            //olvMonsters.AddObjects(MonsterList.getMonsterList());

            olvStats.AddObjects(_statsList.Models);
            olvSpells.AddObjects(_statsList.Models);
            olvEquipStatistics.AddObjects(_statsList.Models);
            olvMiscellaneous.AddObjects(_statsList.Models);
            olvInitialInfo.AddObjects(_initialInfoList.Models);
            olvWeaponLevelReq.AddObjects(_weaponLevelList.Models);
            olvCurveCalc.AddObjects(_statsList.Models);

            //olvCharacters.AddObjects(CharacterList.getCharacterList());
            //olvBlacksmith.AddObjects(BlacksmithList.getBlacksmithList());
            //olvStoreItems.AddObjects(StoreItemList.getStoreItemList());
            //olvSpells.AddObjects(SpellEntryList.getSpellEntryList());

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = _statsList.Models;

            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X033.bin)|X033.bin|SF3 data (X031.bin)|X031.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
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

        private void frmMain_Resize(object sender, EventArgs e)
        {
            Size newsize = ClientSize;
            newsize.Height -= 24;
            tabMain.Size = newsize;
            //olvMonsters.Size = tabMonsters.ClientSize;
            //olvCharacters.Size = tabCharacters.ClientSize;
            olvStats.Size = tabStats.ClientSize;
            olvSpells.Size = tabSpells.ClientSize;
            olvEquipStatistics.Size = tabEquipStatistics.ClientSize;
            olvMiscellaneous.Size = tabMiscellaneous.ClientSize;
            olvInitialInfo.Size = tabInitialInfo.ClientSize;
            olvWeaponLevelReq.Size = tabWeaponLevelReq.ClientSize;
            olvCurveCalc.Size = tabCurveCalc.ClientSize;
            //olvBlacksmith.Size = tabBlacksmith.ClientSize;
            //olvStoreItems.Size = tabShops.ClientSize;
            //olvSpells.Size = tabSpells.ClientSize;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //olvBlacksmith.FinishCellEdit();
            //olvMonsters.FinishCellEdit();
            //olvCharacters.FinishCellEdit();
            olvStats.FinishCellEdit();
            olvSpells.FinishCellEdit();
            olvEquipStatistics.FinishCellEdit();
            olvMiscellaneous.FinishCellEdit();
            olvInitialInfo.FinishCellEdit();
            olvWeaponLevelReq.FinishCellEdit();
            olvCurveCalc.FinishCellEdit();
            //olvStoreItems.FinishCellEdit();
            //olvSpells.FinishCellEdit();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x033 (.bin)|X033.bin|SF3 data (X031.bin)|X031.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(FileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                FileEditor.saveFile(savefile.FileName);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
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
                //control.BackColor = Color.Aqua;
            }
            /*else if (e.Column.AspectToStringFormat == "{0:1}")
            {
                NumericUpDown control = (NumericUpDown)e.Control;
                control.binary? = true;
            } */
            else if (e.Value is Stats)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(_statsList.Models);
                cb.SelectedItem = e.Value;
                e.Control = cb;
            }
            else if (e.Value is InitialInfo)
            {
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                cb.AutoCompleteMode = AutoCompleteMode.Append;
                cb.ValueMember = "Value";
                cb.DisplayMember = "Name";
                cb.Items.AddRange(_initialInfoList.Models);
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
            if (e.Value is Stats)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                Stats value = (Stats)((ComboBox)e.Control).SelectedItem;
                property.SetValue(e.RowObject, value, null);
            }
            else if (e.Value is InitialInfo)
            {
                PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                InitialInfo value = (InitialInfo)((ComboBox)e.Control).SelectedItem;
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
            frmMain_Resize(this, new EventArgs());
        }

        public static class Globals
        {
            public static int scenario = 1;
            //public static int customOffset = 0x00000000;
        }

        public static class Debugs
        {
            public static int debugs = 0;
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (Debugs.debugs == 0)
            {
                Debugs.debugs = 1;
            }
            else
            {
                Debugs.debugs = 0;
            }
        }

        private void tabpage6_Click(object sender, EventArgs e)
        {
            olvCurveCalc.ClearObjects();
            olvCurveCalc.AddObjects(_statsList.Models);
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e) => RefreshCurveGraph();

        private void RefreshCurveGraph()
        {
            var curveGraphData = new List<CurveGraphDataPoint>();

            int index = cbCurveGraphCharacter.SelectedIndex;
            Stats stats = (index >= 0 && index < _statsList.Models.Length) ? _statsList.Models[index] : null;

            bool isPromoted = stats?.IsPromoted ?? false;

            if (stats != null)
            {
                int level;
                int value;
                for (int i = 0; i < 7; ++i)
                {
                    // TODO: Get stat from stat index
                    // TODO: Don't hard-code level ranges
                    switch (i)
                    {
                        case 0:
                            level = 1;
                            value = stats.HPCurve1;
                            break;
                        case 1:
                            level = 5;
                            value = stats.HPCurve5;
                            break;
                        case 2:
                            level = 10;
                            value = stats.HPCurve10;
                            break;
                        case 3:
                            level = isPromoted ? 15 : 12;
                            value = stats.HPCurve12_15;
                            break;
                        case 4:
                            level = isPromoted ? 20 : 14;
                            value = stats.HPCurve14_20;
                            break;
                        case 5:
                            level = isPromoted ? 30 : 17;
                            value = stats.HPCurve17_30;
                            break;
                        case 6:
                            level = isPromoted ? 99 : 20;
                            value = stats.HPCurve20_99;
                            break;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                    curveGraphData.Add(new CurveGraphDataPoint(level, value));
                }
            }

            CurveGraph.ChartAreas[0].AxisX.Maximum = isPromoted ? 101 : 21;
            CurveGraph.ChartAreas[0].AxisX.Interval = isPromoted ? 10 : 5;
            CurveGraph.ChartAreas[0].AxisY.Maximum = isPromoted ? 150 : 100;
            CurveGraph.ChartAreas[0].AxisY.Interval = 10;
            CurveGraph.DataSource = curveGraphData;
            CurveGraph.DataBind();
        }
    }
}
