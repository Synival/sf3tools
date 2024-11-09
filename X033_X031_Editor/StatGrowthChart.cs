using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CommonLib.Statistics;
using SF3.Statistics;
using SF3.Tables;
using SF3.Types;
using ProbableStatsDict = System.Collections.Generic.Dictionary<SF3.Types.StatType, SF3.X033_X031_Editor.StatGrowthChart.ProbableStats>;
using StatDict = System.Collections.Generic.Dictionary<SF3.Types.StatType, double>;

namespace SF3.X033_X031_Editor {
    public class StatGrowthChart {
        public readonly struct ProbableStats {
            public ProbableStats(ProbableValueSet pvs, double target) {
                ProbableValueSet = pvs;
                Target = target;

                Likely = pvs.GetWeightedAverage();
                AtPercentages = new double[] {
                    pvs.GetWeightedMedianAt(0.005),
                    pvs.GetWeightedMedianAt(0.25),
                    pvs.GetWeightedMedianAt(0.75),
                    pvs.GetWeightedMedianAt(0.995)
                };
            }

            public string MakeReport(double minimumProbabilityCutoff = 0.01) {
                var report =
                    "Target: " + Target.ToString("N2") + "\n" +
                    "Likely: " + Likely.ToString("N2") + "\n" +
                    "---------------------------\n";

                var totalPool = ProbableValueSet.Sum(x => x.Value);
                var adjustedSet = ProbableValueSet
                    .Where(x => (x.Value / totalPool) * 100.0 >= minimumProbabilityCutoff)
                    .ToDictionary(x => x.Key, x => x.Value);
                totalPool = adjustedSet.Sum(x => x.Value);

                var currentTotalProb = 0.0;
                var lastKey = adjustedSet.Last().Key;

                foreach (var kv in adjustedSet) {
                    var probability = (kv.Value / totalPool) * 100.0;
                    var nextTotalProb = (kv.Key == lastKey) ? 100.0 : (currentTotalProb + probability);

                    report += kv.Key + ": " + probability.ToString("N2") + "% (" + currentTotalProb.ToString("N2") + " - " + nextTotalProb.ToString("N2") + ")\n";
                    currentTotalProb = nextTotalProb;
                }
                return report;
            }

            public ProbableValueSet ProbableValueSet { get; }
            public double Target { get; }

            public double Likely { get; }
            public double[] AtPercentages { get; }
        }

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

            public string MakeReport(StatType stat) {
                return "Lv" + Level + " " + stat.ToString() + ":\n" +
                    "---------------------------\n" +
                    ProbableStats[stat].MakeReport();
            }

            public int Level { get; }
            public ProbableStatsDict ProbableStats { get; }
        }

        public StatGrowthChart(Chart curveGraph) {
            CurveGraph = curveGraph;
            curveGraph.MouseMove += OnMouseMove;
        }

        public Chart CurveGraph { get; }

        private ToolTip _mouseoverTooltip = new ToolTip();
        private ProbableStatsDataPoint[] _probableStats = null;

        private Point? _lastMousePos = null;
        private string _lastTooltipText = null;
        private int? _lastTooltipX = null;
        private int? _lastTooltipY = null;

        private void OnMouseMove(object sender, MouseEventArgs e) {
            // Do nothing if the position is unchanged or no data is available.
            var pos = e.Location;
            if ((_lastMousePos.HasValue && pos == _lastMousePos.Value) || _probableStats == null)
                return;
            _lastMousePos = pos;

            void clearTooltip() {
                _mouseoverTooltip.RemoveAll();
                _lastTooltipText = null;
                _lastTooltipX = null;
                _lastTooltipY = null;
            }

            // Get the first data point under the mouse.
            var hitTest = CurveGraph
                .HitTest(pos.X, pos.Y, true, ChartElementType.DataPoint, ChartElementType.PlottingArea);
            if (!hitTest.Any(x => x.ChartElementType == ChartElementType.PlottingArea)) {
                clearTooltip();
                return;
            }

            var dataPointsUnderMouse = hitTest
                .Where(x => x.ChartElementType == ChartElementType.DataPoint)
                .ToList();
            if (dataPointsUnderMouse.Count == 0) {
                clearTooltip();
                return;
            }

            // Get the data points under the mouse, in order of priority.
            var dataPointUnderMouse =
                dataPointsUnderMouse.FirstOrDefault(x => !x.Series.Name.Contains("Likely")) ??
                dataPointsUnderMouse.FirstOrDefault(x => x.Series.Name.Contains("Likely") && !x.Series.Name.Contains('%')) ??
                dataPointsUnderMouse.FirstOrDefault(x => x.Series.Name.Contains("50%")) ??
                dataPointsUnderMouse.FirstOrDefault(x => x.Series.Name.Contains("99%")) ??
                dataPointsUnderMouse.First();

            StatType? getStatForSeriesName(string name) {
                if (name.Contains("HP"))
                    return StatType.HP;
                else if (name.Contains("MP"))
                    return StatType.MP;
                else if (name.Contains("Atk"))
                    return StatType.Atk;
                else if (name.Contains("Def"))
                    return StatType.Def;
                else if (name.Contains("Agi"))
                    return StatType.Agi;
                else
                    return null;
            }

            // What stat does the data point under the mouse refer to?
            var stat = getStatForSeriesName(dataPointUnderMouse.Series.Name);

            // Now that was have the stat, get the "99%" area for display.
            var dataPoint =
                dataPointsUnderMouse.FirstOrDefault(x => x.Series.Name.Contains("99%") && getStatForSeriesName(x.Series.Name) == stat) ??
                dataPointsUnderMouse.FirstOrDefault(x => x.Series.Name.Contains("50%") && getStatForSeriesName(x.Series.Name) == stat) ??
                dataPointsUnderMouse.FirstOrDefault(x => x.Series.Name.Contains("Likely") && getStatForSeriesName(x.Series.Name) == stat) ??
                dataPointUnderMouse;

            if (stat == null || dataPoint == null) {
                clearTooltip();
                return;
            }

            var x0 = dataPoint.ChartArea.AxisX.ValueToPixelPosition(0.0);
            var xSpan = dataPoint.ChartArea.AxisX.ValueToPixelPosition(1.0) - dataPoint.ChartArea.AxisX.ValueToPixelPosition(0.0);
            var pointIndex = Math.Max(0, (pos.X < x0 + xSpan * ((double) dataPoint.PointIndex + 0.5)) ? dataPoint.PointIndex - 1 : dataPoint.PointIndex);

            var probableStats = _probableStats[pointIndex];
            var tooltipText = probableStats.MakeReport((StatType) stat);
            var tooltipX = (int) dataPoint.ChartArea.AxisX.ValueToPixelPosition(probableStats.Level);
            var tooltipY = (int) dataPoint.ChartArea.AxisY.ValueToPixelPosition(probableStats.ProbableStats[(StatType) stat].Likely);

            if (tooltipText != _lastTooltipText || tooltipX != _lastTooltipX || tooltipY != _lastTooltipY) {
                _mouseoverTooltip.Show(tooltipText, CurveGraph, tooltipX, tooltipY);
                _lastTooltipText = tooltipText;
                _lastTooltipX = tooltipX;
                _lastTooltipY = tooltipY;
            }
        }

