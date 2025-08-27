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

        public class StatsAtLevel {
            public StatsAtLevel(int level, Dictionary<StatType, double> stats) {
                Level = level;
                Stats = stats;
            }

            public int Level { get; }
            public Dictionary<StatType, double> Stats { get; }
        }

        public class ProbableStatsAtLevel {
            public ProbableStatsAtLevel(int level, Dictionary<StatType, ProbableStats> probableStats) {
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
        public Dictionary<int, StatsAtLevel> TargetStatsByLevel { get; private set; }
        public Dictionary<int, ProbableStatsAtLevel> ProbableStatsByLevel { get; private set; }

        public void Recalc() {
            // Data points for the chart.
            var targetStatsAtLevels = new List<StatsAtLevel>();
            var probableStatsAtLevel = new List<ProbableStatsAtLevel>();

            // We'll need to use some different values depending on the promotion level.
            var promotionLevel = (int) Stats.PromotionLevel;
            var isPromoted = Stats.IsPromoted;

            // Function to convert a ProbableValueSet to a ProbableStatsDict.
            Dictionary<StatType, ProbableStats> GetProbableStats(Dictionary<StatType, ProbableValueSet> pvs, Dictionary<StatType, double> targets)
                => pvs.ToDictionary(x => x.Key, x => new ProbableStats(x.Value, targets[x.Key]));

            // Add initial stats for level 1.
            var startStatValues = new Dictionary<StatType, double>();
            foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                var targetStat = GetStatGrowthRange(statType, 0).Begin;
                startStatValues.Add(statType, targetStat);
            }
            targetStatsAtLevels.Add(new StatsAtLevel(1, startStatValues));

            // Get initial probable stats for level 1 (which are the same as startStatValues).
            var currentProbableStatValues = new Dictionary<StatType, ProbableValueSet>();
            foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                currentProbableStatValues[statType] = new ProbableValueSet() {
                    { (int) startStatValues[statType], 1.00 }
                };
            }

            probableStatsAtLevel.Add(new ProbableStatsAtLevel(1, GetProbableStats(currentProbableStatValues, startStatValues)));

            // Populate data points for all stat growth groups, until the max level.
            foreach (var statGrowthGroup in GrowthStats.StatGrowthGroups[isPromoted]) {
                // Add the next target stats.
                var statValues = new Dictionary<StatType, double>();
                foreach (var statType in (StatType[]) Enum.GetValues(typeof(StatType))) {
                    var targetStat = GetStatGrowthRange(statType, statGrowthGroup.GroupIndex).End;
                    statValues.Add(statType, targetStat);
                }
                targetStatsAtLevels.Add(new StatsAtLevel(statGrowthGroup.Range.End, statValues));

                // Add probable stat values for every level in this stat growth group.
                for (var lv = statGrowthGroup.Range.Begin + 1; lv <= statGrowthGroup.Range.End; lv++) {
                    var lowPoint = targetStatsAtLevels.Last(x => x.Level <= lv);
                    var highPoint = targetStatsAtLevels.First(x => x.Level >= lv);

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
                    probableStatsAtLevel.Add(new ProbableStatsAtLevel(lv, GetProbableStats(currentProbableStatValues, targetStats)));
                }
            }

            TargetStatsByLevel = targetStatsAtLevels.ToDictionary(x => x.Level, x => x);
            ProbableStatsByLevel = probableStatsAtLevel.ToDictionary(x => x.Level, x => x);
        }

        private ValueRange<int> GetStatGrowthRange(StatType stat, int groupIndex) {
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

        private double GetAverageStatGrowthPerLevel(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[Stats.IsPromoted][groupIndex].Range.Range);
            return GrowthStats.GetAverageStatGrowthPerLevel(growthValue);
        }

        private string GetAverageStatGrowthPerLevelAsPercent(StatType stat, int groupIndex) {
            var growthValue = GrowthStats.GetStatGrowthValuePerLevel(GetStatGrowthRange(stat, groupIndex).Range, GrowthStats.StatGrowthGroups[Stats.IsPromoted][groupIndex].Range.Range);
            return (DebugGrowthValues ? string.Format("{0:x}", growthValue) + " || " : "") +
                    GrowthStats.GetAverageStatGrowthPerLevelAsPercent(growthValue);
        }

        [TableViewModelColumn(addressField: null, displayOrder:  0, displayGroup: "CurveCalc")] public string HPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 0);
        [TableViewModelColumn(addressField: null, displayOrder:  1, displayGroup: "CurveCalc")] public string HPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 1);
        [TableViewModelColumn(addressField: null, displayOrder:  2, displayGroup: "CurveCalc")] public string HPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 2);
        [TableViewModelColumn(addressField: null, displayOrder:  3, displayGroup: "CurveCalc")] public string HPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 3);
        [TableViewModelColumn(addressField: null, displayOrder:  4, displayGroup: "CurveCalc")] public string HPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 4);
        [TableViewModelColumn(addressField: null, displayOrder:  5, displayGroup: "CurveCalc")] public string HPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.HP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 10, displayGroup: "CurveCalc")] public string MPgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 11, displayGroup: "CurveCalc")] public string MPgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 12, displayGroup: "CurveCalc")] public string MPgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 13, displayGroup: "CurveCalc")] public string MPgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 14, displayGroup: "CurveCalc")] public string MPgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 15, displayGroup: "CurveCalc")] public string MPgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.MP, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 20, displayGroup: "CurveCalc")] public string Atkgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 21, displayGroup: "CurveCalc")] public string Atkgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 22, displayGroup: "CurveCalc")] public string Atkgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 23, displayGroup: "CurveCalc")] public string Atkgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 24, displayGroup: "CurveCalc")] public string Atkgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 25, displayGroup: "CurveCalc")] public string Atkgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Atk, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 30, displayGroup: "CurveCalc")] public string Defgroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 31, displayGroup: "CurveCalc")] public string Defgroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 32, displayGroup: "CurveCalc")] public string Defgroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 33, displayGroup: "CurveCalc")] public string Defgroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 34, displayGroup: "CurveCalc")] public string Defgroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 35, displayGroup: "CurveCalc")] public string Defgroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Def, 5);

        [TableViewModelColumn(addressField: null, displayOrder: 40, displayGroup: "CurveCalc")] public string Agigroup1 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 0);
        [TableViewModelColumn(addressField: null, displayOrder: 41, displayGroup: "CurveCalc")] public string Agigroup2 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 1);
        [TableViewModelColumn(addressField: null, displayOrder: 42, displayGroup: "CurveCalc")] public string Agigroup3 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 2);
        [TableViewModelColumn(addressField: null, displayOrder: 43, displayGroup: "CurveCalc")] public string Agigroup4 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 3);
        [TableViewModelColumn(addressField: null, displayOrder: 44, displayGroup: "CurveCalc")] public string Agigroup5 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 4);
        [TableViewModelColumn(addressField: null, displayOrder: 45, displayGroup: "CurveCalc")] public string Agigroup6 => GetAverageStatGrowthPerLevelAsPercent(StatType.Agi, 5);

        private float? GetLikelyStatAtLevel(StatType statType, int level) {
            var value = ProbableStatsByLevel.TryGetValue(level, out var valueOut) ? (float?) valueOut.ProbableStats[statType].Likely : null;
            return value;
        }

        [TableViewModelColumn(addressField: null, displayOrder: 101, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv01  => GetLikelyStatAtLevel(StatType.HP,   1);
        [TableViewModelColumn(addressField: null, displayOrder: 102, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv02  => GetLikelyStatAtLevel(StatType.HP,   2);
        [TableViewModelColumn(addressField: null, displayOrder: 103, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv03  => GetLikelyStatAtLevel(StatType.HP,   3);
        [TableViewModelColumn(addressField: null, displayOrder: 104, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv04  => GetLikelyStatAtLevel(StatType.HP,   4);
        [TableViewModelColumn(addressField: null, displayOrder: 105, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv05  => GetLikelyStatAtLevel(StatType.HP,   5);
        [TableViewModelColumn(addressField: null, displayOrder: 106, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv06  => GetLikelyStatAtLevel(StatType.HP,   6);
        [TableViewModelColumn(addressField: null, displayOrder: 107, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv07  => GetLikelyStatAtLevel(StatType.HP,   7);
        [TableViewModelColumn(addressField: null, displayOrder: 108, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv08  => GetLikelyStatAtLevel(StatType.HP,   8);
        [TableViewModelColumn(addressField: null, displayOrder: 109, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv09  => GetLikelyStatAtLevel(StatType.HP,   9);
        [TableViewModelColumn(addressField: null, displayOrder: 110, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv10  => GetLikelyStatAtLevel(StatType.HP,  10);
        [TableViewModelColumn(addressField: null, displayOrder: 111, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv11  => GetLikelyStatAtLevel(StatType.HP,  11);
        [TableViewModelColumn(addressField: null, displayOrder: 112, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv12  => GetLikelyStatAtLevel(StatType.HP,  12);
        [TableViewModelColumn(addressField: null, displayOrder: 113, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv13  => GetLikelyStatAtLevel(StatType.HP,  13);
        [TableViewModelColumn(addressField: null, displayOrder: 114, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv14  => GetLikelyStatAtLevel(StatType.HP,  14);
        [TableViewModelColumn(addressField: null, displayOrder: 115, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv15  => GetLikelyStatAtLevel(StatType.HP,  15);
        [TableViewModelColumn(addressField: null, displayOrder: 116, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv16  => GetLikelyStatAtLevel(StatType.HP,  16);
        [TableViewModelColumn(addressField: null, displayOrder: 117, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv17  => GetLikelyStatAtLevel(StatType.HP,  17);
        [TableViewModelColumn(addressField: null, displayOrder: 118, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv18  => GetLikelyStatAtLevel(StatType.HP,  18);
        [TableViewModelColumn(addressField: null, displayOrder: 119, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv19  => GetLikelyStatAtLevel(StatType.HP,  19);
        [TableViewModelColumn(addressField: null, displayOrder: 120, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv20  => GetLikelyStatAtLevel(StatType.HP,  20);
        [TableViewModelColumn(addressField: null, displayOrder: 121, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv21  => GetLikelyStatAtLevel(StatType.HP,  21);
        [TableViewModelColumn(addressField: null, displayOrder: 122, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv22  => GetLikelyStatAtLevel(StatType.HP,  22);
        [TableViewModelColumn(addressField: null, displayOrder: 123, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv23  => GetLikelyStatAtLevel(StatType.HP,  23);
        [TableViewModelColumn(addressField: null, displayOrder: 124, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv24  => GetLikelyStatAtLevel(StatType.HP,  24);
        [TableViewModelColumn(addressField: null, displayOrder: 125, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv25  => GetLikelyStatAtLevel(StatType.HP,  25);
        [TableViewModelColumn(addressField: null, displayOrder: 126, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv26  => GetLikelyStatAtLevel(StatType.HP,  26);
        [TableViewModelColumn(addressField: null, displayOrder: 127, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv27  => GetLikelyStatAtLevel(StatType.HP,  27);
        [TableViewModelColumn(addressField: null, displayOrder: 128, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv28  => GetLikelyStatAtLevel(StatType.HP,  28);
        [TableViewModelColumn(addressField: null, displayOrder: 129, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv29  => GetLikelyStatAtLevel(StatType.HP,  29);
        [TableViewModelColumn(addressField: null, displayOrder: 130, displayFormat: "F1", displayGroup: "LikelyStats")] public float? HP_Lv30  => GetLikelyStatAtLevel(StatType.HP,  30);

        [TableViewModelColumn(addressField: null, displayOrder: 201, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv01  => GetLikelyStatAtLevel(StatType.MP,   1);
        [TableViewModelColumn(addressField: null, displayOrder: 202, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv02  => GetLikelyStatAtLevel(StatType.MP,   2);
        [TableViewModelColumn(addressField: null, displayOrder: 203, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv03  => GetLikelyStatAtLevel(StatType.MP,   3);
        [TableViewModelColumn(addressField: null, displayOrder: 204, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv04  => GetLikelyStatAtLevel(StatType.MP,   4);
        [TableViewModelColumn(addressField: null, displayOrder: 205, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv05  => GetLikelyStatAtLevel(StatType.MP,   5);
        [TableViewModelColumn(addressField: null, displayOrder: 206, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv06  => GetLikelyStatAtLevel(StatType.MP,   6);
        [TableViewModelColumn(addressField: null, displayOrder: 207, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv07  => GetLikelyStatAtLevel(StatType.MP,   7);
        [TableViewModelColumn(addressField: null, displayOrder: 208, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv08  => GetLikelyStatAtLevel(StatType.MP,   8);
        [TableViewModelColumn(addressField: null, displayOrder: 209, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv09  => GetLikelyStatAtLevel(StatType.MP,   9);
        [TableViewModelColumn(addressField: null, displayOrder: 210, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv10  => GetLikelyStatAtLevel(StatType.MP,  10);
        [TableViewModelColumn(addressField: null, displayOrder: 211, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv11  => GetLikelyStatAtLevel(StatType.MP,  11);
        [TableViewModelColumn(addressField: null, displayOrder: 212, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv12  => GetLikelyStatAtLevel(StatType.MP,  12);
        [TableViewModelColumn(addressField: null, displayOrder: 213, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv13  => GetLikelyStatAtLevel(StatType.MP,  13);
        [TableViewModelColumn(addressField: null, displayOrder: 214, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv14  => GetLikelyStatAtLevel(StatType.MP,  14);
        [TableViewModelColumn(addressField: null, displayOrder: 215, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv15  => GetLikelyStatAtLevel(StatType.MP,  15);
        [TableViewModelColumn(addressField: null, displayOrder: 216, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv16  => GetLikelyStatAtLevel(StatType.MP,  16);
        [TableViewModelColumn(addressField: null, displayOrder: 217, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv17  => GetLikelyStatAtLevel(StatType.MP,  17);
        [TableViewModelColumn(addressField: null, displayOrder: 218, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv18  => GetLikelyStatAtLevel(StatType.MP,  18);
        [TableViewModelColumn(addressField: null, displayOrder: 219, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv19  => GetLikelyStatAtLevel(StatType.MP,  19);
        [TableViewModelColumn(addressField: null, displayOrder: 220, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv20  => GetLikelyStatAtLevel(StatType.MP,  20);
        [TableViewModelColumn(addressField: null, displayOrder: 221, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv21  => GetLikelyStatAtLevel(StatType.MP,  21);
        [TableViewModelColumn(addressField: null, displayOrder: 222, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv22  => GetLikelyStatAtLevel(StatType.MP,  22);
        [TableViewModelColumn(addressField: null, displayOrder: 223, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv23  => GetLikelyStatAtLevel(StatType.MP,  23);
        [TableViewModelColumn(addressField: null, displayOrder: 224, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv24  => GetLikelyStatAtLevel(StatType.MP,  24);
        [TableViewModelColumn(addressField: null, displayOrder: 225, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv25  => GetLikelyStatAtLevel(StatType.MP,  25);
        [TableViewModelColumn(addressField: null, displayOrder: 226, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv26  => GetLikelyStatAtLevel(StatType.MP,  26);
        [TableViewModelColumn(addressField: null, displayOrder: 227, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv27  => GetLikelyStatAtLevel(StatType.MP,  27);
        [TableViewModelColumn(addressField: null, displayOrder: 228, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv28  => GetLikelyStatAtLevel(StatType.MP,  28);
        [TableViewModelColumn(addressField: null, displayOrder: 229, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv29  => GetLikelyStatAtLevel(StatType.MP,  29);
        [TableViewModelColumn(addressField: null, displayOrder: 230, displayFormat: "F1", displayGroup: "LikelyStats")] public float? MP_Lv30  => GetLikelyStatAtLevel(StatType.MP,  30);

        [TableViewModelColumn(addressField: null, displayOrder: 301, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv01 => GetLikelyStatAtLevel(StatType.Atk,  1);
        [TableViewModelColumn(addressField: null, displayOrder: 302, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv02 => GetLikelyStatAtLevel(StatType.Atk,  2);
        [TableViewModelColumn(addressField: null, displayOrder: 303, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv03 => GetLikelyStatAtLevel(StatType.Atk,  3);
        [TableViewModelColumn(addressField: null, displayOrder: 304, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv04 => GetLikelyStatAtLevel(StatType.Atk,  4);
        [TableViewModelColumn(addressField: null, displayOrder: 305, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv05 => GetLikelyStatAtLevel(StatType.Atk,  5);
        [TableViewModelColumn(addressField: null, displayOrder: 306, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv06 => GetLikelyStatAtLevel(StatType.Atk,  6);
        [TableViewModelColumn(addressField: null, displayOrder: 307, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv07 => GetLikelyStatAtLevel(StatType.Atk,  7);
        [TableViewModelColumn(addressField: null, displayOrder: 308, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv08 => GetLikelyStatAtLevel(StatType.Atk,  8);
        [TableViewModelColumn(addressField: null, displayOrder: 309, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv09 => GetLikelyStatAtLevel(StatType.Atk,  9);
        [TableViewModelColumn(addressField: null, displayOrder: 310, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv10 => GetLikelyStatAtLevel(StatType.Atk, 10);
        [TableViewModelColumn(addressField: null, displayOrder: 311, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv11 => GetLikelyStatAtLevel(StatType.Atk, 11);
        [TableViewModelColumn(addressField: null, displayOrder: 312, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv12 => GetLikelyStatAtLevel(StatType.Atk, 12);
        [TableViewModelColumn(addressField: null, displayOrder: 313, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv13 => GetLikelyStatAtLevel(StatType.Atk, 13);
        [TableViewModelColumn(addressField: null, displayOrder: 314, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv14 => GetLikelyStatAtLevel(StatType.Atk, 14);
        [TableViewModelColumn(addressField: null, displayOrder: 315, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv15 => GetLikelyStatAtLevel(StatType.Atk, 15);
        [TableViewModelColumn(addressField: null, displayOrder: 316, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv16 => GetLikelyStatAtLevel(StatType.Atk, 16);
        [TableViewModelColumn(addressField: null, displayOrder: 317, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv17 => GetLikelyStatAtLevel(StatType.Atk, 17);
        [TableViewModelColumn(addressField: null, displayOrder: 318, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv18 => GetLikelyStatAtLevel(StatType.Atk, 18);
        [TableViewModelColumn(addressField: null, displayOrder: 319, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv19 => GetLikelyStatAtLevel(StatType.Atk, 19);
        [TableViewModelColumn(addressField: null, displayOrder: 320, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv20 => GetLikelyStatAtLevel(StatType.Atk, 20);
        [TableViewModelColumn(addressField: null, displayOrder: 321, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv21 => GetLikelyStatAtLevel(StatType.Atk, 21);
        [TableViewModelColumn(addressField: null, displayOrder: 322, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv22 => GetLikelyStatAtLevel(StatType.Atk, 22);
        [TableViewModelColumn(addressField: null, displayOrder: 323, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv23 => GetLikelyStatAtLevel(StatType.Atk, 23);
        [TableViewModelColumn(addressField: null, displayOrder: 324, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv24 => GetLikelyStatAtLevel(StatType.Atk, 24);
        [TableViewModelColumn(addressField: null, displayOrder: 325, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv25 => GetLikelyStatAtLevel(StatType.Atk, 25);
        [TableViewModelColumn(addressField: null, displayOrder: 326, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv26 => GetLikelyStatAtLevel(StatType.Atk, 26);
        [TableViewModelColumn(addressField: null, displayOrder: 327, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv27 => GetLikelyStatAtLevel(StatType.Atk, 27);
        [TableViewModelColumn(addressField: null, displayOrder: 328, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv28 => GetLikelyStatAtLevel(StatType.Atk, 28);
        [TableViewModelColumn(addressField: null, displayOrder: 329, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv29 => GetLikelyStatAtLevel(StatType.Atk, 29);
        [TableViewModelColumn(addressField: null, displayOrder: 330, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Atk_Lv30 => GetLikelyStatAtLevel(StatType.Atk, 30);

        [TableViewModelColumn(addressField: null, displayOrder: 401, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv01 => GetLikelyStatAtLevel(StatType.Def,  1);
        [TableViewModelColumn(addressField: null, displayOrder: 402, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv02 => GetLikelyStatAtLevel(StatType.Def,  2);
        [TableViewModelColumn(addressField: null, displayOrder: 403, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv03 => GetLikelyStatAtLevel(StatType.Def,  3);
        [TableViewModelColumn(addressField: null, displayOrder: 404, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv04 => GetLikelyStatAtLevel(StatType.Def,  4);
        [TableViewModelColumn(addressField: null, displayOrder: 405, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv05 => GetLikelyStatAtLevel(StatType.Def,  5);
        [TableViewModelColumn(addressField: null, displayOrder: 406, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv06 => GetLikelyStatAtLevel(StatType.Def,  6);
        [TableViewModelColumn(addressField: null, displayOrder: 407, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv07 => GetLikelyStatAtLevel(StatType.Def,  7);
        [TableViewModelColumn(addressField: null, displayOrder: 408, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv08 => GetLikelyStatAtLevel(StatType.Def,  8);
        [TableViewModelColumn(addressField: null, displayOrder: 409, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv09 => GetLikelyStatAtLevel(StatType.Def,  9);
        [TableViewModelColumn(addressField: null, displayOrder: 410, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv10 => GetLikelyStatAtLevel(StatType.Def, 10);
        [TableViewModelColumn(addressField: null, displayOrder: 411, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv11 => GetLikelyStatAtLevel(StatType.Def, 11);
        [TableViewModelColumn(addressField: null, displayOrder: 412, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv12 => GetLikelyStatAtLevel(StatType.Def, 12);
        [TableViewModelColumn(addressField: null, displayOrder: 413, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv13 => GetLikelyStatAtLevel(StatType.Def, 13);
        [TableViewModelColumn(addressField: null, displayOrder: 414, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv14 => GetLikelyStatAtLevel(StatType.Def, 14);
        [TableViewModelColumn(addressField: null, displayOrder: 415, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv15 => GetLikelyStatAtLevel(StatType.Def, 15);
        [TableViewModelColumn(addressField: null, displayOrder: 416, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv16 => GetLikelyStatAtLevel(StatType.Def, 16);
        [TableViewModelColumn(addressField: null, displayOrder: 417, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv17 => GetLikelyStatAtLevel(StatType.Def, 17);
        [TableViewModelColumn(addressField: null, displayOrder: 418, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv18 => GetLikelyStatAtLevel(StatType.Def, 18);
        [TableViewModelColumn(addressField: null, displayOrder: 419, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv19 => GetLikelyStatAtLevel(StatType.Def, 19);
        [TableViewModelColumn(addressField: null, displayOrder: 420, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv20 => GetLikelyStatAtLevel(StatType.Def, 20);
        [TableViewModelColumn(addressField: null, displayOrder: 421, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv21 => GetLikelyStatAtLevel(StatType.Def, 21);
        [TableViewModelColumn(addressField: null, displayOrder: 422, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv22 => GetLikelyStatAtLevel(StatType.Def, 22);
        [TableViewModelColumn(addressField: null, displayOrder: 423, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv23 => GetLikelyStatAtLevel(StatType.Def, 23);
        [TableViewModelColumn(addressField: null, displayOrder: 424, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv24 => GetLikelyStatAtLevel(StatType.Def, 24);
        [TableViewModelColumn(addressField: null, displayOrder: 425, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv25 => GetLikelyStatAtLevel(StatType.Def, 25);
        [TableViewModelColumn(addressField: null, displayOrder: 426, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv26 => GetLikelyStatAtLevel(StatType.Def, 26);
        [TableViewModelColumn(addressField: null, displayOrder: 427, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv27 => GetLikelyStatAtLevel(StatType.Def, 27);
        [TableViewModelColumn(addressField: null, displayOrder: 428, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv28 => GetLikelyStatAtLevel(StatType.Def, 28);
        [TableViewModelColumn(addressField: null, displayOrder: 429, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv29 => GetLikelyStatAtLevel(StatType.Def, 29);
        [TableViewModelColumn(addressField: null, displayOrder: 430, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Def_Lv30 => GetLikelyStatAtLevel(StatType.Def, 30);

        [TableViewModelColumn(addressField: null, displayOrder: 501, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv01 => GetLikelyStatAtLevel(StatType.Agi,  1);
        [TableViewModelColumn(addressField: null, displayOrder: 502, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv02 => GetLikelyStatAtLevel(StatType.Agi,  2);
        [TableViewModelColumn(addressField: null, displayOrder: 503, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv03 => GetLikelyStatAtLevel(StatType.Agi,  3);
        [TableViewModelColumn(addressField: null, displayOrder: 504, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv04 => GetLikelyStatAtLevel(StatType.Agi,  4);
        [TableViewModelColumn(addressField: null, displayOrder: 505, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv05 => GetLikelyStatAtLevel(StatType.Agi,  5);
        [TableViewModelColumn(addressField: null, displayOrder: 506, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv06 => GetLikelyStatAtLevel(StatType.Agi,  6);
        [TableViewModelColumn(addressField: null, displayOrder: 507, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv07 => GetLikelyStatAtLevel(StatType.Agi,  7);
        [TableViewModelColumn(addressField: null, displayOrder: 508, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv08 => GetLikelyStatAtLevel(StatType.Agi,  8);
        [TableViewModelColumn(addressField: null, displayOrder: 509, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv09 => GetLikelyStatAtLevel(StatType.Agi,  9);
        [TableViewModelColumn(addressField: null, displayOrder: 510, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv10 => GetLikelyStatAtLevel(StatType.Agi, 10);
        [TableViewModelColumn(addressField: null, displayOrder: 511, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv11 => GetLikelyStatAtLevel(StatType.Agi, 11);
        [TableViewModelColumn(addressField: null, displayOrder: 512, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv12 => GetLikelyStatAtLevel(StatType.Agi, 12);
        [TableViewModelColumn(addressField: null, displayOrder: 513, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv13 => GetLikelyStatAtLevel(StatType.Agi, 13);
        [TableViewModelColumn(addressField: null, displayOrder: 514, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv14 => GetLikelyStatAtLevel(StatType.Agi, 14);
        [TableViewModelColumn(addressField: null, displayOrder: 515, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv15 => GetLikelyStatAtLevel(StatType.Agi, 15);
        [TableViewModelColumn(addressField: null, displayOrder: 516, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv16 => GetLikelyStatAtLevel(StatType.Agi, 16);
        [TableViewModelColumn(addressField: null, displayOrder: 517, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv17 => GetLikelyStatAtLevel(StatType.Agi, 17);
        [TableViewModelColumn(addressField: null, displayOrder: 518, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv18 => GetLikelyStatAtLevel(StatType.Agi, 18);
        [TableViewModelColumn(addressField: null, displayOrder: 519, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv19 => GetLikelyStatAtLevel(StatType.Agi, 19);
        [TableViewModelColumn(addressField: null, displayOrder: 520, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv20 => GetLikelyStatAtLevel(StatType.Agi, 20);
        [TableViewModelColumn(addressField: null, displayOrder: 521, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv21 => GetLikelyStatAtLevel(StatType.Agi, 21);
        [TableViewModelColumn(addressField: null, displayOrder: 522, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv22 => GetLikelyStatAtLevel(StatType.Agi, 22);
        [TableViewModelColumn(addressField: null, displayOrder: 523, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv23 => GetLikelyStatAtLevel(StatType.Agi, 23);
        [TableViewModelColumn(addressField: null, displayOrder: 524, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv24 => GetLikelyStatAtLevel(StatType.Agi, 24);
        [TableViewModelColumn(addressField: null, displayOrder: 525, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv25 => GetLikelyStatAtLevel(StatType.Agi, 25);
        [TableViewModelColumn(addressField: null, displayOrder: 526, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv26 => GetLikelyStatAtLevel(StatType.Agi, 26);
        [TableViewModelColumn(addressField: null, displayOrder: 527, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv27 => GetLikelyStatAtLevel(StatType.Agi, 27);
        [TableViewModelColumn(addressField: null, displayOrder: 528, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv28 => GetLikelyStatAtLevel(StatType.Agi, 28);
        [TableViewModelColumn(addressField: null, displayOrder: 529, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv29 => GetLikelyStatAtLevel(StatType.Agi, 29);
        [TableViewModelColumn(addressField: null, displayOrder: 530, displayFormat: "F1", displayGroup: "LikelyStats")] public float? Agi_Lv30 => GetLikelyStatAtLevel(StatType.Agi, 30);
    }
}
