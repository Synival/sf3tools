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
    // TODO: place this somewhere else!
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
        // Used to display version in the application
        private string Version = "0.19";

        private ScenarioType _scenario = (ScenarioType) (-1); // uninitialized value

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
            }
        }

        private StatsList _statsList;
        private InitialInfoList _initialInfoList;
        private WeaponLevelList _weaponLevelList;

        private IX033_X031_FileEditor _fileEditor;

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
            this.tsmiHelp_Version.Text = "Version " + Version;
            Scenario = ScenarioType.Scenario1;
        }

        private bool initialise()
        {
            tsmiFile_SaveAs.Enabled = true;
            tsmiFile_CopyTo.Enabled = true;

            _statsList = new StatsList(_fileEditor);
            if (!_statsList.Load())
            {
                MessageBox.Show("Could not load Resources/classList.xml.");
                return false;
            }

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

            olvStats.ClearObjects();
            olvSpells.ClearObjects();
            olvEquipStatistics.ClearObjects();
            olvMiscellaneous.ClearObjects();
            olvInitialInfo.ClearObjects();
            olvWeaponLevelReq.ClearObjects();
            olvCurveCalc.ClearObjects();

            olvStats.AddObjects(_statsList.Models);
            olvSpells.AddObjects(_statsList.Models);
            olvEquipStatistics.AddObjects(_statsList.Models);
            olvMiscellaneous.AddObjects(_statsList.Models);
            olvInitialInfo.AddObjects(_initialInfoList.Models);
            olvWeaponLevelReq.AddObjects(_weaponLevelList.Models);
            olvCurveCalc.AddObjects(_statsList.Models);

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = _statsList.Models;
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void tsmiFile_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X033.bin)|X033.bin|SF3 data (X031.bin)|X031.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor = new X033_X031_FileEditor(Scenario);
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

        private void tsmiFile_SaveAs_Click(object sender, EventArgs e)
        {
            if (_fileEditor == null)
            {
                return;
            }

            // TODO: refactor out!
            olvStats.FinishCellEdit();
            olvSpells.FinishCellEdit();
            olvEquipStatistics.FinishCellEdit();
            olvMiscellaneous.FinishCellEdit();
            olvInitialInfo.FinishCellEdit();
            olvWeaponLevelReq.FinishCellEdit();
            olvCurveCalc.FinishCellEdit();

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Sf3 x033 (.bin)|X033.bin|SF3 data (X031.bin)|X031.bin|Sf3 datafile (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            savefile.FileName = Path.GetFileName(_fileEditor.Filename);
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                _fileEditor.SaveFile(savefile.FileName);
            }
        }

        private void olvCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e) => Editor.Utils.EnhanceOlvCellEditControl(sender as ObjectListView, e);

        public static class Debugs
        {
            public static bool debugs = false;
        }

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;

        private void tsmiHelp_DebugCurve_Click(object sender, EventArgs e)
        {
            Debugs.debugs = !Debugs.debugs;
            tsmiHelp_DebugCurve.Checked = Debugs.debugs;
        }

        private void tabMain_Click(object sender, EventArgs e)
        {
            olvCurveCalc.ClearObjects();
            if (_statsList != null)
            {
                olvCurveCalc.AddObjects(_statsList.Models);
            }
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e) => RefreshCurveGraph();

        // TODO: this method does way too much work and shouldn't belong in the form. sort it out!!
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
            // NOTE: The actual stat gain caps at (30, 99, 99).
            //       This is different from level gains, which are (20, 99, 99).
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
                                keyValue.Value.GetWeightedMedianAt(0.005),
                                keyValue.Value.GetWeightedMedianAt(0.25),
                                keyValue.Value.GetWeightedMedianAt(0.75),
                                keyValue.Value.GetWeightedMedianAt(0.995)
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

                var range1Series = CurveGraph.Series[statTypeStr + " Range 1 (50% Likely)"];
                range1Series.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                {
                    range1Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[1], dataPoint.ProbableStats[statType].AtPercentages[2]);
                }

                var range2Series = CurveGraph.Series[statTypeStr + " Range 2 (99% Likely)"];
                range2Series.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                {
                    range2Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[0], dataPoint.ProbableStats[statType].AtPercentages[3]);
                }
            }
        }

        private void tsmiFile_CopyTo_Click(object sender, EventArgs e)
        {
            if (_fileEditor == null)
            {
                return;
            }

            // TODO: refactor out!
            olvStats.FinishCellEdit();
            olvSpells.FinishCellEdit();
            olvEquipStatistics.FinishCellEdit();
            olvMiscellaneous.FinishCellEdit();
            olvInitialInfo.FinishCellEdit();
            olvWeaponLevelReq.FinishCellEdit();
            olvCurveCalc.FinishCellEdit();

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SF3 data (X033.bin)|X033.bin|SF3 data (X031.bin)|X031.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";
            if (openfile.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var copyFileEditor = new X033_X031_FileEditor(Scenario);
            if (!copyFileEditor.LoadFile(openfile.FileName))
            {
                MessageBox.Show("Error trying to load file. It is probably in use by another process.");
                return;
            }

            try
            {
                // TODO: refactor out!
                var copyStatsList = new StatsList(copyFileEditor);
                if (!copyStatsList.Load())
                {
                    MessageBox.Show("Could not load Resources/classList.xml.");
                    return;
                }

                var copyInitialInfoList = new InitialInfoList(copyFileEditor);
                if (!copyInitialInfoList.Load())
                {
                    MessageBox.Show("Could not load Resources/classEquip.xml.");
                    return;
                }

                var copyWeaponLevelList = new WeaponLevelList(copyFileEditor);
                if (!copyWeaponLevelList.Load())
                {
                    MessageBox.Show("Could not load Resources/WeaponLevel.xml.");
                    return;
                }

                Utils.BulkCopyProperties<Models.Stats.Stats>(_statsList.Models, copyStatsList.Models);
                Utils.BulkCopyProperties<InitialInfo>(_initialInfoList.Models, copyInitialInfoList.Models);
                Utils.BulkCopyProperties<WeaponLevel>(_weaponLevelList.Models, copyWeaponLevelList.Models);
            }
            catch (System.Reflection.TargetInvocationException)
            {
                //wrong file was selected
                MessageBox.Show("Failed to read file:\n" +
                                "    " + openfile.FileName);
                return;
            }
            catch (FileEditorReadException)
            {
                //wrong file was selected
                MessageBox.Show("Data appears corrupt or invalid:\n" +
                                "    " + openfile.FileName + "\n\n" +
                                "Is this the correct type of file?");
                return;
            }

            if (!copyFileEditor.SaveFile(openfile.FileName))
            {
                MessageBox.Show("Failed to write to " + openfile.FileName);
                return;
            }

            MessageBox.Show("All tables copied successfully.");
        }
    }
}