        // TODO: this method does way too much work and shouldn't belong in the form. sort it out!!
        public void RefreshCurveGraph(StatsTable statsTable, ComboBox cbCurveGraphCharacter) {
            // Data points for the chart.
            var targetStatDataPoints = new List<StatDataPoint>();
            var probableStatsDataPoints = new List<ProbableStatsDataPoint>();

            // Get the stats model for the selected character.
            var index = cbCurveGraphCharacter.SelectedIndex;
            var stats = (index >= 0 && index < statsTable.Rows.Length) ? statsTable.Rows[index] : null;

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
                ProbableStatsDict GetProbableStats(Dictionary<StatType, ProbableValueSet> pvs, Dictionary<StatType, double> targets)
                    => pvs.ToDictionary(x => x.Key, x => new ProbableStats(x.Value, targets[x.Key]));

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

                probableStatsDataPoints.Add(new ProbableStatsDataPoint(1, GetProbableStats(currentProbableStatValues, startStatValues)));

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
                        var lowPoint = targetStatDataPoints.Last(x => x.Level <= lv);
                        var highPoint = targetStatDataPoints.First(x => x.Level >= lv);

                        StatDict targetStats;
                        if (lowPoint == highPoint || lv == lowPoint.Level)
                            targetStats = lowPoint.Stats;
                        else if (lv == highPoint.Level)
                            targetStats = highPoint.Stats;
                        else {
                            var levelRange = highPoint.Level - lowPoint.Level;
                            var rangePercent = (double) (lv - lowPoint.Level) / (double) levelRange;
                            targetStats = lowPoint.Stats.Keys
                                .ToDictionary(x => x, x => lowPoint.Stats[x] + (highPoint.Stats[x] - lowPoint.Stats[x]) * rangePercent);
                        }

                        foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                            var growthValue = stats.GetAverageStatGrowthPerLevel(statType, statGrowthGroup.GroupIndex);
                            var guaranteedGrowth = (int)growthValue;
                            var plusOneProbability = growthValue - guaranteedGrowth;

                            currentProbableStatValues[statType] = currentProbableStatValues[statType].RollNext(val => new ProbableValueSet() {
                                { val + guaranteedGrowth, 1.00 - plusOneProbability },
                                { val + guaranteedGrowth + 1, plusOneProbability }
                            });
                        }
                        probableStatsDataPoints.Add(new ProbableStatsDataPoint(lv, GetProbableStats(currentProbableStatValues, targetStats)));
                    }
                }
            }

            CurveGraph.ChartAreas[0].AxisX.Minimum = 0;
            CurveGraph.ChartAreas[0].AxisX.Maximum = maxLevel;
            CurveGraph.ChartAreas[0].AxisX.Interval = isPromoted ? 10 : 5;
            CurveGraph.ChartAreas[0].AxisY.Maximum = maxValue;
            CurveGraph.ChartAreas[0].AxisY.Interval = promotionLevel == 0 ? 5 : promotionLevel == 1 ? 10 : 20;

            _probableStats = probableStatsDataPoints.ToArray();

            foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                var statTypeStr = statType.ToString();

                var targetSeries = CurveGraph.Series[statTypeStr];
                targetSeries.Points.Clear();
                foreach (var dataPoint in targetStatDataPoints)
                    _ = targetSeries.Points.AddXY(dataPoint.Level, dataPoint.Stats[statType]);

                var likelySeries = CurveGraph.Series["Likely " + statTypeStr];
                likelySeries.Points.Clear();
                foreach (var dataPoint in _probableStats)
                    _ = likelySeries.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].Likely);

                var range1Series = CurveGraph.Series[statTypeStr + " Range 1 (50% Likely)"];
                range1Series.Points.Clear();
                foreach (var dataPoint in _probableStats)
                    _ = range1Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[1], dataPoint.ProbableStats[statType].AtPercentages[2]);

                var range2Series = CurveGraph.Series[statTypeStr + " Range 2 (99% Likely)"];
                range2Series.Points.Clear();
                foreach (var dataPoint in _probableStats)
                    _ = range2Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[0], dataPoint.ProbableStats[statType].AtPercentages[3]);
            }
        }
    }
}
