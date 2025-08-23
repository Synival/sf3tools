using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables.Shared;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class StatGrowthChartControl : UserControl {
        public StatGrowthChartControl() {
            InitializeComponent();
        }

        public StatGrowthChartControl(StatGrowthStatisticsTable statGrowthStatistics) {
            InitializeComponent();
            StatGrowthStatistics = statGrowthStatistics;
        }

        private StatGrowthStatisticsTable _statStatistics = null;
        public StatGrowthStatisticsTable StatGrowthStatistics {
            get => _statStatistics;
            set {
                if (_statStatistics != value) {
                    _statStatistics = value;
                    cbCurveGraphCharacter.DataSource = StatGrowthStatistics.StatsTable.Rows;
                    cbCurveGraphCharacter.DisplayMember = "Name";

                    RecalcData();
                    RefreshChart();
                }
            }
        }

        private void CurveGraphCharacterComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            RecalcData();
            RefreshChart();
        }

        private ToolTip _mouseoverTooltip = new ToolTip();
        private Point? _lastMousePos = null;
        private string _lastTooltipText = null;
        private int? _lastTooltipX = null;
        private int? _lastTooltipY = null;

        private StatGrowthStatistics GetCurerntGrowthStatistics() {
            var index = cbCurveGraphCharacter.SelectedIndex;
            return (index >= 0 && index < StatGrowthStatistics.Length) ? StatGrowthStatistics[index] : null;
        }

        private void CurveGraph_MouseMove(object sender, MouseEventArgs e) {
            // Do nothing if the position is unchanged or no data is available.
            var pos = e.Location;
            var statistics = GetCurerntGrowthStatistics();
            if (_lastMousePos.HasValue && pos == _lastMousePos.Value || statistics?.ProbableStatsByLevel == null)
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
            var pointIndex = Math.Max(0, pos.X < x0 + xSpan * (dataPoint.PointIndex + 0.5) ? dataPoint.PointIndex - 1 : dataPoint.PointIndex);

            var probableStats = statistics.ProbableStatsByLevel.Values.ToArray()[pointIndex];
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

        public void RecalcData() {
            var index = cbCurveGraphCharacter.SelectedIndex;
            var statistics = (index >= 0 && index < StatGrowthStatistics.Length ? StatGrowthStatistics[index] : null);
            statistics?.Recalc();
        }

        public void RefreshChart() {
            // We'll need to use some different values depending on the promotion level.
            var statistics     = GetCurerntGrowthStatistics();
            var promotionLevel = (int?)(statistics?.Stats?.PromotionLevel) ?? 0;
            var isPromoted     = promotionLevel >= 1;
            var maxLevel       = isPromoted ? 40 : 20;
            var maxValue       = promotionLevel == 0 ? 50 : promotionLevel == 1 ? 100 : 200;
            var targetStats    = statistics?.TargetStatsByLevel?.Values?.ToArray() ?? [];
            var probableStats  = statistics?.ProbableStatsByLevel?.Values?.ToArray() ?? [];

            CurveGraph.ChartAreas[0].AxisX.Minimum  = 0;
            CurveGraph.ChartAreas[0].AxisX.Maximum  = maxLevel;
            CurveGraph.ChartAreas[0].AxisX.Interval = isPromoted ? 10 : 5;
            CurveGraph.ChartAreas[0].AxisY.Maximum  = maxValue;
            CurveGraph.ChartAreas[0].AxisY.Interval = promotionLevel == 0 ? 5 : promotionLevel == 1 ? 10 : 20;

            foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                var statTypeStr = statType.ToString();

                var targetSeries = CurveGraph.Series[statTypeStr];
                targetSeries.Points.Clear();
                foreach (var dataPoint in targetStats)
                    _ = targetSeries.Points.AddXY(dataPoint.Level, dataPoint.Stats[statType]);

                var likelySeries = CurveGraph.Series["Likely " + statTypeStr];
                likelySeries.Points.Clear();
                foreach (var dataPoint in probableStats)
                    _ = likelySeries.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].Likely);

                var range1Series = CurveGraph.Series[statTypeStr + " Range 1 (50% Likely)"];
                range1Series.Points.Clear();
                foreach (var dataPoint in probableStats)
                    _ = range1Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[1], dataPoint.ProbableStats[statType].AtPercentages[2]);

                var range2Series = CurveGraph.Series[statTypeStr + " Range 2 (99% Likely)"];
                range2Series.Points.Clear();
                foreach (var dataPoint in probableStats)
                    _ = range2Series.Points.AddXY(dataPoint.Level, dataPoint.ProbableStats[statType].AtPercentages[0], dataPoint.ProbableStats[statType].AtPercentages[3]);
            }
        }
    }
}
