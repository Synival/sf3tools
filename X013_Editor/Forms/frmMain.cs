﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X013_Editor.Models;
using SF3.X013_Editor.Models.Spells;
using SF3.X013_Editor.Models.Presets;
using SF3.X013_Editor.Models.Items;
using SF3.X013_Editor.Models.Stats;
using SF3.X013_Editor.Models.Soulmate;
using SF3.X013_Editor.Models.Soulfail;
using SF3.X013_Editor.Models.MagicBonus;
using SF3.X013_Editor.Models.CritMod;
using SF3.X013_Editor.Models.Critrate;
using SF3.X013_Editor.Models.SpecialChance;
using SF3.X013_Editor.Models.ExpLimit;
using SF3.X013_Editor.Models.HealExp;
using SF3.X013_Editor.Models.WeaponSpellRank;
using SF3.X013_Editor.Models.StatusEffects;
using BrightIdeasSoftware;
using SF3.Editor;
using SF3.Types;

/*

*/

namespace SF3.X013_Editor.Forms
{
    public partial class frmMain : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "16";

        private ScenarioType _scenario = ScenarioType.Scenario1;

        private ItemList _itemList;
        private SpellList _spellList;
        private PresetList _presetList;
        private StatsList _statsList;
        private SoulmateList _soulmateList;
        private SoulfailList _soulfailList;
        private MagicBonusList _magicBonusList;
        private CritModList _critModList;
        private CritrateList _critrateList;
        private SpecialChanceList _specialChanceList;
        private ExpLimitList _expLimitList;
        private HealExpList _healExpList;
        private WeaponSpellRankList _weaponSpellRankList;
        private StatusEffectList _statusEffectList;

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
        private string getStatName(object target)
        {
            return ((Stat)target).StatName;
        }
        private string getSoulmateName(object target)
        {
            return ((Soulmate)target).SoulmateName;
        }
        private string getSoulfailName(object target)
        {
            return ((Soulfail)target).SoulfailName;
        }
        private string getMagicBonus(object target)
        {
            return ((MagicBonus)target).MagicName;
        }
        private string getCritMod(object target)
        {
            return ((CritMod)target).CritModName;
        }
        private string getCritrate(object target)
        {
            return ((Critrate)target).CritrateName;
        }

        private string getSpecialChance(object target)
        {
            return ((SpecialChance)target).SpecialChanceName;
        }
        private string getExpLimit(object target)
        {
            return ((ExpLimit)target).ExpLimitName;
        }
        private string getHealExp(object target)
        {
            return ((HealExp)target).HealExpName;
        }
        private string getWeaponSpellRank(object target)
        {
            return ((WeaponSpellRank)target).WeaponSpellRankName;
        }

