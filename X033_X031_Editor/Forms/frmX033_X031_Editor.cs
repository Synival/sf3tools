using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using SF3.X033_X031_Editor.Models.InitialInfos;
using SF3.X033_X031_Editor.Models.Stats;
using SF3.X033_X031_Editor.Models.WeaponLevel;
using BrightIdeasSoftware;
using System.Linq;
using System.Collections.Generic;
using SF3.Types;
using SF3.Exceptions;

namespace SF3.X033_X031_Editor.Forms
{
    public struct ProbableStats
    {
        public ProbableStats(double likely, double[] atPercentages)
        {
            Likely = likely;
            AtPercentages = atPercentages;
        }

        public double Likely { get; }
        public double[] AtPercentages { get; }
    }
}

namespace SF3.X033_X031_Editor.Forms
{
    using StatDict = Dictionary<StatType, double>;
    using ProbableStatsDict = Dictionary<StatType, ProbableStats>;

    public partial class frmX033_X031_Editor : Form
    {
        //Used to append to state names to stop program loading states from older versions
        private string Version = "018";

        private ScenarioType _scenario = ScenarioType.Scenario1;

        private StatsList _statsList;
        private InitialInfoList _initialInfoList;
        private WeaponLevelList _weaponLevelList;

        private ISF3FileEditor _fileEditor;

        public class StatDataPoint
        {
            public StatDataPoint(int level, StatDict stats)
            {
                Level = level;
                Stats = stats;
            }

            public int Level { get; }
            public StatDict Stats { get; }
        }

        public class ProbableStatsDataPoint
        {
            public ProbableStatsDataPoint(int level, ProbableStatsDict probableStats)
            {
                Level = level;
                ProbableStats = probableStats;
            }

            public int Level { get; }
            public ProbableStatsDict ProbableStats { get; }
        }

        public frmX033_X031_Editor()
        {
            InitializeComponent();
            frmX033_X031_Editor_Resize(this, new EventArgs());

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
            return ((Models.Stats.Stats)target).Name;
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
            _statsList = new StatsList(_fileEditor);
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

            _initialInfoList = new InitialInfoList(_fileEditor);
            if (!_initialInfoList.Load())
            {
                MessageBox.Show("Could not load Resources/classEquip.xml.");
                return false;
            }

            _weaponLevelList = new WeaponLevelList(_fileEditor);
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
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X033.bin)|X033.bin|SF3 data (X031.bin)|X031.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor = new SF3FileEditor(_scenario);
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

        private void frmX033_X031_Editor_Resize(object sender, EventArgs e)
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
            if (_fileEditor == null)
            {
                return;
            }

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
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void frmX033_X031_Editor_FormClosing(object sender, FormClosingEventArgs e)
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
                var value = (Models.Stats.Stats)((ComboBox)e.Control).SelectedItem;
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
            frmX033_X031_Editor_Resize(this, new EventArgs());
        }

        public static class Debugs
        {
            public static bool debugs = false;
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
            Debugs.debugs = !Debugs.debugs;
        }

        private void tabpage6_Click(object sender, EventArgs e)
        {
            olvCurveCalc.ClearObjects();
            if (_statsList != null)
            {
                olvCurveCalc.AddObjects(_statsList.Models);
            }
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e) => RefreshCurveGraph();

