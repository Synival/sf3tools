using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.Statistics;
using SF3.Statistics;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
    public class StatGrowthStatistics : Struct {
        /// <summary>
        /// When enabled, GetAverageStatGrowthPerLevelAsPercent() will show the "growthValue" in its output
        /// </summary>
        public static bool DebugGrowthValues { get; set; } = false;

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
                    .Where(x => x.Value / totalPool * 100.0 >= minimumProbabilityCutoff)
                    .ToDictionary(x => x.Key, x => x.Value);
                totalPool = adjustedSet.Sum(x => x.Value);

                var currentTotalProb = 0.0;
                var lastKey = adjustedSet.Last().Key;

                foreach (var kv in adjustedSet) {
                    var probability = kv.Value / totalPool * 100.0;
                    var nextTotalProb = kv.Key == lastKey ? 100.0 : currentTotalProb + probability;

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
            public StatDataPoint(int level, Dictionary<StatType, double> stats) {
                Level = level;
                Stats = stats;
            }

            public int Level { get; }
            public Dictionary<StatType, double> Stats { get; }
        }

        public class ProbableStatsDataPoint {
            public ProbableStatsDataPoint(int level, Dictionary<StatType, ProbableStats> probableStats) {
                Level = level;
                ProbableStats = probableStats;
            }

            public string MakeReport(StatType stat) {
                return "Lv" + Level + " " + stat.ToString() + ":\n" +
                    "---------------------------\n" +
                    ProbableStats[stat].MakeReport();
            }

            public int Level { get; }
            public Dictionary<StatType, ProbableStats> ProbableStats { get; }
        }

        /// <summary>
        /// *Mathematical* statistics for *character* stats for an individual character at a specific promotion.
        /// </summary>
        /// <param name="stats">Character stats to generate mathematical statistics from.</param>
        public StatGrowthStatistics(Stats stats)
        : base(stats.Data, stats.ID, stats.Name + " (Stats)", stats.Address, stats.Size) {
            Stats = stats;
            Recalc();
        }

        public Stats Stats { get; }
        public StatDataPoint[] TargetStatsDataPoints { get; private set; }
        public ProbableStatsDataPoint[] ProbableStatsDataPoints { get; private set; }

        public void Recalc() {
            // Data points for the chart.
            var targetStatDataPoints = new List<StatDataPoint>();
            var probableStatsDataPoints = new List<ProbableStatsDataPoint>();

            // We'll need to use some different values depending on the promotion level.
            var promotionLevel = (int) Stats.PromotionLevel;
            var isPromoted = Stats.IsPromoted;

            // Default axis ranges.
            // NOTE: The actual stat gain caps at (30, 99, 99).
            //       This is different from level gains, which are (20, 99, 99).
            var maxValue = promotionLevel == 0 ? 50 : promotionLevel == 1 ? 100 : 200;

            // Function to convert a ProbableValueSet to a ProbableStatsDict.
            Dictionary<StatType, ProbableStats> GetProbableStats(Dictionary<StatType, ProbableValueSet> pvs, Dictionary<StatType, double> targets)
                => pvs.ToDictionary(x => x.Key, x => new ProbableStats(x.Value, targets[x.Key]));

            // Add initial stats for level 1.
            var startStatValues = new Dictionary<StatType, double>();
            foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                var targetStat = GetStatGrowthRange(statType, 0).Begin;
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
                var statValues = new Dictionary<StatType, double>();
                foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                    var targetStat = GetStatGrowthRange(statType, statGrowthGroup.GroupIndex).End;
                    statValues.Add(statType, targetStat);
                    maxValue = Math.Max(maxValue, targetStat);
                }
                targetStatDataPoints.Add(new StatDataPoint(statGrowthGroup.Range.End, statValues));

                // Add probable stat values for every level in this stat growth group.
                for (var lv = statGrowthGroup.Range.Begin + 1; lv <= statGrowthGroup.Range.End; lv++) {
                    var lowPoint = targetStatDataPoints.Last(x => x.Level <= lv);
                    var highPoint = targetStatDataPoints.First(x => x.Level >= lv);

                    Dictionary<StatType, double> targetStats;
                    if (lowPoint == highPoint || lv == lowPoint.Level)
                        targetStats = lowPoint.Stats;
                    else if (lv == highPoint.Level)
                        targetStats = highPoint.Stats;
                    else {
                        var levelRange = highPoint.Level - lowPoint.Level;
                        var rangePercent =  (lv - lowPoint.Level) / (double) levelRange;
                        targetStats = lowPoint.Stats.Keys
                            .ToDictionary(x => x, x => lowPoint.Stats[x] + (highPoint.Stats[x] - lowPoint.Stats[x]) * rangePercent);
                    }

                    foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                        var growthValue = GetAverageStatGrowthPerLevel(statType, statGrowthGroup.GroupIndex);
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

            TargetStatsDataPoints   = targetStatDataPoints.ToArray();
            ProbableStatsDataPoints = probableStatsDataPoints.ToArray();
        }

        public ValueRange<int> GetStatGrowthRange(StatType stat, int groupIndex) {
            switch (stat) {
                case StatType.HP:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.HPCurve1,     Stats.HPCurve5);
                        case 1: return new ValueRange<int>(Stats.HPCurve5,     Stats.HPCurve10);
                        case 2: return new ValueRange<int>(Stats.HPCurve10,    Stats.HPCurve12_15);
                        case 3: return new ValueRange<int>(Stats.HPCurve12_15, Stats.HPCurve14_20);
                        case 4: return new ValueRange<int>(Stats.HPCurve14_20, Stats.HPCurve17_30);
                        case 5: return new ValueRange<int>(Stats.HPCurve17_30, Stats.HPCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.MP:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.MPCurve1,     Stats.MPCurve5);
                        case 1: return new ValueRange<int>(Stats.MPCurve5,     Stats.MPCurve10);
                        case 2: return new ValueRange<int>(Stats.MPCurve10,    Stats.MPCurve12_15);
                        case 3: return new ValueRange<int>(Stats.MPCurve12_15, Stats.MPCurve14_20);
                        case 4: return new ValueRange<int>(Stats.MPCurve14_20, Stats.MPCurve17_30);
                        case 5: return new ValueRange<int>(Stats.MPCurve17_30, Stats.MPCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Atk:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.AtkCurve1,     Stats.AtkCurve5);
                        case 1: return new ValueRange<int>(Stats.AtkCurve5,     Stats.AtkCurve10);
                        case 2: return new ValueRange<int>(Stats.AtkCurve10,    Stats.AtkCurve12_15);
                        case 3: return new ValueRange<int>(Stats.AtkCurve12_15, Stats.AtkCurve14_20);
                        case 4: return new ValueRange<int>(Stats.AtkCurve14_20, Stats.AtkCurve17_30);
                        case 5: return new ValueRange<int>(Stats.AtkCurve17_30, Stats.AtkCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Def:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.DefCurve1,     Stats.DefCurve5);
                        case 1: return new ValueRange<int>(Stats.DefCurve5,     Stats.DefCurve10);
                        case 2: return new ValueRange<int>(Stats.DefCurve10,    Stats.DefCurve12_15);
                        case 3: return new ValueRange<int>(Stats.DefCurve12_15, Stats.DefCurve14_20);
                        case 4: return new ValueRange<int>(Stats.DefCurve14_20, Stats.DefCurve17_30);
                        case 5: return new ValueRange<int>(Stats.DefCurve17_30, Stats.DefCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                case StatType.Agi:
                    switch (groupIndex) {
                        case 0: return new ValueRange<int>(Stats.AgiCurve1,     Stats.AgiCurve5);
                        case 1: return new ValueRange<int>(Stats.AgiCurve5,     Stats.AgiCurve10);
                        case 2: return new ValueRange<int>(Stats.AgiCurve10,    Stats.AgiCurve12_15);
                        case 3: return new ValueRange<int>(Stats.AgiCurve12_15, Stats.AgiCurve14_20);
                        case 4: return new ValueRange<int>(Stats.AgiCurve14_20, Stats.AgiCurve17_30);
                        case 5: return new ValueRange<int>(Stats.AgiCurve17_30, Stats.AgiCurve30_99);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public double GetAverageStatGrowthPerLevel(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[Stats.IsPromoted][groupIndex].Range.Range);
            return GrowthStats.GetAverageStatGrowthPerLevel(growthValue);
        }

        public string GetAverageStatGrowthPerLevelAsPercent(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[Stats.IsPromoted][groupIndex].Range.Range);
            return (DebugGrowthValues ? string.Format("{0:x}", growthValue) + " || " : "") +
                    GrowthStats.GetAverageStatGrowthPerLevelAsPercent(growthValue);
        }

        [TableViewModelColumn(addressField: null, displayOrder: 200, displayGroup: "CurveCalc")] public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 201, displayGroup: "CurveCalc")] public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 202, displayGroup: "CurveCalc")] public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 203, displayGroup: "CurveCalc")] public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 204, displayGroup: "CurveCalc")] public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 205, displayGroup: "CurveCalc")] public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 210, displayGroup: "CurveCalc")] public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 211, displayGroup: "CurveCalc")] public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 212, displayGroup: "CurveCalc")] public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 213, displayGroup: "CurveCalc")] public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 214, displayGroup: "CurveCalc")] public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 215, displayGroup: "CurveCalc")] public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 220, displayGroup: "CurveCalc")] public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 221, displayGroup: "CurveCalc")] public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 222, displayGroup: "CurveCalc")] public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 223, displayGroup: "CurveCalc")] public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 224, displayGroup: "CurveCalc")] public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 225, displayGroup: "CurveCalc")] public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 230, displayGroup: "CurveCalc")] public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 231, displayGroup: "CurveCalc")] public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 232, displayGroup: "CurveCalc")] public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 233, displayGroup: "CurveCalc")] public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 234, displayGroup: "CurveCalc")] public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 235, displayGroup: "CurveCalc")] public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 240, displayGroup: "CurveCalc")] public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 241, displayGroup: "CurveCalc")] public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 242, displayGroup: "CurveCalc")] public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 243, displayGroup: "CurveCalc")] public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 244, displayGroup: "CurveCalc")] public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 245, displayGroup: "CurveCalc")] public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);
    }
}
