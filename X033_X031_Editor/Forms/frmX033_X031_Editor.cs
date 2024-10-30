using System;
using System.Collections.Generic;
using BrightIdeasSoftware;
using CommonLib.Statistics;
using SF3.Editor.Extensions;
using SF3.Editor.Forms;
using SF3.FileEditors;
using SF3.Models;
using SF3.Statistics;
using SF3.Types;
using static SF3.Editor.Extensions.TabControlExtensions;
using ProbableStatsDict = System.Collections.Generic.Dictionary<SF3.Types.StatType, SF3.X033_X031_Editor.Forms.ProbableStats>;
using StatDict = System.Collections.Generic.Dictionary<SF3.Types.StatType, double>;

namespace SF3.X033_X031_Editor.Forms {
    // TODO: place this somewhere else!
    public readonly struct ProbableStats {
        public ProbableStats(double likely, double[] atPercentages) {
            Likely = likely;
            AtPercentages = atPercentages;
        }

        public double Likely { get; }
        public double[] AtPercentages { get; }
    }

    public partial class frmX033_X031_Editor : EditorForm {
        // Used to display version in the application
        protected override string Version => "0.21";

        public new IX033_X031_FileEditor FileEditor => base.FileEditor as IX033_X031_FileEditor;

        public class StatDataPoint {
            public StatDataPoint(int level, StatDict stats) {
                Level = level;
                Stats = stats;
            }

            public int Level { get; }
            public StatDict Stats { get; }
        }

        public class ProbableStatsDataPoint {
            public ProbableStatsDataPoint(int level, ProbableStatsDict probableStats) {
                Level = level;
                ProbableStats = probableStats;
            }

            public int Level { get; }
            public ProbableStatsDict ProbableStats { get; }
        }

        public frmX033_X031_Editor() {
            InitializeComponent();
            InitializeEditor(menuStrip2);
        }

        protected override string FileDialogFilter => "SF3 Data (X033.BIN;X031.BIN)|X033.BIN;X031.BIN|" + base.FileDialogFilter;

        protected override IFileEditor MakeFileEditor() => new X033_X031_FileEditor(Scenario);

        protected override bool OnLoad() {
            if (!base.OnLoad())
                return false;

            if (!tabMain.PopulateAndToggleTabs(new List<PopulateTabConfig>() {
                new PopulateTabConfig(tabStats, olvStats, FileEditor.StatsTable),
                new PopulateTabConfig(tabSpells, olvSpells, FileEditor.StatsTable),
                new PopulateTabConfig(tabEquipStatistics, olvEquipStatistics, FileEditor.StatsTable),
                new PopulateTabConfig(tabMiscellaneous, olvMiscellaneous, FileEditor.StatsTable),
                new PopulateTabConfig(tabInitialInfo, olvInitialInfo, FileEditor.InitialInfoTable),
                new PopulateTabConfig(tabWeaponLevelReq, olvWeaponLevelReq, FileEditor.WeaponLevelTable),
                new PopulateTabConfig(tabCurveCalc, olvCurveCalc, FileEditor.StatsTable)
            })) {
                return false;
            }

            // Update curve graph controls.
            cbCurveGraphCharacter.DataSource = FileEditor.StatsTable.Rows;
            cbCurveGraphCharacter.DisplayMember = "Name";

            return true;
        }

        private void olvCellEditStarting(object sender, CellEditEventArgs e) => (sender as ObjectListView).EnhanceOlvCellEditControl(e);

        private void tsmiDebug_DebugCurve_Click(object sender, EventArgs e) {
            Stats.DebugGrowthValues = !Stats.DebugGrowthValues;
            tsmiDebug_DebugCurve.Checked = Stats.DebugGrowthValues;
        }

        private void tabMain_Click(object sender, EventArgs e) {
            olvCurveCalc.ClearObjects();
            if (FileEditor?.StatsTable != null)
                olvCurveCalc.AddObjects(FileEditor?.StatsTable.Rows);
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e) => RefreshCurveGraph();