        private void RefreshCurveGraph()
        {
            // Data points for the chart.
            var targetStatDataPoints = new List<StatDataPoint>();
            var probableStatsDataPoints = new List<ProbableStatsDataPoint>();

            // Get the stats model for the selected character.
            int index = cbCurveGraphCharacter.SelectedIndex;
            Models.Stats.Stats stats = (index >= 0 && index < _statsList.Models.Length) ? _statsList.Models[index] : null;

            // We'll need to use some different values depending on the promotion level.
            int promotionLevel = (int?)stats?.PromotionLevel ?? 0;
            bool isPromoted = promotionLevel >= 1;

            // Default axis ranges.
            int maxLevel = isPromoted ? 40 : 20;
            int maxValue = promotionLevel == 0 ? 50 : promotionLevel == 1 ? 100 : 200;

            // Did we find stats? If so, populate our data sets.
            if (stats != null)
            {
                // Function to convert a ProbableValueSet to a ProbableStatsDict.
                Func<Dictionary<StatType, ProbableValueSet>, ProbableStatsDict> GetProbableStats = (pvs) =>
                {
                    var probableStats = new ProbableStatsDict();
                    foreach (var keyValue in pvs)
                    {
                        probableStats.Add(keyValue.Key, new ProbableStats(
                            keyValue.Value.GetWeightedAverage(),
                            new double[] {
                                keyValue.Value.GetWeightedMedianAt(0.025),
                                keyValue.Value.GetWeightedMedianAt(0.25),
                                keyValue.Value.GetWeightedMedianAt(0.75),
                                keyValue.Value.GetWeightedMedianAt(0.975)
                            }
                        ));
                    }
                    return probableStats;
                };

                // Add initial stats for level 1.
                var startStatValues = new StatDict();
                foreach (var statType in (StatType[])Enum.GetValues(typeof(StatType)))
                {
                    var targetStat = stats.GetStatGrowthRange(statType, 0).Begin;
                    startStatValues.Add(statType, targetStat);
                    maxValue = Math.Max(maxValue, targetStat);
                }
                targetStatDataPoints.Add(new StatDataPoint(1, startStatValues));

                // Get initial probable stats for level 1 (which are the same as startStatValues).
                var currentProbableStatValues = new Dictionary<StatType, ProbableValueSet>();
                foreach (var statType in (StatType[])Enum.GetValues(typeof(StatType)))
                {
                    currentProbableStatValues[statType] = new ProbableValueSet()
                    {
                        { (int) startStatValues[statType], 1.00 }
                    };
                }
                probableStatsDataPoints.Add(new ProbableStatsDataPoint(1, GetProbableStats(currentProbableStatValues)));

                // Populate data points for all stat growth groups, until the max level.
                foreach (var statGrowthGroup in Stats.StatGrowthGroups[isPromoted])
                {
                    // Add the next target stats.
                    var statValues = new StatDict();
                    foreach (var statType in (StatType[])Enum.GetValues(typeof(StatType)))
                    {
                        var targetStat = stats.GetStatGrowthRange(statType, statGrowthGroup.GroupIndex).End;
                        statValues.Add(statType, targetStat);
                        maxValue = Math.Max(maxValue, targetStat);
                    }
                    targetStatDataPoints.Add(new StatDataPoint(statGrowthGroup.Range.End, statValues));

                    // Add probable stat values for every level in this stat growth group.
                    for (int lv = statGrowthGroup.Range.Begin + 1; lv <= statGrowthGroup.Range.End; lv++)
                    {
                        foreach (var statType in (StatType[])Enum.GetValues(typeof(StatType)))
                        {
                            var growthValue = stats.GetAverageStatGrowthPerLevel(statType, statGrowthGroup.GroupIndex);
                            var guaranteedGrowth = (int)growthValue;
                            var plusOneProbability = growthValue - (double)guaranteedGrowth;

                            currentProbableStatValues[statType] = currentProbableStatValues[statType].RollNext(val => new ProbableValueSet() {
                                { val + guaranteedGrowth, 1.00 - plusOneProbability },
                                { val + guaranteedGrowth + 1, plusOneProbability }
                            });
                        }
                        probableStatsDataPoints.Add(new ProbableStatsDataPoint(lv, GetProbableStats(currentProbableStatValues)));
                    }
                }
            }

            CurveGraph.ChartAreas[0].AxisX.Minimum = 0;
            CurveGraph.ChartAreas[0].AxisX.Maximum = maxLevel;
            CurveGraph.ChartAreas[0].AxisX.Interval = isPromoted ? 10 : 5;
            CurveGraph.ChartAreas[0].AxisY.Maximum = maxValue;
            CurveGraph.ChartAreas[0].AxisY.Interval = promotionLevel == 0 ? 5 : promotionLevel == 1 ? 10 : 20;

            foreach (var statType in (StatType[])Enum.GetValues(typeof(StatType)))
            {
                var statTypeStr = statType.ToString();

                var targetSeries = CurveGraph.Series[statTypeStr];
                targetSeries.Points.Clear();
                foreach (var dataPoint in targetStatDataPoints)
                {
                    targetSeries.Points.AddXY(dataPoint.Level, dataPoint.Stats[statType]);
                }

                var likelySeries = CurveGraph.Series["Likely " + statTypeStr];
                likelySeries.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                {
                    likelySeries.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].Likely);
                }

                var range1Series = CurveGraph.Series[statTypeStr + " Range 1"];
                range1Series.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                {
                    range1Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[1], dataPoint.ProbableStats[statType].AtPercentages[2]);
                }

                var range2Series = CurveGraph.Series[statTypeStr + " Range 2"];
                range2Series.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                {
                    range2Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[0], dataPoint.ProbableStats[statType].AtPercentages[3]);
                }
            }
        }
    }
}
