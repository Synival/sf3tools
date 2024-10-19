using System;
using BrightIdeasSoftware;
using System.Collections.Generic;
using SF3.Types;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;

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
    using static SF3.Editor.Extensions.TabControlExtensions;

    public partial class frmX033_X031_Editor : EditorForm
    {
        // Used to display version in the application
        private string Version = "0.20";

        new public IX033_X031_FileEditor FileEditor => base.FileEditor as IX033_X031_FileEditor;

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
            BaseTitle = this.Text + " " + Version;

            this.tsmiHelp_Version.Text = "Version " + Version;

            EventHandler onScenarioChanged = (obj, eargs) =>
            {
                tsmiScenario_Scenario1.Checked = (Scenario == ScenarioType.Scenario1);
                tsmiScenario_Scenario2.Checked = (Scenario == ScenarioType.Scenario2);
                tsmiScenario_Scenario3.Checked = (Scenario == ScenarioType.Scenario3);
                tsmiScenario_PremiumDisk.Checked = (Scenario == ScenarioType.PremiumDisk);
            };

            ScenarioChanged += onScenarioChanged;
            onScenarioChanged(null, EventArgs.Empty);

            FileIsLoadedChanged += (obj, eargs) =>
            {
                tsmiFile_SaveAs.Enabled = IsLoaded == true;
                tsmiFile_CopyTablesFrom.Enabled = IsLoaded == true;
                tsmiFile_Close.Enabled = IsLoaded == true;
            };

            FinalizeForm();
        }

        protected override string FileDialogFilter => "SF3 data (X033.bin;X031.bin)|X033.bin;X031.bin|Binary File (*.bin)|*.bin|" + "All Files (*.*)|*.*";

        protected override IFileEditor MakeFileEditor() => new X033_X031_FileEditor(Scenario);

        protected override bool OnLoad()
        {
            if (!base.OnLoad())
            {
                return false;
            }

            if (!tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>()
            {
                new PopulateTabConfig(tabStats, olvStats, FileEditor.StatsList),
                new PopulateTabConfig(tabSpells, olvSpells, FileEditor.StatsList),
                new PopulateTabConfig(tabEquipStatistics, olvEquipStatistics, FileEditor.StatsList),
                new PopulateTabConfig(tabMiscellaneous, olvMiscellaneous, FileEditor.StatsList),
                new PopulateTabConfig(tabInitialInfo, olvInitialInfo, FileEditor.InitialInfoList),
                new PopulateTabConfig(tabWeaponLevelReq, olvWeaponLevelReq, FileEditor.WeaponLevelList),
                new PopulateTabConfig(tabCurveCalc, olvCurveCalc, FileEditor.StatsList)
            }))
            {
                return false;
            }

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = FileEditor.StatsList.Models;
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void olvCellEditStarting(object sender, CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiFile_Open_Click(object sender, EventArgs e) => OpenFileDialog();
        private void tsmiFile_SaveAs_Click(object sender, EventArgs e) => SaveFileDialog();
        private void tsmiFile_Close_Click(object sender, EventArgs e) => CloseFile();
        private void tsmiFile_CopyTablesFrom_Click(object sender, EventArgs e) => CopyTablesFrom();
        private void tsmiFile_Exit_Click(object sender, EventArgs e) => Close();

        private void tsmiScenario_Scenario1_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario1;
        private void tsmiScenario_Scenario2_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario2;
        private void tsmiScenario_Scenario3_Click(object sender, EventArgs e) => Scenario = ScenarioType.Scenario3;
        private void tsmiScenario_PremiumDisk_Click(object sender, EventArgs e) => Scenario = ScenarioType.PremiumDisk;

        private void tsmiHelp_DebugCurve_Click(object sender, EventArgs e)
        {
            Models.X033_X031.Stats.Stats.DebugGrowthValues = !Models.X033_X031.Stats.Stats.DebugGrowthValues;
            tsmiHelp_DebugCurve.Checked = Models.X033_X031.Stats.Stats.DebugGrowthValues;
        }

        private void tabMain_Click(object sender, EventArgs e)
        {
            olvCurveCalc.ClearObjects();
            if (FileEditor?.StatsList != null)
            {
                olvCurveCalc.AddObjects(FileEditor?.StatsList.Models);
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
            Models.X033_X031.Stats.Stats stats = (index >= 0 && index < FileEditor.StatsList.Models.Length) ? FileEditor.StatsList.Models[index] : null;

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
    }
}