        // TODO: this method does way too much work and shouldn't belong in the form. sort it out!!
        private void RefreshCurveGraph() {
            // Data points for the chart.
            var targetStatDataPoints = new List<StatDataPoint>();
            var probableStatsDataPoints = new List<ProbableStatsDataPoint>();

            // Get the stats model for the selected character.
            var index = cbCurveGraphCharacter.SelectedIndex;
            var stats = (index >= 0 && index < FileEditor.StatsTable.Rows.Length) ? FileEditor.StatsTable.Rows[index] : null;

            // We'll need to use some different values depending on the promotion level.
            var promotionLevel = (int?)stats?.PromotionLevel ?? 0;
            var isPromoted = promotionLevel >= 1;

            // Default axis ranges.
            // NOTE: The actual stat gain caps at (30, 99, 99).
            //       This is different from level gains, which are (20, 99, 99).
            var maxLevel = isPromoted ? 40 : 20;
            var maxValue = promotionLevel == 0 ? 50 : promotionLevel == 1 ? 100 : 200;

            // Did we find stats? If so, populate our data sets.
            if (stats != null) {
                // Function to convert a ProbableValueSet to a ProbableStatsDict.
                ProbableStatsDict GetProbableStats(Dictionary<StatType, ProbableValueSet> pvs) {
                    var probableStats = new ProbableStatsDict();
                    foreach (var keyValue in pvs) {
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
                }

                // Add initial stats for level 1.
                var startStatValues = new StatDict();
                foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                    var targetStat = stats.GetStatGrowthRange(statType, 0).Begin;
                    startStatValues.Add(statType, targetStat);
                    maxValue = Math.Max(maxValue, targetStat);
                }
                targetStatDataPoints.Add(new StatDataPoint(1, startStatValues));

                // Get initial probable stats for level 1 (which are the same as startStatValues).
                var currentProbableStatValues = new Dictionary<StatType, ProbableValueSet>();
                foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                    currentProbableStatValues[statType] = new ProbableValueSet() {
                        { (int) startStatValues[statType], 1.00 }
                    };
                }

                probableStatsDataPoints.Add(new ProbableStatsDataPoint(1, GetProbableStats(currentProbableStatValues)));

                // Populate data points for all stat growth groups, until the max level.
                foreach (var statGrowthGroup in GrowthStats.StatGrowthGroups[isPromoted]) {
                    // Add the next target stats.
                    var statValues = new StatDict();
                    foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                        var targetStat = stats.GetStatGrowthRange(statType, statGrowthGroup.GroupIndex).End;
                        statValues.Add(statType, targetStat);
                        maxValue = Math.Max(maxValue, targetStat);
                    }
                    targetStatDataPoints.Add(new StatDataPoint(statGrowthGroup.Range.End, statValues));

                    // Add probable stat values for every level in this stat growth group.
                    for (var lv = statGrowthGroup.Range.Begin + 1; lv <= statGrowthGroup.Range.End; lv++) {
                        foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                            var growthValue = stats.GetAverageStatGrowthPerLevel(statType, statGrowthGroup.GroupIndex);
                            var guaranteedGrowth = (int)growthValue;
                            var plusOneProbability = growthValue - guaranteedGrowth;

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

            foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                var statTypeStr = statType.ToString();

                var targetSeries = CurveGraph.Series[statTypeStr];
                targetSeries.Points.Clear();
                foreach (var dataPoint in targetStatDataPoints)
                    _ = targetSeries.Points.AddXY(dataPoint.Level, dataPoint.Stats[statType]);

                var likelySeries = CurveGraph.Series["Likely " + statTypeStr];
                likelySeries.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                    _ = likelySeries.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].Likely);

                var range1Series = CurveGraph.Series[statTypeStr + " Range 1 (50% Likely)"];
                range1Series.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                    _ = range1Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[1], dataPoint.ProbableStats[statType].AtPercentages[2]);

                var range2Series = CurveGraph.Series[statTypeStr + " Range 2 (99% Likely)"];
                range2Series.Points.Clear();
                foreach (var dataPoint in probableStatsDataPoints)
                    _ = range2Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[0], dataPoint.ProbableStats[statType].AtPercentages[3]);
            }
        }
    }
}