        private string getStatusEffect(object target)
        {
            return ((StatusEffect)target).StatusEffectName;
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

            _itemList = new ItemList(_scenario);
            if (!_itemList.Load())
            {
                MessageBox.Show("Could not load Resources/itemList.xml.");
                return false;
            }

            _spellList = new SpellList(_scenario);
            if (!_spellList.Load())
            {
                MessageBox.Show("Could not load Resources/characters.xml.");
                return false;
            }

            _presetList = new PresetList(_scenario);
            if (!_presetList.Load())
            {
                MessageBox.Show("Could not load Resources/ExpList.xml.");
                return false;
            }

            _statsList = new StatsList(_scenario);
            if (!_statsList.Load())
            {
                MessageBox.Show("Could not load Resources/StatList.xml.");
                return false;
            }

            _soulmateList = new SoulmateList(_scenario);
            if (!_soulmateList.Load())
            {
                MessageBox.Show("Could not load Resources/SoulmateList.xml.");
                return false;
            }

            _soulfailList = new SoulfailList(_scenario);
            if (!_soulfailList.Load())
            {
                MessageBox.Show("Could not load Resources/Soulfail.xml.");
                return false;
            }

            _magicBonusList = new MagicBonusList(_scenario);
            if (!_magicBonusList.Load())
            {
                MessageBox.Show("Could not load Resources/MagicBonus.xml.");
                return false;
            }

            _critModList = new CritModList(_scenario);
            if (!_critModList.Load())
            {
                MessageBox.Show("Could not load Resources/CritModList.xml.");
                return false;
            }

            _critrateList = new CritrateList(_scenario);
            if (!_critrateList.Load())
            {
                MessageBox.Show("Could not load Resources/CritrateList.xml.");
                return false;
            }

            _specialChanceList = new SpecialChanceList(_scenario);
            if (!_specialChanceList.Load())
            {
                MessageBox.Show("Could not load Resources/SpecialChanceList.xml.");
                return false;
            }
            _expLimitList = new ExpLimitList(_scenario);
            if (!_expLimitList.Load())
            {
                MessageBox.Show("Could not load Resources/ExpLimitList.xml.");
                return false;
            }
            _healExpList = new HealExpList(_scenario);
            if (!_healExpList.Load())
            {
                MessageBox.Show("Could not load Resources/HealExpList.xml.");
                return false;
            }
            _weaponSpellRankList = new WeaponSpellRankList(_scenario);
            if (!_weaponSpellRankList.Load())
            {
                MessageBox.Show("Could not load Resources/WeaponSpellRankListList.xml.");
                return false;
            }
            _statusEffectList = new StatusEffectList(_scenario);
            if (!_statusEffectList.Load())
            {
                MessageBox.Show("Could not load Resources/StatusGroupList.xml.");
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
            objectListView9.ClearObjects();
            objectListView10.ClearObjects();
            objectListView11.ClearObjects();
            objectListView12.ClearObjects();
            objectListView13.ClearObjects();

            //olvPresets.ClearObjects();
            //olvSpells.ClearObjects();
            //olvStoreItems.ClearObjects();

            //olvMonsters.AddObjects(MonsterList.getMonsterList());

            olvItems.AddObjects(_itemList.Models);
            objectListView1.AddObjects(_presetList.Models);
            objectListView2.AddObjects(_spellList.Models);
            objectListView3.AddObjects(_statsList.Models);
            objectListView4.AddObjects(_soulmateList.Models);
            objectListView5.AddObjects(_soulfailList.Models);
            objectListView6.AddObjects(_magicBonusList.Models);
            objectListView7.AddObjects(_critModList.Models);
            objectListView8.AddObjects(_critrateList.Models);
            objectListView9.AddObjects(_specialChanceList.Models);
            objectListView10.AddObjects(_expLimitList.Models);
            objectListView11.AddObjects(_healExpList.Models);
            objectListView12.AddObjects(_weaponSpellRankList.Models);
            objectListView13.AddObjects(_statusEffectList.Models);

            //olvCharacters.AddObjects(CharacterList.getCharacterList());
            //olvBlacksmith.AddObjects(BlacksmithList.getBlacksmithList());
            //olvStoreItems.AddObjects(StoreItemList.getStoreItemList());
            //olvSpells.AddObjects(SpellEntryList.getSpellEntryList());
            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 scn3 data (X013.bin)|X013.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
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
            objectListView9.Size = tabPage9.ClientSize;
            objectListView10.Size = tabPage10.ClientSize;
            objectListView11.Size = tabPage11.ClientSize;
            objectListView12.Size = tabPage12.ClientSize;
            objectListView13.Size = tabPage13.ClientSize;
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
            objectListView9.FinishCellEdit();
            objectListView10.FinishCellEdit();
            objectListView11.FinishCellEdit();
            objectListView12.FinishCellEdit();
            objectListView13.FinishCellEdit();
            //olvStoreItems.FinishCellEdit();
            //olvSpells.FinishCellEdit();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x013 (.bin)|X013.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
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
                /*} else if (e.Value is Spell) {
                    PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                    Spell value = (Spell)((ComboBox)e.Control).SelectedItem;
                    property.SetValue(e.RowObject, value, null);
                } else if (e.Value is Preset) {
                    PropertyInfo property = e.RowObject.GetType().GetProperty(e.Column.AspectName);
                    Preset value = (Preset)((ComboBox)e.Control).SelectedItem;
                    property.SetValue(e.RowObject, value, null);*/
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
    }
}
